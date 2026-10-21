using ClearParent.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
namespace ClearParent.Areas.Identity.Pages.Account;

[AllowAnonymous]
public sealed class RegisterModel(UserManager<ApplicationUser> users, IEmailSender email, IConfiguration configuration) : PageModel
{
    [BindProperty] public RegistrationInput Input { get; set; } = new();
    public string? TermsUrl => configuration["Legal:TermsUrl"];
    public string? PrivacyUrl => configuration["Legal:PrivacyUrl"];
    public bool RegistrationAvailable => Uri.TryCreate(TermsUrl, UriKind.Absolute, out var terms) && terms.Scheme == "https"
        && Uri.TryCreate(PrivacyUrl, UriKind.Absolute, out var privacy) && privacy.Scheme == "https"
        && !string.IsNullOrWhiteSpace(configuration["Legal:Version"]) && !string.IsNullOrWhiteSpace(configuration["Email:Host"])
        && !string.IsNullOrWhiteSpace(configuration["Email:From"]);
    public sealed class RegistrationInput
    {
        [Required, EmailAddress] public string Email { get; set; } = "";
        [Required, StringLength(100)] public string FirstName { get; set; } = "";
        [Required, StringLength(100)] public string LastName { get; set; } = "";
        [Required, StringLength(128, MinimumLength = 12), DataType(DataType.Password)] public string Password { get; set; } = "";
        [Compare(nameof(Password)), DataType(DataType.Password)] public string ConfirmPassword { get; set; } = "";
        [Range(typeof(bool), "true", "true", ErrorMessage = "Accept the Terms of Service and Privacy Policy to register.")] public bool AcceptLegal { get; set; }
        // Honeypot complements IP rate limiting without CAPTCHA or a third-party tracking service.
        public string? Website { get; set; }
    }
    public void OnGet() { }
    public async Task<IActionResult> OnPostAsync()
    {
        if (!RegistrationAvailable) { ModelState.AddModelError("", "Registration is unavailable until the service is configured."); return Page(); }
        if (!string.IsNullOrEmpty(Input.Website)) { ModelState.AddModelError("", "Registration could not be completed."); return Page(); }
        if (!ModelState.IsValid) return Page();
        var user = new ApplicationUser { UserName = Input.Email, Email = Input.Email, FirstName = Input.FirstName,
            LastName = Input.LastName, LegalAcceptedAt = DateTimeOffset.UtcNow, LegalVersion = configuration["Legal:Version"] };
        var result = await users.CreateAsync(user, Input.Password);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors) ModelState.AddModelError("", error.Description);
            return Page();
        }
        var token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(await users.GenerateEmailConfirmationTokenAsync(user)));
        var link = Url.Page("/Account/ConfirmEmail", null, new { area = "Identity", userId = user.Id, code = token }, Request.Scheme)!;
        try { await email.SendEmailAsync(Input.Email, "Confirm your ClearParent email", $"Confirm your account: <a href='{HtmlEncoder.Default.Encode(link)}'>Verify email</a>"); }
        catch (Exception ex) when (ex is System.Net.Mail.SmtpException or InvalidOperationException)
        {
            ModelState.AddModelError("", "Your account is pending verification. Email delivery failed; use Resend email confirmation to retry.");
            return Page();
        }
        return RedirectToPage("RegisterConfirmation", new { email = Input.Email });
    }
}
