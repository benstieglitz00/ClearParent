using Microsoft.Data.SqlClient;

namespace ClearParent.Data;

public static class SqlConnectionPolicy
{
    public const string LocalDevelopmentConnection = "Server=.\\SQLEXPRESS;Database=ClearParent_Development;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;Connect Timeout=5";

    public static string Validate(string connection, bool development)
    {
        var value = new SqlConnectionStringBuilder(connection);
        if (value.Encrypt == SqlConnectionEncryptOption.Optional)
            throw new InvalidOperationException("SQL Server connections must use encryption.");
        if (value.TrustServerCertificate && (!development || !IsLocal(value.DataSource)))
            throw new InvalidOperationException("TrustServerCertificate is permitted only for local Development SQL Server connections.");
        return value.ConnectionString;
    }

    private static bool IsLocal(string source)
    {
        if (source.StartsWith("tcp:", StringComparison.OrdinalIgnoreCase)) source = source[4..];
        if (source.StartsWith("lpc:", StringComparison.OrdinalIgnoreCase)) source = source[4..];
        var host = source.Split('\\', ',')[0];
        return host is "." or "(local)" || host.Equals("localhost", StringComparison.OrdinalIgnoreCase)
            || host == "127.0.0.1" || host == "[::1]"
            || host.Equals(Environment.MachineName, StringComparison.OrdinalIgnoreCase)
            || host.Equals("(localdb)", StringComparison.OrdinalIgnoreCase);
    }
}
