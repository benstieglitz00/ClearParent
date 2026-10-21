using ClearParent.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
namespace ClearParent.Security;

public sealed class AuditedUserManager(IUserStore<ApplicationUser> store, IOptions<IdentityOptions> options,
    IPasswordHasher<ApplicationUser> hasher, IEnumerable<IUserValidator<ApplicationUser>> userValidators,
    IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators, ILookupNormalizer normalizer,
    IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<ApplicationUser>> logger,
    ApplicationDbContext db, IHttpContextAccessor accessor)
    : UserManager<ApplicationUser>(store, options, hasher, userValidators, passwordValidators, normalizer, errors, services, logger)
{
    private async Task<IdentityResult> Record(ApplicationUser user, string action, Func<Task<IdentityResult>> operation)
    {
        await using var transaction = db.Database.IsRelational() && db.Database.CurrentTransaction == null
            ? await db.Database.BeginTransactionAsync() : null;
        var result = await operation();
        var context = accessor.HttpContext;
        var device = context?.Request.Headers.UserAgent.ToString();
        db.AuditEvents.Add(new AuditEvent { UserId = user.Id, Action = action, Success = result.Succeeded,
            IpAddress = context?.Connection.RemoteIpAddress?.ToString(), Device = device?[..Math.Min(device.Length,512)] });
        await db.SaveChangesAsync();
        if (transaction != null) await transaction.CommitAsync();
        return result;
    }
    public override Task<IdentityResult> CreateAsync(ApplicationUser user) => Record(user, "AccountCreated", () => base.CreateAsync(user));
    public override Task<IdentityResult> ConfirmEmailAsync(ApplicationUser user, string token) => Record(user, "EmailConfirmed", () => base.ConfirmEmailAsync(user, token));
    public override Task<IdentityResult> ResetPasswordAsync(ApplicationUser user, string token, string newPassword) => Record(user, "PasswordReset", () => base.ResetPasswordAsync(user, token, newPassword));
    public override Task<IdentityResult> ChangePasswordAsync(ApplicationUser user, string currentPassword, string newPassword) => Record(user, "PasswordChanged", () => base.ChangePasswordAsync(user, currentPassword, newPassword));
    public override Task<IdentityResult> SetTwoFactorEnabledAsync(ApplicationUser user, bool enabled) => Record(user, enabled ? "MfaEnabled" : "MfaDisabled", () => base.SetTwoFactorEnabledAsync(user, enabled));
    public override Task<IdentityResult> ChangeEmailAsync(ApplicationUser user, string email, string token) => Record(user, "EmailChanged", () => base.ChangeEmailAsync(user, email, token));
    public override Task<IdentityResult> ResetAuthenticatorKeyAsync(ApplicationUser user) => Record(user, "AuthenticatorReset", () => base.ResetAuthenticatorKeyAsync(user));
}
