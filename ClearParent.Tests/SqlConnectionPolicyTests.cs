using ClearParent.Data;
namespace ClearParent.Tests;
public class SqlConnectionPolicyTests
{
    [Theory]
    [InlineData(".\\SQLEXPRESS", true, true)]
    [InlineData("localhost,1433", true, true)]
    [InlineData("tcp:127.0.0.1,1433", true, true)]
    [InlineData(".\\SQLEXPRESS", false, false)]
    [InlineData("sql.example.invalid", true, false)]
    [InlineData("localhost.example.invalid", true, false)]
    public void CertificateBypassRequiresLocalDevelopment(string server, bool development, bool allowed)
    {
        var connection = $"Server={server};Database=Synthetic;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
        if (allowed) Assert.NotEmpty(SqlConnectionPolicy.Validate(connection, development));
        else Assert.Throws<InvalidOperationException>(() => SqlConnectionPolicy.Validate(connection, development));
    }
    [Fact]
    public void EncryptionCannotBeDisabled() => Assert.Throws<InvalidOperationException>(() => SqlConnectionPolicy.Validate("Server=localhost;Encrypt=False", true));
    [Fact]
    public void ProductionValidatesCertificate() => Assert.NotEmpty(SqlConnectionPolicy.Validate("Server=sql.example.invalid;Encrypt=True;TrustServerCertificate=False", false));
}
