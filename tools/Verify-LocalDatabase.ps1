param([ValidateSet('.\SQLEXPRESS', 'localhost\SQLEXPRESS')][string]$Server = '.\SQLEXPRESS', [string]$Database = 'ClearParent_Development')
$ErrorActionPreference = 'Stop'
# Read-only verification. No schema/data mutation or permission changes.
$query = @"
SET NOCOUNT ON;
IF NOT EXISTS (SELECT 1 FROM sys.dm_exec_connections WHERE session_id = @@SPID AND encrypt_option = 'TRUE')
    THROW 51001, 'SQL transport encryption was not confirmed.', 1;
IF (SELECT COUNT(*) FROM sys.tables WHERE name IN ('AspNetUsers','Sessions','Preferences','AuditEvents','Children')) <> 5
    THROW 51002, 'Required application tables are missing.', 1;
IF NOT EXISTS (SELECT 1 FROM sys.triggers WHERE name = 'TR_AuditEvents_AppendOnly' AND is_disabled = 0 AND parent_id = OBJECT_ID('dbo.AuditEvents'))
    THROW 51003, 'Append-only audit trigger is missing or disabled.', 1;
IF NOT EXISTS (SELECT 1 FROM dbo.__EFMigrationsHistory WHERE MigrationId = '20261021012209_AccountFoundation')
    THROW 51004, 'Expected migration 20261021012209_AccountFoundation is missing.', 1;
IF NOT EXISTS (SELECT 1 FROM dbo.__EFMigrationsHistory WHERE MigrationId = '20261021012722_DeviceSessions')
    THROW 51004, 'Expected migration 20261021012722_DeviceSessions is missing.', 1;
IF NOT EXISTS (SELECT 1 FROM dbo.__EFMigrationsHistory WHERE MigrationId = '20261021013053_RegistrationConsent')
    THROW 51004, 'Expected migration 20261021013053_RegistrationConsent is missing.', 1;
IF NOT EXISTS (SELECT 1 FROM dbo.__EFMigrationsHistory WHERE MigrationId = '20261021013132_AppendOnlyAudit')
    THROW 51004, 'Expected migration 20261021013132_AppendOnlyAudit is missing.', 1;
IF NOT EXISTS (SELECT 1 FROM dbo.__EFMigrationsHistory WHERE MigrationId = '20261021013427_AuditWriteCompatibility')
    THROW 51004, 'Expected migration 20261021013427_AuditWriteCompatibility is missing.', 1;
IF NOT EXISTS (SELECT 1 FROM dbo.__EFMigrationsHistory WHERE MigrationId = '20261021021739_ChildProfiles')
    THROW 51004, 'Expected migration 20261021021739_ChildProfiles is missing.', 1;

SELECT DB_NAME() AS DatabaseName, encrypt_option FROM sys.dm_exec_connections WHERE session_id = @@SPID;
SELECT MigrationId, ProductVersion FROM dbo.__EFMigrationsHistory ORDER BY MigrationId;
SELECT name FROM sys.tables WHERE name IN ('AspNetUsers','Sessions','Preferences','AuditEvents','Children') ORDER BY name;
SELECT name, is_disabled FROM sys.triggers WHERE parent_id = OBJECT_ID('dbo.AuditEvents');
SELECT c.name, TYPE_NAME(c.user_type_id) AS TypeName, c.max_length, c.is_nullable
FROM sys.columns c WHERE c.object_id = OBJECT_ID('dbo.Children') ORDER BY c.column_id;
SELECT name, is_unique FROM sys.indexes WHERE object_id = OBJECT_ID('dbo.AspNetUsers') AND name = 'EmailIndex';
"@
& sqlcmd -S $Server -d $Database -E -N -C -l 5 -b -Q $query
if ($LASTEXITCODE -ne 0) { throw 'SQL verification failed; do not treat the database schema as verified.' }

