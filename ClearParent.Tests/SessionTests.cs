using Microsoft.AspNetCore.DataProtection;
using ClearParent;
using ClearParent.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ClearParent.Tests;
public class SessionTests
{
    [Fact]
    public async Task RevokedSessionCookieStopsAuthenticatingAndMutationsRequireCsrfToken()
    {
        var database = Guid.NewGuid().ToString();
        await using var app = Program.BuildApplication([], builder =>
        {
            builder.WebHost.UseTestServer();
            builder.Configuration.AddInMemoryCollection(new Dictionary<string,string?> { ["ConnectionStrings:ClearParent"] = "Server=unused;Database=unused;Integrated Security=True" });
            builder.Services.RemoveAll<ApplicationDbContext>();
            builder.Services.RemoveAll<DbContextOptions<ApplicationDbContext>>();
            builder.Services.RemoveAll<Microsoft.EntityFrameworkCore.Infrastructure.IDbContextOptionsConfiguration<ApplicationDbContext>>();
            builder.Services.AddDbContext<ApplicationDbContext>(o => o.UseInMemoryDatabase(database));
            builder.Services.AddDataProtection().UseEphemeralDataProtectionProvider();
        });
        app.MapGet("/synthetic/signin", async (UserManager<ApplicationUser> users, SignInManager<ApplicationUser> signIn) =>
        {
            var user = new ApplicationUser { UserName = "synthetic-session@example.invalid", Email = "synthetic-session@example.invalid", EmailConfirmed = true };
            Assert.True((await users.CreateAsync(user, "Synthetic-Session-175!Ab")).Succeeded);
            await signIn.SignInAsync(user, false);
            return Results.Ok();
        }).AllowAnonymous();
        await app.StartAsync();
        var client = app.GetTestClient(); client.BaseAddress = new Uri("https://localhost");
        var login = await client.GetAsync("/synthetic/signin");
        Assert.True(login.IsSuccessStatusCode);
        var cookie = login.Headers.GetValues("Set-Cookie").Single(x => x.StartsWith(".AspNetCore.Identity.Application="));
        client.DefaultRequestHeaders.Add("Cookie", cookie.Split(';')[0]);
        var devices = await client.GetAsync("/Account/Devices");
        Assert.Equal(System.Net.HttpStatusCode.OK, devices.StatusCode);
        var csrfCookie = devices.Headers.GetValues("Set-Cookie").Single(x => x.StartsWith(".AspNetCore.Antiforgery."));
        var token = System.Text.RegularExpressions.Regex.Match(await devices.Content.ReadAsStringAsync(), "name=\"__RequestVerificationToken\"[^>]*value=\"([^\"]+)\"").Groups[1].Value;
        Assert.NotEmpty(token);
        client.DefaultRequestHeaders.Remove("Cookie");
        client.DefaultRequestHeaders.Add("Cookie", cookie.Split(';')[0] + "; " + csrfCookie.Split(';')[0]);
        var childForm = await client.GetAsync("/Children/Create");
        Assert.Equal(System.Net.HttpStatusCode.OK, childForm.StatusCode);
        Assert.Contains("__RequestVerificationToken", await childForm.Content.ReadAsStringAsync());
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, (await client.PostAsync("/Children/Create", new FormUrlEncodedContent(new Dictionary<string,string> { ["Name"] = "Synthetic child" }))).StatusCode);
        var childCreated = await client.PostAsync("/Children/Create", new FormUrlEncodedContent(new Dictionary<string,string> {
            ["Name"] = "Synthetic <child>", ["Birthday"] = "2020-01-01", ["UserId"] = "synthetic-other", ["__RequestVerificationToken"] = token }));
        Assert.Equal(System.Net.HttpStatusCode.Redirect, childCreated.StatusCode);
        var childList = await client.GetStringAsync("/Children");
        Assert.Contains("Synthetic &lt;child&gt;", childList);
        Assert.Contains("2020-01-01", childList);
        var profileSaved = await client.PostAsync("/Account/Profile", new FormUrlEncodedContent(new Dictionary<string,string> {
            ["FirstName"] = "Synthetic", ["LastName"] = "Parent", ["UserId"] = "synthetic-other", ["__RequestVerificationToken"] = token }));
        Assert.Equal(System.Net.HttpStatusCode.Redirect, profileSaved.StatusCode);
        using (var profileScope = app.Services.CreateScope())
        {
            var profileDb = profileScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var savedUser = await profileDb.Users.SingleAsync();
            Assert.Equal("Synthetic", savedUser.FirstName);
            Assert.Equal("Parent", savedUser.LastName);
            Assert.Equal(savedUser.Id, (await profileDb.AuditEvents.IgnoreQueryFilters().SingleAsync(x => x.Action == "ProfileUpdated")).UserId);
        }
        var foreignId = Guid.NewGuid();
        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            db.Sessions.Add(new AccountSession { Id = foreignId, UserId = "synthetic-other" });
            await db.SaveChangesAsync();
        }
        var foreignRevoke = await client.PostAsync("/Account/Revoke", new FormUrlEncodedContent(new Dictionary<string,string> { ["id"] = foreignId.ToString(), ["__RequestVerificationToken"] = token }));
        Assert.Equal(System.Net.HttpStatusCode.NotFound, foreignRevoke.StatusCode);
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, (await client.PostAsync("/Account/RevokeAll", new StringContent(""))).StatusCode);
        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var session = await db.Sessions.IgnoreQueryFilters().SingleAsync(x => x.UserId != "synthetic-other");
            session.RevokedAt = DateTimeOffset.UtcNow;
            await db.SaveChangesAsync();
        }
        Assert.Equal(System.Net.HttpStatusCode.Redirect, (await client.GetAsync("/Account/Devices")).StatusCode);
        await app.StopAsync();
    }
}
 
