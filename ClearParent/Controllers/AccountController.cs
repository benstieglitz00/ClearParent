using ClearParent.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
namespace ClearParent.Controllers;

public sealed class PreferencesInput
{
    [Required, StringLength(100)] public string TimeZone { get; set; } = "UTC";
    [Required, RegularExpression("^[a-z]{2}(-[A-Z]{2})?$")] public string Language { get; set; } = "en";
    public bool AiSuggestions { get; set; }
}

public sealed class ProfileInput
{
    [Required, StringLength(100)] public string FirstName { get; set; } = "";
    [Required, StringLength(100)] public string LastName { get; set; } = "";
}

[Authorize]
public sealed class AccountController(ApplicationDbContext db, UserManager<ApplicationUser> users) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Profile()
    {
        var user = await users.GetUserAsync(User);
        return user == null ? Challenge() : View(new ProfileInput { FirstName = user.FirstName, LastName = user.LastName });
    }
    [HttpPost]
    public async Task<IActionResult> Profile(ProfileInput input)
    {
        if (!ModelState.IsValid) return View(input);
        var user = await users.GetUserAsync(User);
        if (user == null) return Challenge();
        user.FirstName = input.FirstName.Trim();
        user.LastName = input.LastName.Trim();
        db.AuditEvents.Add(new AuditEvent { UserId = user.Id, Action = "ProfileUpdated", Success = true,
            IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString(), Device = Request.Headers.UserAgent.ToString()[..Math.Min(Request.Headers.UserAgent.ToString().Length,512)] });
        await db.SaveChangesAsync();
        TempData["Saved"] = "Profile saved.";
        return RedirectToAction(nameof(Profile));
    }
    [HttpGet]
    public async Task<IActionResult> Preferences()
    {
        var value = await db.Preferences.SingleOrDefaultAsync();
        return View(new PreferencesInput { TimeZone = value?.TimeZone ?? "UTC", Language = value?.Language ?? "en", AiSuggestions = value?.AiSuggestions ?? false });
    }
    [HttpPost]
    public async Task<IActionResult> Preferences(PreferencesInput input)
    {
        if (!TimeZoneInfo.TryFindSystemTimeZoneById(input.TimeZone ?? "", out _)) ModelState.AddModelError(nameof(input.TimeZone), "Choose a valid time zone.");
        if (!ModelState.IsValid) return View(input);
        var id = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var value = await db.Preferences.SingleOrDefaultAsync();
        if (value == null) { value = new AccountPreference { UserId = id }; db.Preferences.Add(value); }
        value.TimeZone = input.TimeZone!; value.Language = input.Language; value.AiSuggestions = input.AiSuggestions;
        db.AuditEvents.Add(new AuditEvent { UserId = id, Action = "PreferencesUpdated", Success = true,
            IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString(), Device = Request.Headers.UserAgent.ToString()[..Math.Min(Request.Headers.UserAgent.ToString().Length,512)] });
        await db.SaveChangesAsync();
        TempData["Saved"] = "Preferences saved.";
        return RedirectToAction(nameof(Preferences));
    }
    [HttpGet]
    public IActionResult EmailMfa() => View();

    [HttpPost]
    public async Task<IActionResult> EmailMfa(string password, bool enabled)
    {
        var user = await users.GetUserAsync(User);
        if (user == null) return Challenge();
        if (string.IsNullOrEmpty(password) || !await users.CheckPasswordAsync(user, password))
        {
            ModelState.AddModelError("", "Enter your current password to change MFA.");
            return View();
        }
        if (enabled && !user.EmailConfirmed)
        {
            ModelState.AddModelError("", "Verify your email address first.");
            return View();
        }
        var result = await users.SetTwoFactorEnabledAsync(user, enabled);
        if (!result.Succeeded) { ModelState.AddModelError("", "Unable to change MFA. Try again."); return View(); }
        return RedirectToAction(nameof(Security));
    }
    [HttpGet]
    public async Task<IActionResult> Devices() => View(await db.Sessions.OrderByDescending(x => x.LastActivityAt).Take(100).ToListAsync());

    [HttpPost]
    public async Task<IActionResult> Revoke(Guid id)
    {
        var session = await db.Sessions.SingleOrDefaultAsync(x => x.Id == id);
        if (session == null) return NotFound();
        session.RevokedAt ??= DateTimeOffset.UtcNow;
        db.AuditEvents.Add(new AuditEvent { UserId = User.FindFirstValue(ClaimTypes.NameIdentifier), Action = "SessionRevoked", Success = true,
            IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString(), Device = Request.Headers.UserAgent.ToString()[..Math.Min(Request.Headers.UserAgent.ToString().Length,512)] });
        await db.SaveChangesAsync();
        return RedirectToAction(nameof(Devices));
    }
    [HttpPost]
    public async Task<IActionResult> RevokeAll()
    {
        foreach (var session in await db.Sessions.Where(x => x.RevokedAt == null).ToListAsync()) session.RevokedAt = DateTimeOffset.UtcNow;
        db.AuditEvents.Add(new AuditEvent { UserId = User.FindFirstValue(ClaimTypes.NameIdentifier), Action = "AllSessionsRevoked", Success = true });
        await db.SaveChangesAsync();
        return RedirectToAction(nameof(Devices));
    }
    [HttpGet]
    public async Task<IActionResult> Security() => View(await db.AuditEvents.OrderByDescending(x => x.Timestamp).Take(100).ToListAsync());
}

