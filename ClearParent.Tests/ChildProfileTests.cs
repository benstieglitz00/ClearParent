using ClearParent.Data;
using ClearParent.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
namespace ClearParent.Tests;
public class ChildProfileTests
{
    private static HttpContextAccessor Identity(string? id) => new() { HttpContext = new DefaultHttpContext {
        User = id == null ? new ClaimsPrincipal() : new ClaimsPrincipal(new ClaimsIdentity([new Claim(ClaimTypes.NameIdentifier, id)], "test")) } };
    [Fact]
    public async Task CreationIsAuditedAndOtherAccountsCannotReadOrWriteProfiles()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        var identity = Identity("synthetic-a");
        await using var db = new ApplicationDbContext(options, identity);
        var controller = new ChildrenController(db) { ControllerContext = new ControllerContext { HttpContext = identity.HttpContext! } };
        Assert.IsType<RedirectToActionResult>(await controller.Create(new ChildInput { Name = "Synthetic Child", Birthday = new DateOnly(2020, 1, 1) }));
        var child = await db.Children.SingleAsync();
        Assert.Equal("synthetic-a", child.UserId);
        Assert.Equal("ChildProfileCreated", (await db.AuditEvents.SingleAsync()).Action);
        await using var other = new ApplicationDbContext(options, Identity("synthetic-b"));
        Assert.Empty(await other.Children.ToListAsync());
        var stolen = await other.Children.IgnoreQueryFilters().SingleAsync();
        stolen.Name = "Unauthorized change";
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => other.SaveChangesAsync());
        await using var anonymous = new ApplicationDbContext(options, Identity(null));
        Assert.Empty(await anonymous.Children.ToListAsync());
    }
    [Fact]
    public async Task FutureBirthdaysAreRejectedWithoutWritingData()
    {
        var identity = Identity("synthetic-a");
        await using var db = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options, identity);
        var controller = new ChildrenController(db) { ControllerContext = new ControllerContext { HttpContext = identity.HttpContext! } };
        Assert.IsType<ViewResult>(await controller.Create(new ChildInput { Name = "Synthetic", Birthday = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(2)) }));
        Assert.Empty(await db.Children.ToListAsync());
        Assert.Empty(await db.AuditEvents.ToListAsync());
    }
}
