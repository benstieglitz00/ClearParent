# ClearParent Codex operating instructions

You are implementing ClearParent in the existing Git repository. Read `docs/SourceSpecification.docx` (authoritative), `docs/FunctionalSpecification.md` (text extraction), `docs/ImplementationPlan.md`, and `docs/Progress.md` before coding. Inspect the existing solution before modifying it. Never replace existing working code without justification.

## Autonomous execution
- Continue through approved tasks in `docs/ImplementationPlan.md` without requesting routine confirmation. Build, test, repair, and proceed.
- Work in small vertical slices; record test commands/results and completion status in `docs/Progress.md` after each slice.
- Preserve acceptance criteria and distinguish MVP, future scope, and TBD product decisions. Do not claim completion from scaffolding alone.
- If context/session ends, resume from Progress.md and git status. Never overwrite uncommitted user changes.
- Use EF Core migrations for schema changes. Never run destructive migrations against unknown/non-disposable databases; stop for approval.
- Do not deploy, purchase services, expose secrets, send external messages, or push sensitive information without explicit permission.
- No real child, medical, court, or family records in seed/test data. Use synthetic fixtures.
- Enforce server-side per-user authorization on every source, search, report, and AI retrieval path. Secure storage and transport; no secrets in Git.
- Preserve original evidence bytes immutably, store SHA-256 hashes, derived OCR/AI separately, and append audit/chain-of-custody records.
- AI output must never silently change canonical user data; require user acceptance and preserve provenance.
- If a product rule is TBD, log it in `docs/OpenDecisions.md`; choose only a reversible development default and label it explicitly.
- Prefer functional integration tests over placeholder screens. Record known limitations honestly.
