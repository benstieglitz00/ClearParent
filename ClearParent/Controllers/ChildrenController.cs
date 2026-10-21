using ClearParent.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
namespace ClearParent.Controllers;
public sealed class ChildInput
{
    [Required, StringLength(100)] public string Name { get; set; } = "";
    [DataType(DataType.Date)] public DateOnly? Birthday { get; set; }
}
[Authorize]
public sealed class ChildrenController(ApplicationDbContext db) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index() => View(await db.Children.OrderBy(x => x.Name).ToListAsync());
    [HttpGet]
    public IActionResult Create() => View(new ChildInput());
    [HttpPost]
    public async Task<IActionResult> Create(ChildInput input)
    {
        if (string.IsNullOrWhiteSpace(input.Name)) ModelState.AddModelError(nameof(input.Name), "Enter a name.");
        if (input.Birthday > DateOnly.FromDateTime(DateTime.UtcNow)) ModelState.AddModelError(nameof(input.Birthday), "Birthday cannot be in the future.");
        if (!ModelState.IsValid) return View(input);
        var owner = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        db.Children.Add(new ChildProfile { UserId = owner, Name = input.Name.Trim(), Birthday = input.Birthday });
        db.AuditEvents.Add(new AuditEvent { UserId = owner, Action = "ChildProfileCreated", Success = true,
            IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
            Device = Request.Headers.UserAgent.ToString()[..Math.Min(Request.Headers.UserAgent.ToString().Length, 512)] });
        await db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
