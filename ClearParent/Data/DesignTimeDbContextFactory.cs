using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
namespace ClearParent.Data;

public sealed class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")
            ?? Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
        var development = environment.Equals("Development", StringComparison.OrdinalIgnoreCase);
        var connection = Environment.GetEnvironmentVariable("ConnectionStrings__ClearParent")
            ?? (development ? SqlConnectionPolicy.LocalDevelopmentConnection
                : throw new InvalidOperationException("Configure ConnectionStrings__ClearParent. Local defaults require DOTNET_ENVIRONMENT=Development."));
        connection = SqlConnectionPolicy.Validate(connection, development);
        return new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlServer(connection).Options, new HttpContextAccessor());
    }
}
