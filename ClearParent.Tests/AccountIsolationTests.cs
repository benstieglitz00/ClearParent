using ClearParent.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ClearParent.Tests;
public class AccountIsolationTests
{
    private static HttpContextAccessor Identity(string? id) => new() { HttpContext = new DefaultHttpContext {
        User = id == null ? new ClaimsPrincipal() : new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, id) }, "test")) } };

    [Fact]
    public async Task PreferencesAndAuditAreIsolatedAndAnonymousReadsFailClosed()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        foreach (var user in new[] { "synthetic-a", "synthetic-b" })
        {
            await using var db = new ApplicationDbContext(options, Identity(user));
            db.Preferences.Add(new AccountPreference { UserId = user });
            db.AuditEvents.Add(new AuditEvent { UserId = user, Action = "SyntheticTest", Success = true });
            await db.SaveChangesAsync();
        }
        await using var own = new ApplicationDbContext(options, Identity("synthetic-a"));
        Assert.Equal("synthetic-a", (await own.Preferences.SingleAsync()).UserId);
        Assert.Equal("synthetic-a", (await own.AuditEvents.SingleAsync()).UserId);
        await using var anonymous = new ApplicationDbContext(options, Identity(null));
        Assert.Empty(await anonymous.Preferences.ToListAsync());
        Assert.Empty(await anonymous.AuditEvents.ToListAsync());
    }
    [Fact]
    public async Task CrossAccountWritesAreRejected()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        await using var db = new ApplicationDbContext(options, Identity("synthetic-a"));
        db.Preferences.Add(new AccountPreference { UserId = "synthetic-b" });
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => db.SaveChangesAsync());
    }
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task AuditEventsCannotBeChangedOrDeleted(bool delete)
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        await using var db = new ApplicationDbContext(options, Identity("synthetic-a"));
        var audit = new AuditEvent { UserId = "synthetic-a", Action = "SyntheticTest" };
        db.AuditEvents.Add(audit); await db.SaveChangesAsync();
        if (delete) db.AuditEvents.Remove(audit); else audit.Action = "Changed";
        await Assert.ThrowsAsync<InvalidOperationException>(() => db.SaveChangesAsync());
    }
}
