using ClearParent.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
namespace ClearParent.Security;

public sealed class AuditedSignInManager(UserManager<ApplicationUser> userManager,
    IHttpContextAccessor contextAccessor, IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory,
    IOptions<IdentityOptions> optionsAccessor, ILogger<SignInManager<ApplicationUser>> logger,
    IAuthenticationSchemeProvider schemes, IUserConfirmation<ApplicationUser> confirmation,
    ApplicationDbContext db) : SignInManager<ApplicationUser>(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
{
    private AuditEvent Event(string? userId, string action, bool success) => new()
    {
        UserId = userId, Action = action, Success = success,
        IpAddress = Context.Connection.RemoteIpAddress?.ToString(),
        Device = Context.Request.Headers.UserAgent.ToString()[..Math.Min(Context.Request.Headers.UserAgent.ToString().Length,512)]
    };

    public override async Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure)
    {
        // Always count failures; the caller cannot disable brute-force protection.
        var result = await base.PasswordSignInAsync(userName, password, isPersistent, true);
        if (!result.Succeeded)
        {
            var user = await UserManager.FindByNameAsync(userName);
            db.AuditEvents.Add(Event(user?.Id, result.RequiresTwoFactor ? "MfaChallengeRequired" : "LoginFailed", false));
            await db.SaveChangesAsync();
        }
        return result;
    }

    public override async Task SignInWithClaimsAsync(ApplicationUser user, AuthenticationProperties? authenticationProperties, IEnumerable<Claim> additionalClaims)
    {
        var session = new AccountSession { UserId = user.Id, IpAddress = Context.Connection.RemoteIpAddress?.ToString(),
            Device = Context.Request.Headers.UserAgent.ToString()[..Math.Min(Context.Request.Headers.UserAgent.ToString().Length,512)] };
        db.Sessions.Add(session);
        user.LastLoginAt = DateTimeOffset.UtcNow;
        db.AuditEvents.Add(Event(user.Id, "LoginSucceeded", true));
        await db.SaveChangesAsync();
        await base.SignInWithClaimsAsync(user, authenticationProperties, additionalClaims.Append(new Claim("clearparent_session", session.Id.ToString())));
    }
    public override async Task SignOutAsync()
    {
        var userId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId != null)
        {
            if (Guid.TryParse(Context.User.FindFirstValue("clearparent_session"), out var id))
            {
                var session = await db.Sessions.FindAsync(id);
                if (session?.UserId == userId) session.RevokedAt = DateTimeOffset.UtcNow;
            }
            db.AuditEvents.Add(Event(userId, "Logout", true));
            await db.SaveChangesAsync();
        }
        await base.SignOutAsync();
    }
}
