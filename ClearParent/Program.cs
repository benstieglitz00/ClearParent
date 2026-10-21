using ClearParent.Data;
using ClearParent.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.RateLimiting;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace ClearParent;
public class Program
{
    public static void Main(string[] args) => BuildApplication(args).Run();
    public static WebApplication BuildApplication(string[] args, Action<WebApplicationBuilder>? configure = null)
    {
        var builder = WebApplication.CreateBuilder(new WebApplicationOptions { Args = args, ApplicationName = typeof(Program).Assembly.GetName().Name });
        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();
        builder.Logging.AddFilter("Microsoft.AspNetCore.Hosting.Diagnostics", LogLevel.Warning);
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
            SqlConnectionPolicy.Validate(builder.Configuration.GetConnectionString("ClearParent")
                ?? throw new InvalidOperationException("Configure ConnectionStrings:ClearParent using user secrets or environment variables."), builder.Environment.IsDevelopment())));
        builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
        {
            options.SignIn.RequireConfirmedEmail = true;
            options.User.RequireUniqueEmail = true;
            options.Password.RequiredLength = 12;
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
        }).AddRoles<IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddSignInManager<AuditedSignInManager>().AddUserManager<AuditedUserManager>();
        builder.Services.Configure<DataProtectionTokenProviderOptions>(options => options.TokenLifespan = TimeSpan.FromHours(1));
        builder.Services.AddTransient<IEmailSender, SmtpEmailSender>();
        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.HttpOnly = true;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            options.Cookie.SameSite = SameSiteMode.Lax;
            options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
            options.SlidingExpiration = true;
            options.Events.OnValidatePrincipal = async context =>
            {
                await SecurityStampValidator.ValidatePrincipalAsync(context);
                if (context.Principal?.Identity?.IsAuthenticated != true) return;
                var db = context.HttpContext.RequestServices.GetRequiredService<ApplicationDbContext>();
                var userId = context.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
                var session = Guid.TryParse(context.Principal.FindFirstValue("clearparent_session"), out var id)
                    ? await db.Sessions.IgnoreQueryFilters().SingleOrDefaultAsync(x => x.Id == id && x.UserId == userId) : null;
                if (session == null || session.RevokedAt != null || session.LastActivityAt < DateTimeOffset.UtcNow.AddMinutes(-30))
                {
                    context.RejectPrincipal();
                    await context.HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
                    return;
                }
                session.LastActivityAt = DateTimeOffset.UtcNow;
                await db.SaveChangesAsync();
            };
        });
        builder.Services.Configure<SecurityStampValidatorOptions>(options =>
        {
            options.ValidationInterval = TimeSpan.Zero;
            options.OnRefreshingPrincipal = context =>
            {
                var session = context.CurrentPrincipal?.FindFirst("clearparent_session");
                if (session != null && context.NewPrincipal?.Identity is ClaimsIdentity identity) identity.AddClaim(session);
                return Task.CompletedTask;
            };
        });
        builder.Services.AddAuthorization(options => options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build());
        builder.Services.AddControllersWithViews(options => options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()));
        builder.Services.AddRazorPages(options =>
        {
            options.Conventions.AllowAnonymousToPage("/Index");
            options.Conventions.AllowAnonymousToPage("/Privacy");
            options.Conventions.AllowAnonymousToPage("/Error");
        });
        builder.Services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
                RateLimitPartition.GetFixedWindowLimiter(context.Connection.RemoteIpAddress?.ToString() ?? "unknown", _ =>
                    new FixedWindowRateLimiterOptions { PermitLimit = 60, Window = TimeSpan.FromMinutes(1), QueueLimit = 0 }));
        });
        configure?.Invoke(builder);
        var app = builder.Build();
        if (!app.Environment.IsDevelopment()) { app.UseExceptionHandler("/Error"); app.UseHsts(); }
        app.UseHttpsRedirection();
        app.Use(async (context, next) =>
        {
            if (!context.Request.IsHttps && (context.Request.Path.StartsWithSegments("/Identity") || (context.Request.Path.StartsWithSegments("/Account") || context.Request.Path.StartsWithSegments("/Children"))))
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("Use HTTPS to access your account.");
                return;
            }
            if (context.Request.Path.StartsWithSegments("/Identity") || (context.Request.Path.StartsWithSegments("/Account") || context.Request.Path.StartsWithSegments("/Children")))
            {
                context.Response.Headers.CacheControl = "no-store";
                context.Response.Headers.Pragma = "no-cache";
            }
            await next();
        });
        app.UseRouting();
        app.UseRateLimiter();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapStaticAssets();
        app.MapControllerRoute("children", "Children/{action=Index}/{id?}", new { controller = "Children" });
        app.MapControllerRoute("default", "{controller=Account}/{action=Preferences}/{id?}");
        app.MapRazorPages().WithStaticAssets();
        return app;
    }
}
