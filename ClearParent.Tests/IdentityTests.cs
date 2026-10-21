using ClearParent;
using ClearParent.Data;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ClearParent.Tests;
public class IdentityTests
{
    [Fact]
    public async Task ConfirmationAndResetTokensValidateAndCannotBeReusedAfterPasswordChange()
    {
        var services = new ServiceCollection();
        services.AddLogging(); services.AddHttpContextAccessor();
        services.AddDataProtection().UseEphemeralDataProtectionProvider();
        services.AddDbContext<ApplicationDbContext>(o => o.UseInMemoryDatabase(Guid.NewGuid().ToString()));
        services.AddIdentityCore<ApplicationUser>(o => { o.User.RequireUniqueEmail = true; o.Password.RequiredLength = 12; })
            .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
        using var provider = services.BuildServiceProvider();
        using var scope = provider.CreateScope();
        var manager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var user = new ApplicationUser { UserName = "synthetic@example.invalid", Email = "synthetic@example.invalid" };
        Assert.True((await manager.CreateAsync(user, "Synthetic-Test-173!Ab")).Succeeded);
        Assert.False(user.EmailConfirmed);
        var confirmation = await manager.GenerateEmailConfirmationTokenAsync(user);
        Assert.False((await manager.ConfirmEmailAsync(user, confirmation + "invalid")).Succeeded);
        Assert.True((await manager.ConfirmEmailAsync(user, confirmation)).Succeeded);
        var reset = await manager.GeneratePasswordResetTokenAsync(user);
        Assert.False((await manager.ResetPasswordAsync(user, reset, "weak")).Succeeded);
        Assert.True((await manager.ResetPasswordAsync(user, reset, "Synthetic-Reset-174!Ab")).Succeeded);
        Assert.False((await manager.ResetPasswordAsync(user, reset, "Synthetic-Reuse-175!Ab")).Succeeded);
        Assert.False(await manager.CheckPasswordAsync(user, "Synthetic-Test-173!Ab"));
        Assert.True(await manager.CheckPasswordAsync(user, "Synthetic-Reset-174!Ab"));
        Assert.True((await manager.SetTwoFactorEnabledAsync(user, true)).Succeeded);
        var emailCode = await manager.GenerateTwoFactorTokenAsync(user, TokenOptions.DefaultEmailProvider);
        Assert.True(await manager.VerifyTwoFactorTokenAsync(user, TokenOptions.DefaultEmailProvider, emailCode));
        Assert.False(await manager.VerifyTwoFactorTokenAsync(user, TokenOptions.DefaultEmailProvider, "000000-invalid"));
        var recovery = (await manager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10))!.ToList();
        Assert.Equal(10, recovery.Count);
        Assert.True((await manager.RedeemTwoFactorRecoveryCodeAsync(user, recovery[0])).Succeeded);
        Assert.False((await manager.RedeemTwoFactorRecoveryCodeAsync(user, recovery[0])).Succeeded);
        var duplicate = new ApplicationUser { UserName = "different", Email = "synthetic@example.invalid" };
        Assert.False((await manager.CreateAsync(duplicate, "Synthetic-Test-173!Ab")).Succeeded);
    }

    [Theory]
    [InlineData("/Account/Preferences")]
    [InlineData("/Account/Security")]
    [InlineData("/Account/Devices")]
    [InlineData("/Account/Profile")]
    [InlineData("/Children")]
    [InlineData("/Children/Create")]
    public async Task ProtectedMvcRoutesChallengeAnonymousRequests(string path)
    {
        await using var app = Program.BuildApplication([], builder =>
        {
            builder.WebHost.UseTestServer();
            builder.Configuration.AddInMemoryCollection(new Dictionary<string,string?> { ["ConnectionStrings:ClearParent"] = "Server=unused;Database=unused;Integrated Security=True" });
        });
        await app.StartAsync();
        var client = app.GetTestClient(); client.BaseAddress = new Uri("https://localhost");
        var response = await client.GetAsync(path);
        Assert.Equal(System.Net.HttpStatusCode.Redirect, response.StatusCode);
        Assert.Contains("/Identity/Account/Login", response.Headers.Location!.OriginalString);
        await app.StopAsync();
    }
}
