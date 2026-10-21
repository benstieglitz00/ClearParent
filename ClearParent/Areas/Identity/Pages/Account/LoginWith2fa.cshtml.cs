using ClearParent.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
namespace ClearParent.Areas.Identity.Pages.Account;

[AllowAnonymous]
public sealed class LoginWith2faModel(SignInManager<ApplicationUser> signIn, UserManager<ApplicationUser> users, IEmailSender email) : PageModel
{
    [BindProperty] public CodeInput Input { get; set; } = new();
    [BindProperty(SupportsGet = true)] public bool RememberMe { get; set; }
    [BindProperty(SupportsGet = true)] public string? ReturnUrl { get; set; }
    public string? Status { get; set; }
    public sealed class CodeInput
    {
        [Required, StringLength(12)] public string Code { get; set; } = "";
        public bool UseEmail { get; set; }
        public bool TrustDevice { get; set; }
    }
    public async Task<IActionResult> OnGetAsync() => await signIn.GetTwoFactorAuthenticationUserAsync() == null ? RedirectToPage("Login") : Page();
    public async Task<IActionResult> OnPostSendEmailAsync()
    {
        var user = await signIn.GetTwoFactorAuthenticationUserAsync();
        if (user == null) return RedirectToPage("Login");
        var address = await users.GetEmailAsync(user);
        if (address == null || !await users.IsEmailConfirmedAsync(user)) return RedirectToPage("Login");
        var code = await users.GenerateTwoFactorTokenAsync(user, TokenOptions.DefaultEmailProvider);
        try { await email.SendEmailAsync(address, "ClearParent sign-in code", $"Your sign-in code is {System.Text.Encodings.Web.HtmlEncoder.Default.Encode(code)}."); Status = "Check your email for your sign-in code."; }
        catch (Exception ex) when (ex is System.Net.Mail.SmtpException or InvalidOperationException) { Status = "Email delivery failed. Try again or use an authenticator or recovery code."; }
        Input.UseEmail = true;
        ModelState.Clear();
        return Page();
    }
    public async Task<IActionResult> OnPostAsync()
    {
        if (await signIn.GetTwoFactorAuthenticationUserAsync() == null) return RedirectToPage("Login");
        if (!ModelState.IsValid) return Page();
        var result = Input.UseEmail
            ? await signIn.TwoFactorSignInAsync(TokenOptions.DefaultEmailProvider, Input.Code.Replace(" ", ""), RememberMe, Input.TrustDevice)
            : await signIn.TwoFactorAuthenticatorSignInAsync(Input.Code.Replace(" ", "").Replace("-", ""), RememberMe, Input.TrustDevice);
        if (result.Succeeded) return LocalRedirect(Url.IsLocalUrl(ReturnUrl) ? ReturnUrl! : "/Account/Preferences");
        ModelState.AddModelError("", "Unable to sign in. Check your code or try again later.");
        return Page();
    }
}
