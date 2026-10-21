using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClearParent.Data.Migrations
{
    /// <inheritdoc />
    public partial class AppendOnlyAudit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                EXEC(N'CREATE TRIGGER [dbo].[TR_AuditEvents_AppendOnly]
                ON [dbo].[AuditEvents]
                INSTEAD OF UPDATE, DELETE
                AS
                BEGIN
                    SET NOCOUNT ON;
                    THROW 51000, ''Audit events are append-only.'', 1;
                END');
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TRIGGER [dbo].[TR_AuditEvents_AppendOnly]");
        }
    }
}
