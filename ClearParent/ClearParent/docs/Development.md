# Development setup

The existing .NET 10 project is retained. MVC account screens coexist with Razor Pages and ASP.NET Core Identity UI. Requirements and progress are nested in `ClearParent/ClearParent/docs/`.

## Configuration

Use user secrets or environment variables. Never put credentials in appsettings files or Git. The project has a UserSecretsId; no secrets were created in this session.

Required configuration keys:

- `ConnectionStrings:ClearParent`: SQL Server connection with encryption enabled. For local development, use a new dedicated development database and Windows integrated security. Production needs validated certificates, least-privilege SQL permissions and encryption at rest.
- `Email:Host`, `Email:Port` (default 587), `Email:From`, optionally `Email:Username` and `Email:Password`. SMTP requires TLS. No email was sent externally in this session; tests substitute an in-memory sender.
- `Legal:TermsUrl`, `Legal:PrivacyUrl`: approved HTTPS document URLs; `Legal:Version`: the version accepted at registration. Registration is unavailable without these and email configuration. The starter Privacy page is not an approved legal policy.

Environment variable names replace `:` with `__`, for example `ConnectionStrings__ClearParent`. Use the HTTPS launch profile:

```powershell
dotnet run --project ClearParent --launch-profile https
```

Account and Identity paths reject plaintext transport when HTTPS redirection cannot be applied. Configure trusted proxy forwarding explicitly before deploying behind a TLS-terminating proxy. No forwarded headers are blindly trusted.

## Build and test

```powershell
dotnet restore ClearParent.slnx
dotnet build ClearParent.slnx --no-restore
dotnet test ClearParent.slnx --no-build
python tools/inventory_spec.py
```

The sandbox's Windows TLS/security-package failure prevents normal NuGet requests. Cached .NET 10.0.11 packages restored with `--ignore-failed-sources -p:NuGetAudit=false`. A patched Pkcs package was downloaded from NuGet's HTTPS flat-container endpoint using Python into an ignored local `.packages` directory and restored locally. This is an environment workaround; run a full dependency vulnerability audit in a functioning network environment before release. `dotnet list package --vulnerable --include-transitive` failed TLS in this session.

Automated tests use synthetic `.invalid` addresses, ephemeral encryption keys, EF InMemory and ASP.NET TestServer. They verify application behavior but do not validate SQL Server constraints, transactions, trigger syntax, encrypted storage, SMTP delivery or production hosting. No real family records are fixtures.

## Database

EF Core migrations live in `ClearParent/Data/Migrations/`. The reviewed forward SQL script is `AccountFoundation.sql` beside this document. Application startup never creates, migrates, drops or seeds a database.

After validating an explicit development-only connection, apply the additive migrations using:

```powershell
dotnet ef database update --project ClearParent
```

Do not run database downgrade, drop, reset, or production migrations without approval. Design-time tooling uses `ConnectionStrings__ClearParent` or a local integrated-security fallback only when the environment is Development. Runtime reads configuration, including the credential-free local Development settings; other environments require explicit configuration. SQL Express is running, but both ODBC and Microsoft.Data.SqlClient failed encryption negotiation in this sandbox. The attempted update targeted a new development database and failed before schema creation. Database application and SQL integration tests remain blocked.

The audit trigger prevents UPDATE/DELETE through ordinary SQL DML; EF disables SQL OUTPUT for that table to support the trigger. This is append-only enforcement, not a cryptographic immutability or administrator-proof guarantee. Restrict the application's SQL principal before deployment.

Identity's data-protection keys, database backups, database/storage encryption, key rotation and restore rehearsal require operational configuration and validation. No production-readiness claim is made.

## Resume

Read AGENTS.md, Progress.md, OpenDecisions.md, Traceability.md and current Git status before continuing. `.git` is read-only in this environment: local commits failed because `.git/index.lock` could not be created. Changes remain in the working tree; no commits or deployments occurred.


## 2026-10-20 continuation: local SQL and Git diagnosis

The Development appsettings file now supplies a credential-free Windows integrated-security connection to `.\SQLEXPRESS`, database `ClearParent_Development`, with encryption and local certificate trust. Environment/user-secret overrides take precedence. Runtime and design-time tooling share `SqlConnectionPolicy`: encryption cannot be disabled; `TrustServerCertificate=True` requires both the Development environment and a recognized local host. Production tooling has no development fallback. A local SQL alias can still redirect connections; keep local SQL aliases under workstation administration.

Read-only diagnosis found SQL Express running, `ForceEncryption=0`, TCP disabled, and SQL Browser stopped. Local instance connections may use shared memory; neither enabling TCP nor starting Browser is justified by these results. ODBC 17 reports encryption unsupported / no security-package credentials. Microsoft.Data.SqlClient reports error 20 during encryption initialization before database inspection. Certificate trust does not repair this client/security-package failure. No SQL configuration, Windows permissions, services, encryption policies, or existing data were changed. The precise host-level cause is not yet established; retry the following in a normal Windows terminal to distinguish sandbox context from workstation TLS/SSPI configuration.

```powershell
cd C:\Users\benst\source\repos\ClearParent
$env:DOTNET_ENVIRONMENT = 'Development'
$env:ConnectionStrings__ClearParent = 'Server=.\SQLEXPRESS;Database=ClearParent_Development;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;Connect Timeout=5'
sqlcmd -S '.\SQLEXPRESS' -E -N -C -l 5 -b -Q "SELECT @@SERVERNAME, encrypt_option FROM sys.dm_exec_connections WHERE session_id = @@SPID"
dotnet ef migrations list --project ClearParent
dotnet ef database update --project ClearParent
```

Use only the dedicated development database above. Review the generated idempotent `AccountFoundation.sql` before applying it. `tools/Verify-LocalDatabase.ps1` provides read-only assertions for encrypted transport, expected migrations/tables, and an enabled audit trigger, plus column/index inspection. The workstation requires signed PowerShell scripts and rejected this unsigned script; no execution policy was changed. Run it only under an approved signing/execution workflow. Static migration/model verification does not verify a live schema or trigger enforcement.

Git origin is `https://github.com/benstieglitz00/ClearParent.git`. Read-only ACL inspection shows explicit deny entries on `.git`; `git add -A` fails creating `.git/index.lock`. Do not remove locks or change ACLs to work around this restriction. From an authorized ordinary terminal, inspect and commit the working tree manually:

```powershell
cd C:\Users\benst\source\repos\ClearParent
git status
git diff --check
git diff
# Review untracked files and ensure no secrets or local data are included.
git add -A
git diff --cached --stat
git diff --cached
git commit -m "Add account foundation, local SQL policy, and child profiles"
```

No commit or push was performed by the agent.
