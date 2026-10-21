# Implementation plan (initial; refine after full spec audit)

## Phase 0 — Repository and requirements inventory
- [x] Inspect existing solution, target framework, Git state and tests; do not recreate project.
- [ ] Inventory every numbered module, requirement ID, MVP acceptance criterion, dependency, and TBD policy from source DOCX. Resolve repeated section 9 and heading numbering without dropping requirements.
- [ ] Create requirement traceability matrix with source section, status, test, and implementation path.
- [ ] Validate SQL Server instance and connection via environment/user secrets; no credentials committed.

Inventory extraction and module dependency index exist; detailed requirement triage and SQL validation remain incomplete. See Traceability.md and Progress.md.

## Phase 1 — Security and account foundation
- [ ] ASP.NET Core Identity, verified email, password reset, MFA design, account preferences and audit framework.
- [ ] EF Core SQL Server context, migrations, test database and authorization/tenant isolation tests.

## Phase 2 — Core documentation
- [ ] Child profiles, contacts, journal drafts/publish/version history, evidence originals/hash/scan/quarantine, and timeline projections.

## Phase 3 — Planning and accountability
- [ ] Calendar and recurrence, parenting plan/rules, parenting time scheduled vs actual, exchange outcomes, expenses/reimbursements.

## Phase 4 — Retrieval and reporting
- [ ] Communication import, OCR/transcripts, cross-module search, report builder, PDF/DOCX export, notifications.

## Phase 5 — AI, billing and polish
- [ ] Grounded AI with citations and explicit save approvals; Stripe test-mode billing; dashboard, analytics, import, responsive/mobile strategy.

## Release gates
- [ ] Automated builds, unit/integration/security tests and manual acceptance test plan.
- [ ] Privacy/security review, retention/backups, restore rehearsal, external provider configuration, performance and accessibility.

Do not mark any phase complete until its MVP acceptance criteria have executable evidence. Explicit future-scope features remain backlog unless user promotes them.


## 2026-10-20 continuation status

Phase 0 and Phase 1 remain open: SQL client encryption initialization prevents live schema validation, full requirement triage is incomplete, and the account/security acceptance gaps in Traceability.md remain. Source 1.29 includes subscriptions/billing, intentionally scheduled in Phase 5; completion of the account foundation must not be represented as full User Management acceptance. Source 14.31 includes downstream modules and operational backup/encryption gates.

At explicit user direction, Phase 2 has started with owned child-profile creation/listing and an additive migration while these validation gaps remain. Contacts, journal/versioning, evidence preservation, and timeline projections remain pending.
