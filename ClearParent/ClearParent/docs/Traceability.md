# Phase 1 requirement traceability

Source locators refer to the preserved Word specification and RequirementInventory.json. These are partial feature slices, not module or release completion claims. Database execution remains unvalidated.

| Source | Implemented path | Executable evidence | Remaining acceptance work |
|---|---|---|---|
| 1.3–1.5; UM-EV-001–006 | Areas/Identity/Pages/Account/Register; Identity confirmation UI; Security/SmtpEmailSender | RegistrationTests: legal consent, honeypot rejection, pending account, captured verification email, invalid/unverified access; IdentityTests: invalid confirmation token | Configure real transport and versioned legal documents; expiry/resend HTTP tests; delivery retry/outbox |
| 1.6; UM-AUTH-001–010 | Program.cs; Security/AuditedSignInManager | IdentityTests: protected-route challenges; SessionTests: real cookie; RegistrationTests: verified login | Dedicated rate-limit, brute-force/lockout, Remember Me, and generic-error acceptance tests |
| 1.7, 1.20; 14.12–14.13 | AccountController.Devices/Revoke/RevokeAll; Data/AccountSession; cookie validation | SessionTests: device view, CSRF failure, cross-user revoke denied, revoked cookie rejected | Test revoke-all HTTP path, timeout, reset invalidation, trusted device revocation; rename/OS parsing/geolocation |
| 1.8 | Identity reset/recovery UI; AuditedUserManager | IdentityTests: weak password rejected; old password invalid; successful reset token cannot be reused | Full HTTP recovery, explicit expiry test, delivery failure behavior |
| 1.9; UM-MFA-001–008 | LoginWith2fa page; AccountController.EmailMfa; Identity authenticator/recovery management | IdentityTests: valid/invalid email codes and single-use recovery codes; registration HTTP test opens security management | Full MFA HTTP flows, TOTP clock cases, trusted-device lifetime and revocation, failed-factor audit coverage |
| 1.10, 1.14 | Registration stores first/last names; AccountController.Preferences | AccountIsolationTests: per-user preferences, anonymous denial, cross-user write rejection | Full editable profile, all preference groups, contacts and contact change history; existing language setting does not translate UI |
| 1.22; 14.7 | Data/AuditEvent; AuditedUserManager/SignInManager; append-only SQL trigger migration | AccountIsolationTests: update/delete rejected; RegistrationTests: creation/confirmation audited | SQL trigger execution, all required event fields and coverage, operational monitoring, audit export and retention |
| 1.23–1.24; 14.3–14.6 | Identity roles; global authenticated fallback; secure cookies; CSRF; TLS guard | Anonymous MVC challenge, CSRF rejection, user-scoped query tests | Full role matrix future scope; SQL/storage encryption, key management and transport deployment validation remain release gates |
| 1.15–1.19 | Not implemented; planned Phase 5 per approved plan | None | Stripe test-mode billing, subscriptions, entitlements, invoices and payment lifecycle |
| 1.29; 14.31 | Partial foundation only | Tests above | Module-wide acceptance incomplete; no certification or release-readiness claim |

All other modules remain unimplemented. ModuleInventory.md records dependency order; RequirementInventory.json preserves every source paragraph, tables, requirement IDs, repeated numbering, and classification slots. Detailed requirement triage is still required; scope labels describe containing sections rather than automatically approving a feature.


## Continuation coverage

| Source | Implementation | Evidence | Remaining |
|---|---|---|---|
| 1.10.1 first/last name | AccountController.Profile; Views/Account/Profile | SessionTests HTTP save, ignores supplied foreign UserId, persists own names and audit; anonymous challenge | Preferred name, avatar, location and full contact history |
| 1.10.2 child names/birthdays; P325-327 | ChildProfile; ChildrenController; Views/Children; ChildProfiles migration | ChildProfileTests owner isolation, anonymous reads, foreign tracked-write rejection, future-date rejection; SessionTests CSRF, form submission, HTML encoding | Edit/version history, relationships to other modules; SQL execution |
| 14.3 encrypted transport | SqlConnectionPolicy; runtime/design-time integration | SqlConnectionPolicyTests local-development-only trust, remote/production rejection, encryption required | Working SQL client/SSPI context and live TLS validation |

Phase 2 has begun at user direction while Phase 0/1 validation gaps remain open. This supersedes the earlier statement that all other modules are unimplemented. Phase 2 is a limited child-profile creation/list slice, not completion of Core Documentation.
