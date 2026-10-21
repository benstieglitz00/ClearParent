using ClearParent;
using ClearParent.Data;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Net;
using System.Text.RegularExpressions;

namespace ClearParent.Tests;
public class RegistrationTests
{
    private sealed class CapturedEmail : IEmailSender
    {
        public List<string> Messages { get; } = [];
        public Task SendEmailAsync(string email, string subject, string htmlMessage) { Messages.Add(htmlMessage); return Task.CompletedTask; }
    }
    [Fact]
    public async Task RegistrationRequiresLegalConsentRejectsHoneypotAndSendsVerificationWithoutActivatingAccount()
    {
        var database = Guid.NewGuid().ToString();
        var mail = new CapturedEmail();
        await using var app = Program.BuildApplication([], builder =>
        {
            builder.WebHost.UseTestServer();
            builder.Configuration.AddInMemoryCollection(new Dictionary<string,string?> {
                ["ConnectionStrings:ClearParent"] = "Server=unused;Database=unused;Integrated Security=True",
                ["Legal:TermsUrl"] = "https://example.invalid/terms", ["Legal:PrivacyUrl"] = "https://example.invalid/privacy", ["Legal:Version"] = "synthetic-v1",
                ["Email:Host"] = "example.invalid", ["Email:From"] = "synthetic@example.invalid" });
            builder.Services.RemoveAll<ApplicationDbContext>();
            builder.Services.RemoveAll<DbContextOptions<ApplicationDbContext>>();
            builder.Services.RemoveAll<Microsoft.EntityFrameworkCore.Infrastructure.IDbContextOptionsConfiguration<ApplicationDbContext>>();
            builder.Services.AddDbContext<ApplicationDbContext>(o => o.UseInMemoryDatabase(database));
            builder.Services.AddDataProtection().UseEphemeralDataProtectionProvider();
            builder.Services.RemoveAll<IEmailSender>(); builder.Services.AddSingleton<IEmailSender>(mail);
        });
        await app.StartAsync();
        var client = app.GetTestClient(); client.BaseAddress = new Uri("https://localhost");
        var page = await client.GetAsync("/Identity/Account/Register");
        Assert.Equal(HttpStatusCode.OK, page.StatusCode);
        var html = await page.Content.ReadAsStringAsync();
        var csrf = Regex.Match(html, "name=\"__RequestVerificationToken\"[^>]*value=\"([^\"]+)\"").Groups[1].Value;
        Assert.NotEmpty(csrf);
        var cookie = page.Headers.GetValues("Set-Cookie").Single(x => x.StartsWith(".AspNetCore.Antiforgery."));
        client.DefaultRequestHeaders.Add("Cookie", cookie.Split(';')[0]);
        var fields = new Dictionary<string,string> { ["__RequestVerificationToken"] = csrf, ["Input.Email"] = "synthetic-register@example.invalid", ["Input.FirstName"] = "Synthetic", ["Input.LastName"] = "Fixture", ["Input.Password"] = "Synthetic-Register-176!Ab", ["Input.ConfirmPassword"] = "Synthetic-Register-176!Ab" };
        var missingConsent = await client.PostAsync("/Identity/Account/Register", new FormUrlEncodedContent(fields));
        Assert.Contains("Accept the Terms", await missingConsent.Content.ReadAsStringAsync());
        fields["Input.AcceptLegal"] = "true"; fields["Input.Website"] = "bot";
        Assert.Equal(HttpStatusCode.OK, (await client.PostAsync("/Identity/Account/Register", new FormUrlEncodedContent(fields))).StatusCode);
        using (var scope = app.Services.CreateScope()) Assert.Empty(await scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Users.ToListAsync());
        fields.Remove("Input.Website");
        var registered = await client.PostAsync("/Identity/Account/Register", new FormUrlEncodedContent(fields));
        Assert.Equal(HttpStatusCode.Redirect, registered.StatusCode);
        Assert.Single(mail.Messages);
        using (var scope = app.Services.CreateScope())
        {
            var users = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var user = (await users.FindByEmailAsync(fields["Input.Email"]))!;
            Assert.False(user.EmailConfirmed); Assert.Equal("synthetic-v1", user.LegalVersion); Assert.NotNull(user.LegalAcceptedAt);
            var signIn = scope.ServiceProvider.GetRequiredService<SignInManager<ApplicationUser>>();
            Assert.True((await signIn.CheckPasswordSignInAsync(user, fields["Input.Password"], true)).IsNotAllowed);
        }
        var link = WebUtility.HtmlDecode(Regex.Match(mail.Messages[0], "href='([^']+)'").Groups[1].Value);
        Assert.Equal(HttpStatusCode.OK, (await client.GetAsync(link)).StatusCode);
        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            Assert.True((await db.Users.SingleAsync()).EmailConfirmed);
            Assert.Contains(await db.AuditEvents.IgnoreQueryFilters().ToListAsync(), x => x.Action == "AccountCreated" && x.Success);
            Assert.Contains(await db.AuditEvents.IgnoreQueryFilters().ToListAsync(), x => x.Action == "EmailConfirmed" && x.Success);
        }
        var login = await client.PostAsync("/Identity/Account/Login", new FormUrlEncodedContent(new Dictionary<string,string> {
            ["__RequestVerificationToken"] = csrf, ["Input.Email"] = fields["Input.Email"], ["Input.Password"] = fields["Input.Password"] }));
        Assert.Equal(HttpStatusCode.Redirect, login.StatusCode);
        var authCookie = login.Headers.GetValues("Set-Cookie").Single(x => x.StartsWith(".AspNetCore.Identity.Application="));
        client.DefaultRequestHeaders.Remove("Cookie");
        client.DefaultRequestHeaders.Add("Cookie", authCookie.Split(';')[0]);
        Assert.Equal(HttpStatusCode.OK, (await client.GetAsync("/Identity/Account/Manage/TwoFactorAuthentication")).StatusCode);
        Assert.Equal(HttpStatusCode.OK, (await client.GetAsync("/Account/Preferences")).StatusCode);
        await app.StopAsync();
    }
}
