# Open product decisions

Extract all “Product Decisions Still Required” sections into a decision register before implementing irreversible policy choices. Examples: retention periods, evidence storage provider, file quotas, payment pricing, authentication timeouts, AI provider and data handling, parenting-time lateness threshold, and mobile implementation strategy.

## Reversible development defaults (not approved production policy)
- Password: Identity complexity defaults, minimum 12 characters; lockout: 5 failures / 15 minutes.
- Inactivity cookie: 30 minutes with sliding expiration; Identity persistent-cookie behavior for Remember Me pending explicit policy.
- Confirmation/reset tokens: 1 hour. Trusted-device lifetime and recovery count remain pending.
- Language `en`, zone `UTC`, AI suggestions off by default. Language preference is storage only; translations are not implemented.
- No automatic retention purge, evidence deletion, production deployment, or database migration on application startup.
- See DecisionRegister.md for all extracted source decision paragraphs. None is resolved by extraction.


## Child-profile development slice

Child profiles currently store an owner, display name and optional birthday. This is a reversible development baseline for source 1.10.2, not an approved complete child-profile policy. Duplicate names are allowed because names do not establish identity. Shared access, editing/version history, deletion/recovery and additional fields remain unimplemented. Birthdays use date-only values and reject future UTC dates. Other modules should reference these canonical profile IDs instead of copying child records.
