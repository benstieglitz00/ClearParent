using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ClearParent.Data;

public sealed class ApplicationUser : IdentityUser
{
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? LastLoginAt { get; set; }
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public DateTimeOffset? LegalAcceptedAt { get; set; }
    public string? LegalVersion { get; set; }
}

public sealed class AccountPreference
{
    public string UserId { get; set; } = "";
    public string TimeZone { get; set; } = "UTC";
    public string Language { get; set; } = "en";
    public bool AiSuggestions { get; set; }
}

public sealed class AuditEvent
{
    public long Id { get; set; }
    public string? UserId { get; set; }
    public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.UtcNow;
    public string Action { get; set; } = "";
    public bool Success { get; set; }
    public string? IpAddress { get; set; }
    public string? Device { get; set; }
}

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
    IHttpContextAccessor accessor) : IdentityDbContext<ApplicationUser>(options)
{
    private string? CurrentUserId => accessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
    public DbSet<AccountSession> Sessions => Set<AccountSession>();
    public DbSet<ChildProfile> Children => Set<ChildProfile>();
    public DbSet<AccountPreference> Preferences => Set<AccountPreference>();
    public DbSet<AuditEvent> AuditEvents => Set<AuditEvent>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<ChildProfile>().Property(x => x.UserId).HasMaxLength(450);
        builder.Entity<ChildProfile>().HasIndex(x => new { x.UserId, x.Name });
        builder.Entity<ChildProfile>().HasQueryFilter(x => x.UserId == CurrentUserId && CurrentUserId != null);
        builder.Entity<ChildProfile>().HasOne<ApplicationUser>().WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
        builder.Entity<AccountSession>().Property(x => x.UserId).HasMaxLength(450);
        builder.Entity<AccountSession>().Property(x => x.IpAddress).HasMaxLength(64);
        builder.Entity<AccountSession>().Property(x => x.Device).HasMaxLength(512);
        builder.Entity<AccountSession>().HasIndex(x => new { x.UserId, x.LastActivityAt });
        builder.Entity<AccountSession>().HasQueryFilter(x => x.UserId == CurrentUserId);
        builder.Entity<AccountSession>().HasOne<ApplicationUser>().WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
        builder.Entity<ApplicationUser>().Property(x => x.FirstName).HasMaxLength(100);
        builder.Entity<ApplicationUser>().Property(x => x.LastName).HasMaxLength(100);
        builder.Entity<ApplicationUser>().Property(x => x.LegalVersion).HasMaxLength(100);
        builder.Entity<ApplicationUser>().HasIndex(x => x.NormalizedEmail).IsUnique()
            .HasFilter("[NormalizedEmail] IS NOT NULL");
        builder.Entity<AccountPreference>().HasKey(x => x.UserId);
        builder.Entity<AccountPreference>().Property(x => x.UserId).HasMaxLength(450);
        builder.Entity<AccountPreference>().Property(x => x.TimeZone).HasMaxLength(100);
        builder.Entity<AccountPreference>().Property(x => x.Language).HasMaxLength(20);
        builder.Entity<AccountPreference>().HasOne<ApplicationUser>().WithOne().HasForeignKey<AccountPreference>(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
        builder.Entity<AccountPreference>().HasQueryFilter(x => x.UserId == CurrentUserId);
        builder.Entity<AuditEvent>().ToTable("AuditEvents", table => table.UseSqlOutputClause(false));
        builder.Entity<AuditEvent>().HasQueryFilter(x => x.UserId == CurrentUserId && CurrentUserId != null);
        builder.Entity<AuditEvent>().Property(x => x.UserId).HasMaxLength(450);
        builder.Entity<AuditEvent>().Property(x => x.Action).HasMaxLength(100);
        builder.Entity<AuditEvent>().Property(x => x.IpAddress).HasMaxLength(64);
        builder.Entity<AuditEvent>().Property(x => x.Device).HasMaxLength(512);
        builder.Entity<AuditEvent>().HasIndex(x => new { x.UserId, x.Timestamp });
    }

    private void EnforceIntegrity()
    {
        foreach (var entry in ChangeTracker.Entries<ChildProfile>().Where(x => x.State != EntityState.Unchanged))
            if (CurrentUserId == null || entry.Entity.UserId != CurrentUserId ||
                (entry.State != EntityState.Added && entry.OriginalValues.GetValue<string>(nameof(ChildProfile.UserId)) != CurrentUserId))
                throw new UnauthorizedAccessException("Child profiles belong to the authenticated account.");
        if (ChangeTracker.Entries<AuditEvent>().Any(x => x.State is EntityState.Modified or EntityState.Deleted))
            throw new InvalidOperationException("Audit events are append-only.");
        foreach (var entry in ChangeTracker.Entries<AccountPreference>().Where(x => x.State != EntityState.Unchanged))
            if (CurrentUserId == null || entry.Entity.UserId != CurrentUserId ||
                (entry.State != EntityState.Added && entry.OriginalValues.GetValue<string>(nameof(AccountPreference.UserId)) != CurrentUserId))
                throw new UnauthorizedAccessException("Preferences belong to the authenticated account.");
    }
    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        EnforceIntegrity();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }
    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        EnforceIntegrity();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
}
