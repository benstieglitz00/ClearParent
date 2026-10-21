
# ClearParent — Functional Specification


# 1. User Management

Document Version: 1.0
 Module: User Management
 Status: Draft
 Purpose: Engineering / Product / QA specification


## 1.1 Purpose

The User Management module provides the account and identity foundation for ClearParen. It is responsible for securely identifying users, authenticating access, storing user profile information, managing subscriptions, providing account security controls, storing preferences, tracking active devices and sessions, and recording account activity.

All protected ClearParent information must require an authenticated user session unless explicitly designated as publicly accessible.

The architecture must initially support individual parent accounts while remaining extensible for future role-based and shared-access functionality.


## 1.2 Functional Scope


## 1.3 User Account Model

Each registered user shall have a unique account.

Primary account attributes

Primary email addresses must be unique across the platform. The source also requires users to be able to maintain a secondary email address and change the primary email address.


## 1.4 Registration


### 1.4.1 Registration Workflow

A visitor shall be able to create an account using an email address and password.

Registration screen

The registration screen shall collect:

The registration requirements explicitly require email/password registration, validation, prevention of duplicate email accounts, acceptance of legal agreements, email verification, user-friendly validation messages, and automated-registration protection.


### 1.4.2 Registration Processing

When the user submits registration:

User submits registration

↓

Validate form

↓

Validate CAPTCHA (I don’t want to use captcha, use something else)

↓

Check email uniqueness

↓

Validate password policy

↓

Create pending account

↓

Generate verification token

↓

Send verification email

↓

Display verification-required page

The account shall not be considered fully activated until email verification is completed.


### 1.4.3 Registration Errors

The application shall provide user-friendly errors for conditions such as:


## 1.5 Email Verification

Every newly created account must verify ownership of its primary email address.


### 1.5.1 Verification 3.6.1 Workflow

Registration complete

↓

Verification email sent

↓

User opens verification link

↓

Token validated

↓

Account activated

↓

User notified of successful verification


### 1.5.2 Functional Requirements


## 1.6 Authentication

Users must securely authenticate before accessing protected application data. The source specifies email/password authentication, Remember Me, session management, timeout behavior, automatic inactivity logout, device recognition, and last-login information.


### 1.6.1 Login Workflow

User enters email + password

↓

Validate credentials

↓

Check account state

↓

Check lockout state

↓

Require MFA when enabled

↓

Create secure session

↓

Record login audit event

↓

Update last login

↓

Redirect to application


### 1.6.2 Login Requirements


## 1.7 Session Management

The application shall maintain server-recognized authenticated sessions.

Each active session shall be associated with information sufficient to support the device-management functionality, including device, browser, IP address, approximate login location, and last activity where available.


### 1.7.1 Session Lifecycle

Login

↓

Session created

↓

User activity updates session

↓

Session remains valid

↓

Timeout / logout / revocation

↓

Session invalidated

A session shall terminate when the user explicitly logs out, the inactivity timeout is reached, the session is revoked from Device Management, all sessions are revoked, or another security condition invalidates the session.


## 1.8 Password Reset

Users must be able to recover access using the Forgot Password process. The supplied requirements call for secure reset emails, expiring tokens, single-use links, password confirmation, and password-strength validation.


### 1.8.1Password Reset Workflow

User selects Forgot Password

↓

Enter email

↓

System processes request

↓

Password reset email sent if eligible

↓

User opens reset link

↓

Validate token + expiration

↓

Enter new password twice

↓

Validate password strength

↓

Update password

↓

Invalidate reset token

↓

Record security event

A reset token shall be unusable after successful password change.


## 1.9 Multi-Factor Authentication

The initial implementation shall support email verification codes and authenticator-app TOTP. SMS and passkeys are future functionality. Users shall also have recovery codes and trusted-device support.


### 1.9.1 MFA Functions


## 1.10 User Profile

Users shall maintain profile information as well as parenting-specific profile information.

The parenting profile requirements include number of children, children's names and birthdays, parenting schedule, parenting-plan upload, court jurisdiction, and important contacts.

Profile Sections


### 1.10.1 Personal Information


### 1.10.2 Parenting Information

Where another ClearParent module owns the canonical version of an object, User Management should reference that object rather than maintaining competing copies.

For example, a future dedicated child-profile or parenting-plan module should own its data and expose it to User Management.


## 1.11 Contact Information

Users shall be able to maintain account and family contact information.

Changes must maintain a complete history, and sensitive contact information must be protected in storage and transmission.


### 1.11.1 Email

The account shall support:

Changing the primary email shall require authentication and shall trigger appropriate email verification.


### 1.11.2 Phone Numbers

A user may maintain multiple phone numbers categorized as:

One phone number may be designated as the primary number.

SMS notification functionality is future scope.


## 1.12 Emergency Contacts

Users may maintain emergency contact information.

Each contact shall support:

Supported relationship values include Parent, Grandparent, Sibling, Relative, Friend, Neighbor, Guardian, and Other.


## 1.13 Communication Preferences

Users shall control how application notifications are delivered.

Initial supported channels are email and in-application notifications. Push and SMS are identified as future capabilities.

Notification categories shall support preferences for:


## 1.14 User Preferences

User preferences shall persist across sessions.

The source defines five preference groups.


## Appearance


## Notifications

Email notifications, push notifications, SMS notifications, weekly summaries, daily reminders, and calendar reminders shall be represented by preference settings. Availability of a preference does not require implementation of future delivery channels before those channels are released.


## Dashboard

Users shall be able to configure a default landing page, favorite widgets, quick actions, and default date range.


## Journal

Users shall be able to configure a default entry category, default child, auto-save preference, and auto-tagging preference.


## AI

Users shall be able to configure writing style, report tone, preferred summary length, and whether AI suggestions are enabled.


## Privacy

Users shall be able to configure report sharing defaults, password requirements before exports, sensitive-information blurring, and automatic personal-information redaction.


## 1.15 Subscription Management

Subscription Management shall govern entitlement to paid ClearParent functionality. Users must be able to manage subscriptions without contacting customer support.


## Supported Plans

The source provides different feature entitlements for these plans, including journal/evidence limits on Free, full individual functionality on Professional, shared functionality on Family, client workspaces for Legal Professional, and organization/team functionality for Enterprise.


## 1.16 Subscription Lifecycle

The platform shall recognize the following states:

Subscription state determines the application's available features.


## Subscription Actions

The user shall be able to view, upgrade, downgrade, cancel, reactivate, pause, resume, switch between monthly and annual billing, manage auto-renewal, inspect renewal information, inspect billing history, manage payment methods, and manage free trials.

The source leaves the precise effective-date rules for certain upgrades and downgrades open, stating that changes may occur immediately or at the next billing cycle depending on billing rules. That policy therefore remains a product decision rather than being invented by this specification.


## 1.17 Free Trials

Eligible plans shall support free trials.

User begins eligible trial

↓

Trial subscription created

↓

Premium trial access granted

↓

Expiration reminder issued

↓

User upgrades/cancels OR trial expires

↓

Subscription transitions according to billing policy

Administrators shall be able to extend trial periods.


## 1.18 Billing History

Users shall have access to historical billing activity.

Each billing record may include:

Invoices and receipts shall be downloadable where available.


## 1.19 Payment Methods

The application shall not directly store payment-card information.

Stripe is the required initial payment processor and shall handle secure payment processing, recurring subscriptions, invoices, and payment notifications.

Users shall be able to add, remove, update, and designate default payment methods. The system shall support replacement of expired methods and automated retry of failed payments.

Payment-related events must be written to the application's audit log.


## 1.20 Device Management

The application shall expose an authenticated-device/session management screen.

Each displayed session should contain:

The user shall be able to revoke one session or sign out all devices.

A revoked session must no longer be accepted for authenticated requests.


## 1.21 Security Center

The application shall expose a Security Center containing:

These requirements come directly from the supplied security-center specification.


## 1.22 Audit Logging

Account-related activity shall be recorded in a persistent security audit log.

Required event examples include account creation, login/logout, failed login, email and password changes, MFA changes, subscription changes, profile updates, account deletion, and device changes.


## Audit Record

The source specifically requires timestamp, user, device, IP, performed action, and success/failure result.

Changes to contact information require additional change-history information including the previous value and new value.


## 1.23 Authorization and Roles

The MVP may initially support only an individual parent account.

The architecture nevertheless must support future role-based access control.

Future roles identified by the product specification are Parent, Attorney, Parenting Coach, Therapist, Guardian ad Litem, Mediator, Read-Only Guest, and System Administrator. Permissions will ultimately determine which records, reports, evidence, and administrative functionality each role may access or modify.

The MVP authorization model should therefore avoid hard-coding assumptions that every account will permanently have identical permissions.


## 1.24 Security Requirements

Sensitive user and parenting information shall be encrypted both at rest and in transit.

Authentication controls shall include password hashing, rate limiting, account lockout, brute-force protection, secure cookies, and CSRF protection.

Payment-card data shall remain outside ClearParent infrastructure and be processed through a PCI-compliant payment provider.

Application logs and user-facing errors must not expose passwords, authentication secrets, MFA secrets, reset tokens, payment credentials, or other security-sensitive information.


## 1.25 Core Application Flows


### Account creation

Register

→ Verify Email

→ Account Activated

→ Login

→ Optional MFA Setup

→ Profile Setup

→ Application Dashboard


### Returning user

Login

→ Credential Validation

→ MFA if enabled

→ Session Created

→ Application


### Account recovery

Forgot Password

→ Reset Email

→ Token Validation

→ New Password

→ Login


### Subscription

Choose Plan

→ Stripe Checkout / Payment

→ Subscription Created

→ Entitlements Updated

→ Billing Event Audited


### Failed recurring payment

Stripe Payment Attempt

→ Failure

→ Subscription Status Updated

→ Retry Processing

→ Billing Notification

→ Entitlement Rules Applied


## 1.26 Proposed Domain Relationships

The implementation should conceptually support the following relationships:

User

├── Profile

├── Email Addresses

├── Phone Numbers

├── Emergency Contacts

├── Preferences

├── MFA Methods

├── Sessions / Devices

├── Subscription

│     ├── Billing History

│     └── Payment Provider References

├── Audit Events

└── Future Roles / Permissions

This structure keeps identity, authentication, billing, personalization, and auditing logically separated while remaining attached to the user's account.


## 1.27 Non-Functional Requirements


### Security

All protected endpoints require appropriate authentication and authorization.


### Privacy

Sensitive parenting information must never become publicly accessible because of an account configuration or subscription change.


### Reliability

Account authentication, password recovery, and billing state changes must be designed to tolerate retryable operations without creating duplicate users, payments, or subscription changes.


### Auditability

Security-sensitive operations shall generate traceable audit events.


### Extensibility

The implementation must permit future social authentication, additional MFA methods, multiple account roles, family sharing, professional workspaces, and enterprise access without requiring replacement of the basic user identity model.


## 1.28 MVP Versus Future Scope

The initial implementation should focus on email/password authentication, email verification, password reset, profile management, Stripe subscriptions, supported MFA, user preferences, device management, security information, and auditing.

Features explicitly described by the source as future functionality—such as Microsoft, Google and Apple login; invite-based registration; SMS MFA; passkeys; expanded contact sharing; additional payment providers; and full collaborative role-based access—should be designed for but do not need to be implemented as part of the initial release.


## 1.29 Acceptance Criteria

The User Management module shall be considered functionally complete when a user can register, verify an email address, securely authenticate, recover a forgotten password, manage profile/contact information, configure available security options and preferences, view/manage authenticated devices, purchase and manage a supported subscription, inspect billing information, and view relevant account security information.

Account-sensitive actions must generate appropriate audit records, payment information must remain handled through the designated PCI-compliant payment provider, and the underlying architecture must remain capable of supporting additional identity methods, roles, and shared-access functionality in future versions. This matches the success criteria in the source specification.


## 1.30 Product Decisions Still Required

Before engineering considers this specification fully locked, several policies need explicit product decisions: exact password complexity rules, lockout threshold and duration, default session timeout, Remember Me duration, verification-token expiration, password-reset expiration, MFA recovery-code count, trusted-device lifetime, Stripe upgrade/downgrade proration behavior, exact plan pricing, free-tier usage limits, trial duration, consequences of Past Due/Payment Failed/Suspended states, data-retention policy after cancellation or account deletion, default notification preferences, and whether sensitive profile changes require password or MFA re-authentication.

Those values are deliberately left as TBD because the supplied requirements define the capabilities but do not specify those policies.


# 2. Parenting Journal Functional Specification

Document Version: 1.0
 Module: Parenting Journal
 Status: Draft
 Purpose: Engineering / Product / QA specification


## 1.1 Purpose

The Parenting Journal module is the primary documentation workspace within ClearParent Log. It allows users to create factual, organized, searchable records of parenting-related events, observations, communications, milestones, and supporting context.

The module must support structured journal creation while remaining flexible enough for users to document a broad range of parenting events. Journal entries shall integrate with Evidence Management, Timeline, Search, AI, Reports, Contacts, Calendar, and Parenting Time.

The module should encourage objective documentation without preventing users from recording their own observations or notes.


## 1.2 Functional Scope

The Parenting Journal module shall support:

Journal entry creation

Journal editing

Draft entries

Published entries

Archiving

Soft deletion

Deleted-entry recovery

Categories

Custom categories

Tags

Importance levels

Mood

Location

Weather

Child associations

Evidence associations

Related journal entries

Rich text content

Search and filtering

Timeline integration

AI assistance

Report integration

Audit history

Future functionality includes voice journaling, OCR processing, collaborative entries, offline editing, mobile widgets, handwriting recognition, and smart reminders.


## 2.3 Journal Entry Data Model

Each journal entry shall have a unique system-generated identifier.


### 3.3.1 Core Fields


### 3.3.2 Relationship Fields

A journal entry may be associated with:

One or more children

One or more categories

Multiple tags

Multiple evidence records

Multiple related journal entries

Timeline event

Reports

Future calendar events or parenting-time records where applicable


## 2.4 Journal Entry Status

The journal shall support the following lifecycle states:

The lifecycle shall generally follow:

New Entry

↓

Draft

↓

Published

↓

Archived

↓

Deleted

Users may also delete drafts before publication.

A published entry must not be silently converted back into an unpublished state if doing so would remove it from the historical timeline without an audit record.


## 2.5 Create Journal Entry


### 2.5.1 Workflow

User selects New Journal Entry

↓

Entry form opens

↓

User enters journal information

↓

Auto-save begins

↓

User may save as Draft

OR

User selects Publish

↓

Validation occurs

↓

Journal entry saved

↓

If Published:

Create/update Timeline record

Index entry for Search

Make entry available to Reports and AI


### 2.5.2 Entry Form

The entry form shall support:

Title

Event date

Event time

Child selection

Entry type

Category selection

Description

Notes

Tags

Importance

Mood

Location

Weather

Linked evidence

Related entries


### 2.5.3 Validation

At minimum:

Title shall be required.

Event Date shall be required.

Description shall be required for publishing.

Drafts may contain incomplete information.

Invalid dates shall be rejected.

Linked records must belong to records the user is authorized to access.


## 2.6 Rich Text Editor

Description and supported note fields shall provide rich text editing.

Supported formatting includes:

Headings

Bold

Italic

Underline

Bulleted lists

Numbered lists

Hyperlinks

Paragraph formatting

Embedded images where supported

The editor should support standard keyboard shortcuts and copy/paste.

Rich content shall be sanitized before storage or display to prevent malicious script injection.


## 2.7 Draft Entries

Users shall be able to create and retain unfinished journal entries.


## Functional Requirements


## Auto-Save

Auto-save should occur periodically and when meaningful changes occur.

The system should provide visual status such as:

Saving

Saved

Save Failed

If an auto-save fails, the application should retain unsaved content locally when technically possible and notify the user.


## 2.8 Publishing

Selecting Publish shall transition a valid draft into a Published journal entry.

On publication, the system shall:

Validate required fields.

Set the entry status to Published.

Record the publication timestamp.

Generate an audit event.

Create a corresponding Timeline entry.

Update the Search index.

Make the journal entry available to AI retrieval.

Make the entry available for report inclusion.

Publication must not alter the user's original wording unless the user explicitly applies an AI-generated rewrite.


## 2.9 Editing Published Entries

Users shall be able to edit published journal entries.

Each update shall:

Update the record's Modified Date.

Preserve previous values in version/audit history.

Record who made the change.

Record when the change occurred.

Update Search data.

Update associated Timeline display information where applicable.

The Timeline event should continue pointing to the same underlying journal entry.


## 2.10 Categories

A journal entry may belong to one or more categories.

Default categories include:

Parenting Time

Parenting Exchange

Communication

Therapy

Medical

School

Activities

Expenses

Behavioral Observation

Milestone

Court

Parenting Coach

Guardian ad Litem

Attorney

Safety Concern

Positive Memory

General Notes


## Category Requirements

Category deletion behavior requires a product decision when existing records reference the category.


## 2.11 Tags

Tags provide lightweight, flexible organization.

Users shall be able to:

Add multiple tags

Remove tags

Create new tags

Reuse existing tags

Search by tag

Filter by tag

Examples include:

Vacation

Exchange

Missed Visit

School

Medical

Therapy

Holiday

Birthday

Conflict

Positive

Communication

Court

Parenting Plan

Sports

Travel

Tag names should be normalized to reduce accidental duplicates such as School and school, although the exact case-sensitivity rule remains a product decision.


## 2.12 Importance / Severity

Each entry may optionally be assigned an importance level.

Supported values:

Low

Medium

High

Critical

Importance shall be usable for:

Search filtering

Timeline highlighting

Report filtering

Dashboard summaries

AI prioritization

Importance shall not automatically determine whether an event is legally or clinically significant.


## 2.13 Mood

Mood is optional metadata representing the user's selected emotional state.

Supported initial values include:

Happy

Calm

Concerned

Frustrated

Sad

Excited

Grateful

Neutral

Other

If Other is selected, the interface may allow supplemental text.

Mood shall remain optional and shall not be required for publishing.

Mood may later be available for analytics.


## 2.14 Location

Location information is optional.

The journal shall support:

GPS location

Manual address

City

State

Venue

School

Medical Office

Court

Home

Other


## Location Rules

Location shall not be required.

GPS access shall require device/user permission.

The application must not collect GPS automatically when permission has not been granted.

Users shall be able to manually enter or remove location information.

Location metadata shall follow normal application security and privacy rules.

Future integration may support standardized place records.


## 2.15 Weather

Weather is optional.

Supported information may include:

Condition

Temperature

Notes

Example conditions:

Sunny

Cloudy

Rain

Snow

Windy

Initial versions may permit manual entry.

Automatic weather lookup based on event location/date is future functionality.


## 2.16 Child Association

A journal entry may be associated with one or more children.

Users shall be able to:

Select one child

Select multiple children

Create entries without a child association where appropriate

Child associations shall support:

Search

Filtering

Reports

Timeline display

AI retrieval

Analytics

Only children accessible to the authenticated user's workspace shall be available for selection.


## 2.17 Evidence Linking

Journal entries shall support associations with one or more Evidence Management records.

Supported evidence types may include:

Photos

Videos

PDFs

Emails

Text Messages

OFW Messages

Voice Recordings

Audio Files

Court Documents

Medical Records

School Documents

Screenshots

External Links


## Linking


## Journal Entry

↓

Select Add Evidence

↓

Search existing evidence OR upload new evidence

↓

Select evidence

↓

Create association

↓

Evidence displayed in journal entry

The journal entry should store a relationship to the evidence record rather than duplicate the evidence file.


## Relationship Rules

One journal entry may have multiple evidence items.

One evidence item may be linked to multiple journal entries.

Removing a relationship shall not delete the evidence itself.

Deleting evidence should not silently delete the journal entry.

Evidence permissions shall be honored.


## 2.18 Related Entries

Journal entries may be connected to other journal entries.

Examples include:

Multiple exchanges

Therapy sessions

Court proceedings

Medical treatment

School incidents

Ongoing communication threads

Users shall be able to:

Search for an existing journal entry

Link it as related

Remove the relationship

Navigate directly to the related entry

Relationships should generally be reciprocal for navigation.

Example:

Journal Entry A

↕

Related Entry

↕

Journal Entry B

Deleting the relationship shall not delete either journal entry.


## 2.19 Archive

Users shall be able to archive journal entries.

Archived entries shall:

Remain stored

Remain searchable

Remain accessible through applicable reports

Retain evidence relationships

Retain audit history

Be excluded from normal active-journal views unless the user includes archived records

Users shall be able to restore an archived entry to Published status.


## 2.20 Delete and Recovery

Journal deletion shall use soft deletion during the configured recovery period.

Published Entry

↓

Delete

↓

Deleted Items

↓

Restore

OR

Retention expires / permanent delete

Deleted entries shall:

Be hidden from standard journal views

Be hidden from normal Timeline views

Remain in the audit history

Remain recoverable during the configured retention period

Restoring an entry shall restore applicable Timeline and Search visibility.

Permanent deletion behavior must comply with application data-retention policies.


## 2.21 Journal List

The main journal page shall display a list of journal entries.

Each row/card should provide sufficient identifying information such as:

Title

Event date/time

Child

Category

Importance

Status

Tags

Evidence indicator

The journal list should support:

Sorting

Filtering

Search

Pagination or infinite scrolling

Quick entry access


## 2.22 Search and Filtering

Users shall be able to search journal records by free text and structured metadata.

Supported filters include:

Date Range

Child

Category

Tags

Mood

Importance

Location

Entry Type

Linked Evidence

Keyword

Author

Draft Status

Search shall include:

Title

Description

Notes

Categories

Tags

Relevant metadata

The broader Global Search module may additionally index journal content and linked evidence.


## 2.23 Timeline Integration

Every Published journal entry shall automatically generate or maintain a Timeline event.

Timeline data shall include:

Date and Time

Title

Category

Child

Linked Evidence

Related Events


## Timeline Behavior

Journal Published

↓

Timeline Event Created

Journal Updated

↓

Timeline Event Updated

Journal Deleted

↓

Timeline Event Hidden/Removed

Journal Restored

↓

Timeline Event Restored

The Timeline record shall link back to the source journal entry.

The system should avoid duplicating a Timeline entry when the same journal entry is edited multiple times.


## 2.24 Report Integration

Published journal entries shall be eligible for Report Builder inclusion.

Users shall be able to select journal entries using criteria such as:

Date

Child

Category

Tag

Importance

Entry Type

Draft entries shall not be included in standard reports.

Reports may include:

Journal title

Date/time

Description

Categories

Linked evidence references

Selected metadata


## 2.25 AI Integration

The Parenting Journal shall integrate with the application's AI Assistant.

Supported functions include:

Neutral rewrite

Summary generation

Category suggestions

Tag suggestions

Related-entry detection

Duplicate-event detection

Timeline generation

Report generation

Missing-information suggestions

Natural-language search


## Neutral Rewrite

User writes journal entry

↓

Select "Rewrite Neutrally"

↓

AI generates suggestion

↓

Original text remains unchanged

↓

User compares output

↓

Accept / Edit / Reject

The AI must not automatically overwrite user-authored content.


## AI Content Identification

AI-generated or AI-modified text shall be clearly identifiable before it is saved.

The system should maintain sufficient provenance to determine whether content was:

User authored

AI generated

AI modified and user accepted


## 2.26 Duplicate Detection

The AI may identify potentially duplicate journal entries.

Duplicate detection may consider:

Similar date/time

Same child

Same category

Similar text

Similar linked evidence

Related participants

A duplicate suggestion shall not automatically merge records.

Users should be able to:

Ignore suggestion

Mark records as related

Review entries

Merge in a future feature if supported


## 2.27 Missing Information Suggestions

AI may identify potentially incomplete entries.

Examples:

No child selected

No category

Mentioned evidence not attached

Missing event date details

Possible related entry

Suggestions should be advisory only.

The user shall remain in control of all journal information.


## 2.28 Audit History

All meaningful changes to published journal records shall be audited.

Audit information shall include:

Journal Entry ID

Timestamp

User

Action

Previous Values

Updated Values

Device where available

IP Address where available

Auditable actions include:

Creation

Publication

Editing

Category changes

Tag changes

Evidence linking/unlinking

Related-entry changes

Archiving

Restoration

Deletion

Draft auto-save operations may be handled differently from published-entry history to prevent excessive audit noise; exact behavior is a product decision.


## 2.29 Version History

Published entries should maintain historical versions sufficient to review previous content.

A version may include:

Version number

Modified date/time

Modified by

Description content

Notes

Categories

Tags

Importance

Child associations

Users should eventually be able to view differences between versions.

Whether end users may restore an older journal version requires a product decision.


## 2.30 Security

All journal information shall be encrypted:

In transit

At rest

The application shall enforce authorization before allowing any journal read, write, delete, restore, search, report, evidence-linking, or AI operation.

A user must never be able to access another user's private journal records unless future workspace-sharing permissions explicitly allow it.

Search indexes, AI retrieval systems, backups, and exports must honor the same security model.


## 2.31 Performance Requirements

The journal must remain usable as the volume of historical entries grows.

The implementation should support:

Efficient paginated retrieval

Indexed metadata filtering

Indexed text search

Lazy loading of large evidence previews

Asynchronous AI operations where appropriate

Asynchronous evidence processing where appropriate

Search and filtering should not require loading the user's complete journal dataset into the client.


## 2.32 Desktop and Mobile Behavior

The journal shall initially support responsive web access.

When mobile application functionality is available, users shall also be able to:

Create entries

Edit entries

Save drafts

Publish entries

Add photos

Link evidence

Use supported AI tools

Mobile-specific functionality such as voice journaling and quick-entry widgets remains future scope.


## 2.33 Error Handling

The application shall provide clear error handling for common failures.

Examples include:

Error messages shall not expose stack traces, SQL information, internal IDs, credentials, or other implementation-sensitive details.


## 2.34 Suggested API Surface

The exact API implementation may vary, but the module should conceptually support operations equivalent to:

POST   /journal

GET    /journal

GET    /journal/{id}

PUT    /journal/{id}

DELETE /journal/{id}

POST   /journal/{id}/publish

POST   /journal/{id}/archive

POST   /journal/{id}/restore

POST   /journal/{id}/evidence

DELETE /journal/{id}/evidence/{evidenceId}

POST   /journal/{id}/related

DELETE /journal/{id}/related/{relatedId}

GET    /journal/{id}/versions

GET    /journal/{id}/audit

GET    /journal/categories

POST   /journal/categories

GET    /journal/tags

POST   /journal/{id}/ai/rewrite

POST   /journal/{id}/ai/summarize

POST   /journal/{id}/ai/suggestions

These are logical interfaces rather than a requirement for specific REST URL conventions.


## 2.35 Proposed Domain Relationships

JournalEntry

├── Author

├── Children[]

├── Categories[]

├── Tags[]

├── Evidence[]

├── RelatedEntries[]

├── Mood

├── Location

├── Weather

├── Versions[]

├── AuditEvents[]

└── TimelineEvent

This structure should avoid duplicating canonical Evidence, Child, User, or Timeline data.


## 2.36 MVP Scope

The initial Parenting Journal MVP should include:

Create entry

Edit entry

Draft entry

Publish entry

Archive

Soft delete

Restore

Child selection

Default and custom categories

Tags

Importance

Optional mood

Optional manual location

Optional manual weather

Rich text

Evidence linking

Related entries

Search/filter

Timeline integration

Report availability

Neutral AI rewrite

AI summarization

Suggested tags/categories

Audit history


## 2.37 Future Scope

The following should remain future functionality unless explicitly promoted into MVP:

Voice-to-journal

AI-created first drafts

Speech-to-text transcription

OCR directly within journal attachments

Photo annotation

Handwriting recognition

Collaborative journal editing

Offline editing

Mobile quick-entry widgets

Smart documentation reminders

Automatic weather retrieval


## 2.38 Acceptance Criteria

The Parenting Journal module shall be considered functionally complete when:

An authenticated user can create a journal entry.

The user can save an incomplete entry as a Draft.

Drafts auto-save and can be resumed.

Drafts do not appear in the Timeline or standard reports.

A valid Draft can be Published.

Published entries automatically appear in the Timeline.

The user can edit published entries.

Previous published values remain auditable.

The user can archive an entry.

The user can soft-delete an entry.

Deleted entries can be restored during the configured retention period.

The user can associate one or more children.

The user can assign categories and custom categories.

The user can assign multiple tags.

The user can assign an importance level.

Mood, location, and weather remain optional.

The user can link multiple evidence records.

Evidence remains stored independently when unlinked from a journal entry.

The user can link related journal entries.

The user can search journal content and metadata.

The user can combine supported filters.

Published entries can be incorporated into reports.

AI can provide summaries and neutral rewrite suggestions.

AI output cannot overwrite original text without user approval.

Journal records and relationships follow application authorization rules.

Journal data is encrypted in transit and at rest.

Meaningful modifications are recorded in the audit history.

Journal search remains functional as entry volume increases.


## 2.39 Product Decisions Still Required

Before this module is considered implementation-locked, the following should be explicitly decided:

Maximum title length

Maximum journal entry length

Whether Description alone is required or Title + Description

Exact auto-save interval

Deleted-entry retention period

Whether deleted records can ever be permanently deleted manually

Whether categories can be deleted when already in use

Whether tags are case sensitive

Whether users can define custom mood values

Whether location stores exact GPS coordinates or only display location

Whether location should be stripped from exports by default

Whether Published entries can be returned to Draft

Whether old journal versions can be restored

Whether AI revisions create a new journal version automatically

Whether embedded images are stored as Evidence items automatically

Maximum number of children, tags, categories, evidence items, and related entries per journal record

Whether draft auto-saves should generate audit records

How Archived entries should behave in Timeline and standard reports

Whether Critical importance creates notifications or is display-only

How duplicate detection thresholds should be configured


# 3. Calendar Functional Specification

Document Version: 1.0
 Module: Calendar
 Status: Draft
 Purpose: Engineering / Product / QA specification


## 3.1 Purpose

The Calendar module provides the centralized scheduling and planning system for ClearParent Log. It allows users to create, manage, view, organize, and track parenting-related events, appointments, deadlines, exchanges, reminders, and recurring schedules from a single interface.

The Calendar shall act as both a forward-looking planning tool and a historical record of scheduled events. It must integrate with Parenting Journal, Parenting Time, Timeline, Contacts, Evidence Management, Reports, Parenting Plan, Notifications, and AI so that scheduled events remain connected to supporting records and documentation.


## 3.2 Functional Scope

The Calendar module shall support:

Calendar event creation

Calendar event editing

Calendar event deletion

Multiple calendar views

Parenting schedules

Parenting exchanges

Holiday schedules

School events

Medical appointments

Therapy appointments

Court dates

Parenting coach meetings

Reminders

Recurring events

Child associations

Contact associations

Location information

Attachments and evidence links

Journal entry associations

Parenting plan associations

Search and filtering

Timeline integration

Notification integration

AI interaction

Audit history

Future functionality includes external calendar synchronization, shared calendars, travel-time estimates, GPS check-in, offline access, voice-created events, and advanced AI scheduling.


## 3.3 Calendar Event Data Model

Every calendar event shall have a unique system-generated identifier.


### 3.3.1 Core Fields


### 3.3.2 Relationship Fields

An event may be associated with:

One or more children

One or more contacts

Journal entries

Evidence

Parenting Time records

Parenting Plan

Reports

Timeline events

Reminders

Recurrence rules

Follow-up tasks


## 3.4 Event Types

The system shall support at minimum:

Parenting Time

Parenting Exchange

Overnight Visit

Holiday Parenting Time

Vacation

Make-Up Parenting Time

Missed Parenting Time

Supervised Visit

Parenting Plan Phase Change

School

Medical

Therapy

Parenting Coach

Court

Attorney Meeting

Guardian ad Litem Meeting

Mediation

Arbitration

Parenting Evaluation

Activity

Birthday

Holiday

Deadline

Reminder

Custom

Event types should remain extensible so additional types can be introduced without changing the core calendar architecture.


## 3.5 Event Status

Calendar events shall support status values appropriate to their event type.

Initial statuses should include:

Scheduled

Confirmed

Completed

Cancelled

Rescheduled

Missed

Pending

Parenting exchange events may additionally support:

Completed

Late

Missed

Modified

Refused

The exact allowed status list may vary by event type.


## 3.6 Create Calendar Event


### 3.6.1 Workflow

User selects New Event

↓

Event form opens

↓

User selects event type

↓

Relevant fields displayed

↓

User enters details

↓

User configures recurrence/reminders if needed

↓

Validate

↓

Save event

↓

Create/update Timeline reference where applicable

↓

Schedule Notifications

↓

Index event for Search and AI

The interface should dynamically present fields relevant to the selected event type.

For example, a Court event may display Court, Judge, Attorney, and Case Number fields, while a Therapy event may display Therapist, Participants, and Session Type.


## 3.7 Calendar Views

The module shall provide:

Daily View

Weekly View

Monthly View

Yearly View

Agenda View

Timeline View

Upcoming Events View

Users shall be able to:

Move forward and backward through dates

Jump directly to today

Jump to a selected date

Open an event from any view

Create an event from applicable calendar views

The selected calendar view should be retained as a user preference where appropriate.


## 3.8 Daily View

Daily View shall display all events occurring on a selected date.

The view should distinguish:

All-day events

Timed events

Overlapping events

Parenting time blocks

Reminders

Selecting an event shall open its details.


## 3.9 Weekly View

Weekly View shall display seven consecutive days.

Users should be able to identify:

Event start/end times

Parenting time blocks

Scheduling conflicts

Multiple events occurring on the same day


## 3.10 Monthly View

Monthly View shall display a traditional month grid.

Each day may display:

Event indicators

Event titles where space permits

Parenting schedule indicators

Important reminders

Selecting a date should allow users to view or create events for that date.


## 3.11 Yearly View

Yearly View shall provide a high-level overview of the user's calendar.

It may prioritize:

Parenting schedule blocks

Holidays

Vacations

School breaks

Court dates

Important recurring events

Detailed event content may require drilling into a month or date.


## 3.12 Agenda View

Agenda View shall display events as a chronological list.

Each item should show:

Date

Time

Event title

Event type

Child

Location

Status

Agenda View shall be suitable for reviewing upcoming appointments and activities without relying on a grid interface.


## 3.13 Upcoming Events View

The application shall provide a view focused on future events.

Supported time ranges may include:

Today

Next 7 Days

Next 30 Days

Custom Date Range

The exact default range remains a product decision.


## 3.14 Parenting Schedule

The Calendar shall support parenting-schedule events including:

Parenting Time

Parenting Exchanges

Overnight Visits

Holiday Parenting Time

Vacation Time

Make-Up Parenting Time

Missed Parenting Time

Supervised Visits

Parenting Plan Phase Changes


### 3.14.1 Parenting Event Fields

A parenting event may contain:

Start Date

Start Time

End Date

End Time

Exchange Location

Children

Notes

Status

Linked Journal Entry

Linked Parenting Plan

When a dedicated Parenting Time module owns the canonical parenting record, the Calendar should reference that record rather than maintain a conflicting duplicate.


## 3.15 Parenting Exchanges

The system shall allow users to schedule and record exchanges.

Exchange fields include:

Scheduled Pickup Time

Actual Pickup Time

Scheduled Drop-off Time

Actual Drop-off Time

Exchange Location

Parent Present

Children Present

Arrival Status

Late Arrival Indicator

Missed Exchange Indicator

Notes

Linked Photos / Evidence

Linked Journal Entry

Where Parenting Time owns detailed exchange records, Calendar should display and link to those records.


## 3.16 Late Exchange Handling

If actual exchange information is available, the system may calculate whether an exchange occurred later than scheduled.

Example:

Scheduled Pickup: 10:00 AM

Actual Pickup:    10:25 AM

Difference:       25 minutes

Status:           Late

The definition of how many minutes constitute a Late status remains a product/business-rule decision.


## 3.17 Holiday Scheduling

The Calendar shall support standard and custom holidays.

Examples include:

Christmas

Thanksgiving

Easter

Mother's Day

Father's Day

Memorial Day

Labor Day

Fourth of July

School Holidays

User-Defined Holidays

Users shall be able to create or modify holiday events.

Where a Parenting Plan contains holiday overrides, the Calendar shall support representing the override separately from the regular parenting schedule.


## 3.18 Parenting Plan Overrides

A holiday, vacation, or other parenting-plan provision may temporarily override the regular parenting schedule.

The system should conceptually support:

Regular Schedule

↓

Parenting Plan Holiday Rule

↓

Applicable Date Detected

↓

Override Displayed

The system shall clearly distinguish between:

Normal parenting schedule

Holiday override

Manually modified schedule

Automatic interpretation of complex parenting-plan language may be handled by future Parenting Plan/AI functionality and should not be assumed unless implemented.


## 3.19 School Events

The Calendar shall support:

First Day of School

Last Day of School

Parent-Teacher Conferences

Performances

Sports Events

School Holidays

Report Cards

Testing Days

Field Trips

School Meetings

Custom School Events

School event fields may include:

School

Child

Teacher

Date/Time

Location

Notes

Attachments


## 3.20 Medical Appointments

Supported medical event types include:

Doctor

Dentist

Orthodontist

Vision

Therapy

Counseling

Specialist

Emergency Care

Custom Medical Appointment

Medical event fields may include:

Provider

Location

Date

Time

Child

Notes

Medical Documents

Insurance Information

Follow-Up Appointment

Sensitive medical information shall follow application security controls.


## 3.21 Therapy Events

The system shall support:

Individual Therapy

Family Therapy

Parenting Coach

Counseling

Psychological Evaluation

Parenting Evaluation

Fields may include:

Therapist

Location

Date/Time

Participants

Session Notes

Follow-Up Tasks

Linked Journal Entries

Detailed clinical notes should only be stored if the user chooses to enter them.


## 3.22 Court and Legal Events

Supported legal event types include:

Hearing

Trial

Mediation

Arbitration

Attorney Meeting

Deposition

Guardian ad Litem Meeting

Parenting Evaluation

Custom Legal Event

Fields may include:

Court

Judge

Attorney

Case Number

Date/Time

Location

Documents

Notes

Follow-Up Tasks

Users shall be able to associate supporting Evidence records with legal events.


## 3.23 Parenting Coach Meetings

Parenting Coach events may include:

Coach

Date

Time

Meeting Type

Participants

Agenda

Notes

Follow-Up Actions

Linked Reports

Linked Journal Entries


## 3.24 Child Associations

An event may be associated with:

One child

Multiple children

No child, when appropriate

Child selection shall affect:

Search

Filtering

Reports

AI responses

Timeline display

Dashboard summaries

Only children available within the current user's authorized workspace shall be selectable.


## 3.25 Contact Associations

Calendar events may reference contacts such as:

Therapist

Doctor

Attorney

Parenting Coach

Teacher

Guardian ad Litem

Mediator

Other Professional

Calendar should reference the canonical Contact record rather than copy the full contact details into each event.

Users should be able to open the associated Contact from the event.


## 3.26 Attachments and Evidence

Users shall be able to link evidence to events.

Examples include:

PDFs

Photos

Court Documents

Medical Documents

School Documents

Emails

Screenshots

The Calendar shall store relationships to Evidence Management records rather than duplicating the underlying files.

Removing an evidence relationship must not delete the evidence itself.


## 3.27 Journal Integration

A calendar event may be linked to one or more journal entries.

Examples:

Therapy Appointment

↓

Session occurs

↓

User writes Journal Entry

↓

Journal Entry linked to Calendar Event

The user shall be able to navigate between the calendar event and linked journal record.

Calendar events should not automatically generate narrative journal content unless the user explicitly requests an AI or journal workflow.


## 3.28 Recurring Events

The Calendar shall support recurring events.

Initial patterns include:

Daily

Weekly

Bi-Weekly

Monthly

Yearly

Custom

Examples include:

Weekly parenting time

Weekly therapy

Bi-weekly parenting schedule

Monthly parenting coach meeting

Annual birthday

Recurring school activity

Medication reminder

Child support payment


## 3.29 Recurrence Model

A recurring event should conceptually consist of:

Recurrence Series

├── Master Event

├── Recurrence Rule

├── Occurrence 1

├── Occurrence 2

└── Exceptions / Modified Occurrences

The system must avoid creating unmanageable duplicate data for long-running recurring series.


## 3.30 Editing Recurring Events

When modifying a recurring event, the user should be offered appropriate options such as:

This Event Only

This and Future Events

Entire Series

The exact options supported for MVP should be finalized before implementation.

Individual occurrences may be modified without necessarily changing the recurrence series.


## 3.31 Deleting Recurring Events

Deletion should similarly distinguish between:

One occurrence

Future occurrences

Entire series

Deleted instances should retain enough system information to prevent them from being regenerated unexpectedly from the recurrence rule.


## 3.32 Reminders

Users shall be able to add one or more reminders to a calendar event.

Supported preset intervals include:

At Event Time

15 Minutes Before

30 Minutes Before

1 Hour Before

1 Day Before

1 Week Before

Custom Interval

An event may support multiple reminders.

Example:

Court Hearing — Aug 20 at 9:00 AM

Reminder 1: 1 week before

Reminder 2: 1 day before

Reminder 3: 1 hour before


## 3.33 Reminder Delivery

Calendar reminders shall integrate with the Notifications module.

Depending on user preferences and supported delivery channels, reminders may eventually be delivered through:

In-App Notification

Email

Push Notification

SMS

The Calendar should be responsible for the reminder configuration; Notifications should be responsible for actual delivery.


## 3.34 Follow-Up Tasks

Events may create or link to follow-up tasks.

Examples:

Upload therapy notes

Submit reimbursement

Complete journal entry

File court document

Schedule follow-up appointment

If a dedicated Tasks module exists later, Calendar should reference the task rather than maintain a separate task implementation.


## 3.35 Search

Calendar events shall be searchable through Calendar Search and Global Search.

Searchable fields should include:

Title

Notes

Event Type

Category

Child

Contact

Location

Tags

Relevant linked-record metadata


## 3.36 Filtering

Users shall be able to filter events by:

Child

Date Range

Event Type

Category

Location

Contact

Status

Tags

Importance

Multiple filters should be combinable.

Example:

Child: Vera

Event Type: Therapy

Date Range: Aug 1–Aug 31

Status: Completed


## 3.37 Sorting

List-based views should support logical sorting such as:

Date ascending

Date descending

Event Type

Child

Importance

Calendar-grid views shall naturally organize events chronologically.


## 3.38 Timeline Integration

Calendar events shall integrate with the Timeline when appropriate.

At minimum, completed or historically significant events may appear in Timeline.

Timeline records should reference the source Calendar event.

The precise rule governing whether every future scheduled event appears in Timeline versus only completed/historical events remains a product decision.

A recommended architecture is:

Future Scheduled Event

↓

Calendar

Completed/Historical Event

↓

Calendar + Timeline

This avoids turning the historical Timeline into a future schedule.


## 3.39 AI Integration

The AI Assistant shall be able to retrieve and reason over authorized Calendar data.

Examples include:

"When is my next parenting exchange?"

"Show therapy appointments this month."

"What events are scheduled next week?"

"Summarize court events."

"What appointments involve Zaia this month?"

AI responses should link back to source calendar events where possible.


## 3.40 Scheduling Conflict Detection

The AI or Calendar logic may identify overlapping events.

Example:

Therapy

Wednesday 10:00–11:00

Court Hearing

Wednesday 10:30–12:00

→ Scheduling Conflict

Conflict detection should be informational and should not automatically modify events.


## 3.41 Available-Time Suggestions

The AI may suggest available time based on existing calendar events.

For example:

"You appear to be available Wednesday between 1:00 PM and 4:00 PM."

Availability suggestions must only use calendar data the user is authorized to access.

Future external-calendar integration may expand this functionality.


## 3.42 AI Event Summaries

AI may generate summaries such as:

Today's Schedule

This Week

Upcoming Court Events

Upcoming Parenting Time

Therapy Schedule

School Schedule

AI-generated summaries shall remain derived content and shall not alter source calendar events.


## 3.43 Reporting

Users shall control which calendar events are included in reports.

Events may be selected using:

Date Range

Child

Event Type

Category

Status

Importance

Report Builder may include:

Event Date

Time

Title

Event Type

Location

Child

Notes

Related Journal Entries

Evidence References


## 3.44 Audit Logging

All meaningful calendar changes shall generate audit records.

Auditable actions include:

Event Created

Event Updated

Event Cancelled

Event Deleted

Event Restored

Status Changed

Recurrence Changed

Reminder Added

Reminder Removed

Evidence Linked

Evidence Unlinked

Journal Entry Linked

Journal Entry Unlinked

Audit records should include:

Event ID

Timestamp

User

Action

Previous Values

New Values

Device where available

IP Address where available


## 3.45 Deletion and Recovery

Calendar event deletion should use the application's configured soft-delete/recovery architecture where applicable.

Deleted events shall be removed from standard Calendar views.

If restored, the event should also restore applicable:

Recurrence relationships

Reminder configuration

Timeline relationship

Journal links

Evidence links

Permanent deletion shall follow the application's retention policy.


## 3.46 Security

All Calendar information shall be encrypted:

In transit

At rest

Authorization shall be enforced for:

Reading events

Creating events

Editing events

Deleting events

Linking evidence

Linking contacts

Linking journal entries

Searching events

Exporting events

AI retrieval

Users shall only see Calendar events they are authorized to access.


## 3.47 Privacy

Sensitive Calendar information may include:

Medical appointments

Therapy appointments

Court dates

Child schedules

Addresses

Exchange locations

The application should not expose sensitive details through public links or unauthenticated interfaces.

Notification lock-screen behavior should be controlled by the Notifications/Security modules.


## 3.48 External Calendar Synchronization

External calendar synchronization is future scope.

Potential providers include:

Google Calendar

Microsoft Outlook

Apple Calendar

The future integration model should consider:

One-way export

One-way import

Two-way synchronization

Conflict resolution

Duplicate detection

Calendar selection

Authentication/token storage

These behaviors should not be assumed until a separate synchronization specification is written.


## 3.49 Performance Requirements

The Calendar shall remain responsive with years of historical and future events.

The implementation should support:

Date-range-based event loading

Indexed event dates

Indexed child/event-type filtering

Efficient recurring-event expansion

Pagination for Agenda views

Lazy loading of evidence

Background notification scheduling

The application should not retrieve all Calendar history each time a monthly view loads.


## 3.50 Time Handling

All stored dates and times should use a consistent internal time model.

The application must preserve the user's intended local event time.

Although advanced time-zone functionality is listed as future scope, the architecture should avoid storing ambiguous date/time data that prevents later time-zone support.

All-day events should be modeled separately from timed events.


## 3.51 Error Handling

Examples include:

Errors shall not expose internal infrastructure details.


## 3.52 Suggested API Surface

The exact technical implementation may differ, but the module should conceptually support:

POST   /calendar/events

GET    /calendar/events

GET    /calendar/events/{id}

PUT    /calendar/events/{id}

DELETE /calendar/events/{id}

POST   /calendar/events/{id}/cancel

POST   /calendar/events/{id}/restore

POST   /calendar/events/{id}/reminders

DELETE /calendar/events/{id}/reminders/{reminderId}

POST   /calendar/events/{id}/evidence

DELETE /calendar/events/{id}/evidence/{evidenceId}

POST   /calendar/events/{id}/journal

DELETE /calendar/events/{id}/journal/{journalId}

POST   /calendar/recurrence

PUT    /calendar/recurrence/{id}

DELETE /calendar/recurrence/{id}

GET    /calendar/upcoming

GET    /calendar/search

These are logical interfaces and do not mandate REST architecture.


## 3.53 Proposed Domain Relationships

CalendarEvent

├── EventType

├── Children[]

├── Contacts[]

├── Location

├── Evidence[]

├── JournalEntries[]

├── ParentingPlan

├── ParentingTimeRecord

├── Reminders[]

├── RecurrenceRule

├── FollowUpTasks[]

├── TimelineEvent

└── AuditEvents[]

Canonical data from other modules should be referenced rather than duplicated.


## 3.54 MVP Scope

The initial Calendar MVP should include:

Daily View

Weekly View

Monthly View

Agenda View

Upcoming Events

Event creation

Event editing

Event cancellation

Event deletion/recovery

Parenting events

Exchanges

Holidays

School

Medical

Therapy

Court

Parenting Coach

Child associations

Contact associations

Notes

Locations

Evidence links

Journal links

Basic recurrence

Reminders

Search

Filtering

Notifications integration

Timeline integration

AI retrieval

Audit logging

Yearly View may also be included if implementation effort is low.


## 3.55 Future Scope

The following should remain future functionality unless explicitly promoted to MVP:

Google Calendar synchronization

Outlook synchronization

Apple Calendar synchronization

Shared family calendars

Advanced color-coded calendars

Resource scheduling

Traffic and travel-time estimation

Weather integration

GPS exchange check-ins

Attendance tracking

Advanced time-zone support

Offline calendar editing

Calendar home-screen widgets

Voice-created events

Advanced AI scheduling assistant


## 3.56 Acceptance Criteria

The Calendar module shall be considered functionally complete when:

Authenticated users can create Calendar events.

Users can edit and cancel existing events.

Users can delete and restore events according to retention rules.

Events can have start and end dates/times.

All-day events are supported.

Users can navigate Daily, Weekly, Monthly, Agenda, and Upcoming views.

Users can quickly return to today's date.

Users can schedule parenting time.

Users can record parenting exchanges.

Users can schedule holidays and parenting-plan overrides.

Users can create school events.

Users can create medical appointments.

Users can create therapy appointments.

Users can create court and legal events.

Users can schedule parenting coach meetings.

Events can be associated with one or more children.

Events can reference Contacts.

Events can reference Journal entries.

Events can reference Evidence.

Users can configure reminders.

Notifications are generated from active reminder configurations.

Users can create recurring events.

Individual recurring occurrences can be handled independently where supported.

Users can search Calendar events.

Users can combine supported filters.

Calendar information is accessible to the AI Assistant.

AI can answer natural-language questions based on Calendar records.

Calendar data can be included in reports.

Applicable historical events integrate with Timeline.

Calendar modifications generate audit history.

Calendar data is encrypted in transit and at rest.

Authorization is enforced across all Calendar operations.


## 3.57 Product Decisions Still Required

Before the Calendar specification is implementation-locked, the following should be finalized:

Exact list of event statuses

Whether every event type may use custom statuses

Default Calendar view

Whether Yearly View is MVP

Default Upcoming Events range

Required versus optional End Time

Whether events may span multiple days

Default event duration

Default time increments in the UI

Definition of a Late parenting exchange

How parenting-plan overrides are entered and displayed

Whether Parenting Plan schedules are manually entered or automatically parsed

Whether Calendar or Parenting Time owns canonical parenting schedule records

Whether Calendar or Contacts owns provider/attorney/teacher details

Which events automatically appear in Timeline

Whether cancelled events remain visible by default

Soft-delete retention period

Whether recurring event edits support "this and future"

Maximum recurrence horizon

Maximum number of reminders per event

Default reminder settings

Whether reminder delivery requires confirmed event status

Whether follow-up tasks are stored directly by Calendar or a future Tasks module

Exact handling of overlapping parenting events

Time-zone storage strategy for MVP

Whether users may manually create holidays or select from a predefined holiday library

Whether changing a recurring parenting schedule automatically updates future events

Whether completed Calendar events automatically prompt the user to create a Journal entry


# 4. Evidence Management

Document Version: 1.0

Module: Evidence Management

Status: Draft

Purpose: Engineering / Product / QA specification


## 4.1 Purpose

The Evidence Management module serves as the centralized repository for all supporting documentation within ClearParent Log. It allows users to securely upload, organize, search, preview, retrieve, link, analyze, version, export, and audit evidence related to parenting events, communications, legal proceedings, medical records, school activities, financial records, and other significant information.

The module shall act as the single source of truth for supporting documentation across the application. Original files shall be preserved without modification while derived artifacts such as thumbnails, previews, OCR text, speech transcripts, AI summaries, and search indexes are created separately.


### 4.1.1 Objectives

The system shall:

• Securely store supported evidence types.

• Preserve original files without alteration.

• Automatically extract and generate searchable metadata.

• Allow evidence to be linked to records across the application.

• Support OCR, speech transcription, AI analysis, and natural-language search.

• Maintain complete version history, audit history, and chain-of-custody records.

• Support inclusion of evidence in reports and exhibits.

• Prevent accidental loss through soft deletion, recovery, and backup.


## 4.2 Functional Scope

The Evidence Management module shall support:

• Evidence upload and bulk upload.

• File validation and malware scanning.

• Original-file preservation.

• Evidence metadata.

• Categories and tags.

• Folders, collections, favorites, and archive.

• OCR and speech-to-text processing.

• AI analysis and metadata suggestions.

• Duplicate detection.

• Evidence relationships.

• Document preview.

• Version management.

• Search and filtering.

• Timeline and Report Builder integration.

• Access control, audit history, and chain of custody.

• Soft deletion and recovery.

Future functionality includes direct cloud imports, Gmail and Outlook integrations, OFW synchronization, face recognition, handwriting recognition, object recognition, digital signatures, watermarking, immutable evidence locking, and secure professional sharing.


## 4.3 Evidence Record Model

Every uploaded or imported evidence item shall become an Evidence Record with a unique immutable Evidence ID.


### 4.3.1 Core Fields

Each Evidence Record shall support:

• Evidence ID

• Title

• Description

• Original File Name

• File Extension

• MIME Type

• File Size

• Upload Date and Time

• Event Date and Time

• Created By

• Modified By

• Status

• Category

• Tags

• Importance

• Source

• Location

• Child or Children

• Related Contacts

• Current Version


### 4.3.2 System-Generated Metadata

The system may generate:

• File hash

• OCR text

• OCR confidence

• AI summary

• AI keywords

• AI categories

• AI suggested tags

• Named entities

• Detected dates

• Image recognition results

• Speech-to-text transcript

• Duplicate score

• Page count

• Image dimensions

• Audio duration

• Video duration

• Processing timestamps


### 4.3.3 Evidence Status

Supported statuses shall include:

• Processing

• Available

• Archived

• Deleted

• Quarantined

• Failed Processing

Status shall determine whether the evidence appears in normal browsing, search, reports, exports, and AI retrieval.


## 4.4 Supported Evidence Types


### 4.4.1 File Types

The system shall support, subject to configured validation rules:

• Images: JPG, JPEG, PNG, HEIC, GIF

• Videos: MP4, MOV, AVI

• Audio: MP3, WAV, M4A

• PDF documents

• Microsoft Word documents

• Microsoft Excel files

• Text files

• ZIP archives

• Email files: EML and MSG

• Screenshots

• Scanned documents


### 4.4.2 Evidence Content Types

The system shall support content including:

• OFW message exports

• Text message exports

• Call logs

• Voicemail recordings

• Social media screenshots

• Court orders

• Parenting plans

• Medical records

• School records

• Police reports

• Receipts

• Financial statements

• Insurance documents

• Photographs

• Videos

• External URLs


## 4.5 Upload Methods


### 4.5.1 Supported Upload Methods

Users shall be able to add evidence through:

• File picker

• Drag and drop

• Bulk upload

• ZIP import

• Mobile camera

• Mobile photo library

• Audio recording

• Document scanner

• Email import

Cloud storage import is future scope.


### 4.5.2 Upload Workflow

The upload workflow shall:

1. Accept one or more files.

2. Validate extension, MIME type, and size.

3. Scan files for malware or other prohibited content.

4. Generate a file hash.

5. Check for exact or probable duplicates.

6. Store the original file.

7. Extract technical metadata.

8. Generate supported previews and thumbnails.

9. Queue OCR, transcription, and AI analysis as applicable.

10. Index searchable content.

11. Mark the Evidence Record Available when required processing is complete.

Long-running processing shall occur asynchronously and shall not prevent the user from continuing to use the application.


## 4.6 File Validation


### 4.6.1 File Type Validation

The system shall:

• Compare file extensions with detected MIME types.

• Reject unsupported file types.

• Reject blocked executables and scripts.

• Prevent disguised executable content from being accepted as supported evidence.

• Display a user-friendly validation error.


### 4.6.2 File Size Validation

Maximum file sizes shall be configurable and may vary by subscription plan. Files exceeding the user's permitted limit shall be rejected before processing when technically possible.


### 4.6.3 Malware Scanning

All uploaded files shall be scanned before becoming generally available. Quarantined files shall not appear in reports, AI retrieval, or general search until cleared.


### 4.6.4 Duplicate Validation

The system shall generate a SHA-256 hash for each original file. Exact hash matches shall be flagged as exact duplicates. Similarity-based duplicate detection may additionally use filename, metadata, OCR text, AI content, or visual similarity.

The system shall not silently merge or discard evidence. The user shall be presented with an appropriate choice such as keeping the existing evidence, creating a new version, or keeping a separate duplicate.


## 4.7 Storage and Original File Preservation


### 4.7.1 Original File

The original uploaded file shall never be modified or overwritten. All processing shall create separate derived artifacts.


### 4.7.2 Derived Artifacts

Derived artifacts may include:

• Thumbnail

• Preview rendition

• OCR text

• Speech transcript

• AI summary

• AI metadata

• Search index content


### 4.7.3 Storage Separation

The architecture should logically separate:

• Binary object storage

• Evidence metadata

• Search indexes

• AI/vector indexes

• Audit records

• Version records


## 4.8 Metadata Management


### 4.8.1 User-Managed Metadata

Users shall be able to manage:

• Title

• Description

• Event date and time

• Child

• Category

• Tags

• Source

• Importance

• Location

• Related contacts


### 4.8.2 Extracted Metadata

The system may extract:

• Original filename

• File size

• File type

• Created/modified dates when available

• Image dimensions

• Camera metadata

• GPS metadata

• Document page count

• Audio/video duration


### 4.8.3 Metadata History

Changes to user-managed metadata for published evidence shall be recorded in audit history, including previous and new values where appropriate.


## 4.9 Categories

The system shall provide default evidence categories including:

• Communication

• Parenting Time

• Parenting Exchange

• School

• Medical

• Therapy

• Legal

• Financial

• Activities

• Photos

• Videos

• Court

• Parenting Coach

• Guardian ad Litem

• Attorney

• Safety

• General

Users shall be able to create custom categories. Category changes shall not alter the original evidence file.


## 4.10 Tags

Users shall be able to assign multiple tags to one evidence item.

Example tags include:

• Missed Visit

• Vacation

• Exchange

• School

• Medical

• Therapy

• Holiday

• Court

• Communication

• Positive

• Conflict

• Birthday

• Travel

• Emergency

Tags shall be searchable and usable in reports. AI may suggest tags, but suggestions shall require user acceptance before becoming user-managed tags.


## 4.11 OCR Processing


### 4.11.1 OCR Eligibility

The system shall perform OCR on supported documents and images, including:

• PDFs

• Scanned documents

• Images

• Court orders

• Medical records

• School documents

• Receipts

• Letters


### 4.11.2 OCR Workflow

For eligible evidence, the system shall:

1. Queue OCR processing.

2. Extract machine-readable text.

3. Store OCR output separately from the original file.

4. Store available confidence information.

5. Index OCR text for search.

6. Make OCR text available to authorized AI operations.


### 4.11.3 OCR Integrity

OCR text shall be treated as machine-generated derived content. It shall not replace or alter the original evidence. Corrections, if supported, shall be stored separately rather than rewriting the OCR result without history.


## 4.12 AI Analysis


### 4.12.1 AI Capabilities

AI analysis may:

• Generate summaries.

• Extract important dates.

• Identify names.

• Identify locations.

• Identify organizations.

• Suggest categories.

• Suggest tags.

• Identify possible related journal entries.

• Identify possible related calendar events.

• Highlight significant events.

• Detect missing metadata.

• Suggest timeline events.

• Assist duplicate detection.


### 4.12.2 AI Provenance

AI-generated content shall be clearly distinguishable from user-authored or source-derived evidence. AI output shall never change the original file.


### 4.12.3 User Approval

AI suggestions that create or modify user-managed metadata or relationships shall require user approval unless the user has explicitly configured an approved automatic workflow in a future release.


## 4.13 Evidence Linking


### 4.13.1 Supported Relationships

Evidence shall be linkable to:

• Parenting Journal entries

• Calendar events

• Timeline events

• Reports

• Contacts

• Parenting Time records

• Expenses

• Communications

• Court cases

• Medical records

• School events


### 4.13.2 Relationship Rules

One evidence item may be linked to multiple records simultaneously. Related modules shall reference the Evidence Record rather than duplicate the underlying file.

Removing a relationship shall not delete the evidence. Deleting evidence shall not silently delete related application records.


## 4.14 Collections, Folders, Favorites, and Archive


### 4.14.1 Folders

Users may organize evidence in folders where folder functionality is enabled. Folder placement shall not change the canonical Evidence ID or file identity.


### 4.14.2 Collections

Collections shall provide virtual groupings of evidence. An evidence item may belong to multiple collections without creating additional copies.

Users shall be able to create, rename, archive, and delete collections. Deleting a collection shall not delete the evidence it references.


### 4.14.3 Favorites

Users shall be able to mark evidence as a Favorite. Favorites are organizational metadata and shall not affect report inclusion, chain of custody, or file integrity.


### 4.14.4 Archive

Archived evidence shall remain stored and retain metadata, links, versions, audit history, and chain-of-custody records. Archived evidence shall be hidden from default active views but available when archive filters are enabled.


## 4.15 Version Management


### 4.15.1 Version Creation

Users shall be able to replace a document by creating a new version rather than overwriting the existing file.


### 4.15.2 Version Metadata

Each version shall maintain:

• Version number

• Upload date

• Uploaded by

• File hash

• File size

• Processing status

• Change notes


### 4.15.3 Version Preservation

Previous versions shall remain recoverable according to retention policy. Creating a new version shall not destroy the original version or historical audit trail.


### 4.15.4 Reprocessing

A new version shall trigger relevant processing such as preview generation, OCR, AI analysis, duplicate checks, and search indexing.


## 4.16 Preview


### 4.16.1 General Preview

Users shall be able to preview supported evidence without downloading the original file.


### 4.16.2 Image Preview

Image preview should support:

• Zoom

• Rotation

• Full-screen viewing

• Thumbnail display

• Metadata display


### 4.16.3 PDF Preview

PDF preview should support:

• Page navigation

• Zoom

• Thumbnail navigation

• Search within OCR/text content when available


### 4.16.4 Video Preview

Video preview should support:

• Play/pause

• Seek

• Volume

• Full screen

• Duration


### 4.16.5 Audio Preview

Audio preview should support:

• Play/pause

• Seek

• Volume

• Transcript display when available


### 4.16.6 OCR and AI Preview

Users shall be able to view OCR text and AI summaries alongside evidence. Machine-generated content shall be visually identified as such.


## 4.17 Search and Filtering


### 4.17.1 Searchable Content

Evidence Search shall index:

• Title

• Description

• Original filename

• Categories

• Tags

• Child

• Contact

• Event date

• Upload date

• Source

• Location

• OCR text

• AI summary

• AI keywords

• Speech transcript

• Relevant relationship metadata


### 4.17.2 Filters

Users shall be able to filter evidence by:

• Child

• Category

• Tags

• Date range

• File type

• Contact

• Source

• Author

• Location

• Importance

• Collection

• Favorite status

• Archive status

• Processing status

• OCR availability

• AI processing status

• Linked record type

Multiple filters shall be combinable.


### 4.17.3 Natural Language Search

The AI Assistant shall support natural-language queries such as:

• Find every OFW message mentioning school.

• Show every photo taken during parenting exchanges.

• Find every document related to therapy.

• Show all evidence from July 2026.

• Find every receipt for medical expenses.

AI search results shall remain subject to normal authorization rules.


## 4.18 Report Integration


### 4.18.1 Evidence Selection

The Report Builder shall allow users to select Evidence Records without duplicating the source evidence.


### 4.18.2 Exhibit Functions

Report Builder integration should support:

• Evidence references

• Exhibit numbering

• Exhibit ordering

• Thumbnails

• OCR excerpts

• AI summaries

• Supporting-document exports


### 4.18.3 Source Integrity

Report output may include copies or renderings for export, but the authoritative Evidence Record and original file shall remain unchanged.


## 4.19 Timeline Integration

Evidence may create or support Timeline events. Examples include:

• Court order added

• Parenting plan added

• Medical record added

• Receipt uploaded

• Photograph captured

• OFW export imported

Timeline records shall reference the Evidence Record rather than duplicate the file.


## 4.20 Security and Permissions


### 4.20.1 Encryption

Evidence shall be encrypted:

• In transit

• At rest

• In backups where applicable


### 4.20.2 Authorization

The application shall enforce authorization for:

• View

• Preview

• Upload

• Download

• Replace

• Archive

• Restore

• Delete

• Export

• Share

• Link

• Modify metadata


### 4.20.3 Secure Downloads

Downloads shall require an authenticated and authorized session. Temporary download URLs, when used, shall expire.


### 4.20.4 Sensitive Data

Evidence may contain medical, legal, financial, educational, and child-related information. Search indexes, AI retrieval, previews, exports, and backups shall follow the same authorization model as the source evidence.


## 4.21 Audit History


### 4.21.1 Auditable Events

The system shall record significant evidence actions including:

• Upload

• Preview

• Download

• Replace

• Archive

• Restore

• Delete

• Export

• Share

• Metadata change

• Category change

• Tag change

• Evidence link

• Evidence unlink

• Permission change

• OCR completion

• AI analysis completion


### 4.21.2 Audit Record Fields

Audit events should include:

• Audit ID

• Evidence ID

• Timestamp

• User

• Action

• Result

• Device when available

• IP address when available

• Previous values when applicable

• New values when applicable

Audit history shall not be editable by end users.


## 4.22 Chain of Custody


### 4.22.1 Purpose

The system shall maintain a chain-of-custody history sufficient to show the major lifecycle events for evidence and to help establish that the original stored file has remained unchanged.


### 4.22.2 Integrity Verification

Each original version shall maintain a cryptographic file hash. The system shall be able to compare stored and recalculated hashes to detect unexpected file corruption or alteration.


### 4.22.3 Chain Events

Chain-of-custody records may include:

• Initial upload/import

• Hash generation

• Storage

• Version creation

• Preview

• Download

• Export

• Sharing

• Archive

• Restore

• Deletion

• Permission change

• Processing events


### 4.22.4 Immutable History

Historical chain-of-custody and audit events shall not be rewritten. Corrections, when necessary, shall be represented as new events.


## 4.23 Deletion and Recovery

Evidence deletion shall use soft deletion during the configured recovery period.

Deleted evidence shall:

• Be excluded from normal browsing.

• Be excluded from normal search.

• Retain audit history.

• Retain version history.

• Remain recoverable during the configured retention period.

Restoration shall restore eligible relationships and visibility.

Permanent deletion shall follow application retention and legal-hold policies.


## 4.24 Backup and Recovery

Backups shall preserve:

• Original files

• Evidence metadata

• Versions

• OCR

• AI metadata

• Relationships

• Collections

• Audit history

• Chain of custody

Recovery shall preserve Evidence IDs and version identifiers where technically feasible.


## 4.25 Performance and Scalability

The module shall remain usable with large historical repositories.

The implementation should support:

• Asynchronous processing

• Background OCR and AI jobs

• Concurrent uploads

• Paginated evidence lists

• Date-range and metadata indexing

• Search indexes

• Lazy-loaded previews

• Cached thumbnails

• Independently scalable storage and processing components

The application shall not require downloading all evidence metadata or file contents to the client to perform ordinary search or browsing.


## 4.26 Error Handling

The system shall provide user-friendly errors while preserving source evidence whenever possible.

Examples:

• Unsupported file type: reject before processing.

• File too large: reject with plan/configuration guidance.

• Malware detected: quarantine and prevent access.

• Network interruption: allow retry or resumable upload where supported.

• OCR failure: retain original evidence and allow retry.

• AI failure: retain original evidence and allow retry.

• Preview failure: allow original download if authorized and otherwise safe.

• Relationship failure: do not create a partial relationship.

• Search indexing failure: retain evidence and queue reprocessing.

Errors shall not expose internal stack traces, credentials, secrets, or infrastructure details.


## 4.27 Suggested API Surface


### 4.27.1 Evidence Operations

Conceptual operations include:

• POST /evidence

• GET /evidence

• GET /evidence/{id}

• PUT /evidence/{id}

• DELETE /evidence/{id}

• POST /evidence/{id}/archive

• POST /evidence/{id}/restore


### 4.27.2 Upload and Version Operations

Conceptual operations include:

• POST /evidence/upload

• POST /evidence/bulk-upload

• POST /evidence/zip-import

• POST /evidence/{id}/versions

• GET /evidence/{id}/versions

• GET /evidence/{id}/versions/{versionId}


### 4.27.3 Processing Operations

Conceptual operations include:

• POST /evidence/{id}/ocr

• GET /evidence/{id}/ocr

• POST /evidence/{id}/analyze

• GET /evidence/{id}/analysis

• POST /evidence/{id}/reprocess


### 4.27.4 Relationship Operations

Conceptual operations include:

• POST /evidence/{id}/relationships

• DELETE /evidence/{id}/relationships/{relationshipId}

• GET /evidence/{id}/relationships


### 4.27.5 Search and Preview Operations

Conceptual operations include:

• GET /evidence/search

• GET /evidence/{id}/preview

• GET /evidence/{id}/thumbnail


### 4.27.6 Audit Operations

Conceptual operations include:

• GET /evidence/{id}/audit

• GET /evidence/{id}/chain-of-custody

These routes are logical interfaces and do not require a REST architecture.


## 4.28 Proposed Domain Relationships

The domain should conceptually support:

Evidence

• Owner / Workspace

• Original File

• Versions

• Metadata

• Children

• Categories

• Tags

• Contacts

• Collections

• OCR Result

• AI Analysis

• Preview

• Journal Entries

• Calendar Events

• Timeline Events

• Reports

• Parenting Time

• Expenses

• Communications

• Audit Events

• Chain-of-Custody Events

• Permissions

Canonical records from other modules shall be referenced rather than duplicated.


## 4.29 MVP Scope

The initial Evidence Management MVP should include:

• File upload

• Drag and drop

• Bulk upload

• Common image/document/audio/video formats

• Original-file preservation

• Malware scanning

• File hashing

• Metadata editing

• Default/custom categories

• Tags

• Child association

• Evidence relationships

• OCR for supported documents

• AI summary

• AI category/tag suggestions

• Keyword/OCR search

• Filtering

• Image/PDF/audio/video preview where supported

• Collections

• Favorites

• Archive

• Version management

• Journal integration

• Calendar integration

• Timeline integration

• Report Builder integration

• Encryption

• Audit history

• Chain-of-custody history

• Soft delete and restore


## 4.30 Future Scope

Future versions may include:

• Gmail integration

• Outlook integration

• Google Drive import

• OneDrive import

• Dropbox import

• OFW direct synchronization

• Face recognition

• Object recognition

• Handwriting recognition

• Automated exhibit books

• Automated chronological evidence binders

• Digital signatures

• Watermarking

• Legal hold

• Immutable evidence locking

• Secure attorney portals

• Therapist access

• Parenting coach access

• Guardian ad Litem access

• Temporary sharing links


## 4.31 Acceptance Criteria

The Evidence Management module shall be considered functionally complete when:

1. Authenticated users can upload supported evidence.

2. Files are validated before becoming available.

3. Original evidence remains unchanged.

4. Evidence receives a unique immutable identifier.

5. File hashes are generated for original versions.

6. Users can view and edit permitted metadata.

7. Users can assign categories and tags.

8. Users can associate evidence with one or more children.

9. OCR is performed on supported files.

10. OCR text becomes searchable.

11. AI can generate supported summaries and suggestions.

12. AI-generated content is distinguishable from source evidence.

13. Users can link evidence to supported application records.

14. Removing a relationship does not delete the evidence.

15. Users can preview supported evidence without downloading it.

16. Users can search evidence by metadata and content.

17. Users can combine supported filters.

18. Users can organize evidence into collections and favorites.

19. Users can archive and restore evidence.

20. Users can create new document versions without overwriting old versions.

21. Previous versions remain recoverable according to policy.

22. Evidence can be incorporated into reports.

23. Evidence can support Timeline events.

24. Authorization is enforced for all evidence operations.

25. Evidence is encrypted in transit and at rest.

26. Significant evidence operations are audited.

27. Chain-of-custody records are preserved.

28. Evidence can be soft-deleted and restored during the configured retention period.

29. Backup and recovery preserve evidence integrity and relationships.

30. Search and browsing remain responsive as evidence volume grows.


## 4.32 Product Decisions Still Required

Before implementation is locked, Product shall define:

• Maximum file size by subscription tier.

• Total storage quotas.

• Allowed MIME types and extensions.

• Maximum concurrent uploads.

• ZIP extraction and archive limits.

• Malware scanning provider and quarantine policy.

• OCR provider and supported languages.

• OCR confidence and retry policy.

• AI provider and model selection.

• AI usage limits and cost controls.

• Whether AI analysis is automatic or user-triggered by plan.

• Duplicate similarity thresholds.

• Search ranking rules.

• Whether search history and saved searches are included.

• Tag normalization and case-sensitivity rules.

• Category deletion and merge behavior.

• Maximum number of relationships per evidence item.

• Maximum versions retained.

• Version retention policy.

• Soft-delete retention period.

• Permanent deletion rules.

• Legal-hold behavior.

• Exact chain-of-custody events exposed to users.

• Whether preview activity should always be audited.

• External sharing permissions.

• Watermarking and download restrictions.

• Whether archived evidence appears in AI search by default.

• Which evidence uploads automatically create Timeline events.

• Whether external URLs are snapshotted or stored only as references.


# 5. Communication Management

Document Version: 1.0

Module: Communication Management

Status: Draft

Purpose: Engineering / Product / QA specification


## 5.1 Purpose

The Communication Management module provides a centralized system for storing, organizing, searching, reviewing, and linking parenting-related communications. It is intended to capture communication records that may originate from email, OurFamilyWizard (OFW), text-message exports, call logs, voicemail recordings, screenshots, and other supported communication sources represented by the supplied requirements.

The module shall preserve the original imported or uploaded communication artifact while generating searchable metadata, OCR text, speech-to-text transcripts, AI summaries, and relationships to other application records. Communication records shall integrate with the Parenting Journal, Calendar, Timeline, Evidence Management, Reports, Contacts, Parenting Time, Expenses, and AI Assistant.


### 5.1.1 Source Alignment

The source material supplied for this module primarily described Evidence Management. This functional specification therefore focuses on the communication-related capabilities explicitly represented in that source, including OFW message exports, text-message exports, email files, call logs, voicemail recordings, social-media screenshots, OCR, transcription, AI analysis, search, linking, reporting, version history, security, and audit history. Communication-specific behaviors not supported by the supplied requirements are identified as product decisions rather than silently assumed.


## 5.2 Functional Scope

The Communication Management module shall support:

• Importing and storing supported communication records.

• Email-file ingestion where supported.

• OFW message export ingestion.

• Text-message export ingestion.

• Call-log ingestion.

• Voicemail and audio-recording ingestion.

• Screenshot and image-based communication capture.

• Communication metadata.

• Categories and tags.

• Child and contact associations.

• OCR and speech-to-text processing.

• AI summaries and metadata suggestions.

• Duplicate detection.

• Linking to records across the application.

• Search and filtering.

• Preview and playback of supported communication artifacts.

• Report and Timeline integration.

• Version history where a communication artifact is replaced.

• Encryption, authorization, audit history, and chain-of-custody support.

• Soft deletion and recovery.


## 5.3 Communication Record Model

Every imported or uploaded communication item shall become a Communication Record with a unique immutable Communication ID.


### 5.3.1 Core Fields

Each Communication Record shall support:

• Communication ID

• Title or Subject

• Description or Notes

• Communication Type

• Source

• Communication Date and Time

• Import or Upload Date and Time

• Sender when available

• Recipient or Recipients when available

• Related Contacts

• Child or Children

• Category

• Tags

• Importance

• Location when relevant

• Original File Name when applicable

• File Type

• File Size

• Created By

• Modified By

• Status

• Current Version when versioning applies

• Linked Journal Entries

• Linked Calendar Events

• Linked Timeline Events

• Linked Reports

• Linked Parenting Time Records

• Linked Expenses when relevant


### 5.3.2 System-Generated Metadata

The system may generate:

• OCR text

• OCR confidence

• Speech-to-text transcript

• AI summary

• AI keywords

• AI categories

• AI suggested tags

• Detected names

• Detected dates

• Detected locations

• Detected organizations

• Duplicate score

• File hash

• Processing timestamps

• Version number


### 5.3.3 Communication Status

Supported statuses shall include:

• Processing

• Available

• Archived

• Deleted

• Quarantined when applicable

• Failed Processing

Status shall determine whether the Communication Record appears in normal browsing, search, reports, exports, Timeline results, and AI retrieval.


## 5.4 Supported Communication Types


### 5.4.1 Imported Message Types

The system shall support communication artifacts represented in the source requirements, including:

• OFW message exports

• Text-message exports

• Email files in EML format

• Email files in MSG format

• Call logs

• Voicemail recordings

• Social-media screenshots

• Communication screenshots

• Audio recordings

• External URLs that reference communication content


### 5.4.2 Supported File Formats

Communication records may be represented by supported files such as:

• PDF

• JPG

• JPEG

• PNG

• HEIC

• GIF

• MP3

• WAV

• M4A

• EML

• MSG

• TXT

• ZIP archives containing supported records

Additional file types may be added according to Evidence Management validation rules.


## 5.5 Import and Capture Methods


### 5.5.1 Supported Methods

Users shall be able to add communication records through:

• File upload

• Drag and drop

• Bulk upload

• ZIP import

• Email-file import

• Mobile camera capture for screenshots or documents

• Mobile photo-library upload

• Audio upload

• Document scanner where appropriate

Future direct integrations may include Gmail, Outlook, and OFW synchronization.


### 5.5.2 Import Workflow

The import workflow shall:

1. Accept one or more supported files or records.

2. Validate file extension, MIME type, and size where applicable.

3. Scan uploaded files for malware or prohibited content.

4. Generate a cryptographic file hash when a source file exists.

5. Check for exact or probable duplicates.

6. Preserve the original uploaded or imported artifact.

7. Extract available technical and communication metadata.

8. Generate previews or thumbnails where supported.

9. Queue OCR, speech transcription, and AI analysis as applicable.

10. Index searchable content.

11. Mark the Communication Record Available when required processing is complete.

Long-running processing shall occur asynchronously and shall not prevent the user from continuing to use the application.


## 5.6 Validation and Duplicate Detection


### 5.6.1 File Validation

The system shall:

• Compare file extensions with detected MIME types.

• Reject unsupported file types.

• Reject blocked executables and scripts.

• Enforce configured file-size limits.

• Provide user-friendly validation errors.


### 5.6.2 Duplicate Detection

Where a source file exists, the system shall generate a SHA-256 hash. Exact hash matches shall be flagged as exact duplicates. Similarity-based duplicate detection may additionally compare:

• Sender and recipient

• Communication date and time

• Subject or title

• OCR text

• Message content

• Transcript content

• File metadata

• AI summaries

The system shall not silently merge or discard communication records. The user shall be given an appropriate review option.


## 5.7 Metadata Management


### 5.7.1 User-Managed Metadata

Users shall be able to manage:

• Title or Subject

• Description or Notes

• Communication Type

• Category

• Tags

• Importance

• Child associations

• Contact associations

• Event date and time when manually corrected or supplemented

• Source

• Location when relevant


### 5.7.2 Extracted Metadata

The system may extract:

• Original file name

• File size

• File type

• Created or modified dates when available

• Sender

• Recipient or recipients

• Subject line

• Message date and time

• Phone number or contact details when represented in the source

• Audio duration

• Page count

• Image dimensions


### 5.7.3 Metadata History

Changes to user-managed metadata shall be recorded in the audit history, including previous and new values where appropriate.


## 5.8 Categories and Tags


### 5.8.1 Categories

The system shall support default categories such as:

• Communication

• Parenting Time

• Parenting Exchange

• School

• Medical

• Therapy

• Legal

• Financial

• Court

• Parenting Coach

• Guardian ad Litem

• Attorney

• Safety

• General

Users shall be able to create custom categories. Category changes shall not alter the original communication artifact.


### 5.8.2 Tags

Users shall be able to assign multiple tags to a Communication Record. Example tags include:

• Exchange

• School

• Medical

• Therapy

• Holiday

• Court

• Communication

• Conflict

• Positive

• Birthday

• Travel

• Emergency

• Missed Visit

Tags shall be searchable and usable in reports. AI-suggested tags shall require user approval before becoming user-managed tags.


## 5.9 OCR and Speech-to-Text


### 5.9.1 OCR

The system shall perform OCR on supported communication images and documents, including screenshots, scanned PDFs, and image-based exports. OCR output shall be stored separately from the original artifact and indexed for search.


### 5.9.2 Speech-to-Text

The system may generate transcripts for supported voicemail and audio recordings. Transcripts shall be treated as machine-generated derived content and shall not replace or alter the original audio.


### 5.9.3 Processing Integrity

OCR text and transcripts shall be visually identified as machine-generated. Corrections, if supported, shall be stored as separate user-managed information rather than overwriting generated content without history.


## 5.10 AI Analysis


### 5.10.1 AI Capabilities

AI analysis may:

• Generate communication summaries.

• Extract important dates.

• Identify names.

• Identify locations.

• Identify organizations.

• Suggest categories.

• Suggest tags.

• Identify possible related Journal entries.

• Identify possible related Calendar events.

• Identify possible related Parenting Time records.

• Highlight significant events.

• Detect missing metadata.

• Suggest Timeline events.

• Assist duplicate detection.

• Support natural-language search.


### 5.10.2 AI Provenance

AI-generated content shall be clearly distinguishable from original communication content, OCR text, transcripts, and user-authored notes. AI output shall never modify the original communication artifact.


### 5.10.3 User Approval

AI suggestions that modify user-managed metadata or create relationships shall require user approval unless an approved automatic workflow is explicitly configured in a future release.


## 5.11 Linking and Relationships


### 5.11.1 Supported Relationships

Communication Records shall be linkable to:

• Parenting Journal entries

• Calendar events

• Timeline events

• Reports

• Contacts

• Parenting Time records

• Expenses

• Evidence records

• Court-related records

• Medical records

• School events


### 5.11.2 Relationship Rules

One Communication Record may be linked to multiple records simultaneously. Related modules shall reference the Communication Record or its preserved source artifact rather than create unnecessary duplicate records.

Removing a relationship shall not delete the Communication Record. Deleting a Communication Record shall not silently delete related application records.


## 5.12 Search and Filtering


### 5.12.1 Searchable Content

Communication Search shall index:

• Title or Subject

• Description or Notes

• Sender

• Recipient or Recipients

• Communication Type

• Source

• Child

• Contacts

• Category

• Tags

• Communication Date

• Import Date

• OCR text

• Speech transcript

• AI summary

• AI keywords

• Relevant relationship metadata


### 5.12.2 Filters

Users shall be able to filter communications by:

• Child

• Contact

• Communication Type

• Source

• Category

• Tags

• Date range

• Author

• Importance

• Status

• OCR availability

• Transcript availability

• AI processing status

• Linked record type

Multiple filters shall be combinable.


### 5.12.3 Natural-Language Search

The AI Assistant shall support natural-language queries such as:

• Find every OFW message mentioning school.

• Show communications related to therapy.

• Find text-message records from July 2026.

• Show communications linked to parenting exchanges.

• Find voicemail records connected to a court event.

AI search results shall remain subject to normal authorization rules.


## 5.13 Preview and Playback


### 5.13.1 Message and Document Preview

Users shall be able to preview supported email files, PDFs, screenshots, and message exports without downloading the original artifact where technically supported.


### 5.13.2 Audio Playback

Voicemail and audio communication records should support:

• Play and pause

• Seek

• Volume control

• Duration display

• Transcript display when available


### 5.13.3 OCR and AI Preview

Users shall be able to view OCR text, transcripts, and AI summaries alongside the source communication. Machine-generated content shall be visually identified as such.


## 5.14 Timeline Integration

Communication Records may create or support Timeline events. Examples include:

• OFW message imported

• Text-message exchange imported

• Email communication added

• Voicemail added

• Call log added

• Communication screenshot added

Timeline events shall link directly to the associated Communication Record or preserved source artifact.


## 5.15 Report Integration


### 5.15.1 Report Selection

The Report Builder shall allow users to include Communication Records in reports without modifying or replacing the original source artifact.


### 5.15.2 Report Functions

Report integration should support:

• Communication references

• Date and time

• Sender and recipient where available

• Source

• Selected message content where supported

• OCR excerpts

• Transcript excerpts

• AI summaries

• Related Journal or Timeline references

• Supporting-document exports


## 5.16 Version Management


### 5.16.1 Version Creation

Where a communication artifact is replaced or re-imported as a new version, the system shall preserve prior versions rather than overwrite the previous source file.


### 5.16.2 Version Metadata

Each version shall maintain:

• Version number

• Upload or import date

• Uploaded or imported by

• File hash when applicable

• File size

• Processing status

• Change notes when supplied


### 5.16.3 Version Preservation

Previous versions shall remain recoverable according to retention policy. Creating a new version shall not destroy the original version or its audit trail.


## 5.17 Security and Permissions


### 5.17.1 Encryption

Communication data shall be encrypted:

• In transit

• At rest

• In backups where applicable


### 5.17.2 Authorization

The application shall enforce authorization for:

• View

• Preview

• Upload or import

• Download

• Replace

• Archive

• Restore

• Delete

• Export

• Share

• Link

• Modify metadata


### 5.17.3 Sensitive Data

Communication records may contain legal, medical, financial, educational, and child-related information. Search indexes, AI retrieval, previews, exports, and backups shall follow the same authorization model as the source communication record.


## 5.18 Audit History and Chain of Custody


### 5.18.1 Auditable Events

The system shall record significant communication actions including:

• Upload or import

• Preview

• Download

• Replace

• Archive

• Restore

• Delete

• Export

• Share

• Metadata change

• Category change

• Tag change

• Link

• Unlink

• Permission change

• OCR completion

• Speech-to-text completion

• AI analysis completion


### 5.18.2 Audit Record Fields

Audit events should include:

• Audit ID

• Communication ID

• Timestamp

• User

• Action

• Result

• Device when available

• IP address when available

• Previous values when applicable

• New values when applicable

Audit history shall not be editable by end users.


### 5.18.3 Chain-of-Custody Support

Where the Communication Record is backed by an uploaded source file, the system shall maintain the file hash and lifecycle history necessary to demonstrate that the stored source artifact has not been silently altered. Historical audit and chain-of-custody events shall not be rewritten.


## 5.19 Deletion and Recovery

Communication deletion shall use soft deletion during the configured recovery period.

Deleted Communication Records shall:

• Be excluded from normal browsing.

• Be excluded from normal search.

• Retain audit history.

• Retain version history where applicable.

• Remain recoverable during the configured retention period.

Restoration shall restore eligible relationships and visibility. Permanent deletion shall follow application retention and legal-hold policies.


## 5.20 Backup and Recovery

Backups shall preserve, where applicable:

• Original communication artifacts

• Communication metadata

• Versions

• OCR text

• Speech transcripts

• AI metadata

• Relationships

• Audit history

• Chain-of-custody information

Recovery shall preserve Communication IDs and version identifiers where technically feasible.


## 5.21 Performance and Scalability

The module shall remain usable with large historical communication repositories. The implementation should support:

• Asynchronous processing

• Background OCR, transcription, and AI jobs

• Bulk imports

• Paginated communication lists

• Date-range and metadata indexing

• Search indexes

• Lazy-loaded previews

• Cached thumbnails where applicable

• Independently scalable storage and processing components

The application shall not require downloading all communication content to the client to perform ordinary search or browsing.


## 5.22 Error Handling

The system shall provide user-friendly errors while preserving source communication artifacts whenever possible.

Examples include:

• Unsupported file type: reject before processing.

• File too large: reject with configuration guidance.

• Malware detected: quarantine and prevent access.

• Network interruption: allow retry where supported.

• OCR failure: retain original artifact and allow retry.

• Transcription failure: retain original audio and allow retry.

• AI failure: retain original communication and allow retry.

• Preview failure: allow original download if authorized and otherwise safe.

• Relationship failure: do not create a partial relationship.

• Search-indexing failure: retain the Communication Record and queue reprocessing.

Errors shall not expose internal stack traces, credentials, secrets, or infrastructure details.


## 5.23 Suggested API Surface


### 5.23.1 Communication Operations

Conceptual operations include:

• POST /communications

• GET /communications

• GET /communications/{id}

• PUT /communications/{id}

• DELETE /communications/{id}

• POST /communications/{id}/archive

• POST /communications/{id}/restore


### 5.23.2 Import Operations

Conceptual operations include:

• POST /communications/import

• POST /communications/bulk-import

• POST /communications/zip-import

• POST /communications/email-import


### 5.23.3 Processing Operations

Conceptual operations include:

• POST /communications/{id}/ocr

• POST /communications/{id}/transcribe

• POST /communications/{id}/analyze

• POST /communications/{id}/reprocess

• GET /communications/{id}/analysis


### 5.23.4 Relationship and Search Operations

Conceptual operations include:

• POST /communications/{id}/relationships

• DELETE /communications/{id}/relationships/{relationshipId}

• GET /communications/{id}/relationships

• GET /communications/search

• GET /communications/{id}/preview

• GET /communications/{id}/audit

These routes are logical interfaces and do not require a REST architecture.


## 5.24 MVP Scope

The initial Communication Management MVP should include:

• OFW export import

• Text-message export import

• EML and MSG import where technically supported

• Call-log import

• Voicemail and audio upload

• Screenshot upload

• Original-artifact preservation

• File validation and malware scanning

• Metadata editing

• Categories and tags

• Child and contact associations

• OCR for supported documents and images

• Speech-to-text for supported audio

• AI summaries

• AI category and tag suggestions

• Keyword, OCR, and transcript search

• Filtering

• Preview and audio playback where supported

• Journal integration

• Calendar integration

• Timeline integration

• Report Builder integration

• Evidence Management integration

• Encryption

• Audit history

• Soft delete and restore


## 5.25 Future Scope

Future versions may include:

• Direct Gmail integration

• Direct Outlook integration

• Direct OFW synchronization

• Automatic cloud imports

• Scheduled synchronization

• Secure professional sharing

• Automated communication grouping

• Enhanced duplicate detection

• Automated chronological communication binders

• Watermarking

• Legal hold

• Immutable source locking


## 5.26 Acceptance Criteria

The Communication Management module shall be considered functionally complete when:

1. Authenticated users can import or upload supported communication records.

2. Supported files are validated before becoming available.

3. Original communication artifacts remain unchanged.

4. Communication Records receive unique immutable identifiers.

5. Users can view and edit permitted metadata.

6. Users can assign categories and tags.

7. Users can associate communications with children and contacts.

8. OCR is performed on supported communication documents and screenshots.

9. Speech-to-text is performed on supported audio where enabled.

10. OCR text and transcripts become searchable.

11. AI can generate supported summaries and suggestions.

12. AI-generated content is distinguishable from source communications.

13. Users can link communications to supported application records.

14. Removing a relationship does not delete the Communication Record.

15. Users can preview or play supported communication artifacts.

16. Users can search communications by metadata and content.

17. Users can combine supported filters.

18. Communication Records can be incorporated into reports.

19. Communication Records can support Timeline events.

20. Authorization is enforced for all communication operations.

21. Communication data is encrypted in transit and at rest.

22. Significant communication operations are audited.

23. Communication Records can be soft-deleted and restored during the configured retention period.

24. Backup and recovery preserve communication integrity and relationships.

25. Search and browsing remain responsive as communication volume grows.


## 5.27 Product Decisions Still Required

Before implementation is locked, Product shall define:

• Exact communication types supported in MVP.

• Whether the module stores individual messages or only imported communication artifacts.

• Whether conversation threads are separate entities.

• How sender and recipient identity is normalized to Contacts.

• OFW export formats that will be officially supported.

• Text-message export formats that will be officially supported.

• Whether Gmail and Outlook are MVP or future-only integrations.

• Maximum file size and total storage limits.

• Duplicate similarity thresholds.

• OCR provider and supported languages.

• Speech-to-text provider and supported languages.

• AI provider, model selection, and usage limits.

• Whether AI analysis is automatic or user-triggered.

• Whether call logs are informational records or treated as evidence artifacts.

• Whether communication screenshots are indexed as Communications, Evidence, or both.

• Whether archived communications appear in AI search by default.

• Which communication imports automatically create Timeline events.

• Soft-delete retention period.

• Permanent deletion rules.

• Legal-hold behavior.

• External sharing permissions.
• Whether external URLs are snapshotted or stored only as references.

• Whether preview activity is always audited


# 6. Parenting Time


## 6.1 Purpose


### 6.1.1 Overview

The Parenting Time module serves as the authoritative system for planning, recording, validating, and reporting all parenting time within the ClearParent Log platform. It maintains a complete historical record of scheduled parenting time, actual parenting time, exchanges, cancellations, denials, make-up parenting time, and parenting plan compliance.

Unlike a traditional calendar, the Parenting Time module compares what was expected to occur under the parenting plan with what actually occurred. This comparison enables users to accurately document parenting plan compliance, identify missed or denied parenting time, calculate parenting statistics, and generate comprehensive reports for personal records, attorneys, parenting coaches, therapists, mediators, Guardian ad Litems, and court proceedings.

The Parenting Time module serves as the system of record for parenting schedule compliance while integrating with Calendar, Parenting Journal, Timeline, Evidence Management, Contacts, Notifications, AI Assistant, and Report Builder.


### 6.1.2 Objectives

The Parenting Time module shall:

• Track all scheduled parenting time.

• Record actual parenting time.

• Record parenting exchanges.

• Record schedule modifications.

• Track parenting plan compliance.

• Record missed parenting time.

• Record denied parenting time.

• Record cancelled parenting events.

• Track make-up parenting time.

• Automatically calculate parenting statistics.

• Generate parenting compliance reports.

• Link parenting events to journal entries.

• Link parenting events to supporting evidence.

• Provide historical parenting analytics.


## 6.2 Functional Scope

The Parenting Time module includes the following capabilities.


### Schedule Management

• Scheduled Parenting Time

• Holiday Parenting Time

• Vacation Parenting Time

• Summer Schedule

• School Breaks

• Temporary Schedule Changes

• Parenting Plan Phase Changes


### Parenting Time Recording

• Actual Parenting Time

• Parenting Exchanges

• Pickup Tracking

• Drop-off Tracking

• Arrival Tracking

• Departure Tracking


### Exception Tracking

• Missed Parenting Time

• Parenting Time Denials

• Cancellations

• Schedule Modifications

• Late Exchanges

• Early Pickups

• Late Returns

• Make-Up Parenting Time


### Reporting

• Parenting Statistics

• Compliance Reports

• Parenting Percentage

• Historical Reports

• AI Analysis


### Integration

The module shall integrate with:

• Calendar

• Parenting Journal

• Timeline

• Evidence Management

• Contacts

• Parenting Plan

• AI Assistant

• Notifications

• Dashboard

• Report Builder


## 6.3 Parenting Time Data Model

Every parenting event shall be represented by a Parenting Time Record.

A Parenting Time Record stores both scheduled information and actual information so compliance can be calculated automatically.


### 6.3.1 Parenting Time Record

Each Parenting Time Record shall contain a unique immutable identifier.

Example

PT-000001

PT-000002

PT-000003

The identifier shall never change regardless of updates.


### 6.3.2 Core Properties

Each Parenting Time Record shall contain:

• Parenting Time ID

• Event Type

• Status

• Child or Children

• Scheduled Start Date

• Scheduled Start Time

• Scheduled End Date

• Scheduled End Time

• Actual Start Date

• Actual Start Time

• Actual End Date

• Actual End Time

• Duration

• Parenting Plan Reference

• Exchange Location

• Notes

• Created Date

• Updated Date

• Created By

• Updated By


### 6.3.3 Relationships

A Parenting Time Record may be associated with:

• Parenting Journal Entries

• Calendar Events

• Timeline Events

• Evidence

• Reports

• Parenting Plan

• Contacts

• Notifications

• AI Insights

• Expenses


## 6.4 Parenting Schedule


### 6.4.1 Overview

The Parenting Schedule defines when parenting time is expected to occur under the parenting plan.

The schedule serves as the baseline against which actual parenting time is compared.

Schedule information should originate from the Parenting Plan whenever possible but may also support temporary modifications.


### 6.4.2 Supported Schedule Types

The system shall support:

• Regular Parenting Time

• Overnight Visits

• Holiday Parenting Time

• Vacation Parenting Time

• School Breaks

• Summer Parenting Schedule

• Parenting Plan Phase Changes

• Temporary Modifications

• Court Ordered Changes

• Supervised Parenting Time


### 6.4.3 Schedule Fields

Each scheduled parenting event shall contain:

• Child or Children

• Scheduled Start Date

• Scheduled Start Time

• Scheduled End Date

• Scheduled End Time

• Exchange Location

• Parenting Plan Reference

• Schedule Type

• Status

• Notes


## 6.5 Scheduled Parenting Events

Every scheduled parenting event shall progress through a lifecycle.

Scheduled

↓

Upcoming

↓

In Progress

↓

Completed

↓

Historical

Exceptions include:

Scheduled

↓

Cancelled

↓

Historical

or

Scheduled

↓

Missed

↓

Historical


### 6.5.1 Schedule Validation

Before a parenting event is saved, the system shall validate:

• Start Date

• End Date

• Start Time

• End Time

• Child Selection

• Parenting Plan Reference

• Schedule Conflicts

• Required Fields

Invalid schedules shall not be saved.


### 6.5.2 Parenting Plan Association

Each scheduled parenting event should reference the applicable Parenting Plan provision when available.

This association allows reports to identify which provision governed the scheduled parenting time and supports future compliance analysis.


## 6.6 Actual Parenting Time


### 6.6.1 Overview

The Parenting Time module shall record what actually occurred during a parenting event.

Actual parenting information is separate from scheduled information.

The system shall automatically compare both values.


### 6.6.2 Actual Parenting Fields

Each parenting event may contain:

• Actual Pickup Time

• Actual Drop-off Time

• Actual Start Time

• Actual End Time

• Actual Duration

• Parent Present

• Children Present

• Exchange Participants

• Notes

• Journal Entry

• Linked Evidence

• Supporting Communications


### 6.6.3 Automatic Comparison

The application shall compare:

• Scheduled Start

• Actual Start

• Scheduled End

• Actual End

• Scheduled Duration

• Actual Duration

The resulting comparison shall determine:

• On Time

• Late Pickup

• Late Drop-off

• Missed Parenting Time

• Denied Parenting Time

• Partial Parenting Time

• Make-Up Required

The comparison engine shall be reusable by reporting, AI analysis, and statistics calculations.


## 6.7 Parenting Time Lifecycle

Every Parenting Time Record shall move through a defined lifecycle.

Scheduled

↓

Upcoming

↓

Exchange

↓

Active Parenting Time

↓

Completed

↓

Historical

Alternative outcomes include:

Scheduled

↓

Cancelled

Scheduled

↓

Denied

Scheduled

↓

Missed

Scheduled

↓

Modified

Each lifecycle transition shall generate an audit history record and update the Timeline where appropriate.


# 7. Expense Tracking


## 7.1 Purpose


### 7.1.1 Overview

The Expense Tracking module provides users with the ability to record, organize, and manage all parenting-related expenses. It serves as a centralized financial record for documenting child-related costs, tracking reimbursements, maintaining receipts, and generating reports for budgeting, reimbursement requests, legal proceedings, or tax purposes.

The module shall support detailed expense categorization, receipt management, reimbursement tracking, recurring expenses, search, statistics, AI-assisted processing, reporting, and integration with related ClearParent Log modules.


### 7.1.2 Objectives

The Expense Tracking module shall:

• Record all child-related expenses.

• Organize expenses by category and child.

• Track reimbursements and outstanding balances.

• Store receipts and supporting documentation.

• Generate financial reports.

• Link expenses to parenting events, journal entries, and evidence.

• Provide historical financial records for legal or personal use.


## 7.2 Functional Scope

The module shall support:

• Expense creation and editing.

• Expense categorization.

• Child associations.

• Vendor and payment-method tracking.

• Receipt and document attachments.

• Reimbursement tracking.

• Recurring expenses.

• Search and filtering.

• Financial statistics and dashboards.

• AI-assisted categorization and OCR.

• Duplicate expense detection.

• Reporting and export.

• Timeline, Journal, Calendar, Evidence, Contacts, Dashboard, and Report Builder integration.

• Security, authorization, audit history, deletion, and recovery.


## 7.3 Expense Data Model


### 7.3.1 Expense Record

Each expense shall be represented by a unique Expense Record with an immutable Expense ID.


### 7.3.2 Core Properties

Each Expense Record shall support:

• Expense ID

• Expense Title

• Description

• Expense Date

• Amount

• Currency

• Category

• Child or Children

• Vendor or Business

• Payment Method

• Paid By

• Responsible Party

• Notes

• Tax Deductible flag, where applicable

• Reimbursable flag

• Reimbursement Status

• Due Date, where applicable

• Created Date

• Modified Date

• Created By

• Modified By

• Status


### 7.3.3 Relationships

An Expense Record may be associated with:

• Parenting Journal entries

• Calendar events

• Timeline events

• Evidence records

• Contacts

• Parenting Time records

• Medical appointments

• School activities

• Reports

• Reimbursement records


## 7.4 Expense Categories


### 7.4.1 Standard Categories

The system shall provide standard categories including:

• Medical

• Dental

• Vision

• Mental Health / Therapy

• School

• Tuition

• School Supplies

• Extracurricular Activities

• Sports

• Music Lessons

• Camps

• Child Care

• Clothing

• Shoes

• Food

• Groceries

• Restaurants

• Travel

• Transportation

• Gas

• Airfare

• Hotels

• Entertainment

• Gifts

• Technology

• Miscellaneous


### 7.4.2 Custom Categories

Users shall be able to create custom categories. Custom categories shall be searchable and reportable and shall not modify historical expense amounts or reimbursement calculations.


## 7.5 Expense Records


### 7.5.1 Required Fields

At minimum, an expense shall require the fields designated by Product as mandatory. The required-field set is TBD and may include Expense Date, Amount, Currency, Category, and Paid By.


### 7.5.2 Validation Rules

The system shall validate:

• Amount is numeric and greater than or equal to zero unless adjustment records are explicitly supported.

• Currency is a supported currency.

• Expense Date is valid.

• Required associations are present.

• Reimbursement values do not exceed business-rule limits.

• Attachments comply with Evidence Management validation rules.


### 7.5.3 Editing

Users with permission shall be able to edit user-managed expense fields. Every material change shall be recorded in audit history, including prior and updated values where applicable.


## 7.6 Payment Methods


### 7.6.1 Supported Methods

Supported payment methods shall include:

• Cash

• Credit Card

• Debit Card

• Bank Transfer

• Check

• Digital Wallet

• Insurance

• Other


### 7.6.2 Payment Method Rules

Payment method values shall be used for search, filtering, statistics, and reporting. Sensitive payment credentials shall not be stored unless separately designed and approved.


## 7.7 Receipt Management


### 7.7.1 Supported Attachments

Users shall be able to attach supporting documentation including:

• Photos

• Receipts

• PDF Invoices

• Email Confirmations

• Bank Statements

• Insurance Explanations of Benefits

• Screenshots

• Other Documents


### 7.7.2 Mobile Receipt Capture

The mobile application should allow users to photograph or scan paper receipts and associate them with an Expense Record.


### 7.7.3 OCR Processing

Supported receipts and invoices may be processed with OCR. Extracted information may include:

• Vendor

• Date

• Amount

• Taxes

• Line-item descriptions

• Payment method indicators

OCR-generated values shall be presented as suggestions and shall not silently overwrite user-entered information.


### 7.7.4 Receipt Integrity

Original receipt files shall be preserved through Evidence Management and shall not be modified by OCR, preview generation, or AI processing.


## 7.8 Reimbursements


### 7.8.1 Reimbursement Record

Each reimbursement shall support:

• Amount Requested

• Amount Received

• Date Requested

• Date Received

• Responsible Party

• Payment Method

• Status

• Notes

• Supporting Documentation

• Related Expense or Expenses


### 7.8.2 Reimbursement Status

Supported statuses shall include:

• Not Requested

• Requested

• Pending

• Partially Paid

• Paid

• Overdue

• Waived


### 7.8.3 Outstanding Balance

The system shall calculate outstanding reimbursement balances from requested and received amounts.

Outstanding Balance = Amount Requested − Amount Received

The system shall prevent negative outstanding balances unless overpayments are explicitly supported.


### 7.8.4 Partial Payments

Partial reimbursements shall preserve the original amount requested and maintain a payment history rather than overwriting the reimbursement request.


### 7.8.5 Reimbursement Lifecycle

A reimbursement may progress through states such as:

Not Requested → Requested → Pending → Partially Paid → Paid

Alternative outcomes may include Overdue or Waived.


## 7.9 Recurring Expenses


### 7.9.1 Supported Recurrence

Users shall be able to configure recurring expenses with patterns including:

• Weekly

• Monthly

• Quarterly

• Annually

• Custom


### 7.9.2 Examples

Recurring expenses may include:

• Child Support

• Tuition

• Therapy

• Sports Fees

• Insurance Premiums

• Monthly Activities

• Music Lessons


### 7.9.3 Recurring Expense Generation

Recurring rules shall create scheduled expense instances according to the configured recurrence pattern. Editing a recurring series should support appropriate series/occurrence behavior, with final behavior defined by Product.


## 7.10 Search and Filtering


### 7.10.1 Searchable Content

Users shall be able to search expenses by:

• Child

• Date Range

• Category

• Vendor

• Amount

• Payment Method

• Reimbursement Status

• Tags

• Keywords


### 7.10.2 Combined Filters

Multiple filters shall be combinable. Search results shall respect authorization rules and shall not expose restricted financial records.


## 7.11 Statistics


### 7.11.1 Calculated Metrics

The system shall calculate statistics including:

• Total Expenses

• Monthly Expenses

• Annual Expenses

• Expenses by Child

• Expenses by Category

• Expenses by Vendor

• Reimbursements Requested

• Reimbursements Received

• Outstanding Reimbursements

• Average Monthly Expenses

• Highest Expense Categories

• Spending Trends


### 7.11.2 Dashboard Presentation

Statistics should be available through charts, graphs, and dashboard widgets. Calculations shall be based on the same canonical Expense and Reimbursement records used by reports.


## 7.12 AI Integration


### 7.12.1 AI Capabilities

The AI Assistant may assist with:

• Expense categorization

• Receipt OCR interpretation

• Duplicate expense detection

• Missing receipt detection

• Spending summaries

• Reimbursement calculations

• Natural-language questions


### 7.12.2 Natural-Language Examples

Supported questions may include:

• How much did I spend on school expenses this year?

• Show all unreimbursed medical expenses.

• How much have I spent on Vera's activities?

• Find every expense over $100.

• Generate a reimbursement report for July.


### 7.12.3 AI Provenance

AI-generated classifications, summaries, and recommendations shall be clearly identified as machine-generated and shall not modify source financial records without user approval.


## 7.13 Reports


### 7.13.1 Available Reports

The Expense Tracking module shall support reports including:

• Expense Summary

• Monthly Expense Report

• Annual Expense Report

• Child Expense Report

• Category Expense Report

• Medical Expense Report

• School Expense Report

• Activity Expense Report

• Outstanding Reimbursements

• Reimbursement History

• Receipt Report

• Tax Report, where applicable


### 7.13.2 Report Content

Reports may include:

• Expense totals

• Category totals

• Child totals

• Charts

• Reimbursement status

• Supporting receipts

• Linked evidence

• Date ranges

• Vendor information


### 7.13.3 Report Source Integrity

Reports shall reference canonical expense and evidence records rather than creating independent duplicate financial records.


## 7.14 Timeline Integration


### 7.14.1 Timeline Events

Expense-related Timeline events may include:

• Expense Created

• Expense Updated

• Reimbursement Requested

• Reimbursement Received

• Reimbursement Overdue

• Receipt Added

Timeline entries shall link back to the originating Expense or Reimbursement Record.


## 7.15 Module Integration


### 7.15.1 Parenting Journal

Expenses may be linked to relevant Journal entries.


### 7.15.2 Calendar

Expenses may be linked to Calendar events such as medical appointments, school activities, therapy sessions, and travel.


### 7.15.3 Evidence Management

Receipts and supporting financial documents shall be managed through Evidence Management and referenced by Expense Records.


### 7.15.4 Contacts

Vendors, providers, responsible parties, and other relevant people or organizations may be linked through Contacts.


### 7.15.5 Report Builder

Expenses and reimbursements shall be selectable for inclusion in custom reports.


### 7.15.6 Dashboard

Expense totals, reimbursement balances, and spending summaries may appear in Dashboard widgets.


## 7.16 Notifications

The module may generate notifications for:

• Reimbursement due dates

• Overdue reimbursements

• Missing receipts

• Recurring expense creation

• Expense-related reminders

Notification delivery shall be owned by the Notifications module.


## 7.17 Security


### 7.17.1 Encryption

Financial information shall be encrypted in transit and at rest.


### 7.17.2 Authorization

Authorization shall govern actions including:

• View

• Create

• Edit

• Delete

• Restore

• Export

• Attach Evidence

• Manage Reimbursements

• Generate Reports


### 7.17.3 Sensitive Financial Information

The module shall avoid storing full payment credentials unless explicitly required by a future approved design. Financial exports, AI retrieval, and search shall follow the same authorization rules as source Expense Records.


## 7.18 Audit History


### 7.18.1 Auditable Events

The system shall audit significant actions including:

• Expense creation

• Expense modification

• Expense deletion

• Expense restoration

• Receipt upload

• Receipt removal

• Reimbursement creation

• Reimbursement modification

• Reimbursement payment

• Export

• Report generation where required


### 7.18.2 Audit Data

Audit records should include:

• Audit ID

• Expense or Reimbursement ID

• Timestamp

• User

• Action

• Result

• Previous values where applicable

• New values where applicable

Audit records shall not be editable by end users.


## 7.19 Performance

The module shall support large historical financial datasets without requiring all records to be loaded client-side. Expense lists shall support pagination or equivalent incremental loading, and filtering/statistics should use indexed or aggregated data where appropriate.


## 7.20 Error Handling

The system shall provide clear user-facing errors for conditions including:

• Invalid amount

• Unsupported currency

• Missing required field

• Attachment processing failure

• Receipt OCR failure

• Duplicate expense warning

• Reimbursement calculation conflict

• Report-generation failure

• Unauthorized operation

Failed OCR or AI processing shall not cause loss of the underlying Expense Record or receipt.


## 7.21 Suggested API Surface


### 7.21.1 Expense Operations

Conceptual operations may include:

• POST /expenses

• GET /expenses

• GET /expenses/{id}

• PUT /expenses/{id}

• DELETE /expenses/{id}

• POST /expenses/{id}/restore


### 7.21.2 Reimbursement Operations

Conceptual operations may include:

• POST /expenses/{id}/reimbursements

• GET /expenses/{id}/reimbursements

• PUT /reimbursements/{id}

• POST /reimbursements/{id}/payments


### 7.21.3 Receipt Operations

Conceptual operations may include:

• POST /expenses/{id}/receipts

• GET /expenses/{id}/receipts

• DELETE /expenses/{id}/receipts/{receiptId}

• POST /expenses/{id}/extract-receipt


### 7.21.4 Reporting and Search

Conceptual operations may include:

• GET /expenses/search

• GET /expenses/statistics

• POST /expenses/reports

These routes are logical interfaces and do not require a REST architecture.


## 7.22 Domain Model

The conceptual domain model shall include:

Expense

├── Expense Category

├── Child Associations

├── Vendor / Contact

├── Payment Method

├── Receipt / Evidence References

├── Reimbursements

│   └── Reimbursement Payments

├── Recurrence Rule

├── Journal Relationships

├── Calendar Relationships

├── Timeline Relationships

├── Reports

└── Audit History


## 7.23 MVP Scope

The initial Expense Tracking MVP should include:

• Expense creation and editing

• Standard and custom categories

• Child associations

• Vendor tracking

• Payment methods

• Receipt attachments

• Reimbursement tracking

• Outstanding balance calculation

• Recurring expenses

• Search and filtering

• Basic statistics

• AI-assisted categorization

• Receipt OCR where supported

• Journal integration

• Calendar integration

• Timeline integration

• Evidence integration

• Report Builder integration

• Encryption

• Audit history

• Soft delete and restore


## 7.24 Future Enhancements

Future versions may include:

• Bank account synchronization

• Credit card transaction imports

• Enhanced automatic receipt scanning

• Advanced AI-powered expense categorization

• Budget planning

• Spending alerts

• Mileage tracking

• Tax preparation exports

• Insurance claim tracking

• Shared reimbursement workflows

• QuickBooks integration

• Additional accounting software integrations


## 7.25 Acceptance Criteria

The Expense Tracking module shall be considered functionally complete when:

1. Authorized users can create and manage Expense Records.

2. Expenses can be assigned to children and categories.

3. Users can create custom categories.

4. Supported payment methods can be recorded.

5. Receipts and supporting documents can be attached.

6. Original receipt evidence is preserved.

7. Reimbursement requests can be created and tracked.

8. Partial reimbursements are supported.

9. Outstanding reimbursement balances are calculated correctly.

10. Recurring expenses can generate future expense instances.

11. Users can search and filter Expense Records.

12. Financial statistics are calculated from canonical records.

13. AI can provide supported categorization and analysis.

14. AI-generated information is identifiable as machine-generated.

15. Expense Records can be linked to Journal entries.

16. Expense Records can be linked to Calendar events.

17. Expense Records can be linked to Timeline events.

18. Expense Records can reference Evidence records.

19. Reports can include expenses, reimbursements, and receipts.

20. Authorization is enforced for financial operations.

21. Financial data is encrypted in transit and at rest.

22. Significant modifications are audited.

23. Deleted records are recoverable during the configured retention period.

24. Search and reporting remain usable as record volume grows.


## 7.26 Product Decisions Still Required

Before implementation is locked, Product shall define:

• Required expense fields.

• Default currency behavior.

• Multi-currency support requirements.

• Whether negative expenses or adjustment records are supported.

• Custom category limits and behavior.

• Reimbursement percentage and allocation rules.

• Whether one reimbursement may cover multiple expenses.

• Overpayment behavior.

• Overdue calculation rules.

• Default reimbursement due dates.

• Recurring-series edit behavior.

• Supported receipt file formats and size limits.

• OCR provider and confidence thresholds.

• Duplicate-expense detection thresholds.

• Tax-deductible field behavior and disclaimers.

• Financial retention periods.

• Soft-delete retention period.

• Export formats.

• Whether expense creation automatically creates Timeline events.

• Whether reimbursement reminders are enabled by default.

• Whether shared reimbursement workflows are MVP or future scope.


# 8. Timeline


## 8.1 Purpose


### 8.1.1 Overview

The Timeline module is the central historical view of the ClearParent Log platform. It automatically aggregates significant records and events from across the application into a unified chronological history so users can understand what occurred, when it occurred, and how related records connect to one another.

The Timeline shall reduce the need to navigate between individual modules when reviewing case history. It shall provide a single, searchable, filterable, and reportable view of parenting-related activity while preserving links to the originating records.

The Timeline shall support long-term historical review, AI-assisted analysis, report generation, and pattern identification. It shall not replace source modules or become an independent duplicate source of truth. Timeline entries shall reference canonical records maintained by their originating modules.


### 8.1.2 Objectives

The Timeline module shall:

• Automatically create a chronological history of significant parenting-related events.

• Aggregate information from all integrated modules.

• Provide a single location for reviewing historical events.

• Support advanced search and filtering.

• Link directly to original source records.

• Support report generation and AI analysis.

• Help users identify trends, patterns, and relationships between events.

• Preserve source-record security and authorization rules.

• Support many years of historical data without requiring manual timeline maintenance.


## 8.2 Functional Scope

The Timeline module shall include:

• Automatic timeline event creation.

• Chronological sorting and grouping.

• Multiple timeline views.

• Search and compound filtering.

• Event categorization.

• Direct navigation to source records.

• Cross-record linking.

• Timeline statistics.

• Report integration.

• AI integration.

• Dashboard integration.

• Security inheritance.

• Audit history.

• Deletion and recovery behavior tied to source records.


## 8.3 Timeline Event Model


### 8.3.1 Timeline Event Record

Each timeline event shall be represented by a Timeline Event Record with a unique immutable Timeline Event ID.

A Timeline Event Record shall be a derived or referenced representation of a source record whenever a canonical source record exists. The Timeline shall not silently create a second editable copy of source data.


### 8.3.2 Core Fields

Each Timeline Event Record shall support:

• Timeline Event ID

• Event Title

• Event Type

• Event Date

• Event Time

• Start Date/Time where applicable

• End Date/Time where applicable

• Child or Children

• Category

• Summary

• Importance

• Status

• Location where applicable

• Related Contacts

• Source Module

• Source Record ID

• Linked Journal Entry

• Linked Evidence

• Linked Calendar Event

• Linked Parenting Time Record

• Linked Expense Record

• Linked Report

• Tags

• Created Date

• Updated Date

• Created By or Source Process

• Updated By or Source Process


### 8.3.3 Source Metadata

For system-generated timeline events, the system shall record:

• Source Module

• Source Record ID

• Source Record Type

• Source Event Date

• Timeline Created Date

• Last Synchronization Date

• Generation Method

Generation Method may include:

• Automatic

• Imported

• User Note

• AI Generated

• System Generated


### 8.3.4 Timeline Event Status

Timeline events may reflect statuses inherited from their source records. The Timeline shall not invent a conflicting status when the source module already owns the record lifecycle.

Examples may include:

• Scheduled

• Completed

• Cancelled

• Missed

• Denied

• Pending

• Draft

• Published

• Archived

• Deleted

Final status normalization across modules is a Product Decision.


## 8.4 Timeline Event Sources


### 8.4.1 Supported Source Events

The Timeline shall support events generated from:

• Parenting Journal Entries

• Parenting Time

• Parenting Exchanges

• Missed Parenting Time

• Make-Up Parenting Time

• Communications

• OFW Messages

• Emails

• Text Messages

• Phone Calls

• Voicemails

• Photos

• Videos

• Documents

• Evidence Uploads

• School Events

• Medical Appointments

• Therapy Sessions

• Parenting Coach Meetings

• Guardian ad Litem Meetings

• Attorney Meetings

• Court Hearings

• Mediation

• Arbitration

• Expenses

• Reimbursements

• Calendar Events

• AI Generated Reports

• User Notes

• System Notifications


### 8.4.2 Event Creation Rules

Each integrated module shall define which significant source actions create Timeline events.

Examples may include:

• Journal entry published.

• Parenting time completed.

• Parenting exchange recorded.

• Parenting time denied.

• Evidence uploaded.

• Communication imported.

• Expense created.

• Reimbursement requested or received.

• Calendar event completed.

• Report generated.

The exact event-generation matrix shall be defined by Product and Engineering to avoid excessive noise from low-value system actions.


### 8.4.3 Idempotency

Automatic event generation shall be idempotent. Reprocessing the same source action shall not create duplicate Timeline events for the same source record and event type.


## 8.5 Event Types and Visual Categorization


### 8.5.1 Supported Event Types

Timeline events shall support visual categories including:

• Parenting Time

• Communication

• Medical

• School

• Legal

• Financial

• Therapy

• Activities

• Photos

• Documents

• Reports

• Notes

• AI Generated

• Other


### 8.5.2 Visual Treatment

Each event type should have a consistent icon and visual identifier. Color may be used to improve readability, provided accessibility requirements are met and meaning is not communicated by color alone.


### 8.5.3 Category Mapping

Source modules shall map their records to Timeline event categories. A source record may have both a source-specific type and a higher-level Timeline category.

Example:

• Source Type: Parenting Time Denial

• Timeline Category: Parenting Time


## 8.6 Chronological History


### 8.6.1 Ordering

Timeline events shall be ordered chronologically using their effective event date and time.

The system shall define deterministic behavior for records that have:

• Date but no time.

• Identical timestamps.

• Date ranges.

• Imported timestamps with unknown time zones.

These rules are Product Decisions and shall be consistent across Timeline, Search, Reports, and AI.


### 8.6.2 Supported Date Navigation

Users shall be able to:

• View today's events.

• View events by day.

• View events by week.

• View events by month.

• View events by year.

• Jump to a specific date.

• Scroll continuously through historical events.


### 8.6.3 Historical Scale

The Timeline shall support many years of historical data without loading the complete history into the client at once.

The system shall use pagination, cursor-based loading, virtualized rendering, or equivalent scalable techniques.


## 8.7 Timeline Views


### 8.7.1 Supported Views

Users shall be able to choose from:

• Chronological List

• Daily Timeline

• Weekly Timeline

• Monthly Timeline

• Agenda View

• Compact View

• Detailed View


### 8.7.2 Chronological List

The Chronological List shall display events in date/time order and shall be optimized for case-history review.


### 8.7.3 Compact View

Compact View shall minimize descriptive content while preserving key identifying information such as date, type, title, child, and status.


### 8.7.4 Detailed View

Detailed View shall display expanded summaries, relationships, and source links where appropriate.


### 8.7.5 View Persistence

The user's selected Timeline view may be retained as a user preference. Whether this behavior is MVP is a Product Decision.


## 8.8 Timeline Entry Display


### 8.8.1 Required Display Information

A Timeline entry should display, where available:

• Event Title

• Event Type

• Date

• Time

• Child or Children

• Category

• Summary

• Importance

• Status

• Related Contacts

• Location

• Linked record indicators


### 8.8.2 Source Navigation

Selecting a Timeline entry shall provide access to the original source record when the user has permission to view that record.


### 8.8.3 Missing Source Records

If a source record has been deleted, archived, or is no longer accessible, the Timeline shall follow the source module's retention and authorization rules. It shall not expose inaccessible source content merely because a Timeline reference exists.


## 8.9 Search and Filtering


### 8.9.1 Search

Users shall be able to search Timeline events using keywords across searchable Timeline fields and supported indexed source metadata.


### 8.9.2 Filters

Supported filters shall include:

• Date Range

• Child

• Event Type

• Category

• Contact

• Tags

• Importance

• Status

• Keywords

• Location

• Source Module


### 8.9.3 Combined Filters

Users shall be able to combine multiple filters simultaneously.

Examples:

• Child + Date Range + Parenting Time

• Legal + Attorney Contact + Year

• Therapy + Child + Status

• Financial + Date Range + Category


### 8.9.4 Filter Persistence

Saved filters and reusable Timeline views may be supported in a future release unless explicitly included in MVP.


## 8.10 Linking and Relationships


### 8.10.1 Supported Relationships

Timeline events may link to:

• Journal Entries

• Evidence

• Calendar Events

• Parenting Time

• Expenses

• Reports

• Contacts

• Court Cases

• Medical Records

• School Records

• Communications

• Parenting Plan references


### 8.10.2 Canonical Relationships

Timeline relationships shall reference canonical source records rather than duplicating linked content.


### 8.10.3 Navigation

Users shall be able to navigate from a Timeline event to an authorized linked source record with a single interaction where technically practical.


### 8.10.4 Relationship Visibility

The system may display relationship indicators such as:

• Evidence attached

• Journal entry linked

• Related communication

• Related expense

• Related event


## 8.11 AI Integration


### 8.11.1 AI Capabilities

The AI Assistant shall be able to use authorized Timeline data to support questions and summaries such as:

• Summarize everything that happened last month.

• Show every therapy session.

• Find every denied parenting exchange.

• Create a chronological report for my attorney.

• Show every event involving school.

• What happened between June and August?

• Summarize all communication before the last court hearing.


### 8.11.2 Trend Analysis

AI may identify:

• Recurring events.

• Repeated issue categories.

• Changes in event frequency.

• Gaps in documentation.

• Significant changes over time.

• Potential relationships between events.


### 8.11.3 AI Provenance

AI-generated summaries, groupings, labels, and observations shall be clearly identified as AI-generated.

AI analysis shall not modify canonical source records without explicit user action.


### 8.11.4 Authorization

AI retrieval shall respect the same access controls that apply to the underlying source records.


## 8.12 Reports


### 8.12.1 Timeline-Based Reports

Users shall be able to generate reports from Timeline events, including:

• Chronological History

• Parenting Time History

• Communication Timeline

• Medical Timeline

• School Timeline

• Therapy Timeline

• Legal Timeline

• Financial Timeline

• Custom Timeline


### 8.12.2 Report Selection

Users should be able to generate reports using:

• Selected Timeline events.

• Current Timeline filters.

• Date ranges.

• Event categories.

• Children.

• Contacts.


### 8.12.3 Supporting Material

Reports may include linked:

• Evidence

• Journal entries

• Supporting documentation

• Communication references

• Expense records


### 8.12.4 Source Integrity

Timeline reports shall derive from canonical records and Timeline references. Generating a report shall not create independent duplicate source records.


## 8.13 Statistics


### 8.13.1 Summary Metrics

The Timeline shall provide summary metrics including:

• Total Events

• Events by Month

• Events by Category

• Parenting Time Events

• Communications

• Expenses

• Medical Events

• School Events

• Legal Events


### 8.13.2 Dashboard Presentation

Statistics should be available through charts, summary cards, and Dashboard widgets.


### 8.13.3 Calculation Consistency

Timeline statistics shall use the same event-selection and authorization rules as Timeline search and reporting.


## 8.14 Integration


### 8.14.1 Parenting Journal

Published or otherwise qualifying Journal entries shall create or update Timeline references according to Journal integration rules.


### 8.14.2 Parenting Time

Qualifying parenting-time events, exchanges, denials, missed time, and make-up time shall be represented on the Timeline.


### 8.14.3 Calendar

Calendar events shall be represented according to configured Timeline inclusion rules.


### 8.14.4 Communications

Communication records such as OFW messages, emails, texts, calls, and voicemails may appear as Timeline events while retaining links to canonical Communication records.


### 8.14.5 Evidence Management

Evidence uploads may generate Timeline events. Evidence linked to other records shall remain canonical in Evidence Management.


### 8.14.6 Expense Tracking

Expenses and reimbursements may generate Timeline events according to configured inclusion rules.


### 8.14.7 Contacts

Contacts shall provide relationship metadata rather than independently generating Timeline events unless a specific contact-related activity warrants one.


### 8.14.8 Reports

Generated reports may be represented as Timeline events where appropriate.


### 8.14.9 Notifications

Notifications may appear on the Timeline only when Product determines they are historically meaningful. Routine delivery notifications should not automatically create noise in the case history.


### 8.14.10 Parenting Plan

Parenting Plan changes, phase changes, or referenced provisions may be represented when applicable.


## 8.15 Automatic Timeline Management


### 8.15.1 No Duplicate Manual Maintenance

Users shall not be required to manually recreate records on the Timeline that already exist in source modules.


### 8.15.2 User Notes

The Timeline may support direct User Note entries when the user needs to document information that does not originate from another module.


### 8.15.3 Synchronization

When a source record changes, the Timeline representation shall update according to the source integration contract.


### 8.15.4 Historical Preservation

Changes to source records shall not silently erase audit history. The Timeline shall show the current source representation while audit history retains change details where required.


## 8.16 Security and Permissions


### 8.16.1 Permission Inheritance

Timeline events shall inherit access restrictions from their underlying source records.


### 8.16.2 Unauthorized Content

The Timeline shall not reveal titles, summaries, metadata, attachment indicators, or other sensitive information from records the user is not authorized to view, unless explicitly permitted by the security model.


### 8.16.3 Encryption

Timeline data shall be encrypted in transit and at rest consistent with platform security standards.


### 8.16.4 Exports

Timeline exports and reports shall apply the same authorization rules as interactive Timeline views.


## 8.17 Audit History


### 8.17.1 Auditable Events

The system shall audit significant Timeline actions including:

• User Note creation.

• User Note modification.

• User Note deletion.

• Manual importance changes where supported.

• Manual category changes where supported.

• Report generation where required.

• Export operations where required.

• Administrative changes to Timeline rules.


### 8.17.2 Derived Event Changes

Changes caused automatically by source modules shall be traceable to the relevant source action or synchronization process.


### 8.17.3 Audit Record Fields

Audit records should include:

• Audit ID

• Timeline Event ID

• Source Record ID where applicable

• Timestamp

• User or System Process

• Action

• Previous Values where applicable

• New Values where applicable

• Result

Audit records shall not be editable by end users.


## 8.18 Deletion and Recovery


### 8.18.1 Source-Owned Deletion

For derived Timeline events, deletion behavior shall be controlled by the originating source record.

Deleting a Timeline representation shall not automatically delete its canonical source record unless the product explicitly exposes a source-record delete action.


### 8.18.2 User Notes

Direct Timeline User Notes may support soft deletion and recovery according to configured retention policy.


### 8.18.3 Recovery

If a source record is restored, its Timeline representation shall be restored or regenerated when applicable.


## 8.19 Performance and Scalability

The Timeline shall support long-running user histories containing large numbers of events.

The implementation should support:

• Incremental loading.

• Pagination or cursor-based retrieval.

• Indexed filtering.

• Indexed chronological sorting.

• Efficient child and category filtering.

• Cached or aggregated statistics where appropriate.

• Background synchronization for derived events where appropriate.

The UI shall not require loading all historical events into memory at once.


## 8.20 Error Handling

The system shall provide clear handling for:

• Source record unavailable.

• Source record deleted.

• Timeline synchronization failure.

• Duplicate event generation attempt.

• Invalid date or time metadata.

• Search failure.

• Report generation failure.

• Unauthorized source access.

• AI processing failure.

Timeline synchronization or AI failures shall not modify or destroy canonical source records.


## 8.21 Suggested API Surface


### 8.21.1 Timeline Query Operations

Conceptual operations may include:

• GET /timeline

• GET /timeline/{id}

• GET /timeline/search

• GET /timeline/statistics


### 8.21.2 User Note Operations

Conceptual operations may include:

• POST /timeline/notes

• PUT /timeline/notes/{id}

• DELETE /timeline/notes/{id}

• POST /timeline/notes/{id}/restore


### 8.21.3 Relationship Operations

Conceptual operations may include:

• GET /timeline/{id}/relationships

• POST /timeline/{id}/relationships where manual linking is supported

• DELETE /timeline/{id}/relationships/{relationshipId} where manual unlinking is supported


### 8.21.4 Reporting Operations

Conceptual operations may include:

• POST /timeline/reports

• POST /timeline/export


### 8.21.5 Synchronization Operations

Internal operations may include:

• Create Timeline event from source event.

• Update Timeline event from source change.

• Remove or suppress Timeline event based on source state.

• Rebuild Timeline projection.

These routes are conceptual interfaces and do not require a REST architecture.


## 8.22 Proposed Domain Relationships

The conceptual model shall include:

Timeline Event

├── Source Module

├── Source Record

├── Child Associations

├── Category

├── Related Contacts

├── Tags

├── Evidence References

├── Journal References

├── Calendar References

├── Parenting Time References

├── Communication References

├── Expense References

├── Report References

├── Parenting Plan References

└── Audit History

The Timeline Event should generally behave as a projection or reference layer over source-module records rather than an independent duplicate domain object.


## 8.23 MVP Scope

The initial Timeline MVP should include:

• Automatic creation of qualifying Timeline events.

• Chronological list view.

• Daily, weekly, monthly, agenda, compact, and detailed views where feasible.

• Date navigation.

• Search.

• Combined filtering.

• Child filtering.

• Event-type filtering.

• Category filtering.

• Direct source-record navigation.

• Journal integration.

• Parenting Time integration.

• Calendar integration.

• Communication integration.

• Evidence integration.

• Expense integration.

• Contacts relationships.

• Report integration.

• Basic AI Timeline queries.

• Basic Timeline statistics.

• Security inheritance.

• Audit history.

• Scalable historical loading.


## 8.24 Future Enhancements

Future versions may include:

• Interactive graphical timelines.

• Side-by-side timeline comparisons.

• Parenting plan compliance overlays.

• AI-generated event grouping.

• Timeline annotations.

• Bookmarking significant events.

• Timeline sharing with attorneys, therapists, and parenting coaches.

• Event clustering.

• Relationship mapping between events.

• Automatic identification of recurring patterns and trends.

• Saved Timeline searches.

• Saved Timeline views.

• Advanced event correlation.


## 8.25 Acceptance Criteria

The Timeline module shall be considered functionally complete when:

1. Qualifying records from integrated modules automatically appear in the Timeline.

2. Timeline events are ordered chronologically.

3. Users can navigate by date.

4. Users can search Timeline events.

5. Users can combine supported filters.

6. Users can filter by child.

7. Users can filter by event type and category.

8. Timeline events identify their source module and source record.

9. Authorized users can open source records from Timeline entries.

10. Timeline events do not duplicate canonical source data unnecessarily.

11. Journal records integrate with the Timeline.

12. Parenting Time records integrate with the Timeline.

13. Calendar events integrate with the Timeline.

14. Communications integrate with the Timeline.

15. Evidence events integrate with the Timeline.

16. Expense and reimbursement events integrate with the Timeline.

17. Reports can be generated from Timeline events.

18. Timeline statistics are available.

19. AI can answer supported questions using authorized Timeline data.

20. AI-generated analysis is identifiable as machine-generated.

21. Timeline permissions respect underlying source permissions.

22. Unauthorized source content is not exposed.

23. Significant manual Timeline changes are audited.

24. Deleting or hiding a Timeline representation does not unintentionally delete a canonical source record.

25. Historical loading remains usable as record volume grows.


## 8.26 Product Decisions Still Required

Before implementation is locked, Product shall define:

• Which source actions create Timeline events.

• Whether every source record creates one Timeline event or multiple lifecycle events.

• Timeline event deduplication keys.

• Canonical event timestamp rules.

• Date-only event ordering behavior.

• Time-zone handling for imported events.

• Source status normalization.

• Importance levels and whether they are user-editable.

• Event-category taxonomy.

• Icon and color mapping.

• Which Notifications are historically significant enough for Timeline inclusion.

• Whether AI-generated reports automatically create Timeline events.

• Whether direct User Notes are MVP.

• Whether users can manually add relationships between Timeline events and source records.

• Whether users can manually hide Timeline events without changing source records.

• Saved filter behavior.

• Saved view behavior.

• Default Timeline view.

• Timeline retention policy.

• Search indexing strategy.

• Maximum export size.

• Report export formats.

• Whether shared Timeline views are MVP or future scope.

• Whether graphical Timeline visualization is future-only.

9. Search

9.1 Purpose

9.1.1 Overview

The Search module provides a unified search experience across the ClearParent Log platform. It allows users to locate information regardless of the source module in which that information was created, reducing the need to navigate through individual areas of the application.

The Search module shall index authorized content from integrated modules and return results through a single centralized interface. Search shall support exact text retrieval, structured filtering, OCR-derived text, and AI-assisted semantic retrieval while preserving the security and provenance of the underlying source records.

Search shall function as a retrieval layer over canonical source records. Search results shall reference and navigate to originating records rather than create editable duplicate records.

9.1.2 Objectives

The Search module shall:

• Provide a single search interface for the entire application.

• Search all integrated modules simultaneously.

• Support keyword, phrase, partial word, wildcard, Boolean, natural language, and semantic search where configured.

• Search OCR-extracted document text.

• Search AI-generated summaries and derived metadata where authorized.

• Return relevant results with source context and matching information.

• Allow users to filter and refine results.

• Support AI-powered semantic retrieval.

• Preserve source permissions and security controls.

• Automatically index newly created and updated records.

9.2 Functional Scope

The Search module shall include:

• Global search.

• Module-wide indexing.

• Keyword and phrase search.

• Partial-word and wildcard search where supported.

• Boolean query support where supported.

• Natural-language search.

• Semantic AI search.

• OCR content indexing.

• Search result ranking.

• Highlighted matching terms.

• Structured filters.

• Saved searches.

• Search history.

• Recent searches.

• Search suggestions.

• Direct source-record navigation.

• AI retrieval integration.

• Dashboard integration.

• Security-aware result filtering.

• Scalable indexing and retrieval.

9.3 Search Index Model

9.3.1 Search Document

Each searchable source record shall be represented in the search index by a Search Document or equivalent indexed projection.

The Search Document shall not become the canonical record. It shall contain only the fields required to support search, filtering, ranking, authorization, and source navigation.

9.3.2 Core Indexed Fields

A Search Document should support:

• Search Document ID

• Source Module

• Source Record Type

• Source Record ID

• Title

• Searchable Body Text

• Summary

• Event or Record Date

• Created Date

• Updated Date

• Child Associations

• Category

• Tags

• Contact Associations

• File Type

• Event Type

• Author

• Location

• Importance

• Status

• Source

• OCR Text where applicable

• AI Summary where applicable

• Authorization Metadata

• Index Version

• Last Indexed Date

9.3.3 Canonical Source Reference

Each indexed Search Document shall retain a reference to the originating source record.

Selecting a search result shall navigate to the canonical record when the user is authorized to view it.

9.3.4 Derived Content

Derived searchable content may include:

• OCR text

• Speech-to-text transcripts

• AI-generated summaries

• Searchable metadata

• Extracted entities

• Search embeddings

Derived content shall remain distinguishable from user-entered and source-record content.

9.4 Searchable Content

9.4.1 Supported Source Content

The Search module shall index authorized content from:

• Parenting Journal Entries

• Parenting Time

• Calendar Events

• Parenting Exchanges

• Missed Parenting Time

• Communications

• Emails

• OFW Messages

• Text Messages

• Phone Call Logs

• Voice Recordings

• Notes

• Contacts

• Photos

• Videos

• Documents

• PDFs

• Court Orders

• Parenting Plans

• Medical Records

• School Records

• Receipts

• Expense Records

• Reports

• AI Summaries

• OCR Text

• Timeline Events

• Tags

• Categories

9.4.2 Module Contribution Rules

Every integrated module shall define which fields and derived content are contributed to Search.

The indexing contract should define:

• Searchable text fields

• Filterable metadata

• Result title

• Result summary

• Event or record date

• Source navigation target

• Security metadata

• Reindex triggers

9.4.3 Unsupported or Restricted Content

A source module may explicitly mark fields or records as non-searchable when required by security, privacy, legal, or product rules.

Restricted fields shall not be exposed through snippets, suggestions, autocomplete, result counts, AI retrieval, or search history.

9.5 Search Methods

9.5.1 Keyword Search

Keyword search shall locate records containing matching terms in indexed fields.

The system should support configurable tokenization, stemming, normalization, and case-insensitive matching.

9.5.2 Phrase Search

Users shall be able to search for exact or near-exact phrases where the search engine supports phrase matching.

Quoted-search syntax may be supported.

9.5.3 Partial Word Search

Partial-word matching may be supported for names, titles, tags, categories, and other appropriate fields.

The exact minimum prefix length and matching rules are Product Decisions.

9.5.4 Wildcard Search

Wildcard search may be supported for advanced users.

Supported wildcard syntax and limits shall be defined by the selected search technology.

9.5.5 Boolean Search

Boolean operators may include:

• AND

• OR

• NOT

Advanced Boolean syntax shall not be required unless explicitly included in MVP.

9.5.6 Natural-Language Search

Users shall be able to enter conversational queries such as:

• Show every denied parenting exchange.

• Find every document mentioning school.

• Show photos from Comic Con.

• Find receipts over $100.

• Show every therapy session from last summer.

• Find every message mentioning anxiety.

The system shall translate supported natural-language intent into structured search and retrieval operations where possible.

9.5.7 Semantic Search

Semantic search shall use vector or equivalent meaning-based retrieval to identify relevant records even when exact search terms are not present.

Semantic search shall augment, not silently replace, lexical search unless Product explicitly chooses a semantic-first architecture.

9.6 Search Query Model

9.6.1 Query Components

A search request may contain:

• Query Text

• Search Method

• Date Range

• Child Filters

• Module Filters

• Category Filters

• Tag Filters

• Contact Filters

• File Type Filters

• Event Type Filters

• Author Filters

• Location Filters

• Importance Filters

• Status Filters

• Source Filters

• Sort Order

• Page or Cursor

• Result Limit

9.6.2 Query Normalization

The system may normalize:

• Letter case

• Whitespace

• Common punctuation

• Supported date expressions

• Singular and plural forms

• Synonyms where configured

Normalization shall not alter quoted exact-search intent.

9.6.3 Query Validation

The system shall validate query syntax and return understandable errors for unsupported operators, malformed dates, invalid filters, or excessive query complexity.

9.7 OCR Search

9.7.1 OCR Sources

OCR-derived search shall support text extracted from:

• Scanned Documents

• PDF Files

• Images

• Court Orders

• Medical Records

• School Documents

• Receipts

Handwriting recognition is future scope unless separately approved.

9.7.2 OCR Indexing

When OCR processing completes successfully, extracted text shall become searchable without requiring the user to manually copy the text into another record.

9.7.3 OCR Provenance

Search results derived from OCR content should identify that the matching text came from OCR when useful to the user.

OCR text shall not replace the original evidence or document.

9.7.4 OCR Confidence

Low-confidence OCR content may be indexed, excluded, or marked based on configurable confidence rules. Threshold behavior is a Product Decision.

9.8 AI-Powered Search

9.8.1 AI Capabilities

AI-enhanced search shall support:

• Conversational questions

• Concept matching

• Cross-module retrieval

• Result summarization

• Result explanation

• Related-information suggestions

• Supporting-record discovery

• Missing-evidence suggestions where appropriate

9.8.2 Example AI Queries

Supported queries may include:

• What happened before the last court hearing?

• Show everything involving school attendance.

• Summarize communication during spring break.

• Find all evidence supporting missed parenting time.

• Show everything that happened in July.

• Find every expense related to school.

• What evidence supports this report?

• Summarize all communication with the parenting coach.

• Show every document mentioning medication.

• When was the last time this issue occurred?

9.8.3 AI Retrieval Behavior

AI shall retrieve only records the requesting user is authorized to access.

AI may combine lexical, structured, OCR, and semantic retrieval to answer a query.

9.8.4 AI Result Citations

AI-generated answers should include links or references to supporting source records whenever possible.

9.8.5 AI Provenance

AI-generated summaries, inferred relationships, and recommended supporting records shall be clearly identified as AI-generated.

AI shall not modify source records, tags, categories, or evidence classifications without explicit user action.

9.9 Search Filters

9.9.1 Supported Filters

Users shall be able to filter by:

• Date Range

• Child

• Module

• Category

• Tags

• Contact

• File Type

• Event Type

• Author

• Location

• Importance

• Status

• Source

9.9.2 Combined Filters

Multiple filters shall be combinable in the same query.

Examples include:

• Child + Date Range + Module

• Court Documents + Year + Contact

• Expense Records + Category + Amount-related query text

• Communications + Child + Date Range

9.9.3 Filter Counts

The system may display result counts by filter value. Counts shall respect authorization rules.

9.9.4 Filter Persistence

Recent filters may be remembered for convenience. Persistent filter behavior is a Product Decision.

9.10 Search Results

9.10.1 Result Display

Each search result should display, where available:

• Title

• Module

• Date

• Child

• Category

• Summary

• Matching Keywords

• Highlighted Search Terms

• Related Records

• Available Actions

• Source Type

• Status

9.10.2 Search Snippets

The system should generate a contextual snippet around the matching text when applicable.

Search snippets shall not reveal restricted content.

9.10.3 Highlighting

Matching search terms may be highlighted in result titles, summaries, OCR snippets, or body excerpts.

9.10.4 Result Actions

Available actions may include:

• Open Source Record

• Preview

• Add to Report

• Copy Link

• View Related Records

• Filter by Similar Records

Actions shall depend on user permissions and source capabilities.

9.10.5 Empty Results

When no results are found, the system may suggest:

• Broader search terms

• Removing filters

• Alternate spellings

• Related categories

• AI-assisted search

9.11 Ranking and Relevance

9.11.1 Ranking Inputs

Search ranking may consider:

• Exact title matches

• Phrase matches

• Keyword frequency

• Field importance

• Recency

• Child match

• Category match

• Filter match

• Source type

• Semantic similarity

• User-selected sort order

9.11.2 Deterministic Sorting

Users should be able to override relevance sorting with deterministic orders such as:

• Newest First

• Oldest First

• Title

• Module

• Importance

9.11.3 Ranking Transparency

AI or semantic results may provide a brief reason why a result was returned where useful.

The system shall not present speculative relevance explanations as source facts.

9.12 Global Search

9.12.1 Availability

The primary Global Search interface shall be accessible from every major application screen.

9.12.2 Global Search Features

Global Search should support:

• Cross-module search

• Recent searches

• Suggested searches

• Keyboard shortcuts

• Search-as-you-type

• Recent filters

• Direct navigation

9.12.3 Search-as-you-Type

Search-as-you-type may return lightweight suggestions or preliminary results before full query submission.

The minimum input length, debounce interval, and maximum suggestion count are Product Decisions.

9.12.4 Keyboard Access

A keyboard shortcut may open or focus Global Search. The final shortcut is a Product Decision.

9.13 Saved Searches

9.13.1 Save Search

Users shall be able to save frequently used searches when Saved Searches are enabled.

Saved Search data should include:

• Saved Search ID

• Name

• Query Text

• Filters

• Sort Order

• Created Date

• Updated Date

9.13.2 Examples

Saved searches may include:

• Missed Parenting Time

• Medical Expenses

• School Events

• Therapy Sessions

• OFW Communications

• Court Documents

9.13.3 Saved Search Access

Saved searches should be accessible from:

• Search page

• Dashboard

• Global Search shortcuts where supported

9.13.4 Saved Search Ownership

Saved searches shall belong to the user who created them unless future sharing is introduced.

9.14 Search History

9.14.1 Search History Record

The application may maintain a Search History record containing:

• Query Text

• Filters

• Search Method

• Timestamp

• Result Count

9.14.2 Search History Actions

Users shall be able to:

• View recent searches

• Re-run previous searches

• Delete individual searches

• Pin favorite searches

• Clear search history

9.14.3 Privacy

Search history shall be user-specific and shall not be visible to other users unless a future sharing feature explicitly permits it.

9.14.4 Sensitive Query Handling

Product shall determine whether sensitive searches are retained, redacted, or excluded from history.

9.15 Search Suggestions

9.15.1 Suggestion Sources

Suggestions may derive from:

• Recent searches

• Saved searches

• Frequently used filters

• Record titles

• Contacts

• Categories

• Tags

• AI-generated suggestions

9.15.2 Security

Suggestions shall respect the same authorization rules as full search results.

Autocomplete shall not leak restricted record titles or metadata.

9.16 Indexing and Synchronization

9.16.1 Automatic Indexing

New qualifying records shall be indexed automatically without requiring manual indexing.

9.16.2 Update Indexing

When indexed source content changes, the corresponding Search Document shall be updated.

9.16.3 Deletion

When a source record is deleted, archived, restricted, or restored, Search shall update visibility according to source-module rules.

9.16.4 Idempotency

Reprocessing the same source record shall not create duplicate Search Documents for the same indexing identity.

9.16.5 Rebuild

Engineering shall support a controlled reindex or rebuild process for index corruption, schema changes, or search-platform migration.

9.16.6 Index Lag

The target maximum delay between source-record change and search availability is a Product and Engineering decision.

9.17 Integration

9.17.1 Parenting Journal

Published or otherwise searchable Journal entries shall contribute authorized text and metadata to Search.

9.17.2 Parenting Time

Parenting time records, exchanges, denials, missed time, and related fields shall be searchable according to module rules.

9.17.3 Calendar

Calendar events shall contribute searchable titles, descriptions, dates, locations, categories, and relationships where authorized.

9.17.4 Timeline

Timeline events shall be searchable while retaining links to source records.

9.17.5 Evidence Management

Evidence metadata, OCR text, transcripts, and supported derived content shall contribute to Search without replacing canonical Evidence records.

9.17.6 Communications

Communication records including emails, OFW messages, texts, call logs, and transcripts shall be searchable where authorized.

9.17.7 Expense Tracking

Expense titles, vendors, categories, notes, reimbursement data, and OCR receipt text may be searchable.

9.17.8 Contacts

Contact names, organizations, roles, and approved metadata may be indexed.

9.17.9 Reports

Generated report metadata and supported report content may be searchable according to Report Builder rules.

9.17.10 AI Assistant

The AI Assistant shall use Search as a primary retrieval mechanism for authorized application data.

9.17.11 Dashboard

Saved searches, recent searches, and selected search summaries may be surfaced on the Dashboard.

9.18 Security and Permissions

9.18.1 Permission Enforcement

Search results shall respect the security permissions of the underlying source records.

9.18.2 Restricted Metadata

Unauthorized users shall not receive restricted:

• Titles

• Snippets

• OCR text

• Result counts where revealing

• Tags

• Categories

• File metadata

• AI summaries

• Suggestions

9.18.3 Security Filtering

Authorization shall be enforced server-side or at the secure search layer. Client-side filtering alone shall not be considered sufficient.

9.18.4 AI Security

AI retrieval shall use the same permission constraints as Search.

9.18.5 Encryption

Search indexes and search traffic shall be protected consistent with platform encryption standards.

9.19 Audit and Privacy

9.19.1 Search Audit

Product shall define whether searches are audited and at what level of detail.

Possible audited actions include:

• Search execution

• Export from search

• Report creation from search results

• Administrative reindex operations

• Saved search creation or deletion

9.19.2 Search History vs Audit Log

User-visible Search History and administrative/security Audit Logs shall be separate concepts.

Deleting Search History shall not necessarily delete security audit events.

9.19.3 Data Minimization

Search indexes shall store only information required for retrieval, filtering, ranking, security, and source navigation.

9.20 Performance and Scalability

9.20.1 Performance Objectives

Search should return interactive results quickly across large historical datasets.

The exact service-level target is TBD. The source requirement that results return within a few seconds shall be treated as the baseline product expectation.

9.20.2 Scalability

The system shall support increasing volumes of:

• Journal entries

• Communications

• OCR text

• Evidence

• Timeline events

• Expenses

• Reports

• Search history

9.20.3 Pagination

Search results shall use pagination, cursor-based retrieval, or equivalent incremental loading.

9.20.4 Index Optimization

Engineering may use:

• Full-text indexes

• Inverted indexes

• Faceted indexes

• Vector indexes

• Cached filter counts

• Query result caching where safe

9.20.5 Search Isolation

Slow OCR processing, AI embedding generation, or index updates should not block source-record creation.

9.21 Error Handling

9.21.1 Query Errors

The system shall provide clear handling for:

• Invalid query syntax

• Unsupported Boolean expressions

• Invalid date ranges

• Invalid filters

• Excessively broad queries

• Search service unavailable

9.21.2 Indexing Errors

Indexing failures shall not cause loss of canonical source records.

Failed indexing operations should be retryable.

9.21.3 OCR Errors

OCR failure shall not make the original document unavailable. The record may remain searchable through metadata even when OCR text is unavailable.

9.21.4 AI Search Errors

AI or semantic-search failure shall fall back to conventional search where technically possible.

9.21.5 Stale Results

If a result references a source record that has changed or become unavailable, the system shall revalidate authorization and source state before displaying protected content.

9.22 Suggested API Surface

9.22.1 Search Operations

Conceptual operations may include:

• GET /search

• POST /search/query

• POST /search/semantic

• GET /search/suggestions

• GET /search/facets

9.22.2 Saved Search Operations

Conceptual operations may include:

• POST /search/saved

• GET /search/saved

• GET /search/saved/{id}

• PUT /search/saved/{id}

• DELETE /search/saved/{id}

9.22.3 Search History Operations

Conceptual operations may include:

• GET /search/history

• DELETE /search/history/{id}

• DELETE /search/history

• POST /search/history/{id}/pin

9.22.4 Indexing Operations

Internal or administrative operations may include:

• Index source record

• Reindex source record

• Remove source record from index

• Rebuild index

• Rebuild embeddings

• Inspect indexing status

9.22.5 AI Retrieval Operations

Internal interfaces may include:

• Hybrid lexical/semantic retrieval

• Retrieve authorized supporting records

• Retrieve related records

• Generate answer with source references

These interfaces are conceptual and do not require a REST architecture.

9.23 Proposed Domain Relationships

The conceptual model shall include:

Search Document

├── Source Module

├── Source Record

├── Child Associations

├── Categories

├── Tags

├── Contacts

├── OCR Content

├── AI Summary

├── Search Embedding

├── Authorization Metadata

└── Index Metadata

Saved Search

├── User

├── Query

├── Filters

├── Sort Order

└── Saved Search Metadata

Search History

├── User

├── Query

├── Filters

├── Timestamp

└── Result Metadata

9.24 MVP Scope

The initial Search MVP should include:

• Global search interface.

• Cross-module keyword search.

• Phrase search.

• Date filtering.

• Child filtering.

• Module filtering.

• Category filtering.

• Tag filtering.

• Contact filtering.

• Status filtering.

• Search result snippets.

• Highlighted matches where supported.

• Direct navigation to source records.

• Automatic indexing of new records.

• Automatic update of changed records.

• OCR text search where OCR is available.

• Basic natural-language search.

• Basic semantic AI search.

• AI retrieval with source links.

• Search history.

• Saved searches.

• Security-aware results.

• Pagination or incremental loading.

9.25 Future Enhancements

Future versions may include:

• Voice search

• Image similarity search

• Facial recognition search

• Handwriting recognition

• AI-powered predictive suggestions

• Predictive search

• Related document recommendations

• Duplicate record detection

• Cross-case search for legal professionals

• Advanced Boolean search builder

• Search analytics

• Advanced query syntax

• Personalized ranking

• Query intent learning

• Saved-search sharing

• Search result collaboration

9.26 Acceptance Criteria

The Search module shall be considered functionally complete when:

1. Authorized users can search across all integrated modules from one interface.

2. Searchable source records are indexed automatically.

3. Updated source records are reflected in the index.

4. Deleted or restricted source records stop exposing unauthorized search content.

5. Users can perform keyword searches.

6. Users can perform phrase searches where supported.

7. Users can use supported natural-language queries.

8. Users can apply multiple filters simultaneously.

9. OCR-extracted text is searchable when OCR processing is available.

10. Search results identify their source module.

11. Search results link to the canonical source record.

12. Search snippets do not expose unauthorized content.

13. Search results can be paginated or incrementally loaded.

14. Saved searches can be created and rerun when enabled.

15. Search history can be viewed and managed when enabled.

16. AI can retrieve authorized supporting records through Search.

17. AI responses can reference supporting source records where possible.

18. Semantic search does not bypass source permissions.

19. Search-index failures do not modify or destroy source records.

20. Search remains usable as historical data volume grows.

21. Search suggestions do not reveal unauthorized records.

22. Search results and AI retrieval follow the same security model.

9.27 Product Decisions Still Required

Before implementation is locked, Product shall define:

• Primary search engine and indexing architecture.

• Whether Search Documents are stored as a separate projection or generated dynamically.

• Lexical-first, semantic-first, or hybrid default ranking.

• Exact relevance-ranking rules.

• Field weighting.

• Stemming and synonym behavior.

• Partial-word minimum length.

• Wildcard syntax and limits.

• Boolean syntax supported in MVP.

• Natural-language parser responsibilities.

• Semantic embedding provider and model.

• Embedding refresh rules.

• OCR confidence threshold for indexing.

• Whether AI summaries are searchable by default.

• Whether AI-generated content influences ranking.

• Maximum indexing delay after record creation or update.

• Search-result page size.

• Maximum query complexity.

• Search-as-you-type minimum characters and debounce interval.

• Maximum suggestion count.

• Default sort order.

• Available deterministic sort orders.

• Saved Searches included in MVP or future scope.

• Search History included in MVP or future scope.

• Search History retention period.

• Whether users can disable Search History.

• Whether sensitive queries are stored in Search History.

• Whether searches are written to the Audit Log.

• Search analytics retention and privacy rules.

• Whether result counts are hidden when they could reveal restricted data.

• Search index retention behavior after soft deletion.

• Reindex and disaster-recovery strategy.

• Search export capabilities.

• Voice search future architecture.

• Facial-recognition search privacy and consent requirements.

• Cross-case search authorization model for legal professionals.


# 9. Search


## 9.1 Purpose


### 9.1.1 Overview

The Search module provides a unified search experience across the ClearParent Log platform. It allows users to locate information regardless of the source module in which that information was created, reducing the need to navigate through individual areas of the application.

The Search module shall index authorized content from integrated modules and return results through a single centralized interface. Search shall support exact text retrieval, structured filtering, OCR-derived text, and AI-assisted semantic retrieval while preserving the security and provenance of the underlying source records.

Search shall function as a retrieval layer over canonical source records. Search results shall reference and navigate to originating records rather than create editable duplicate records.


### 9.1.2 Objectives

The Search module shall:

• Provide a single search interface for the entire application.

• Search all integrated modules simultaneously.

• Support keyword, phrase, partial word, wildcard, Boolean, natural language, and semantic search where configured.

• Search OCR-extracted document text.

• Search AI-generated summaries and derived metadata where authorized.

• Return relevant results with source context and matching information.

• Allow users to filter and refine results.

• Support AI-powered semantic retrieval.

• Preserve source permissions and security controls.

• Automatically index newly created and updated records.


## 9.2 Functional Scope

The Search module shall include:

• Global search.

• Module-wide indexing.

• Keyword and phrase search.

• Partial-word and wildcard search where supported.

• Boolean query support where supported.

• Natural-language search.

• Semantic AI search.

• OCR content indexing.

• Search result ranking.

• Highlighted matching terms.

• Structured filters.

• Saved searches.

• Search history.

• Recent searches.

• Search suggestions.

• Direct source-record navigation.

• AI retrieval integration.

• Dashboard integration.

• Security-aware result filtering.

• Scalable indexing and retrieval.


## 9.3 Search Index Model


### 9.3.1 Search Document

Each searchable source record shall be represented in the search index by a Search Document or equivalent indexed projection.

The Search Document shall not become the canonical record. It shall contain only the fields required to support search, filtering, ranking, authorization, and source navigation.


### 9.3.2 Core Indexed Fields

A Search Document should support:

• Search Document ID

• Source Module

• Source Record Type

• Source Record ID

• Title

• Searchable Body Text

• Summary

• Event or Record Date

• Created Date

• Updated Date

• Child Associations

• Category

• Tags

• Contact Associations

• File Type

• Event Type

• Author

• Location

• Importance

• Status

• Source

• OCR Text where applicable

• AI Summary where applicable

• Authorization Metadata

• Index Version

• Last Indexed Date


### 9.3.3 Canonical Source Reference

Each indexed Search Document shall retain a reference to the originating source record.

Selecting a search result shall navigate to the canonical record when the user is authorized to view it.


### 9.3.4 Derived Content

Derived searchable content may include:

• OCR text

• Speech-to-text transcripts

• AI-generated summaries

• Searchable metadata

• Extracted entities

• Search embeddings

Derived content shall remain distinguishable from user-entered and source-record content.


## 9.4 Searchable Content


### 9.4.1 Supported Source Content

The Search module shall index authorized content from:

• Parenting Journal Entries

• Parenting Time

• Calendar Events

• Parenting Exchanges

• Missed Parenting Time

• Communications

• Emails

• OFW Messages

• Text Messages

• Phone Call Logs

• Voice Recordings

• Notes

• Contacts

• Photos

• Videos

• Documents

• PDFs

• Court Orders

• Parenting Plans

• Medical Records

• School Records

• Receipts

• Expense Records

• Reports

• AI Summaries

• OCR Text

• Timeline Events

• Tags

• Categories


### 9.4.2 Module Contribution Rules

Every integrated module shall define which fields and derived content are contributed to Search.

The indexing contract should define:

• Searchable text fields

• Filterable metadata

• Result title

• Result summary

• Event or record date

• Source navigation target

• Security metadata

• Reindex triggers


### 9.4.3 Unsupported or Restricted Content

A source module may explicitly mark fields or records as non-searchable when required by security, privacy, legal, or product rules.

Restricted fields shall not be exposed through snippets, suggestions, autocomplete, result counts, AI retrieval, or search history.


## 9.5 Search Methods


### 9.5.1 Keyword Search

Keyword search shall locate records containing matching terms in indexed fields.

The system should support configurable tokenization, stemming, normalization, and case-insensitive matching.


### 9.5.2 Phrase Search

Users shall be able to search for exact or near-exact phrases where the search engine supports phrase matching.

Quoted-search syntax may be supported.


### 9.5.3 Partial Word Search

Partial-word matching may be supported for names, titles, tags, categories, and other appropriate fields.

The exact minimum prefix length and matching rules are Product Decisions.


### 9.5.4 Wildcard Search

Wildcard search may be supported for advanced users.

Supported wildcard syntax and limits shall be defined by the selected search technology.


### 9.5.5 Boolean Search

Boolean operators may include:

• AND

• OR

• NOT

Advanced Boolean syntax shall not be required unless explicitly included in MVP.


### 9.5.6 Natural-Language Search

Users shall be able to enter conversational queries such as:

• Show every denied parenting exchange.

• Find every document mentioning school.

• Show photos from Comic Con.

• Find receipts over $100.

• Show every therapy session from last summer.

• Find every message mentioning anxiety.

The system shall translate supported natural-language intent into structured search and retrieval operations where possible.


### 9.5.7 Semantic Search

Semantic search shall use vector or equivalent meaning-based retrieval to identify relevant records even when exact search terms are not present.

Semantic search shall augment, not silently replace, lexical search unless Product explicitly chooses a semantic-first architecture.


## 9.6 Search Query Model


### 9.6.1 Query Components

A search request may contain:

• Query Text

• Search Method

• Date Range

• Child Filters

• Module Filters

• Category Filters

• Tag Filters

• Contact Filters

• File Type Filters

• Event Type Filters

• Author Filters

• Location Filters

• Importance Filters

• Status Filters

• Source Filters

• Sort Order

• Page or Cursor

• Result Limit


### 9.6.2 Query Normalization

The system may normalize:

• Letter case

• Whitespace

• Common punctuation

• Supported date expressions

• Singular and plural forms

• Synonyms where configured

Normalization shall not alter quoted exact-search intent.


### 9.6.3 Query Validation

The system shall validate query syntax and return understandable errors for unsupported operators, malformed dates, invalid filters, or excessive query complexity.


## 9.7 OCR Search


### 9.7.1 OCR Sources

OCR-derived search shall support text extracted from:

• Scanned Documents

• PDF Files

• Images

• Court Orders

• Medical Records

• School Documents

• Receipts

Handwriting recognition is future scope unless separately approved.


### 9.7.2 OCR Indexing

When OCR processing completes successfully, extracted text shall become searchable without requiring the user to manually copy the text into another record.


### 9.7.3 OCR Provenance

Search results derived from OCR content should identify that the matching text came from OCR when useful to the user.

OCR text shall not replace the original evidence or document.


### 9.7.4 OCR Confidence

Low-confidence OCR content may be indexed, excluded, or marked based on configurable confidence rules. Threshold behavior is a Product Decision.


## 9.8 AI-Powered Search


### 9.8.1 AI Capabilities

AI-enhanced search shall support:

• Conversational questions

• Concept matching

• Cross-module retrieval

• Result summarization

• Result explanation

• Related-information suggestions

• Supporting-record discovery

• Missing-evidence suggestions where appropriate


### 9.8.2 Example AI Queries

Supported queries may include:

• What happened before the last court hearing?

• Show everything involving school attendance.

• Summarize communication during spring break.

• Find all evidence supporting missed parenting time.

• Show everything that happened in July.

• Find every expense related to school.

• What evidence supports this report?

• Summarize all communication with the parenting coach.

• Show every document mentioning medication.

• When was the last time this issue occurred?


### 9.8.3 AI Retrieval Behavior

AI shall retrieve only records the requesting user is authorized to access.

AI may combine lexical, structured, OCR, and semantic retrieval to answer a query.


### 9.8.4 AI Result Citations

AI-generated answers should include links or references to supporting source records whenever possible.


### 9.8.5 AI Provenance

AI-generated summaries, inferred relationships, and recommended supporting records shall be clearly identified as AI-generated.

AI shall not modify source records, tags, categories, or evidence classifications without explicit user action.


## 9.9 Search Filters


### 9.9.1 Supported Filters

Users shall be able to filter by:

• Date Range

• Child

• Module

• Category

• Tags

• Contact

• File Type

• Event Type

• Author

• Location

• Importance

• Status

• Source


### 9.9.2 Combined Filters

Multiple filters shall be combinable in the same query.

Examples include:

• Child + Date Range + Module

• Court Documents + Year + Contact

• Expense Records + Category + Amount-related query text

• Communications + Child + Date Range


### 9.9.3 Filter Counts

The system may display result counts by filter value. Counts shall respect authorization rules.


### 9.9.4 Filter Persistence

Recent filters may be remembered for convenience. Persistent filter behavior is a Product Decision.


## 9.10 Search Results


### 9.10.1 Result Display

Each search result should display, where available:

• Title

• Module

• Date

• Child

• Category

• Summary

• Matching Keywords

• Highlighted Search Terms

• Related Records

• Available Actions

• Source Type

• Status


### 9.10.2 Search Snippets

The system should generate a contextual snippet around the matching text when applicable.

Search snippets shall not reveal restricted content.


### 9.10.3 Highlighting

Matching search terms may be highlighted in result titles, summaries, OCR snippets, or body excerpts.


### 9.10.4 Result Actions

Available actions may include:

• Open Source Record

• Preview

• Add to Report

• Copy Link

• View Related Records

• Filter by Similar Records

Actions shall depend on user permissions and source capabilities.


### 9.10.5 Empty Results

When no results are found, the system may suggest:

• Broader search terms

• Removing filters

• Alternate spellings

• Related categories

• AI-assisted search


## 9.11 Ranking and Relevance


### 9.11.1 Ranking Inputs

Search ranking may consider:

• Exact title matches

• Phrase matches

• Keyword frequency

• Field importance

• Recency

• Child match

• Category match

• Filter match

• Source type

• Semantic similarity

• User-selected sort order


### 9.11.2 Deterministic Sorting

Users should be able to override relevance sorting with deterministic orders such as:

• Newest First

• Oldest First

• Title

• Module

• Importance


### 9.11.3 Ranking Transparency

AI or semantic results may provide a brief reason why a result was returned where useful.

The system shall not present speculative relevance explanations as source facts.


## 9.12 Global Search


### 9.12.1 Availability

The primary Global Search interface shall be accessible from every major application screen.


### 9.12.2 Global Search Features

Global Search should support:

• Cross-module search

• Recent searches

• Suggested searches

• Keyboard shortcuts

• Search-as-you-type

• Recent filters

• Direct navigation


### 9.12.3 Search-as-you-Type

Search-as-you-type may return lightweight suggestions or preliminary results before full query submission.

The minimum input length, debounce interval, and maximum suggestion count are Product Decisions.


### 9.12.4 Keyboard Access

A keyboard shortcut may open or focus Global Search. The final shortcut is a Product Decision.


## 9.13 Saved Searches


### 9.13.1 Save Search

Users shall be able to save frequently used searches when Saved Searches are enabled.

Saved Search data should include:

• Saved Search ID

• Name

• Query Text

• Filters

• Sort Order

• Created Date

• Updated Date


### 9.13.2 Examples

Saved searches may include:

• Missed Parenting Time

• Medical Expenses

• School Events

• Therapy Sessions

• OFW Communications

• Court Documents


### 9.13.3 Saved Search Access

Saved searches should be accessible from:

• Search page

• Dashboard

• Global Search shortcuts where supported


### 9.13.4 Saved Search Ownership

Saved searches shall belong to the user who created them unless future sharing is introduced.


## 9.14 Search History


### 9.14.1 Search History Record

The application may maintain a Search History record containing:

• Query Text

• Filters

• Search Method

• Timestamp

• Result Count


### 9.14.2 Search History Actions

Users shall be able to:

• View recent searches

• Re-run previous searches

• Delete individual searches

• Pin favorite searches

• Clear search history


### 9.14.3 Privacy

Search history shall be user-specific and shall not be visible to other users unless a future sharing feature explicitly permits it.


### 9.14.4 Sensitive Query Handling

Product shall determine whether sensitive searches are retained, redacted, or excluded from history.


## 9.15 Search Suggestions


### 9.15.1 Suggestion Sources

Suggestions may derive from:

• Recent searches

• Saved searches

• Frequently used filters

• Record titles

• Contacts

• Categories

• Tags

• AI-generated suggestions


### 9.15.2 Security

Suggestions shall respect the same authorization rules as full search results.

Autocomplete shall not leak restricted record titles or metadata.


## 9.16 Indexing and Synchronization


### 9.16.1 Automatic Indexing

New qualifying records shall be indexed automatically without requiring manual indexing.


### 9.16.2 Update Indexing

When indexed source content changes, the corresponding Search Document shall be updated.


### 9.16.3 Deletion

When a source record is deleted, archived, restricted, or restored, Search shall update visibility according to source-module rules.


### 9.16.4 Idempotency

Reprocessing the same source record shall not create duplicate Search Documents for the same indexing identity.


### 9.16.5 Rebuild

Engineering shall support a controlled reindex or rebuild process for index corruption, schema changes, or search-platform migration.


### 9.16.6 Index Lag

The target maximum delay between source-record change and search availability is a Product and Engineering decision.


## 9.17 Integration


### 9.17.1 Parenting Journal

Published or otherwise searchable Journal entries shall contribute authorized text and metadata to Search.


### 9.17.2 Parenting Time

Parenting time records, exchanges, denials, missed time, and related fields shall be searchable according to module rules.


### 9.17.3 Calendar

Calendar events shall contribute searchable titles, descriptions, dates, locations, categories, and relationships where authorized.


### 9.17.4 Timeline

Timeline events shall be searchable while retaining links to source records.


### 9.17.5 Evidence Management

Evidence metadata, OCR text, transcripts, and supported derived content shall contribute to Search without replacing canonical Evidence records.


### 9.17.6 Communications

Communication records including emails, OFW messages, texts, call logs, and transcripts shall be searchable where authorized.


### 9.17.7 Expense Tracking

Expense titles, vendors, categories, notes, reimbursement data, and OCR receipt text may be searchable.


### 9.17.8 Contacts

Contact names, organizations, roles, and approved metadata may be indexed.


### 9.17.9 Reports

Generated report metadata and supported report content may be searchable according to Report Builder rules.


### 9.17.10 AI Assistant

The AI Assistant shall use Search as a primary retrieval mechanism for authorized application data.


### 9.17.11 Dashboard

Saved searches, recent searches, and selected search summaries may be surfaced on the Dashboard.


## 9.18 Security and Permissions


### 9.18.1 Permission Enforcement

Search results shall respect the security permissions of the underlying source records.


### 9.18.2 Restricted Metadata

Unauthorized users shall not receive restricted:

• Titles

• Snippets

• OCR text

• Result counts where revealing

• Tags

• Categories

• File metadata

• AI summaries

• Suggestions


### 9.18.3 Security Filtering

Authorization shall be enforced server-side or at the secure search layer. Client-side filtering alone shall not be considered sufficient.


### 9.18.4 AI Security

AI retrieval shall use the same permission constraints as Search.


### 9.18.5 Encryption

Search indexes and search traffic shall be protected consistent with platform encryption standards.


## 9.19 Audit and Privacy


### 9.19.1 Search Audit

Product shall define whether searches are audited and at what level of detail.

Possible audited actions include:

• Search execution

• Export from search

• Report creation from search results

• Administrative reindex operations

• Saved search creation or deletion


### 9.19.2 Search History vs Audit Log

User-visible Search History and administrative/security Audit Logs shall be separate concepts.

Deleting Search History shall not necessarily delete security audit events.


### 9.19.3 Data Minimization

Search indexes shall store only information required for retrieval, filtering, ranking, security, and source navigation.


## 9.20 Performance and Scalability


### 9.20.1 Performance Objectives

Search should return interactive results quickly across large historical datasets.

The exact service-level target is TBD. The source requirement that results return within a few seconds shall be treated as the baseline product expectation.


### 9.20.2 Scalability

The system shall support increasing volumes of:

• Journal entries

• Communications

• OCR text

• Evidence

• Timeline events

• Expenses

• Reports

• Search history


### 9.20.3 Pagination

Search results shall use pagination, cursor-based retrieval, or equivalent incremental loading.


### 9.20.4 Index Optimization

Engineering may use:

• Full-text indexes

• Inverted indexes

• Faceted indexes

• Vector indexes

• Cached filter counts

• Query result caching where safe


### 9.20.5 Search Isolation

Slow OCR processing, AI embedding generation, or index updates should not block source-record creation.


## 9.21 Error Handling


### 9.21.1 Query Errors

The system shall provide clear handling for:

• Invalid query syntax

• Unsupported Boolean expressions

• Invalid date ranges

• Invalid filters

• Excessively broad queries

• Search service unavailable


### 9.21.2 Indexing Errors

Indexing failures shall not cause loss of canonical source records.

Failed indexing operations should be retryable.


### 9.21.3 OCR Errors

OCR failure shall not make the original document unavailable. The record may remain searchable through metadata even when OCR text is unavailable.


### 9.21.4 AI Search Errors

AI or semantic-search failure shall fall back to conventional search where technically possible.


### 9.21.5 Stale Results

If a result references a source record that has changed or become unavailable, the system shall revalidate authorization and source state before displaying protected content.


## 9.22 Suggested API Surface


### 9.22.1 Search Operations

Conceptual operations may include:

• GET /search

• POST /search/query

• POST /search/semantic

• GET /search/suggestions

• GET /search/facets


### 9.22.2 Saved Search Operations

Conceptual operations may include:

• POST /search/saved

• GET /search/saved

• GET /search/saved/{id}

• PUT /search/saved/{id}

• DELETE /search/saved/{id}


### 9.22.3 Search History Operations

Conceptual operations may include:

• GET /search/history

• DELETE /search/history/{id}

• DELETE /search/history

• POST /search/history/{id}/pin


### 9.22.4 Indexing Operations

Internal or administrative operations may include:

• Index source record

• Reindex source record

• Remove source record from index

• Rebuild index

• Rebuild embeddings

• Inspect indexing status


### 9.22.5 AI Retrieval Operations

Internal interfaces may include:

• Hybrid lexical/semantic retrieval

• Retrieve authorized supporting records

• Retrieve related records

• Generate answer with source references

These interfaces are conceptual and do not require a REST architecture.


## 9.23 Proposed Domain Relationships

The conceptual model shall include:

Search Document

├── Source Module

├── Source Record

├── Child Associations

├── Categories

├── Tags

├── Contacts

├── OCR Content

├── AI Summary

├── Search Embedding

├── Authorization Metadata

└── Index Metadata

Saved Search

├── User

├── Query

├── Filters

├── Sort Order

└── Saved Search Metadata

Search History

├── User

├── Query

├── Filters

├── Timestamp

└── Result Metadata


## 9.24 MVP Scope

The initial Search MVP should include:

• Global search interface.

• Cross-module keyword search.

• Phrase search.

• Date filtering.

• Child filtering.

• Module filtering.

• Category filtering.

• Tag filtering.

• Contact filtering.

• Status filtering.

• Search result snippets.

• Highlighted matches where supported.

• Direct navigation to source records.

• Automatic indexing of new records.

• Automatic update of changed records.

• OCR text search where OCR is available.

• Basic natural-language search.

• Basic semantic AI search.

• AI retrieval with source links.

• Search history.

• Saved searches.

• Security-aware results.

• Pagination or incremental loading.


## 9.25 Future Enhancements

Future versions may include:

• Voice search

• Image similarity search

• Facial recognition search

• Handwriting recognition

• AI-powered predictive suggestions

• Predictive search

• Related document recommendations

• Duplicate record detection

• Cross-case search for legal professionals

• Advanced Boolean search builder

• Search analytics

• Advanced query syntax

• Personalized ranking

• Query intent learning

• Saved-search sharing

• Search result collaboration


## 9.26 Acceptance Criteria

The Search module shall be considered functionally complete when:

1. Authorized users can search across all integrated modules from one interface.

2. Searchable source records are indexed automatically.

3. Updated source records are reflected in the index.

4. Deleted or restricted source records stop exposing unauthorized search content.

5. Users can perform keyword searches.

6. Users can perform phrase searches where supported.

7. Users can use supported natural-language queries.

8. Users can apply multiple filters simultaneously.

9. OCR-extracted text is searchable when OCR processing is available.

10. Search results identify their source module.

11. Search results link to the canonical source record.

12. Search snippets do not expose unauthorized content.

13. Search results can be paginated or incrementally loaded.

14. Saved searches can be created and rerun when enabled.

15. Search history can be viewed and managed when enabled.

16. AI can retrieve authorized supporting records through Search.

17. AI responses can reference supporting source records where possible.

18. Semantic search does not bypass source permissions.

19. Search-index failures do not modify or destroy source records.

20. Search remains usable as historical data volume grows.

21. Search suggestions do not reveal unauthorized records.

22. Search results and AI retrieval follow the same security model.


## 9.27 Product Decisions Still Required

Before implementation is locked, Product shall define:

• Primary search engine and indexing architecture.

• Whether Search Documents are stored as a separate projection or generated dynamically.

• Lexical-first, semantic-first, or hybrid default ranking.

• Exact relevance-ranking rules.

• Field weighting.

• Stemming and synonym behavior.

• Partial-word minimum length.

• Wildcard syntax and limits.

• Boolean syntax supported in MVP.

• Natural-language parser responsibilities.

• Semantic embedding provider and model.

• Embedding refresh rules.

• OCR confidence threshold for indexing.

• Whether AI summaries are searchable by default.

• Whether AI-generated content influences ranking.

• Maximum indexing delay after record creation or update.

• Search-result page size.

• Maximum query complexity.

• Search-as-you-type minimum characters and debounce interval.

• Maximum suggestion count.

• Default sort order.

• Available deterministic sort orders.

• Saved Searches included in MVP or future scope.

• Search History included in MVP or future scope.

• Search History retention period.

• Whether users can disable Search History.

• Whether sensitive queries are stored in Search History.

• Whether searches are written to the Audit Log.

• Search analytics retention and privacy rules.

• Whether result counts are hidden when they could reveal restricted data.

• Search index retention behavior after soft deletion.

• Reindex and disaster-recovery strategy.

• Search export capabilities.

• Voice search future architecture.

• Facial-recognition search privacy and consent requirements.

• Cross-case search authorization model for legal professionals.


# 10. AI Assistant


## 10.1 Purpose


### 10.1.1 Overview

The AI Assistant is the intelligence layer of the ClearParent Log platform. It shall provide context-aware assistance across parenting records, communications, evidence, calendar events, expenses, parenting plans, reports, and historical timeline data.

The AI Assistant shall function as an assistant over authorized application data rather than as an independent source of record. AI-generated responses, summaries, recommendations, classifications, and drafts shall remain derived content and shall not silently replace or modify canonical user or source records.


### 10.1.2 Objectives

The AI Assistant shall:

• Answer questions using authorized information stored within the application.

• Reduce the time required to document parenting-related events.

• Improve clarity, neutrality, and organization of written documentation.

• Generate summaries, timelines, and reports.

• Assist users in locating and connecting relevant records.

• Identify trends, patterns, gaps, and potentially related records.

• Help users prepare for meetings, mediation, parenting coaching, and legal proceedings.

• Preserve factual integrity and user control.

• Clearly distinguish AI-generated content from source content.


## 10.2 Functional Scope

The AI Assistant shall include:

• Natural-language interaction.

• Cross-module retrieval.

• Neutral rewriting.

• Summarization.

• Timeline generation.

• Report generation assistance.

• Duplicate and related-record detection.

• Missing-information detection.

• Automatic categorization suggestions.

• Trend and pattern analysis.

• AI-powered search.

• Document analysis.

• Communication drafting assistance.

• Parenting-plan assistance.

• Dedicated AI workspace.

• Conversation history.

• Saved and favorite prompts.

• Generated-content history.

• Security-aware retrieval and generation.

• Audit logging of AI interactions as configured.


## 10.3 AI Interaction Model


### 10.3.1 Conversation

An AI Conversation shall represent a user interaction session with the AI Assistant.

A conversation should support:

• Conversation ID

• User ID

• Title

• Created Date

• Updated Date

• Conversation Status

• Message History

• Referenced Records

• Generated Artifacts

• Saved/Favorite State

• Model or Processing Metadata where appropriate


### 10.3.2 AI Message

Each AI interaction message should support:

• Message ID

• Conversation ID

• Role

• Message Text

• Created Date and Time

• Referenced Source Records

• Generated Content Type

• Retrieval Metadata

• AI Provenance Metadata

• Error or Completion State


### 10.3.3 AI Generated Artifact

AI-generated artifacts may include:

• Summaries

• Rewritten text

• Draft communications

• Reports

• Timelines

• Categorization suggestions

• Related-record suggestions

• Missing-information suggestions

• Insights

Generated artifacts shall be stored separately from canonical source records unless the user explicitly saves an artifact into another module.


### 10.3.4 Source References

AI responses should retain references to supporting records whenever feasible.

A source reference should identify:

• Source Module

• Source Record Type

• Source Record ID

• Display Title

• Relevant Date

• Relevant Excerpt or Match Context where allowed


## 10.4 Natural Language Assistant


### 10.4.1 User Queries

Users shall be able to ask conversational questions without knowing which module contains the answer.

Supported examples include:

• Show every missed parenting exchange.

• Summarize everything that happened in July.

• Find every document mentioning school.

• How many overnights have I had this year?

• Generate a report for my attorney.

• When was the last therapy appointment?

• Show every event involving medical care.


### 10.4.2 Intent Resolution

The AI Assistant shall interpret the user's request and determine whether it requires:

• Search

• Filtering

• Aggregation

• Calculation

• Summarization

• Report generation

• Rewriting

• Document analysis

• Timeline construction

• Parenting-plan retrieval

• Cross-module reasoning


### 10.4.3 Follow-Up Context

Within an active conversation, the AI should preserve relevant conversational context so users can issue follow-up requests such as:

• Show only the school-related items.

• Add the supporting evidence.

• Make the summary shorter.

• Put those events in chronological order.


### 10.4.4 Ambiguity Handling

When a request cannot be resolved reliably from available data, the AI shall identify the ambiguity rather than invent an answer.

The AI may ask for clarification when multiple records, date ranges, children, or interpretations could materially change the result.


## 10.5 Retrieval and Grounding


### 10.5.1 Search Integration

The AI Assistant shall use the Search module as a primary retrieval mechanism for application data.

Retrieval may combine:

• Keyword search

• Structured filters

• OCR search

• Semantic search

• Timeline retrieval

• Direct record lookup


### 10.5.2 Authorization

Only records the authenticated user is authorized to access may be retrieved or used in an AI response.


### 10.5.3 Grounded Responses

Factual claims derived from application data should be grounded in source records whenever possible.

The AI shall not present unsupported assumptions, inferred motives, legal conclusions, or unverified facts as established source data.


### 10.5.4 Source Navigation

Where feasible, AI responses shall provide direct links or references allowing the user to open the underlying records.


### 10.5.5 Missing Evidence

If the available data is insufficient to answer a request confidently, the AI should state that limitation and may identify what additional information would be useful.


## 10.6 Neutral Rewriting


### 10.6.1 Supported Capabilities

The AI shall support rewriting user-provided text to:

• Reduce emotionally charged language.

• Improve neutrality.

• Improve clarity.

• Correct grammar and spelling.

• Remove unnecessary repetition.

• Improve organization.

• Preserve factual meaning.


### 10.6.2 Factual Preservation

Rewriting shall not introduce new facts, dates, allegations, participants, or conclusions that were not present in the source material.


### 10.6.3 User Review

Rewritten text shall be presented as a proposed draft. The user shall retain the ability to edit, regenerate, reject, copy, or save the result.


### 10.6.4 Original Content Preservation

The user's original text shall not be overwritten unless the user explicitly chooses to replace or save over a draft field that permits editing.


## 10.7 Summarization


### 10.7.1 Summary Types

The AI shall support summaries including:

• Daily Summary

• Weekly Summary

• Monthly Summary

• Parenting Time Summary

• Therapy Summary

• School Summary

• Medical Summary

• Communication Summary

• Court Preparation Summary

• Case Summary

• Custom Summary


### 10.7.2 Summary Detail Levels

Users should be able to request:

• Brief

• Standard

• Detailed

The exact length targets for each level are Product Decisions.


### 10.7.3 Summary Sources

Summaries may be generated from:

• A single record

• Multiple selected records

• Search results

• Timeline events

• Date ranges

• Categories

• Tags

• Children

• Contacts

• Entire modules


### 10.7.4 Summary Provenance

Generated summaries should identify their source scope and provide references to supporting records whenever feasible.


## 10.8 Timeline Generation


### 10.8.1 Timeline Sources

AI-generated timelines may include:

• Parenting events

• Communications

• Journal entries

• Court hearings

• Therapy sessions

• School events

• Medical appointments

• Expenses

• Evidence uploads

• Calendar events

• Reimbursements


### 10.8.2 Timeline Parameters

Users should be able to generate a timeline by:

• Date Range

• Topic

• Child

• Category

• Event Type

• Contact

• Source Module

• Keyword


### 10.8.3 Chronology

Timeline output shall order events by relevant event date and time when known.

When exact time is unavailable, the system should preserve the source date and indicate that time is unknown rather than inventing a time.


### 10.8.4 Source Linking

AI-generated timeline items should link back to canonical source records.


## 10.9 Report Generation Assistance


### 10.9.1 Supported Report Types

The AI shall assist with reports including:

• Attorney Reports

• Parenting Coach Reports

• Mediation Summaries

• Court Chronologies

• Parenting Time Reports

• School Reports

• Therapy Reports

• Medical Reports

• Expense Reports

• Custom Reports


### 10.9.2 Report Assembly

The AI may assist with:

• Selecting relevant records

• Organizing sections

• Creating executive summaries

• Building chronologies

• Summarizing communications

• Identifying supporting evidence

• Drafting neutral narrative text

• Creating report-ready headings and descriptions


### 10.9.3 Report Builder Integration

AI-generated report content shall be passed to the Report Builder as proposed content. Users shall review and approve report contents before final generation.


### 10.9.4 Supporting Evidence

The AI may recommend supporting Evidence Records, Journal entries, Communications, Calendar events, Parenting Time records, or other related records.

Recommendations shall not automatically alter report contents without user action.


## 10.10 Duplicate Incident Detection


### 10.10.1 Duplicate Candidates

The AI may identify potential duplicates or closely related records including:

• Journal entries

• Evidence records

• Communications

• Court events

• Expenses

• Parenting exchanges

• Calendar events


### 10.10.2 Duplicate Criteria

Potential duplicate detection may consider:

• Date and time proximity

• Matching participants

• Similar titles

• Similar text

• Same attachment hash

• Same amount and vendor

• Shared evidence

• Similar event attributes


### 10.10.3 User Control

The AI shall only flag or recommend duplicate handling. It shall not merge, delete, or overwrite records automatically.


### 10.10.4 Merge Behavior

If a future merge workflow is implemented, the exact merge rules, source preservation rules, audit behavior, and rollback process are Product Decisions.


## 10.11 Related Record Discovery


### 10.11.1 Relationship Suggestions

The AI may suggest relationships between:

• Journal entries

• Communications

• Evidence

• Parenting Time records

• Exchanges

• Expenses

• Court records

• Medical records

• School records

• Calendar events

• Reports


### 10.11.2 Relationship Basis

Suggested relationships may be based on:

• Shared dates

• Shared children

• Shared contacts

• Shared locations

• Matching keywords

• Semantic similarity

• Existing links

• Shared events


### 10.11.3 Confirmation

Suggested relationships shall not become canonical links unless the user explicitly accepts or saves them where confirmation is required.


## 10.12 Missing Information Detection


### 10.12.1 Supported Gap Detection

The AI may identify records that appear incomplete, including:

• Journal entry without supporting evidence

• Expense without receipt

• Parenting exchange without notes

• Court hearing without outcome

• Medical appointment without documentation

• Timeline gap

• Missing category

• Missing child assignment

• Missing contact association

• Missing follow-up documentation


### 10.12.2 Recommendation Behavior

The AI should explain what appears to be missing and why the additional information may improve record completeness.


### 10.12.3 No Automatic Completion

The AI shall not fill missing fields with invented or inferred values without explicit user approval.


## 10.13 Automatic Categorization


### 10.13.1 Suggested Metadata

The AI may suggest:

• Categories

• Tags

• Importance

• Children

• Contacts

• Event Types

• Related Records

• Report classifications


### 10.13.2 Confidence

AI suggestions may include a confidence indicator or explanation where useful.

The confidence representation and threshold rules are Product Decisions.


### 10.13.3 Confirmation

Users retain full control over accepting, rejecting, or editing AI-generated metadata suggestions.


## 10.14 AI Insights


### 10.14.1 Supported Insights

The AI may analyze historical records for patterns including:

• Parenting time trends

• Communication frequency

• Missed parenting time

• Exchange delays

• Expense trends

• School attendance patterns

• Therapy frequency

• Medical history

• Seasonal patterns

• Recurring event patterns


### 10.14.2 Objective Presentation

Insights shall be presented as observations derived from available records, not as conclusions about intent, character, credibility, diagnosis, or legal outcome.


### 10.14.3 Supporting Records

Insights should provide links or references to supporting records whenever feasible.


### 10.14.4 Calculated Metrics

Where an insight relies on a metric such as parenting hours, late exchanges, expenses, or reimbursement totals, the AI should use the canonical calculation provided by the source module rather than independently inventing a formula.


## 10.15 AI Search


### 10.15.1 Search Behavior

AI Search shall extend the Search module by allowing users to ask contextual questions such as:

• What happened before mediation?

• Show every communication about school.

• How many exchanges were late this year?

• Find all evidence supporting this report.


### 10.15.2 Hybrid Retrieval

AI Search may combine lexical, filtered, OCR, timeline, and semantic retrieval.


### 10.15.3 Result Explanation

The AI may explain why a record was returned, but explanations shall be clearly separated from source facts.


### 10.15.4 Source References

AI Search responses should provide links or references to the supporting records whenever possible.


## 10.16 Document Analysis


### 10.16.1 Supported Documents

Document analysis may support:

• Court Orders

• Parenting Plans

• Medical Records

• School Records

• Emails

• PDFs

• Letters

• Reports

• Receipts

• Other supported Evidence Records


### 10.16.2 Analysis Capabilities

The AI may perform:

• OCR extraction

• Document summarization

• Date extraction

• Name extraction

• Organization identification

• Key event identification

• Action item extraction

• Duplicate detection

• Topic classification

• Relationship suggestions


### 10.16.3 Original Document Preservation

The original document shall remain unchanged. Extracted or AI-generated information shall be stored as derived metadata or proposed data.


### 10.16.4 Extraction Confirmation

Automatically extracted fields that would alter canonical record data shall require user confirmation before being saved, unless an explicitly approved automated workflow exists.


### 10.16.5 OCR Limitations

OCR-derived text shall be treated as derived content and may contain recognition errors. Users shall retain access to the original document for verification.


## 10.17 Communication Assistance


### 10.17.1 Drafting

The AI may assist users in drafting communications based on user instructions and authorized contextual records.


### 10.17.2 Rewrite Capabilities

Communication assistance may include:

• Draft message

• Rewrite message

• Improve professionalism

• Improve clarity

• Reduce emotionally charged language

• Summarize conversation

• Suggest response

• Grammar and spelling review


### 10.17.3 Sending Responsibility

The user shall remain responsible for reviewing and sending any communication.

The AI Assistant shall not automatically send communications unless a future explicitly authorized sending workflow is introduced.


### 10.17.4 Source Distinction

Draft messages generated by AI shall be clearly distinguishable from imported or received communications.


## 10.18 Parenting Plan Assistance


### 10.18.1 Parenting Plan Retrieval

The AI shall be able to use authorized Parenting Plan records and related configured schedule data to answer questions.

Examples include:

• Whose weekend is next?

• What does the parenting plan say about holidays?

• When does the next parenting phase begin?

• How much make-up time is owed?

• Am I following the parenting schedule?


### 10.18.2 Plan References

Responses should reference the relevant parenting-plan provision, page, section, or source record whenever that information is available.


### 10.18.3 Computed Answers

Questions requiring calculations shall use canonical Parenting Time or Calendar calculations where available.


### 10.18.4 Interpretation Limitations

The AI may summarize or explain stored plan language but shall not present legal interpretation, enforceability, or legal advice as authoritative.

If the plan language is ambiguous or the application lacks sufficient configured rules, the AI should state that limitation.


## 10.19 AI Workspace


### 10.19.1 Workspace Features

The AI Workspace should include:

• Conversation history

• Saved prompts

• Favorite prompts

• Suggested prompts

• Generated reports

• Generated summaries

• AI task history

• Recent source references


### 10.19.2 Conversation Continuation

Users shall be able to reopen a previous AI conversation and continue from its stored context, subject to retention and security rules.


### 10.19.3 Saved Prompts

Users may save frequently used prompts for reuse.

Examples include:

• Monthly parenting summary

• Attorney chronology

• Unreimbursed expense summary

• Parenting time compliance summary

• School communication review


### 10.19.4 Generated Artifact Access

Generated artifacts should be accessible from the AI Workspace and from the destination module when explicitly saved there.


## 10.20 User Control and Approval


### 10.20.1 No Silent Modification

The AI shall never silently modify canonical user data.


### 10.20.2 Approval Required

User approval shall be required before AI-generated changes are committed to editable source records.


### 10.20.3 Regeneration

Users shall be able to regenerate AI responses.


### 10.20.4 Editing

Users shall be able to edit generated text before saving where the destination module supports editable content.


### 10.20.5 Rejection

Users shall be able to reject AI suggestions without affecting the original record.


### 10.20.6 Provenance

Saved AI-generated content should retain provenance indicating that AI assisted in its creation where appropriate.


## 10.21 AI Safety and Factual Integrity


### 10.21.1 No Fabrication

The AI shall not invent records, dates, quotations, parenting-plan provisions, evidence, communications, payments, medical facts, school facts, or legal events.


### 10.21.2 Inference Labeling

When the AI makes an inference from available records, the inference shall be clearly distinguished from directly recorded facts.


### 10.21.3 Neutrality

AI-generated case documentation should use neutral, factual language and avoid unnecessary characterization of individuals.


### 10.21.4 Legal and Clinical Boundaries

The AI may assist with organization, summarization, and drafting but shall not represent its output as a substitute for legal, medical, mental-health, or other professional advice.


### 10.21.5 Source Conflict

When source records materially conflict, the AI should present the conflicting records rather than silently choosing one as true unless a canonical source rule exists.


## 10.22 Integration


### 10.22.1 Parenting Journal

The AI may summarize, rewrite, organize, categorize, search, and identify related Journal entries without modifying published source content automatically.


### 10.22.2 Parenting Time

The AI may retrieve Parenting Time records, explain configured metrics, generate summaries, identify patterns, and reference exchanges, denials, missed time, and make-up time.


### 10.22.3 Calendar

The AI may answer questions about authorized events, dates, recurrence, and scheduling data.


### 10.22.4 Timeline

The AI may use Timeline data to construct chronological summaries and identify event sequences.


### 10.22.5 Evidence Management

The AI may summarize evidence, analyze OCR text and transcripts, recommend relationships, and identify potential duplicates while preserving canonical Evidence Records.


### 10.22.6 Communications

The AI may search, summarize, classify, and draft responses to communications while preserving imported source content.


### 10.22.7 Expense Tracking

The AI may categorize expenses, summarize spending, identify missing receipts, answer reimbursement questions, and use canonical financial calculations.


### 10.22.8 Contacts

The AI may use authorized Contact records for entity resolution and contextual responses.


### 10.22.9 Reports

The AI may assist the Report Builder with summaries, chronologies, section organization, and supporting-record selection.


### 10.22.10 Search

The Search module shall serve as a primary retrieval layer for the AI Assistant.


### 10.22.11 Parenting Plan

The AI may retrieve plan language, configured schedules, and parenting-plan metadata.


### 10.22.12 Dashboard

The Dashboard may surface AI-generated summaries, suggested tasks, saved prompts, and recent AI activity where configured.


## 10.23 Security and Permissions


### 10.23.1 Authorization Enforcement

AI retrieval and generation shall respect the same authorization rules as the underlying source records.


### 10.23.2 Restricted Data

The AI shall not reveal restricted source titles, snippets, metadata, attachments, summaries, or inferred details to unauthorized users.


### 10.23.3 Server-Side Enforcement

Authorization shall be enforced before protected source content is provided to the AI processing layer. Client-side filtering alone shall not be sufficient.


### 10.23.4 Encryption

AI interaction data, stored conversations, generated artifacts, and data exchanged with AI processing services shall be protected consistent with platform encryption standards.


### 10.23.5 Provider Data Handling

Retention, training use, geographic processing, and data-handling rules for third-party AI providers are Product, Security, and Legal decisions and shall be documented before production use.


## 10.24 Audit History


### 10.24.1 Auditable Actions

AI-related audit events may include:

• AI query submitted

• Generated artifact created

• Generated artifact saved

• AI suggestion accepted

• AI suggestion rejected where tracked

• AI-generated text inserted into a source record

• Report generated with AI assistance

• Administrative AI configuration change


### 10.24.2 Search History vs AI Audit

User-visible AI conversation history and security/audit records shall be treated as separate concepts.

Deleting a conversation shall not necessarily remove required audit records.


### 10.24.3 Audit Detail

Exact AI prompt retention, response retention, source-reference retention, and audit retention periods are Product and Security decisions.


## 10.25 AI Conversation History


### 10.25.1 History Access

Users should be able to view previous AI conversations according to retention rules.


### 10.25.2 History Actions

Supported actions may include:

• Open conversation

• Rename conversation

• Continue conversation

• Favorite conversation

• Archive conversation

• Delete conversation


### 10.25.3 Conversation Deletion

Deleted conversations should follow the platform's soft-delete and retention rules where applicable.


### 10.25.4 Source Changes

If an AI conversation references a source record that is later changed, restricted, or deleted, the system shall revalidate authorization before showing protected source content through the conversation.


## 10.26 Performance and Scalability


### 10.26.1 Interactive Responses

The AI Assistant should provide responsive interactive behavior for standard questions and drafting tasks.

Exact latency targets are TBD.


### 10.26.2 Long-Running Tasks

Large reports, broad case summaries, document batches, or long timeline generation may be processed as longer-running AI tasks.


### 10.26.3 Data Volume

The system shall support increasing volumes of:

• Conversations

• Messages

• Search results

• Timeline events

• Evidence

• OCR text

• Communications

• Expenses

• Generated reports

• Generated embeddings and derived metadata


### 10.26.4 Isolation

AI processing failures shall not prevent users from accessing or editing canonical application data.


## 10.27 Error Handling


### 10.27.1 Retrieval Failure

If supporting records cannot be retrieved, the AI should identify the limitation rather than fabricate an answer.


### 10.27.2 AI Provider Failure

If the AI provider is unavailable, the system should present an understandable error and preserve the user's unsent or submitted prompt where technically feasible.


### 10.27.3 Partial Results

When only part of the requested source data is available, the AI should identify the scope of the available information.


### 10.27.4 Unsupported Requests

The AI should explain when a requested operation is not supported by the application or available data.


### 10.27.5 Save Failure

A failure to save AI-generated content shall not cause loss or modification of the underlying canonical record.


## 10.28 Suggested API Surface


### 10.28.1 Conversation Operations

Conceptual operations may include:

• POST /ai/conversations

• GET /ai/conversations

• GET /ai/conversations/{id}

• PATCH /ai/conversations/{id}

• DELETE /ai/conversations/{id}

• POST /ai/conversations/{id}/messages


### 10.28.2 Generation Operations

Conceptual operations may include:

• POST /ai/rewrite

• POST /ai/summarize

• POST /ai/timeline

• POST /ai/report

• POST /ai/analyze-document

• POST /ai/categorize

• POST /ai/related-records

• POST /ai/missing-information

• POST /ai/insights


### 10.28.3 Prompt Operations

Conceptual operations may include:

• GET /ai/prompts

• POST /ai/prompts

• PATCH /ai/prompts/{id}

• DELETE /ai/prompts/{id}


### 10.28.4 Generated Artifact Operations

Conceptual operations may include:

• GET /ai/artifacts

• GET /ai/artifacts/{id}

• POST /ai/artifacts/{id}/save

• DELETE /ai/artifacts/{id}


### 10.28.5 Internal Retrieval Operations

Internal interfaces may include:

• Retrieve authorized source records

• Retrieve semantic matches

• Retrieve timeline context

• Retrieve parenting-plan context

• Retrieve canonical metrics

• Build source-reference set

These interfaces are conceptual and do not require a REST architecture.


## 10.29 Proposed Domain Relationships

The conceptual model shall include:

AI Conversation

├── User

├── AI Messages

├── Source References

├── Generated Artifacts

├── Saved/Favorite Metadata

└── Audit Metadata

AI Message

├── Conversation

├── User or Assistant Role

├── Source References

├── Retrieval Metadata

└── Provenance Metadata

AI Generated Artifact

├── Conversation

├── Artifact Type

├── Source References

├── Generated Content

├── Destination Module Reference where saved

└── Provenance Metadata

Saved Prompt

├── User

├── Prompt Text

├── Name

├── Category

└── Favorite Metadata


## 10.30 MVP Scope

The initial AI Assistant MVP should include:

• AI Workspace.

• Natural-language questions over authorized application records.

• Search-based retrieval.

• Source links or references where feasible.

• Neutral rewriting.

• Grammar and clarity assistance.

• Brief, standard, and detailed summaries.

• Basic timeline generation.

• Basic report drafting assistance.

• Communication drafting assistance.

• Parenting-plan question answering from available stored plan data.

• Basic related-record suggestions.

• Basic missing-information suggestions.

• AI categorization suggestions.

• User approval before saving AI-generated changes.

• Conversation history.

• Saved prompts or equivalent reusable prompt capability.

• Security-aware retrieval.

• AI-generated content labeling.

• Audit support for AI actions as configured.


## 10.31 Future Enhancements

Future versions may include:

• Voice conversations

• Speech-to-text documentation

• Real-time meeting assistance

• AI-generated case chronologies

• Predictive reminders

• Parenting-plan compliance monitoring

• Conflict detection

• Evidence quality scoring

• Automated exhibit creation

• AI-generated witness preparation

• Multi-language translation

• Personalized writing style preferences

• Proactive recommendations based on upcoming events

• Advanced agentic workflows

• Cross-document reasoning improvements

• User-configurable AI personas or modes subject to platform safety requirements


## 10.32 Acceptance Criteria

The AI Assistant module shall be considered functionally complete when:

1. Authorized users can ask natural-language questions about application records.

2. AI retrieval respects source-record permissions.

3. AI responses can reference supporting records where feasible.

4. Users can request neutral rewrites without modifying the original text automatically.

5. Users can generate summaries from one or more records.

6. Users can select a summary detail level.

7. Users can generate chronological timelines from authorized records.

8. Users can generate AI-assisted report drafts.

9. Users can use AI to draft or rewrite communications.

10. AI-generated content is clearly distinguishable from user-authored or source content.

11. AI suggestions do not modify canonical records without user approval.

12. AI can suggest categories, tags, children, contacts, event types, or related records where supported.

13. AI can identify potential missing information without inventing field values.

14. AI can identify potential duplicate or related records without automatically merging them.

15. Document analysis preserves the original uploaded document.

16. OCR-derived or extracted values are treated as derived content.

17. Parenting-plan answers reference source plan information where available.

18. AI uses canonical source-module calculations for metrics where available.

19. AI identifies material uncertainty or missing source information instead of fabricating an answer.

20. AI conversations can be stored and reopened according to retention rules.

21. AI processing failures do not modify or destroy canonical source records.

22. AI interactions and saved AI-generated changes can be audited according to configured policy.


## 10.33 Product Decisions Still Required

Before implementation is locked, Product shall define:

• AI provider or providers.

• Model selection strategy.

• Whether multiple model tiers are supported.

• Maximum prompt and context size.

• Retrieval chunking strategy.

• Semantic embedding provider and model.

• Source-ranking and reranking rules.

• Maximum number of source records supplied to an AI request.

• Conversation retention period.

• Whether users can permanently delete AI conversation history.

• Prompt and response audit-retention requirements.

• Whether AI prompts are stored verbatim in audit logs.

• Third-party provider data-retention settings.

• Whether provider training on customer data is permitted; production default should require an explicit approved policy.

• Geographic data-processing requirements.

• AI feature availability by subscription tier.

• Usage limits and rate limits.

• Maximum generated report size.

• Summary length definitions for Brief, Standard, and Detailed.

• Confidence-display model for classifications and extraction.

• Thresholds for duplicate and related-record suggestions.

• Whether AI-generated tags or categories may be bulk accepted.

• Whether AI summaries are automatically indexed by Search.

• Whether AI-generated artifacts create Timeline events.

• Whether AI-generated artifacts are retained after their source records are deleted.

• Whether the AI may proactively surface recommendations without a user prompt.

• Parenting-plan interpretation guardrails.

• Exact legal-disclaimer presentation requirements.

• Whether AI conversation sharing is supported.

• Whether AI-generated content may be exported independently of reports.

• Exact audit events required for AI actions.

• Maximum acceptable latency for interactive requests.

• Long-running task timeout and retry behavior.

• Cost-control and quota behavior.

• Model fallback behavior during provider outages.

• Support for voice, translation, meeting assistance, and agentic workflows in future releases.

11. Report Builder


## 11.1 Purpose


### 11.1.1 Overview

The Report Builder module shall enable users to generate professional, organized reports from authorized information stored throughout ClearParent Log. Reports may be prepared for attorneys, parenting coaches, therapists, mediators, Guardian ad Litems, court proceedings, schools, or personal records.

The Report Builder shall assemble information from multiple modules without requiring users to manually copy source content. The module shall preserve traceability to underlying records and shall distinguish generated report content from canonical source data.


### 11.1.2 Objectives

The Report Builder shall:

• Generate professional reports from application data.

• Support predefined templates and fully custom reports.

• Organize records chronologically or by configurable grouping rules.

• Include supporting evidence and exhibits.

• Support extensive filters and date ranges.

• Allow users to customize report structure and presentation.

• Produce reports suitable for legal, therapeutic, educational, financial, and personal review.

• Minimize manual report preparation.

• Preserve references to underlying source records.

• Require user review of AI-generated content before finalization.


## 11.2 Functional Scope

The Report Builder shall include:

• Report creation.

• Predefined templates.

• Custom report composition.

• Multi-module source selection.

• Filtering and sorting.

• Section ordering.

• Chronological report generation.

• Exhibit management.

• Cover pages.

• Tables of contents.

• Executive summaries.

• Preview and editing.

• Version history.

• Export.

• Secure sharing.

• AI-assisted drafting and summarization.

• Source-record traceability.

• Security-aware report generation.

• Audit support.


## 11.3 Report Data Model


### 11.3.1 Report Record

A Report Record shall represent the canonical saved report definition and its generated outputs.

A Report Record should support:

• Report ID

• Owner/User ID

• Title

• Subtitle

• Report Type

• Template ID

• Description

• Status

• Date Range

• Selected Children

• Selected Modules

• Selected Filters

• Section Configuration

• Exhibit Configuration

• Cover Page Configuration

• Header/Footer Configuration

• Watermark Configuration

• Confidentiality Notice

• Created Date

• Updated Date

• Created By

• Last Modified By

• Current Version

• Generated Output References

• Source Record References

• AI Artifact References

• Sharing State

• Audit Metadata


### 11.3.2 Report Status

Supported report states should include:

• Draft

• Ready for Review

• Finalized

• Exported

• Archived

• Deleted

Exact status transitions are a Product Decision.


### 11.3.3 Canonical Source Behavior

Reports shall reference canonical source records rather than duplicate the source records themselves.

Generated narrative, summaries, formatting, indexes, and exhibit references are report artifacts. Changes to a report shall not silently alter the underlying Journal, Parenting Time, Communication, Evidence, Expense, Calendar, Contact, or Parenting Plan records.


## 11.4 Report Types


### 11.4.1 Predefined Report Types

The system shall provide predefined report templates including:

• Chronological Timeline

• Parenting Journal Summary

• Parenting Time History

• Parenting Plan Compliance

• Parenting Exchange Report

• Missed Parenting Time Report

• Communication Summary

• OFW Communication Report

• School Report

• Medical Report

• Therapy Report

• Parenting Coach Summary

• Guardian ad Litem Summary

• Attorney Report

• Court Preparation Report

• Mediation Summary

• Arbitration Summary

• Expense Report

• Reimbursement Report

• Evidence Summary

• Custom Report


### 11.4.2 Custom Reports

Users shall be able to create a report without selecting a predefined template.

Custom reports should support:

• Custom title

• Custom section order

• Arbitrary combinations of supported source modules

• Custom filters

• Optional summaries

• Optional exhibits

• Optional appendix

• Custom cover page fields

• Custom output settings


## 11.5 Report Source Selection


### 11.5.1 Supported Source Modules

Reports may include authorized information from:

• Parenting Journal

• Parenting Time

• Calendar

• Timeline

• Communications

• Evidence Management

• Expense Tracking

• Contacts

• Court Events

• Therapy

• School

• Medical Records

• AI-generated summaries

• Parenting Plan references


### 11.5.2 Source Selection Rules

The user shall be able to include any supported combination of modules in a single report.

The system shall:

• Exclude records outside the user's authorized scope.

• Preserve a source reference for included records where technically feasible.

• Prevent duplicate insertion of the same canonical source record unless the report design intentionally references it in multiple sections.

• Allow users to include or exclude individual records after initial filtering.


## 11.6 Filtering and Selection


### 11.6.1 Supported Filters

Report filters shall include:

• Date Range

• Child

• Category

• Event Type

• Contact

• Location

• Tags

• Importance

• Status

• Module

• Parenting Plan Phase


### 11.6.2 Combined Filters

Users shall be able to combine multiple filters.

The system shall display the active filter set and the number of records currently included.


### 11.6.3 Manual Inclusion and Exclusion

Users should be able to:

• Include selected matching records.

• Exclude selected matching records.

• Add specific records manually.

• Review included source records before generation.


## 11.7 Report Structure and Layout


### 11.7.1 Layout Options

Users shall be able to configure:

• Cover Page

• Table of Contents

• Executive Summary

• Chronological Timeline

• Group by Category

• Group by Child

• Group by Date

• Group by Event Type

• Section Headers

• Page Numbers

• Headers and Footers

• Watermark

• Confidentiality Notice

• Appendix


### 11.7.2 Section Ordering

Users shall be able to reorder supported report sections before export.


### 11.7.3 Grouping Rules

A report may organize source content by:

• Chronology

• Category

• Child

• Date

• Event Type

• Source Module

The selected grouping shall not modify source data.


## 11.8 Cover Page


### 11.8.1 Cover Page Fields

An optional cover page may include:

• Report Title

• Subtitle

• Date Generated

• Report Date Range

• Child or Children

• Prepared By

• Case Name

• Case Number

• Intended Recipient

• Confidentiality Statement


### 11.8.2 Branding

Company or organization logo support is Future Scope unless separately approved.


## 11.9 Table of Contents

The system shall automatically generate a table of contents for reports configured to include one.

The table of contents should support:

• Major Sections

• Subsections

• Page Numbers

• Hyperlinks for supported digital formats

Table-of-contents entries shall be generated from final report structure rather than manually maintained text.


## 11.10 Chronological Reports


### 11.10.1 Chronological Entry Content

Each chronological entry should support:

• Date

• Time

• Event Type

• Summary

• Child

• Linked Evidence

• Related Contacts

• Notes

• Source Record Reference


### 11.10.2 Ordering

Chronological reports shall order records using the canonical event date/time from the source module where available.

When a precise time is unknown, the report shall preserve the available date and shall not invent a time.


### 11.10.3 Multi-Module Chronology

Chronological reports may combine records from all authorized integrated modules.


## 11.11 Exhibit Management


### 11.11.1 Exhibit Features

The Report Builder shall support:

• Automatic Exhibit Numbering

• Custom Exhibit Labels

• Evidence Index

• Thumbnail Images where supported

• Hyperlinks to Evidence

• Exhibit References

• Exhibit Descriptions

• Appendix placement


### 11.11.2 Numbering Styles

Supported numbering styles should include:

• Exhibit A, Exhibit B, Exhibit C

• Exhibit 1, Exhibit 2, Exhibit 3

Additional numbering formats may be configured in future versions.


### 11.11.3 Evidence References

Exhibits shall reference canonical Evidence Records or other authorized source documents. Report generation shall not alter original evidence files.


### 11.11.4 Exhibit Index

The exhibit index should include:

• Exhibit Label

• Description

• Source Record

• File Type where applicable

• Relevant Date where available

• Report Page or Section Reference where supported


## 11.12 Predefined Templates


### 11.12.1 Attorney Report

The Attorney Report template should prioritize:

• Chronology

• Supporting Evidence

• Parenting Time

• Communications

• Exhibits

• References


### 11.12.2 Parenting Coach Report

The Parenting Coach Report template should prioritize:

• Parenting Time

• Journal Entries

• Activities

• Therapy

• Parenting Goals

• Observations


### 11.12.3 Therapist Summary

The Therapist Summary template should prioritize:

• Therapy Timeline

• Behavioral Observations

• Medical Information

• Journal Entries

• Supporting Notes


### 11.12.4 School Report

The School Report template should prioritize:

• Attendance

• Activities

• Conferences

• Academic Events

• Communications


### 11.12.5 Financial Report

The Financial Report template should prioritize:

• Expenses

• Receipts

• Reimbursements

• Totals

• Charts


### 11.12.6 Template Configuration

Template defaults shall be editable before generation. A template shall act as a starting configuration, not as a locked report.


## 11.13 AI Integration


### 11.13.1 Supported Capabilities

The AI Assistant may assist with:

• Executive summaries

• Neutral rewriting

• Summarization of lengthy timelines

• Section organization

• Missing-evidence suggestions

• Report-title suggestions

• Draft conclusions

• Duplicate-information detection

• Readability improvements


### 11.13.2 Approval

AI-generated report content shall be presented as proposed content.

Users shall approve, edit, regenerate, or reject AI-generated content before finalization.


### 11.13.3 Provenance

Saved AI-generated report content should retain provenance indicating that AI assisted in its creation and should retain references to supporting records where feasible.


### 11.13.4 Limitations

The AI shall not invent facts, source records, quotations, dates, legal conclusions, or evidence.

Generated conclusions shall be framed as summaries or observations based on included records and shall not be represented as authoritative legal findings.


## 11.14 Report Preview


### 11.14.1 Preview Capabilities

Users shall be able to preview a report before export.

Preview should include:

• Live Preview

• Print Preview

• Page Navigation

• Zoom

• Section Navigation

• Edit Before Export


### 11.14.2 Preview Fidelity

The preview should approximate the selected export format as closely as technically feasible.


## 11.15 Editing Before Finalization

Users shall be able to edit report-owned content without modifying canonical source records.

Editable report content may include:

• Report title

• Subtitle

• Executive summary

• Section order

• Section headings

• User-authored narrative

• Exhibit descriptions

• Cover-page fields

• AI-generated draft text

Source quotations or imported source content shall remain traceable to the original records.


## 11.16 Export Formats


### 11.16.1 Supported Formats

The Report Builder shall support export to:

• PDF

• Microsoft Word (.docx)

• HTML

• Plain Text

• Rich Text Format (RTF)

• CSV where appropriate


### 11.16.2 Format Applicability

CSV shall be available only when the selected report content can be meaningfully represented in rows and columns.


### 11.16.3 Future Formats

PowerPoint and eBook formats are Future Scope.


## 11.17 Sharing


### 11.17.1 Supported Actions

Users shall be able to:

• Download reports

• Print reports

• Email reports where an approved email workflow exists

• Generate secure sharing links where secure sharing is enabled

• Password-protect supported exports

• Configure link expiration

• Revoke supported active shares

• Track report downloads where supported


### 11.17.2 Sharing Permissions

Sharing shall not expose source records beyond the selected report contents and authorized sharing scope.


### 11.17.3 Security Controls

Secure sharing may require:

• Authentication

• Expiration

• Password protection

• Download restrictions

• Audit logging

• Watermarking

• Redaction

Exact controls are Product and Security decisions.


## 11.18 Version History


### 11.18.1 Report Versions

Each report version should include:

• Version Number

• Creation Date

• Last Modified Date

• Author

• Generated By

• Export Format

• Source Scope

• Filter Configuration

• Change Summary where available


### 11.18.2 Regeneration

Users shall be able to regenerate a report using updated source information.

Regeneration shall create a traceable new report version or generated artifact rather than silently replacing historical finalized output.


### 11.18.3 Source Changes

If an underlying source record changes after a report is generated, the prior report version shall remain historically identifiable.


## 11.19 Report Finalization

Finalization should indicate that the user has completed review for the current report version.

Finalization shall not:

• Lock or alter source records.

• Certify legal admissibility.

• Certify accuracy beyond the source material.

• Prevent a new report version from being generated.

Exact locking behavior for finalized reports is a Product Decision.


## 11.20 Integration


### 11.20.1 Parenting Journal

Reports may include authorized Journal entries and references to supporting evidence.


### 11.20.2 Parenting Time

Reports may include scheduled/actual parenting time, exchanges, missed time, denials, make-up time, and canonical metrics.


### 11.20.3 Calendar

Reports may include authorized calendar events and scheduling information.


### 11.20.4 Timeline

Timeline records may serve as a chronological source for report assembly.


### 11.20.5 Evidence Management

Evidence Records may be attached or referenced as exhibits without duplicating or modifying original files.


### 11.20.6 Communications

Reports may include imported or logged communications and their source references.


### 11.20.7 Expense Tracking

Reports may include expense, receipt, reimbursement, and canonical financial calculations.


### 11.20.8 Contacts

Contact records may provide participant, provider, school, attorney, therapist, and organization information.


### 11.20.9 AI Assistant

The AI Assistant may generate proposed summaries and organization assistance subject to review.


### 11.20.10 Parenting Plan

Parenting Plan provisions may be referenced where relevant and authorized.


### 11.20.11 Dashboard

Recent reports, draft reports, and report actions may be surfaced on the Dashboard.


## 11.21 Security and Permissions


### 11.21.1 Authorization

The Report Builder shall include only records the requesting user is authorized to access.


### 11.21.2 Derived Content Security

Generated report content shall inherit security restrictions appropriate to its underlying source records.


### 11.21.3 Sensitive Data

Reports may contain medical, school, financial, legal, and child-related information and shall be protected according to platform security requirements.


### 11.21.4 Redaction

Configurable redaction is supported where implemented. Exact redaction scope and whether it is MVP or Future Scope is a Product Decision.


## 11.22 Audit History

Auditable actions may include:

• Report created

• Report edited

• Source selection changed

• Report generated

• AI-generated content accepted

• Report finalized

• Report exported

• Report shared

• Share revoked

• Report deleted

• Report restored

• Report version regenerated


## 11.23 Deletion and Recovery

Reports should follow platform soft-deletion and recovery rules where applicable.

Deleting a Report Record shall not delete canonical source records.

Exact retention duration is a Product Decision.


## 11.24 Performance and Scalability


### 11.24.1 Standard Reports

Standard reports should render and preview responsively for normal case sizes.


### 11.24.2 Long-Running Reports

Large reports with many records, evidence items, exhibits, or AI-generated sections may be processed as long-running generation jobs.


### 11.24.3 Isolation

Report-generation failure shall not modify or destroy source records.


## 11.25 Error Handling

The Report Builder shall:

• Identify inaccessible or missing source records.

• Prevent unauthorized source inclusion.

• Preserve draft configuration after recoverable failures where feasible.

• Report export failures without losing the Report Record.

• Identify unsupported export combinations.

• Avoid fabricating missing report content.


## 11.26 Suggested API Surface


### 11.26.1 Report Operations

Conceptual operations may include:

• POST /reports

• GET /reports

• GET /reports/{id}

• PATCH /reports/{id}

• DELETE /reports/{id}

• POST /reports/{id}/restore


### 11.26.2 Source and Configuration Operations

Conceptual operations may include:

• GET /reports/{id}/sources

• POST /reports/{id}/sources

• DELETE /reports/{id}/sources/{sourceId}

• PATCH /reports/{id}/layout

• PATCH /reports/{id}/filters


### 11.26.3 Generation and Export Operations

Conceptual operations may include:

• POST /reports/{id}/generate

• GET /reports/{id}/preview

• POST /reports/{id}/finalize

• POST /reports/{id}/exports

• GET /reports/{id}/exports


### 11.26.4 Exhibit Operations

Conceptual operations may include:

• GET /reports/{id}/exhibits

• POST /reports/{id}/exhibits

• PATCH /reports/{id}/exhibits/{exhibitId}

• DELETE /reports/{id}/exhibits/{exhibitId}


### 11.26.5 Version and Sharing Operations

Conceptual operations may include:

• GET /reports/{id}/versions

• POST /reports/{id}/regenerate

• POST /reports/{id}/shares

• DELETE /reports/{id}/shares/{shareId}

These interfaces are conceptual and do not require a REST architecture.


## 11.27 Proposed Domain Relationships

Report

├── User

├── Template

├── Report Sections

├── Filters

├── Source Record References

├── Exhibit References

├── AI Generated Artifacts

├── Report Versions

├── Export Artifacts

├── Share Records

└── Audit Metadata

Report Version

├── Report

├── Version Number

├── Source Snapshot Metadata

├── Generated Content

├── Export References

└── Audit Metadata

Exhibit

├── Report

├── Evidence Record

├── Exhibit Label

├── Description

├── Sort Order

└── Source Reference


## 11.28 MVP Scope

The Report Builder MVP should include:

• Predefined report templates.

• Custom report creation.

• Multi-module source selection.

• Date and module filtering.

• Chronological ordering.

• Cover page.

• Table of contents.

• Basic executive summary.

• Exhibit references.

• PDF export.

• Microsoft Word export.

• Report preview.

• Draft and finalized states.

• Version history.

• AI-assisted summaries and neutral rewriting where AI is enabled.

• User approval of AI content.

• Source-record links/references.

• Security-aware report generation.

• Audit support.


## 11.29 Future Enhancements

Future versions may include:

• Court filing templates

• Jurisdiction-specific formats

• Electronic signatures

• Collaborative report editing

• AI-generated executive briefings

• Automated daily, weekly, or monthly reports

• Scheduled report generation

• Interactive web reports

• Custom branding

• White-label reporting

• Advanced citation and exhibit cross-referencing

• Advanced redaction tools

• PowerPoint export

• eBook export


## 11.30 Acceptance Criteria

The Report Builder shall be considered functionally complete for the approved release scope when:

1. Authorized users can create a report from one or more supported modules.

2. Users can select a predefined template or create a custom report.

3. Users can filter included records by supported criteria.

4. Users can manually include or exclude source records.

5. The system preserves links or references to underlying records where feasible.

6. Chronological reports order records using source dates.

7. Users can configure supported report layout options.

8. Multi-page reports can generate a table of contents.

9. Users can add authorized evidence as exhibits.

10. Exhibit numbering is generated consistently according to the selected style.

11. Users can preview reports before export.

12. Users can edit report-owned content without modifying source records.

13. Users can export supported reports to PDF.

14. Users can export supported reports to Microsoft Word.

15. AI-generated report content requires user review before finalization.

16. AI-generated content is distinguishable from source content.

17. Users cannot include source records they are unauthorized to access.

18. Report generation failure does not modify source records.

19. Report versions retain historical traceability.

20. Users can regenerate a report using updated source information.

21. Finalized reports can be exported without certifying legal admissibility or legal conclusions.

22. Significant report, export, sharing, and AI actions can be audited according to policy.


## 11.31 Product Decisions Still Required

Before implementation is locked, Product, Engineering, Security, and Legal shall define:

• Exact report status lifecycle.

• Which report templates are included in MVP.

• Template customization boundaries.

• Maximum report size.

• Maximum number of source records.

• Maximum number and total size of exhibits.

• Whether report generation uses live source data or a generation-time snapshot.

• Exact behavior when source records are later edited or deleted.

• Default chronology sorting rules for records without time values.

• Duplicate source-record behavior across sections.

• Default section ordering for each template.

• Supported custom section types.

• PDF rendering engine.

• Microsoft Word rendering engine.

• HTML, RTF, Plain Text, and CSV MVP timing.

• Password-protected export implementation.

• Watermark behavior.

• Confidentiality notice defaults.

• Redaction MVP scope.

• Sharing-link expiration defaults.

• Whether emailed reports are sent directly by the platform.

• Download-tracking detail.

• Finalization locking behavior.

• Report version retention period.

• Export retention period.

• Whether exports remain downloadable after source deletion.

• AI model/provider used for report assistance.

• AI-generated conclusion guardrails.

• Whether AI summaries become independently searchable.

• Whether reports create Timeline events.

• Whether report generation can run on a schedule.

• Whether report contents may be shared with users who lack access to the original records.

• Exact audit events and retention requirements.

• Accessibility requirements for generated reports.

• Localization and language support.

• Future jurisdiction-specific legal templates.


# 12. Dashboard


## 12.1 Purpose


### 12.1.1 Overview

The Dashboard shall serve as the primary application home screen and provide a personalized, current overview of the user's authorized parenting records, upcoming events, tasks, recent activity, statistics, financial information, parenting-time information, and AI-generated insights.

The Dashboard shall aggregate information from underlying modules without becoming the canonical source of those records. Selecting dashboard data shall navigate the user to the underlying source record whenever applicable.


### 12.1.2 Objectives

The Dashboard shall:

• Provide a centralized overview of the user's parenting records.

• Surface important information without requiring navigation through multiple modules.

• Display current status and upcoming events.

• Highlight outstanding tasks and reminders.

• Provide key statistics and trends.

• Present AI-generated insights and recommendations.

• Allow dashboard customization.

• Provide direct navigation to underlying records.

• Respect authorization inherited from source modules.


## 12.2 Functional Scope

The Dashboard shall include:

• Current-status summary.

• Upcoming events.

• Today's tasks.

• Recent activity.

• Parenting Time summary.

• Parenting Time exceptions.

• Statistics.

• Expense summary.

• Recent Evidence.

• Recent Communications.

• AI Insights.

• Quick Actions.

• Configurable widgets.

• User-specific layout preferences.

• Date-range controls.

• Child filters.

• Direct source navigation.

• Notifications integration.

• Security-aware data aggregation.


## 12.3 Dashboard Data Model


### 12.3.1 Dashboard Configuration

A user Dashboard Configuration should support:

• Dashboard Configuration ID

• User ID

• Default Dashboard View

• Widget Instances

• Widget Order

• Widget Size

• Widget Visibility

• Collapsed State

• Favorite Widgets

• Default Date Range

• Default Child

• Theme Preference where supported

• Color Scheme Preference where supported

• Refresh Preference

• Created Date

• Updated Date


### 12.3.2 Widget Instance

A Widget Instance should support:

• Widget Instance ID

• Widget Type

• Position

• Size

• Visibility

• Collapsed State

• Widget-specific Filters

• Widget-specific Date Range

• Widget-specific Child Filter

• Refresh State

• Error State


### 12.3.3 Canonical Source Behavior

Dashboard cards, charts, summaries, and counts shall be derived views.

The Dashboard shall not become the canonical owner of:

• Calendar events

• Parenting Time records

• Journal entries

• Evidence records

• Communications

• Expenses

• Reports

• Parenting Plan records

• Notifications


## 12.4 Current Status


### 12.4.1 Supported Status Information

The Dashboard shall be able to display:

• Current Parenting Phase

• Current Parenting Schedule

• Current Parenting Time Status

• Next Scheduled Exchange

• Upcoming Court Dates

• Upcoming Therapy Sessions

• Outstanding Reimbursements

• Open Tasks

• Pending Reports

• Recent AI Insights


### 12.4.2 Source of Truth

Each current-status item shall use the canonical source module's data and calculations.

If the source module cannot determine a value, the Dashboard shall show an unavailable or incomplete state rather than inventing one.


## 12.5 Upcoming Events


### 12.5.1 Supported Event Types

Upcoming events may include:

• Parenting Exchanges

• Parenting Time

• School Events

• Medical Appointments

• Therapy Sessions

• Parenting Coach Meetings

• Court Hearings

• Mediation

• Birthdays

• Holidays

• Calendar Reminders


### 12.5.2 Event Display Fields

Each event should display:

• Title

• Date

• Time

• Child

• Location

• Status

Additional source-specific metadata may be displayed where useful.


### 12.5.3 Navigation

Selecting an event shall open the canonical Calendar or source-module record.


### 12.5.4 Ordering

Upcoming events shall be ordered by date/time unless the user chooses a supported alternative order.


## 12.6 Today's Tasks


### 12.6.1 Supported Task Types

Tasks may include:

• Complete Journal Entry

• Upload Evidence

• Submit Expense

• Review AI Draft

• Respond to Communication

• Prepare for Court

• Review Parenting Plan

• Pay Invoice

• Update Calendar

• Follow-Up Reminder


### 12.6.2 Task Actions

Users shall be able to:

• Open the related record

• Mark supported tasks complete

• Postpone supported tasks

• Dismiss supported suggestions where applicable


### 12.6.3 Task Ownership

The Dashboard shall not create a separate duplicate task if the task already exists in another source module. It should reference the canonical task/reminder where one exists.


## 12.7 Recent Activity


### 12.7.1 Supported Activity

Recent Activity may include:

• Journal Entries

• Evidence Uploads

• Parenting Exchanges

• Calendar Events

• Expense Entries

• Communications

• Reports Generated

• AI Activity

• Document Uploads

• Recent Searches


### 12.7.2 Activity Navigation

Each activity item should provide direct navigation to the underlying source record or relevant feature.


### 12.7.3 Activity Ordering

Recent activity shall be ordered by relevant creation, modification, or event timestamp according to the source type.

Exact ordering precedence is a Product Decision.


## 12.8 Parenting Time Summary


### 12.8.1 Supported Metrics

The Dashboard shall be able to display:

• Parenting Time Scheduled

• Parenting Time Completed

• Parenting Time Missed

• Parenting Time Denied

• Make-Up Time Owed

• Upcoming Parenting Time

• Parenting Time Percentage

• Parenting Plan Compliance


### 12.8.2 Canonical Calculations

The Dashboard shall use Parenting Time module calculations rather than independently defining formulas.


### 12.8.3 Trend Visualization

Graphs should support parenting-time trends over time.

Supported periods may include:

• Last 7 Days

• Last 30 Days

• Last 90 Days

• This Year

• All Time


## 12.9 Parenting Time Exceptions

The Dashboard shall prominently surface:

• Missed Parenting Time

• Parenting Time Denials

• Cancelled Visits

• Late Exchanges

• Outstanding Make-Up Time

Each exception should provide access to:

• Parenting Time record

• Related Journal entry where linked

• Supporting Evidence where linked

• Relevant Communication where linked

• Parenting Plan reference where linked


## 12.10 Statistics


### 12.10.1 Supported Metrics

The Dashboard may display:

• Total Journal Entries

• Total Parenting Days

• Total Communications

• Total Evidence Items

• Total Reports

• Total Expenses

• Total Reimbursements

• School Events

• Therapy Sessions

• Medical Appointments

• Court Events


### 12.10.2 Reporting Period

Users should be able to select:

• Last 7 Days

• Last 30 Days

• Last 90 Days

• This Year

• All Time

Additional custom ranges may be supported.


### 12.10.3 Metric Ownership

Statistics shall use canonical source-module calculations where a source module defines the metric.


## 12.11 Expense Summary


### 12.11.1 Supported Financial Information

The Dashboard may display:

• Total Expenses

• Monthly Expenses

• Outstanding Reimbursements

• Medical Expenses

• School Expenses

• Activity Expenses

• Recent Receipts

• Spending Trends


### 12.11.2 Visualization

Charts should support:

• Monthly spending

• Category breakdown

• Reimbursement status where useful


### 12.11.3 Financial Calculation Rules

The Dashboard shall use Expense Tracking canonical amounts and reimbursement calculations.


## 12.12 Recent Evidence


### 12.12.1 Supported Evidence Types

Recent Evidence may include:

• Photos

• Videos

• Documents

• Court Orders

• Medical Records

• School Records

• Receipts

• OFW Imports


### 12.12.2 Evidence Display Fields

Each item should display:

• Thumbnail where supported

• File Type

• Upload Date

• Linked Child

• Category


### 12.12.3 Evidence Security

Thumbnail generation and display shall respect Evidence Management authorization and sensitive-content restrictions.


## 12.13 Recent Communications


### 12.13.1 Supported Communication Types

Recent Communications may include:

• OFW Messages

• Emails

• Text Messages

• Phone Calls

• Voice Recordings


### 12.13.2 Communication Display Fields

Each communication should display:

• Contact

• Date

• Summary

• Status

• Related Journal Entry where linked


### 12.13.3 Source Preservation

Dashboard summaries shall not alter imported Communication content.


## 12.14 AI Insights


### 12.14.1 Supported Insights

AI-generated Dashboard suggestions may include:

• Significant events this week

• Upcoming deadlines

• Parenting Time trends

• Missing documentation

• Missing receipts

• Suggested Journal entries

• Possible duplicate records

• Recommended reports

• Outstanding follow-up items


### 12.14.2 User Control

Users shall be able to:

• Dismiss supported suggestions

• Save supported suggestions

• Open supporting records

• Disable supported AI widgets or recommendations


### 12.14.3 AI Boundaries

AI insights shall:

• Be clearly identified as AI-generated.

• Reference supporting records where feasible.

• Not alter source records without user approval.

• Not present inferred motives, diagnoses, or legal conclusions as established facts.

• Use canonical metrics from source modules.


## 12.15 Quick Actions

The Dashboard shall provide shortcuts for supported common actions including:

• New Journal Entry

• Upload Evidence

• Add Expense

• Create Calendar Event

• Log Parenting Exchange

• Generate Report

• Ask AI

• Search Records

• Scan Document

• Record Voice Note

Quick Actions shall open the appropriate source module workflow.


## 12.16 Widget Framework


### 12.16.1 Supported Widgets

Available widgets may include:

• Calendar

• Parenting Time

• Upcoming Events

• Tasks

• Recent Activity

• Statistics

• Expense Summary

• Communications

• Evidence

• AI Insights

• Reports

• Notes

Weather is Future Scope.


### 12.16.2 Widget Actions

Users should be able to:

• Add Widgets

• Remove Widgets

• Resize Widgets

• Reorder Widgets

• Collapse Widgets

• Save Dashboard Layouts


### 12.16.3 Widget Availability

A widget shall not be available to a user if its source module is unavailable or unauthorized.


## 12.17 Dashboard Customization


### 12.17.1 User Preferences

Customization options may include:

• Default Dashboard View

• Favorite Widgets

• Widget Order

• Theme

• Color Scheme

• Default Date Range

• Default Child

• Dashboard Refresh Interval


### 12.17.2 Persistence

Dashboard preferences shall persist per user.


### 12.17.3 Reset

Users should be able to restore a default layout.


## 12.18 Refresh and Data Freshness


### 12.18.1 Automatic Updates

The Dashboard should update as underlying application information changes.


### 12.18.2 Refresh Behavior

The system may use:

• Event-driven updates

• Polling

• Manual refresh

• Cached summaries

The implementation mechanism is an Engineering Decision.


### 12.18.3 Stale Data

If displayed information may be stale, the Dashboard should expose the most recent refresh time or an appropriate stale-data state.


## 12.19 Navigation

Dashboard content shall provide direct navigation to relevant canonical source records wherever applicable.

Navigation shall preserve user authorization and shall not provide access to records merely because a Dashboard reference exists.


## 12.20 Integration


### 12.20.1 Parenting Journal

The Dashboard may surface recent entries, entry counts, and tasks linked to Journal records.


### 12.20.2 Parenting Time

The Dashboard may surface schedule status, upcoming time, exchanges, exceptions, and canonical metrics.


### 12.20.3 Calendar

The Dashboard may surface upcoming events and reminders.


### 12.20.4 Timeline

The Dashboard may use Timeline information for recent-activity views.


### 12.20.5 Evidence Management

The Dashboard may surface recent evidence and evidence-related tasks.


### 12.20.6 Communications

The Dashboard may surface recent communications and communication-related tasks.


### 12.20.7 Expense Tracking

The Dashboard may surface expenses, reimbursements, receipts, and spending metrics.


### 12.20.8 Contacts

Contact records may provide display information for people and organizations shown in widgets.


### 12.20.9 Reports

The Dashboard may surface draft, pending, and recently generated reports.


### 12.20.10 AI Assistant

The Dashboard may display AI summaries, recommendations, and suggested actions.


### 12.20.11 Search

The Dashboard may provide a Search shortcut and may surface recent search activity if enabled.


### 12.20.12 Parenting Plan

The Dashboard may display the current parenting phase and plan-based milestones where configured.


### 12.20.13 Notifications

The Dashboard may display reminders, unread notifications, and tasks generated from Notifications.


## 12.21 Security and Permissions


### 12.21.1 Authorization

The Dashboard shall display only data the authenticated user is authorized to access.


### 12.21.2 Derived Views

Dashboard aggregates, charts, counts, snippets, and AI insights shall not reveal restricted information from unauthorized source records.


### 12.21.3 Sensitive Data

Sensitive information shall be protected according to platform security requirements.


### 12.21.4 Permission Changes

If a user's source permissions change, Dashboard access shall reflect the change according to the platform's authorization/cache invalidation policy.


## 12.22 Audit History

Significant Dashboard actions may be audited where applicable, including:

• Dashboard preference changes

• Widget configuration changes

• Sensitive source navigation

• AI suggestion acceptance

• Task completion

• Administrative changes to available widgets

Routine page views need not be audited unless required by policy.


## 12.23 Performance and Scalability


### 12.23.1 Initial Load

The Dashboard should prioritize fast rendering of high-value information.

Exact performance targets are Product and Engineering decisions.


### 12.23.2 Progressive Loading

Widgets may load independently so a slow or unavailable source module does not block the entire Dashboard.


### 12.23.3 Caching

Cached summary data may be used where appropriate, but authorization shall be revalidated before restricted information is shown.


## 12.24 Error Handling


### 12.24.1 Widget Failure

If one widget fails, other Dashboard widgets should remain usable.


### 12.24.2 Empty States

Widgets shall provide useful empty states when no matching records exist.


### 12.24.3 Unauthorized Data

Unauthorized records shall be omitted without exposing hidden record titles or metadata.


### 12.24.4 Partial Data

When some source data cannot be loaded, the Dashboard should identify the affected widget or metric rather than presenting incomplete data as complete.


## 12.25 Suggested API Surface


### 12.25.1 Dashboard Configuration

Conceptual operations may include:

• GET /dashboard

• GET /dashboard/configuration

• PATCH /dashboard/configuration

• POST /dashboard/reset


### 12.25.2 Widget Operations

Conceptual operations may include:

• GET /dashboard/widgets

• POST /dashboard/widgets

• PATCH /dashboard/widgets/{id}

• DELETE /dashboard/widgets/{id}

• POST /dashboard/widgets/reorder


### 12.25.3 Dashboard Data

Conceptual operations may include:

• GET /dashboard/status

• GET /dashboard/upcoming-events

• GET /dashboard/tasks

• GET /dashboard/recent-activity

• GET /dashboard/statistics

• GET /dashboard/parenting-time

• GET /dashboard/expenses

• GET /dashboard/evidence

• GET /dashboard/communications

• GET /dashboard/ai-insights

These interfaces are conceptual and do not require a REST architecture.


## 12.26 Proposed Domain Relationships

Dashboard Configuration

├── User

├── Widget Instances

├── Default Date Range

├── Default Child

├── Theme Preferences

└── Refresh Preferences

Widget Instance

├── Dashboard Configuration

├── Widget Type

├── Source Module

├── Widget Filters

├── Layout Metadata

└── Display State

Dashboard View

├── Current Status

├── Upcoming Events

├── Tasks

├── Recent Activity

├── Statistics

├── Parenting Time Summary

├── Expense Summary

├── Evidence Summary

├── Communication Summary

└── AI Insights


## 12.27 MVP Scope

The Dashboard MVP should include:

• Current-status summary.

• Upcoming events.

• Today's tasks.

• Recent activity.

• Parenting Time summary.

• Parenting Time exceptions.

• Core statistics.

• Expense summary.

• Recent Evidence.

• Recent Communications.

• Quick Actions.

• Configurable widgets.

• Widget reorder.

• Widget show/hide.

• Default date range.

• Default child.

• Direct source navigation.

• Security-aware aggregation.

• Basic AI Insights where AI is enabled.


## 12.28 Future Enhancements

Future versions may include:

• Multiple saved dashboard layouts

• Mobile dashboard widgets

• AI-generated daily briefings

• Interactive charts

• Custom KPI dashboards

• Shared dashboards for legal professionals

• Team dashboards

• Smart notifications

• Voice dashboard summaries

• Home-screen widgets

• Predictive analytics

• Personalized productivity recommendations

• Weather widget


## 12.29 Acceptance Criteria

The Dashboard shall be considered functionally complete for the approved release scope when:

1. Users can open a personalized Dashboard after authentication.

2. Dashboard content contains only authorized data.

3. Users can view current status information from supported source modules.

4. Users can view upcoming events.

5. Users can navigate from an upcoming event to its source record.

6. Users can view and act on supported tasks.

7. Users can view recent activity from supported modules.

8. Users can view canonical Parenting Time metrics.

9. Users can view Parenting Time exceptions.

10. Users can view supported financial summary information.

11. Users can view recent Evidence.

12. Users can view recent Communications.

13. Users can use supported Quick Actions.

14. Users can add, remove, reorder, and collapse supported widgets.

15. Dashboard layout preferences persist per user.

16. Users can select a supported reporting period.

17. Dashboard metrics use canonical source-module calculations.

18. AI insights are clearly labeled and do not modify source records automatically.

19. Selecting Dashboard content does not bypass source-module authorization.

20. Failure of one widget does not prevent other widgets from loading.

21. Empty and partial-data states are understandable.

22. Dashboard changes are reflected after the configured refresh period.


## 12.30 Product Decisions Still Required

Before implementation is locked, Product and Engineering shall define:

• Default Dashboard layout.

• Which widgets are included in MVP.

• Minimum and maximum widget sizes.

• Layout grid behavior.

• Whether multiple saved layouts are MVP or Future Scope.

• Default date range.

• Custom date-range support.

• Default-child behavior.

• Exact current-status fields shown by default.

• Recent-activity time window.

• Recent-activity sort precedence.

• Maximum upcoming-event count.

• Task source-of-truth model.

• Task postponement rules.

• Whether the Dashboard has native tasks or only references source-module tasks.

• Whether recent searches are displayed by default.

• Which charts are included in MVP.

• Chart aggregation periods.

• Dashboard refresh interval.

• Whether users can manually refresh.

• Data-caching duration.

• Stale-data indicators.

• Whether widget-specific filters override global filters.

• Theme and color-scheme customization scope.

• AI Insight default state.

• Whether AI Insights are proactively generated or only displayed when already available.

• AI Insight dismissal and retention behavior.

• Whether Quick Actions vary by role or subscription tier.

• Mobile Dashboard MVP scope.

• Dashboard audit events.

• Performance targets.

• Accessibility requirements.

• Shared/team dashboard roadmap.


# 13. Notifications


## 13.1 Purpose


### 13.1.1 Overview

The Notifications module shall provide timely reminders, alerts, updates, and actionable notices related to parenting schedules, exchanges, legal events, school activities, therapy, medical appointments, deadlines, communications, tasks, expenses, reports, AI recommendations, and system events.

Notifications shall be generated from authorized application events and user-configured preferences. The Notifications module shall not become the canonical source of the underlying event or record.


### 13.1.2 Objectives

The Notifications module shall:

• Remind users of upcoming events and deadlines.

• Alert users to important parenting-related activities.

• Reduce missed appointments and parenting exchanges.

• Notify users of new communications and uploaded documents.

• Support multiple delivery methods.

• Allow users to customize notification preferences.

• Provide actionable notifications that link to the related record.

• Respect quiet hours and category preferences.

• Maintain notification history.

• Protect sensitive information.


## 13.2 Functional Scope

The Notifications module shall include:

• Notification generation.

• Custom reminders.

• Event-based reminders.

• Deadline notifications.

• Task notifications.

• Communication notifications.

• AI-generated notifications.

• In-app delivery.

• Email delivery.

• Mobile push delivery.

• Desktop delivery.

• Digest notifications.

• Notification preferences.

• Quiet hours.

• Read/unread state.

• Archive/delete actions.

• Notification history and search.

• Smart grouping.

• Source-record navigation.

• Security-aware content rendering.

• Audit support.


## 13.3 Notification Data Model


### 13.3.1 Notification Record

A Notification Record should support:

• Notification ID

• User ID

• Notification Type

• Category

• Title

• Body/Summary

• Severity or Priority where supported

• Source Module

• Source Record Type

• Source Record ID

• Action URL or Navigation Target

• Scheduled Delivery Date/Time

• Created Date/Time

• Delivered Date/Time

• Read Date/Time

• Archived Date/Time

• Deleted Date/Time

• Delivery Channels

• Delivery State

• Grouping Key

• Digest Reference where applicable

• Security Classification where supported

• Audit Metadata


### 13.3.2 Reminder Record

A custom or source-generated Reminder should support:

• Reminder ID

• User ID

• Title

• Description

• Source Record Reference

• Due Date/Time

• Reminder Timing

• Recurrence Rule where applicable

• Delivery Methods

• Status

• Snooze/Postpone State

• Created Date

• Updated Date


### 13.3.3 Delivery Attempt

A Delivery Attempt should support:

• Delivery Attempt ID

• Notification ID

• Channel

• Attempt Time

• Result

• Provider Message ID where available

• Error Code/Reason where applicable

• Retry Count


### 13.3.4 Notification Status

Notification states may include:

• Scheduled

• Pending

• Delivered

• Read

• Unread

• Archived

• Deleted

• Failed

• Suppressed

Exact lifecycle behavior is a Product Decision.


## 13.4 Notification Types


### 13.4.1 Supported Categories

Notification categories shall include:

• Reminders

• Upcoming Parenting Time

• Parenting Exchanges

• Parenting Time Denials

• Missed Parenting Time

• Court Hearings

• Mediation

• Arbitration

• Attorney Meetings

• Guardian ad Litem Meetings

• Parenting Coach Meetings

• Therapy Appointments

• Medical Appointments

• School Events

• Extracurricular Activities

• Birthdays

• Holidays

• Expense Reimbursements

• Payment Due Dates

• Subscription Notifications

• AI Recommendations

• System Alerts


### 13.4.2 Category Extensibility

The notification architecture should allow additional categories without requiring a redesign of the notification record model.


## 13.5 Custom Reminders


### 13.5.1 Reminder Creation

Users shall be able to create custom reminders for:

• Complete Journal Entry

• Upload Supporting Evidence

• Submit Expense

• Request Reimbursement

• Review Parenting Plan

• Prepare Court Documents

• Call Attorney

• Review AI Draft

• Follow Up on Communication

• Complete Daily Tasks

• Other user-defined reminders


### 13.5.2 Reminder Configuration

Users should be able to configure:

• Reminder date/time

• Reminder timing relative to an event

• Recurrence where supported

• Delivery methods

• Notes

• Related record

• Priority where supported


### 13.5.3 Reminder Actions

Users should be able to:

• Mark reminder complete

• Snooze/postpone

• Edit

• Delete

• Open the related source record


## 13.6 Parenting Exchange Notifications


### 13.6.1 Supported Exchange Notifications

The system shall support:

• Upcoming Exchange

• Exchange Reminder

• Exchange Location

• Exchange Time

• Late Exchange Alert

• Missed Exchange Alert

• Exchange Modification

• Exchange Cancellation


### 13.6.2 Source Links

Exchange notifications should link directly to:

• Associated Calendar event where applicable

• Parenting Time or Exchange record


### 13.6.3 Late and Missed Alerts

Late or missed exchange alerts shall use the canonical Parenting Time rules and configured lateness thresholds rather than defining independent calculations.


## 13.7 Court and Legal Notifications


### 13.7.1 Supported Legal Events

Notifications may include:

• Court Hearings

• Mediation

• Arbitration

• Attorney Meetings

• Filing Deadlines

• Discovery Deadlines

• Court Document Deadlines

• Evidence Submission Deadlines


### 13.7.2 Notification Content

Legal-event notifications may include:

• Date

• Time

• Location

• Participants

• Required Documents

• Associated Notes

• Source Record Link


### 13.7.3 Limitations

The system shall not infer legal deadlines unless the deadline exists in a source record, configured rule, or user-entered record.


## 13.8 School Notifications

School notifications may include:

• Parent-Teacher Conferences

• School Events

• Report Cards

• Field Trips

• Sports Events

• School Holidays

• Testing Days

• Attendance Issues

• School Meetings

Notifications shall reference the relevant source record when one exists.


## 13.9 Therapy and Parenting Coach Notifications

Notifications may include:

• Individual Therapy

• Family Therapy

• Parenting Coach Sessions

• Psychological Evaluations

• Counseling Appointments

• Follow-Up Sessions

Notifications may include preparation notes and follow-up tasks from authorized source records.


## 13.10 Medical Notifications

Medical notifications may include:

• Doctor Appointments

• Dentist

• Orthodontist

• Vision Appointments

• Vaccinations

• Prescription Refills

• Follow-Up Visits

Sensitive medical details shown in notification previews shall respect privacy settings.


## 13.11 Medication Reminders


### 13.11.1 Medication Reminder Fields

Medication reminders may include:

• Medication Name

• Dosage

• Child

• Time

• Frequency

• Instructions

• Notes


### 13.11.2 Completion State

Users shall be able to mark medication reminders:

• Completed

• Skipped


### 13.11.3 Medical Disclaimer Boundary

Medication reminders shall record user-configured information and shall not independently diagnose, prescribe, change dosage, or determine medical instructions.


### 13.11.4 Recurrence

Medication reminders may use recurring reminder rules.

Exact recurrence behavior and whether medication reminders are included in MVP are Product Decisions.


## 13.12 Deadline Notifications


### 13.12.1 Supported Deadlines

Notifications may include:

• Court Filing Deadlines

• Parenting Plan Milestones

• School Forms

• Expense Reimbursements

• Insurance Claims

• Subscription Renewals

• Report Completion

• Custom Deadlines


### 13.12.2 Lead Time

Users should be able to configure one or more reminder lead times where supported.

Default lead times are a Product Decision.


## 13.13 Task Notifications


### 13.13.1 Supported Tasks

Task notifications may include:

• Complete Journal Entry

• Upload Evidence

• Categorize Records

• Review AI Suggestions

• Respond to Messages

• Finish Report

• Update Calendar

• Add Receipt


### 13.13.2 Actionable Completion

Where the source task supports completion, users shall be able to mark the task complete from the notification.


### 13.13.3 Source Task Integrity

Completing a notification shall update the canonical task/reminder state rather than only hiding the notification when a canonical task exists.


## 13.14 Communication Notifications


### 13.14.1 Supported Communication Notifications

Notifications may include:

• New OFW Message

• New Email

• Imported Text Messages

• New Voice Recording

• Shared Report

• New Document Available


### 13.14.2 Navigation

Communication notifications shall link directly to the associated authorized Communication or shared record.


## 13.15 AI Notifications


### 13.15.1 Supported AI Notifications

AI-generated notifications may include:

• Missing Supporting Evidence

• Missing Receipt

• Duplicate Journal Entries

• Suggested Categories

• Parenting Time Discrepancies

• Timeline Gaps

• Report Ready

• AI Summary Available


### 13.15.2 User Control

Users shall be able to disable AI-generated notifications.


### 13.15.3 AI Safety

AI notifications shall:

• Be clearly identified as AI-generated when appropriate.

• Not silently modify source records.

• Provide source references where feasible.

• Avoid presenting speculation as fact.

• Use canonical source-module calculations for metrics.


## 13.16 Delivery Methods


### 13.16.1 Supported Channels

The system shall support:

• In-App Notifications

• Email

• Push Notifications for supported mobile applications

• Desktop Notifications

• Daily Summary Emails

• Weekly Summary Emails

SMS is Future Scope.


### 13.16.2 Independent Channel Control

Users may enable or disable supported delivery methods independently, subject to mandatory security or system notifications defined by policy.


### 13.16.3 Channel Fallback

Whether failed delivery on one channel triggers another channel is a Product Decision.


## 13.17 In-App Notifications


### 13.17.1 Notification Center

The application should provide an in-app Notification Center.

The Notification Center should support:

• Unread count

• Recent notifications

• Read/unread state

• Archive

• Delete

• Search

• Category filters

• Direct navigation


### 13.17.2 Badge State

Unread badge counts shall reflect the user's visible, non-deleted notification state according to configured rules.


## 13.18 Email Notifications


### 13.18.1 Email Delivery

Email notifications may contain:

• Notification title

• Safe summary

• Date/time

• Call-to-action link

• Account-security messaging where applicable


### 13.18.2 Sensitive Content

Sensitive source details should be minimized in email content unless explicitly permitted by policy.


### 13.18.3 Unsubscribe and Preferences

Non-mandatory email notification categories should respect the user's notification preferences.


## 13.19 Push Notifications


### 13.19.1 Mobile Push

Supported mobile clients may receive push notifications.


### 13.19.2 Device Registration

Push delivery shall use registered device tokens or equivalent platform-specific identifiers.


### 13.19.3 Token Revocation

Invalid or revoked push tokens shall be removed or deactivated according to provider behavior.


### 13.19.4 Lock-Screen Privacy

Sensitive notification content shall not appear on a lock screen unless explicitly enabled by the user and allowed by platform policy.


## 13.20 Desktop Notifications

Desktop notifications may be supported through browser or desktop-client notification capabilities.

Users shall be able to enable or disable desktop delivery where platform permission permits.


## 13.21 Notification Preferences


### 13.21.1 Preference Options

Users shall be able to configure:

• Enable or Disable Individual Notification Types

• Quiet Hours

• Notification Frequency

• Delivery Method

• Reminder Timing

• Daily Digest

• Weekly Digest

• Emergency Notifications Only


### 13.21.2 Preference Scope

Preferences may be configured:

• Globally

• By notification category

• By delivery method

Whether per-record preferences are supported is a Product Decision.


### 13.21.3 Precedence

More specific preferences should override global preferences unless a notification is mandatory under platform-security policy.

Exact precedence rules are a Product Decision.


## 13.22 Quiet Hours


### 13.22.1 Quiet-Hour Behavior

During quiet hours, non-urgent notifications may be delayed, grouped, or limited to in-app storage.


### 13.22.2 Exceptions

Whether certain categories can bypass quiet hours is a Product Decision.


### 13.22.3 Time Zone

Quiet hours shall use the user's configured local time zone.

Daylight-saving behavior shall follow the platform's date/time rules.


## 13.23 Notification Timing and Scheduling


### 13.23.1 Relative Reminders

Notifications may be scheduled relative to:

• Event start

• Event end

• Deadline

• Parenting exchange

• Task due date


### 13.23.2 Exact Delivery

The system shall calculate scheduled delivery times using the source record's time zone and user preference rules.


### 13.23.3 Source Changes

If a source event is rescheduled or cancelled, dependent pending notifications shall be recalculated, updated, or cancelled according to notification policy.


## 13.24 Notification History


### 13.24.1 History Actions

Users shall be able to:

• View Notification History

• Mark Notifications as Read

• Mark Notifications as Unread

• Archive Notifications

• Delete Notifications

• Search Notification History


### 13.24.2 History Scope

Notification history shall preserve the notification event and delivery state independently from the source record, subject to retention rules.


### 13.24.3 Source Availability

If a source record is later deleted or access is removed, the notification shall not expose restricted source content through historical previews.


## 13.25 Smart Notifications


### 13.25.1 Grouping

The system should reduce unnecessary notifications by grouping related events.

Examples include:

• Multiple school events combined into one notification

• Daily task summary

• Weekly parenting schedule summary

• Combined court preparation reminder

• Upcoming weekend parenting summary


### 13.25.2 Grouping Safety

Grouping shall not merge unrelated source records in a way that obscures critical deadlines or event-specific actions.


### 13.25.3 Grouping Rules

Exact grouping windows, thresholds, and categories are Product Decisions.


## 13.26 Digests


### 13.26.1 Daily Digest

A Daily Digest may summarize:

• Upcoming events

• Outstanding tasks

• New communications

• Deadlines

• AI recommendations where enabled


### 13.26.2 Weekly Digest

A Weekly Digest may summarize:

• Upcoming parenting schedule

• Major calendar events

• Open tasks

• Reimbursement items

• Reports

• Recent activity


### 13.26.3 Digest Scope

Digests shall include only authorized information and shall respect category and delivery preferences.


## 13.27 Notification Actions


### 13.27.1 Supported Actions

Depending on notification type, actionable notifications may support:

• Open Record

• Mark Complete

• Snooze

• Dismiss

• Mark Read

• Archive

• Review

• Download Report where authorized


### 13.27.2 Authorization

Actions initiated from notifications shall revalidate authorization before modifying or opening the source record.


## 13.28 Integration


### 13.28.1 Parenting Journal

Notifications may remind users to complete Journal entries or notify them of relevant Journal-related tasks.


### 13.28.2 Parenting Time

Notifications may be generated for parenting time, exchanges, denials, missed time, make-up time, and schedule changes.


### 13.28.3 Calendar

Calendar events and reminders may generate notifications using Calendar source data.


### 13.28.4 Timeline

Timeline events may provide context for notifications but Timeline itself need not duplicate delivery logic.


### 13.28.5 Evidence Management

Notifications may remind users to upload supporting Evidence or inform them about completed evidence processing where configured.


### 13.28.6 Communications

New or imported Communications may generate notifications.


### 13.28.7 Expense Tracking

Notifications may be generated for reimbursements, due dates, missing receipts, and recurring financial items.


### 13.28.8 Contacts

Contact information may be used for display context but shall not determine notification authorization.


### 13.28.9 AI Assistant

The AI Assistant may propose notifications and reminders subject to user preference and approval rules.


### 13.28.10 Report Builder

Notifications may be generated when a report is ready, requires review, or has a configured deadline.


### 13.28.11 Dashboard

Dashboard widgets may display unread notifications, reminders, and tasks.


### 13.28.12 Parenting Plan

Configured parenting-plan milestones and schedule events may generate reminders.


## 13.29 Security and Privacy


### 13.29.1 Authorization

Notification content shall include only information the user is authorized to access.


### 13.29.2 Lock-Screen Protection

Notifications shall never expose sensitive information on a device lock screen unless explicitly enabled by the user.


### 13.29.3 Secure Storage

Notification preferences, history, and delivery data shall be securely stored.


### 13.29.4 Source Permission Changes

If permission to a source record is removed, future notification rendering shall not reveal protected source content.


### 13.29.5 Delivery Provider Minimization

Only the minimum data necessary for the selected delivery channel should be sent to third-party delivery providers.


## 13.30 Audit History

Auditable events may include:

• Notification preference change

• Reminder created

• Reminder edited

• Reminder deleted

• Notification generated

• Notification delivery failure

• Notification action completed

• Mandatory security notification sent

• Administrative notification-policy change

Routine read/unread changes may or may not be audited depending on policy.


## 13.31 Performance and Reliability


### 13.31.1 Delivery Reliability

The system should support durable notification scheduling so that application restarts do not silently discard pending notifications.


### 13.31.2 Retry

Failed delivery attempts may be retried according to channel-specific policy.


### 13.31.3 Idempotency

The system should prevent accidental duplicate delivery caused by processing retries where technically feasible.


### 13.31.4 Delivery Timing

Exact delivery latency targets are Product and Engineering decisions.


## 13.32 Error Handling


### 13.32.1 Channel Failure

Failure of one delivery channel shall not corrupt the Notification Record.


### 13.32.2 Invalid Destination

Invalid email addresses, push tokens, or unsupported devices shall generate an appropriate delivery-failure state.


### 13.32.3 Source Failure

If a source record cannot be retrieved, the system shall avoid including unverified source details.


### 13.32.4 Preference Conflict

If a notification is suppressed by user preferences, the system may record the suppression reason.


## 13.33 Suggested API Surface


### 13.33.1 Notification Operations

Conceptual operations may include:

• GET /notifications

• GET /notifications/{id}

• PATCH /notifications/{id}

• DELETE /notifications/{id}

• POST /notifications/{id}/read

• POST /notifications/{id}/unread

• POST /notifications/{id}/archive


### 13.33.2 Reminder Operations

Conceptual operations may include:

• POST /reminders

• GET /reminders

• GET /reminders/{id}

• PATCH /reminders/{id}

• DELETE /reminders/{id}

• POST /reminders/{id}/complete

• POST /reminders/{id}/snooze


### 13.33.3 Preference Operations

Conceptual operations may include:

• GET /notification-preferences

• PATCH /notification-preferences

• GET /notification-preferences/categories/{category}

• PATCH /notification-preferences/categories/{category}


### 13.33.4 Delivery Operations

Internal operations may include:

• Schedule Notification

• Deliver In-App Notification

• Deliver Email Notification

• Deliver Push Notification

• Deliver Desktop Notification

• Retry Failed Delivery

• Build Digest

• Suppress Notification

These interfaces are conceptual and do not require a REST architecture.


## 13.34 Proposed Domain Relationships

Notification

├── User

├── Notification Type

├── Source Record Reference

├── Delivery Attempts

├── Action Target

├── Grouping Metadata

└── Audit Metadata

Reminder

├── User

├── Source Record Reference

├── Recurrence Rule

├── Delivery Preferences

└── Completion State

Notification Preference

├── User

├── Category

├── Delivery Channel

├── Quiet Hours

├── Frequency

├── Digest Settings

└── Reminder Timing

Delivery Attempt

├── Notification

├── Channel

├── Provider Reference

├── Result

└── Retry Metadata


## 13.35 MVP Scope

The Notifications MVP should include:

• In-App Notifications.

• Email Notifications.

• Core event reminders.

• Parenting Exchange reminders.

• Court/legal reminders.

• School reminders.

• Therapy reminders.

• Medical appointment reminders.

• Deadline reminders.

• Task reminders.

• Communication notifications.

• Expense/reimbursement reminders.

• Notification preferences.

• Category enable/disable.

• Reminder timing.

• Quiet hours.

• Notification history.

• Read/unread.

• Archive.

• Direct source links.

• Daily or weekly digest support if approved for MVP.

• Basic AI notifications where AI is enabled.

• Security-aware notification rendering.


## 13.36 Future Enhancements

Future versions may include:

• SMS

• Smart watch delivery

• Voice assistant integration

• Slack notifications

• Microsoft Teams notifications

• Calendar invitations

• Geofenced exchange reminders

• Traffic-aware departure alerts

• AI-generated daily briefings

• Predictive reminders based on user behavior

• Escalation notifications for overdue tasks

• Advanced cross-channel fallback

• More sophisticated smart-grouping rules


## 13.37 Acceptance Criteria

The Notifications module shall be considered functionally complete for the approved release scope when:

1. Users receive supported in-app notifications for configured events.

2. Users can enable or disable supported notification categories.

3. Users can configure supported delivery methods.

4. Users can configure reminder timing.

5. Users can configure quiet hours.

6. Parenting Exchange notifications link to the underlying record.

7. Court/legal notifications use configured source records and do not invent deadlines.

8. School, therapy, and medical reminders can be generated from supported events.

9. Users can create custom reminders.

10. Users can mark supported reminders complete.

11. Users can snooze/postpone supported reminders.

12. Users can view notification history.

13. Users can mark notifications read or unread.

14. Users can archive notifications.

15. Users can search notification history.

16. Communication notifications link to the underlying authorized communication.

17. AI-generated notifications can be disabled.

18. AI notifications are distinguishable from source facts.

19. Notifications shown on lock screens follow privacy settings.

20. Notification actions revalidate authorization before opening or modifying source records.

21. Source-event changes update or cancel pending dependent notifications according to policy.

22. Failed delivery attempts do not corrupt the notification record.

23. Notification retries do not create uncontrolled duplicate notifications.

24. Notification preferences persist securely.

25. Digests contain only authorized information.

26. Significant notification preference and administrative actions can be audited according to policy.


## 13.38 Product Decisions Still Required

Before implementation is locked, Product, Engineering, Security, and Legal shall define:

• Exact notification lifecycle states.

• Which notification categories are enabled by default.

• Mandatory notifications that users cannot disable.

• In-app Notification Center retention period.

• Notification deletion vs archival behavior.

• Default reminder lead times.

• Maximum number of reminder lead times per event.

• Recurring reminder capabilities.

• Medication Reminder MVP status.

• Medication Reminder recurrence rules.

• Quiet-hour default values.

• Categories permitted to bypass quiet hours.

• User time-zone precedence.

• Push provider and mobile-platform strategy.

• Email provider.

• Desktop notification browser/platform support.

• SMS roadmap and provider.

• Delivery retry policy.

• Maximum retry count.

• Channel fallback behavior.

• Notification de-duplication window.

• Smart-grouping rules and time windows.

• Digest generation time.

• Daily Digest default state.

• Weekly Digest default state.

• Digest content-selection rules.

• Whether digests suppress individual notifications.

• Emergency Notifications Only semantics.

• Per-record notification preferences.

• Category/global preference precedence.

• Notification priority/severity model.

• Lock-screen redaction defaults.

• Email content redaction defaults.

• Push payload data-minimization rules.

• Whether AI may proactively create reminders.

• AI notification default state.

• AI notification confidence/threshold rules.

• Whether Report Ready is a system or AI notification.

• Whether notification history creates Timeline events.

• Which read/unread actions are auditable.

• Notification-provider data-retention rules.

• Delivery latency targets.

• Scheduled-job architecture.

• Accessibility requirements.

• Localization and language support.


# 14. Security


## 14.1 Purpose


### 14.1.1 Overview

The Security module provides the platform-wide controls required to protect ClearParent Log accounts, parenting records, legal documents, medical information, communications, financial information, evidence, reports, and AI interactions.

Security shall be treated as a cross-cutting architectural responsibility. Controls defined by this module shall apply consistently to all application modules and shall protect confidentiality, integrity, availability, auditability, recoverability, and user control.


### 14.1.2 Objectives

The Security module shall:

• Protect application data from unauthorized access.

• Preserve the integrity of canonical records and original uploaded files.

• Provide traceability for significant account, record, administrative, export, sharing, and AI actions.

• Support secure authentication, authorization, session management, and device management.

• Support version history, deleted-item recovery, backups, and disaster recovery.

• Provide configurable privacy, retention, and sharing controls.

• Monitor security-significant events and notify users where appropriate.

• Support applicable security and privacy requirements without representing readiness work as certification.


## 14.2 Functional Scope

The Security module shall include:

• Encryption in transit and at rest.

• Secure credential and secret handling.

• Authentication and account-protection controls.

• Authorization and role-based access control.

• Audit history.

• Version history.

• Deleted-item recovery.

• Backup and recovery controls.

• Access logging.

• Device management.

• Session management.

• Data-integrity controls.

• Retention controls.

• Privacy and secure-sharing controls.

• Security monitoring and alerts.

• Security administration and policy configuration.

• Cross-module enforcement.


## 14.3 Security Architecture


### 14.3.1 Defense in Depth

The application shall use multiple layers of security so that failure of a single control does not automatically expose protected information. Controls may exist at the identity, application, API, data, storage, network, and operational layers.


### 14.3.2 Least Privilege

Users, services, background jobs, administrators, and integrations shall receive only the minimum permissions required to perform authorized functions.


### 14.3.3 Secure Defaults

New accounts, records, sharing features, integrations, and administrative settings shall default to the more restrictive configuration unless the user explicitly enables broader access.


### 14.3.4 Server-Side Enforcement

Authorization, ownership, tenancy, and record-access checks shall be enforced by trusted server-side services. Client-side visibility controls shall not be considered sufficient authorization.


## 14.4 Encryption and Key Management


### 14.4.1 Encryption in Transit

All authenticated application traffic and transmission of sensitive information shall use TLS or an equivalent modern encrypted transport. Insecure transport shall not be accepted for authenticated application workflows.


### 14.4.2 Encryption at Rest

Sensitive application data shall be encrypted at rest, including database records, file storage, backups, derived AI content, and other persistent storage where technically applicable.


### 14.4.3 Protected Data Types

Protected data includes, at minimum:

• Journal entries.

• Evidence and attachments.

• Communications.

• Medical and school records.

• Financial information.

• Contacts.

• Parenting plans and court documents.

• Reports.

• AI conversations and generated artifacts.

• Authentication and account-security data.


### 14.4.4 Password Storage

Passwords shall never be stored in plaintext or reversibly encrypted. Password credentials shall be processed using an industry-standard adaptive password hashing mechanism with appropriate salts and parameters.


### 14.4.5 Key and Secret Management

Encryption keys, API credentials, signing keys, service credentials, payment-provider secrets, and AI-provider secrets shall be stored through approved secret-management mechanisms rather than application source code or user-visible configuration.


### 14.4.6 Key Rotation

The platform should support controlled secret and key rotation. Rotation schedules, key hierarchy, and customer-managed key support are Product and Security decisions.


## 14.5 Authentication


### 14.5.1 Supported Authentication

Authentication shall support the capabilities defined by User Management, including:

• Email or username and password.

• Email verification.

• Password reset.

• Multi-factor authentication where enabled.

• Device verification where enabled.

• Session timeout.

• Remember-me behavior where enabled.

• Account lockout or equivalent abuse protection.

• Brute-force protection.


### 14.5.2 Authentication Failure Handling

Failed authentication shall not reveal whether a specific account exists beyond what is required by the approved account-recovery experience.


### 14.5.3 MFA

Where MFA is enabled or required, access shall not be granted until the required factor challenge has completed successfully.


### 14.5.4 Reauthentication

Sensitive operations may require recent authentication or MFA. Candidate operations include password changes, account deletion, role changes, data exports, security-setting changes, billing changes, and creation of broadly accessible sharing links. Exact reauthentication rules are Product and Security decisions.


### 14.5.5 Future Authentication

Future authentication methods may include:

• Passkeys.

• Hardware security keys.

• Single Sign-On.

• Biometric authentication where supported by the client platform.

• Approved identity providers.


## 14.6 Authorization and Roles


### 14.6.1 Authorization Model

The application shall support role-based and record-aware access controls. A user's role alone shall not grant access to records outside that user's authorized account, organization, or explicitly shared scope.


### 14.6.2 Protected Capabilities

Authorization shall govern access to, at minimum:

• Journal Entries.

• Evidence.

• Communications.

• Reports.

• Calendar.

• Parenting Time.

• Contacts.

• Expenses.

• Search results.

• Analytics.

• AI features.

• Administration.

• Export and sharing actions.


### 14.6.3 Roles

Potential roles include:

• Parent.

• Attorney.

• Parenting Coach.

• Therapist.

• Guardian ad Litem.

• Mediator.

• Read-Only User.

• Organization Administrator.

• System Administrator.

Exact MVP roles and permission matrices are Product Decisions.


### 14.6.4 Permission Changes

Role and permission changes shall take effect according to a defined invalidation strategy so that an existing session or cached search result does not continue exposing records after access is removed.


## 14.7 Audit History


### 14.7.1 Auditable Actions

Significant actions shall generate audit events, including where applicable:

• Login and logout.

• Authentication failures.

• Password and MFA changes.

• Record creation, modification, restoration, archival, and deletion.

• Evidence upload and download.

• Report generation and export.

• Subscription and billing changes.

• Settings and permission changes.

• AI interactions and saving AI-generated changes.

• Data sharing.

• Import and export operations.

• Administrative actions.


### 14.7.2 Audit Record Fields

An audit event should contain:

• Audit Event ID.

• Timestamp.

• Acting User or System Actor.

• Device or Session ID where available.

• IP Address where appropriate.

• Action.

• Module.

• Target Record Type and ID.

• Previous Value or Change Summary where applicable.

• New Value or Change Summary where applicable.

• Success or Failure.

• Correlation or Request ID where available.


### 14.7.3 Audit Integrity

Application users shall not be able to edit audit history. Administrative access to audit records shall be restricted and audited.

The implementation shall provide tamper-resistant audit storage appropriate to the platform architecture. The term immutable shall not be used as a legal or cryptographic guarantee unless the selected architecture actually provides that property.


### 14.7.4 Audit Retention

Audit retention duration and any legal-hold behavior are Product, Security, and Legal decisions.


## 14.8 Version History


### 14.8.1 Versioned Records

Where versioning is supported, important editable records may include:

• Journal Entries.

• Parenting Time records.

• Calendar Events.

• Reports.

• Contacts.

• Expenses.

• Evidence metadata.

• Parenting Plans.


### 14.8.2 Version Metadata

Each version should support:

• Version number or identifier.

• Date modified.

• Modified by.

• Change summary.

• Previous values where practical.

• Current values.


### 14.8.3 Version Actions

Authorized users may be able to:

• View version history.

• Compare versions.

• Restore an earlier version where the source module permits restoration.

• Download a previous version where applicable.


### 14.8.4 Canonical History

Restoring a previous version shall create a traceable new state rather than erasing the existence of later historical versions.


## 14.9 Deleted Item Recovery


### 14.9.1 Soft Deletion

Supported records should use soft deletion or equivalent recoverable deletion for a configurable retention period unless a record type requires a different policy.


### 14.9.2 Recoverable Record Types

Recoverable records may include:

• Journal Entries.

• Evidence records.

• Calendar Events.

• Expenses.

• Contacts.

• Reports.

• Notes.

• Attachments.

• AI conversations where retention rules permit.


### 14.9.3 Recovery Actions

Authorized users may be able to:

• View deleted items.

• Restore individual items.

• Restore multiple items.

• Request permanent deletion where permitted.


### 14.9.4 Permanent Deletion

Permanent deletion shall require appropriate authorization and may be blocked by retention, backup, audit, subscription, organizational, or legal-hold requirements. Exact behavior is a Product and Legal decision.


## 14.10 Backup and Disaster Recovery


### 14.10.1 Backup Scope

The system shall maintain backups appropriate to the deployed architecture. Backup coverage should include:

• Databases.

• File storage.

• Application configuration required for recovery.

• Other persistent platform data identified by Engineering.


### 14.10.2 Backup Capabilities

Backup capabilities may include:

• Automated backups.

• Incremental backups.

• Full backups.

• Encrypted backups.

• Point-in-time recovery where supported.

• Retention policies.

• Backup verification.

• Disaster-recovery testing.


### 14.10.3 Recovery Objectives

Recovery Point Objective and Recovery Time Objective values are Product and Engineering decisions and shall be documented before production readiness is declared.


### 14.10.4 Backup Isolation

Backup credentials and storage shall be protected from ordinary application-user access. Where practical, backup controls should reduce the risk that a compromised application account can destroy production data and its backups simultaneously.


## 14.11 Access Logs


### 14.11.1 Access Log Fields

Access logs may record:

• Login time.

• Logout time.

• Device.

• Browser.

• Operating System.

• IP Address.

• Approximate geographic location where derived and permitted.

• Session duration.

• Authentication method.

• Success or failure.


### 14.11.2 User Access History

Users should be able to review security-relevant access history for their own account.


### 14.11.3 Administrative Access

Authorized administrators may review access logs for security and operational purposes. Administrative viewing of sensitive access information should itself be auditable.


## 14.12 Device Management


### 14.12.1 Device Record

A registered or recognized device may include:

• Device ID.

• User-assigned Device Name.

• Browser.

• Operating System.

• First Seen.

• Last Activity.

• Last Login.

• Last Known IP Address.

• Trusted Device Status.


### 14.12.2 Device Actions

Users shall be able to perform supported actions including:

• View recognized devices.

• Rename devices.

• Remove or revoke devices.

• Sign out a specific device.

• Sign out all other devices.

• Trust or untrust a device where trusted-device functionality is enabled.


### 14.12.3 Revocation

Revoking a device shall invalidate applicable active credentials or sessions according to the platform's session model.


## 14.13 Session Management


### 14.13.1 Session Record

Session data may include:

• Session ID.

• Login Time.

• Last Activity.

• Logout or Expiration Time.

• Device.

• Browser.

• IP Address.

• Approximate Location where permitted.

• Authentication Method.

• MFA State.

• Session Status.


### 14.13.2 Session Termination

Users shall be able to terminate active sessions according to account-security policy.


### 14.13.3 Expiration

Sessions shall expire based on configured inactivity and/or absolute-lifetime rules. Exact timeout durations are Product and Security decisions.


### 14.13.4 Token Revocation

Password reset, account compromise response, account suspension, or explicit sign-out-all behavior may invalidate outstanding sessions and refresh credentials according to configured policy.


## 14.14 Data Integrity


### 14.14.1 Original Files

Original uploaded evidence and source documents shall not be silently altered. Any preview, OCR text, transcript, thumbnail, redaction, AI analysis, or transformed copy shall be stored as a separate derived artifact or representation.


### 14.14.2 Integrity Verification

The system should support integrity checks appropriate to each record type, including:

• File hashes or checksums for uploaded evidence.

• Validation of uploaded file length and content type.

• Detection of storage or transfer corruption where supported.

• Referential-integrity checks for linked records.


### 14.14.3 Canonical Record Protection

APIs and application services shall reject unauthorized modification attempts and shall validate concurrency or record version where needed to prevent unintended overwrites.


## 14.15 Data Retention


### 14.15.1 Retention Policy

Retention behavior shall be defined by record type and account or organization policy. Supported concepts may include:

• Long-term or permanent storage.

• Configurable retention periods.

• Automatic archival.

• Soft deletion.

• Secure permanent deletion.

• Export before deletion.


### 14.15.2 User Ownership and Control

Users shall have access to supported export and deletion controls for their data subject to contractual, organizational, audit, backup, and legal obligations.


### 14.15.3 Retention Precedence

If user preferences conflict with required audit, security, organizational, backup, or legal-hold retention, the system shall follow the controlling policy and communicate the limitation where appropriate.


## 14.16 Privacy and Secure Sharing


### 14.16.1 Private by Default

Records shall not become accessible to another user solely because that person is referenced in the record. Sharing shall require an explicit authorized action or an organization-level policy.


### 14.16.2 Sharing Controls

Where secure sharing is implemented, supported controls may include:

• Permission-based sharing.

• Secure sharing links.

• Link expiration.

• Password protection.

• Download restrictions.

• Redacted exports.

• Watermarks.

• Revocation.


### 14.16.3 Share Authorization

A sharing operation shall not grant access to underlying records beyond the scope explicitly selected by the user or policy.


### 14.16.4 Revocation

Users with appropriate permission shall be able to revoke active shares where the sharing mechanism supports revocation.


## 14.17 Security Monitoring and Alerts


### 14.17.1 Security Events

The platform should detect or surface security-significant events such as:

• Multiple failed login attempts.

• Login from a new device.

• Login from a materially new location where location monitoring is enabled.

• Unusual or high-volume export activity.

• Large evidence downloads.

• Password changes.

• MFA changes.

• Account-recovery attempts.

• Permission or role changes.


### 14.17.2 User Notifications

Significant account-security events should generate notifications through the Notifications module according to configured policy.


### 14.17.3 Automated Response

The system may automatically rate limit, challenge, revoke, block, or flag suspicious activity according to defined security rules. Exact thresholds and automated responses are Product and Security decisions.


## 14.18 Compliance and Security Standards


### 14.18.1 Security Baseline

Engineering shall use applicable modern secure-development practices, including protection against relevant OWASP application-security risks.


### 14.18.2 Compliance Support

The architecture may be designed to support future compliance initiatives such as:

• SOC 2 readiness.

• GDPR-related privacy requirements.

• CCPA-related privacy requirements.

• PCI-compliant payment processing through approved payment providers.


### 14.18.3 No Certification Claim

The presence of controls in this specification does not itself establish SOC 2 certification, GDPR compliance, CCPA compliance, PCI DSS compliance, legal admissibility, or any other certification or legal status.


## 14.19 Security Administration


### 14.19.1 Administrative Controls

Authorized administrators may configure supported security settings such as:

• Authentication policies.

• MFA requirements.

• Retention policies.

• Sharing settings.

• Session policies.

• Upload restrictions.

• Role and permission assignments.

• Security-notification settings.


### 14.19.2 High-Risk Administrative Actions

High-risk security configuration changes should require elevated authorization and may require reauthentication or MFA.


### 14.19.3 Auditability

Administrative security changes shall be written to the audit log with previous and new values where applicable.


## 14.20 Cross-Module Integration


### 14.20.1 Covered Modules

Security controls shall apply to:

• User Management.

• Parenting Journal.

• Parenting Time.

• Calendar.

• Timeline.

• Evidence Management.

• Communications.

• Expense Tracking.

• Contacts.

• Reports.

• AI Assistant.

• Dashboard.

• Search.

• Notifications.

• Administration.

• Data Import.

• Analytics.

• Mobile Application.


### 14.20.2 Permission Inheritance

Derived views such as Search, Timeline, Analytics, Dashboard, reports, and AI responses shall not reveal data that the requesting user could not access through the canonical source module.


### 14.20.3 Security Events

Modules shall publish or otherwise record security-relevant actions through the platform audit and monitoring mechanisms.


## 14.21 Mobile Security


### 14.21.1 Local Data

Mobile clients shall protect locally cached sensitive information using platform-appropriate secure storage and encryption capabilities.


### 14.21.2 Lock-Screen Privacy

Sensitive notification content shall not be displayed on a device lock screen unless explicitly permitted by user settings and supported by platform policy.


### 14.21.3 Device Authentication

Mobile applications may use supported device biometric or PIN capabilities as an additional local access control without replacing required server authentication.


### 14.21.4 Remote Logout

Remote session or device revocation shall prevent continued authenticated synchronization once revocation takes effect.


## 14.22 AI Security


### 14.22.1 Authorization Before Retrieval

Protected source data shall be authorization-filtered before being supplied to AI processing.


### 14.22.2 Provider Handling

Third-party AI provider retention, training use, geographic processing, logging, and contractual requirements shall be reviewed before production use.


### 14.22.3 Generated Content

AI-generated output shall inherit the access restrictions of its supporting records unless explicitly saved into a destination with an equal or more restrictive authorized security model.


## 14.23 Import and Upload Security


### 14.23.1 Validation

Imported or uploaded files shall undergo applicable type, size, integrity, and security validation before normal processing.


### 14.23.2 Malicious Content

Files identified as malicious or unsafe shall be quarantined, rejected, or otherwise prevented from normal application processing according to security policy.


### 14.23.3 Original Preservation

Security scanning, OCR, previews, and other processing shall not alter the original imported file.


## 14.24 Export Security


### 14.24.1 Export Authorization

Exports shall include only records the requesting user is authorized to export.


### 14.24.2 Sensitive Exports

Large, complete-account, audit, legal, medical, or other sensitive exports may require recent authentication, MFA, or additional confirmation according to configured policy.


### 14.24.3 Export Audit

Security-significant exports shall be auditable.


### 14.24.4 Export Protection

Where supported, exports may use password protection, secure download links, link expiration, or other controls appropriate to the format and use case.


## 14.25 Security Error Handling


### 14.25.1 Safe Errors

Security-related error messages shall provide enough information for legitimate users to recover without exposing sensitive implementation details, credentials, record existence, or authorization rules.


### 14.25.2 Fail Closed

When authorization or policy evaluation cannot be completed reliably, the system should deny access rather than expose protected information.


### 14.25.3 Audit Failures

Failure to write a noncritical audit event shall be surfaced to monitoring and retried where appropriate. Product and Engineering shall define which security-critical operations must fail if required audit recording cannot be completed.


## 14.26 Performance and Availability


### 14.26.1 Security Overhead

Security controls shall be designed so that authentication, authorization, audit, encryption, and integrity checks remain reliable under expected load.


### 14.26.2 Rate Limiting

The platform should support rate limiting or equivalent abuse protection for authentication, password reset, MFA, APIs, file operations, exports, and other sensitive endpoints.


### 14.26.3 Service Degradation

Failure of optional security-enrichment services such as approximate geolocation should not disable core account access when the platform can safely continue without the enrichment.


## 14.27 Suggested API Surface


### 14.27.1 Sessions and Devices

Conceptual operations may include:

• GET /security/sessions

• DELETE /security/sessions/{id}

• POST /security/sessions/revoke-all

• GET /security/devices

• PATCH /security/devices/{id}

• DELETE /security/devices/{id}


### 14.27.2 Audit and Access History

Conceptual operations may include:

• GET /security/audit

• GET /security/access-history

• GET /security/security-events


### 14.27.3 Recovery and Versions

Conceptual operations may include:

• GET /security/deleted-items

• POST /security/deleted-items/{id}/restore

• DELETE /security/deleted-items/{id}/permanent

• GET /records/{type}/{id}/versions

• POST /records/{type}/{id}/versions/{versionId}/restore


### 14.27.4 Sharing and Privacy

Conceptual operations may include:

• POST /shares

• GET /shares

• DELETE /shares/{id}

• PATCH /privacy/preferences


### 14.27.5 Administrative Security

Conceptual operations may include:

• GET /admin/security/settings

• PATCH /admin/security/settings

• GET /admin/security/events

These paths are conceptual and do not require a REST architecture.


## 14.28 Proposed Domain Relationships

The conceptual model may include:

Security Principal

├── User

├── Roles

├── Permissions

├── Devices

└── Sessions

Audit Event

├── Actor

├── Session

├── Action

├── Target Record

├── Before/After Data

└── Correlation Metadata

Security Event

├── User

├── Device

├── Session

├── Event Type

├── Risk Metadata

└── Notification Reference

Version Record

├── Canonical Record

├── Version Number

├── Modified By

├── Change Data

└── Audit Reference

Deleted Item

├── Canonical Record Reference

├── Deleted By

├── Deleted Date

├── Retention Date

└── Restore State


## 14.29 MVP Scope

The initial Security MVP should include:

• TLS for authenticated application traffic.

• Encryption at rest through platform-managed storage and database capabilities.

• Secure password hashing.

• Email verification and password reset protections.

• MFA support if included in User Management MVP.

• Server-side authorization.

• Account and record ownership enforcement.

• Role support required by MVP.

• Session timeout and logout.

• Device/session visibility appropriate to the authentication platform.

• Audit logging for significant record and account actions.

• File-integrity hashing for evidence where required by Evidence Management.

• Soft deletion and restore for supported records.

• Backup and recovery procedures.

• Secure secret management.

• Security-aware Search, Timeline, Analytics, Reports, and AI retrieval.

• Security notifications for critical account events where supported.


## 14.30 Future Enhancements

Future security capabilities may include:

• Hardware security keys.

• Passkeys.

• Enterprise SSO and identity providers.

• End-to-end encrypted sharing.

• Immutable evidence vaults.

• Legal hold functionality.

• Advanced anomaly detection.

• AI-assisted fraud or abuse detection.

• Security health scoring.

• Continuous penetration-testing integrations.

• Geographic access restrictions.

• Customer-managed encryption keys.

• Advanced Data Loss Prevention controls.

• Enterprise Security Information and Event Management integrations.


## 14.31 Acceptance Criteria

The Security module shall be considered functionally complete for the approved release scope when:

1. Protected application endpoints enforce authenticated access.

2. Users cannot access records outside their authorized scope through direct URLs, APIs, Search, Timeline, Analytics, Reports, or AI retrieval.

3. Passwords are not stored in plaintext or reversibly encrypted form.

4. Sensitive traffic uses encrypted transport.

5. Supported sensitive persisted data is encrypted at rest.

6. Significant authentication, account, record, export, sharing, and administrative events are auditable according to configured policy.

7. Users can terminate supported active sessions.

8. Users can revoke supported recognized devices or their associated sessions.

9. Supported deleted records can be restored during their configured recovery period.

10. Supported versioned records retain historical versions rather than silently overwriting history.

11. Original uploaded evidence and documents are preserved when OCR, previews, analysis, or other derived content is created.

12. Backup procedures exist for production data and recovery has been tested according to the approved release plan.

13. Search snippets and AI responses do not expose data the requesting user is unauthorized to view.

14. Sensitive notification content follows lock-screen privacy settings.

15. Sensitive exports enforce source permissions and are auditable where required.

16. Role and permission changes prevent continued unauthorized access after the configured propagation period.

17. Security errors do not expose secrets or sensitive implementation details.

18. User and administrative security-setting changes are persisted and audited where applicable.

19. File integrity checks are available for evidence according to Evidence Management requirements.

20. AI, import, mobile, and sharing workflows follow the same platform security model.


## 14.32 Product Decisions Still Required

Before implementation is locked, Product, Security, Engineering, and Legal shall define:

• Exact MVP authentication methods.

• MFA availability and whether MFA is optional or mandatory for specific users or actions.

• Password policy and account-lockout thresholds.

• Session inactivity and absolute lifetime values.

• Remember-me duration.

• Trusted-device behavior and lifetime.

• Reauthentication requirements for sensitive operations.

• Initial role and permission matrix.

• Organization and external-sharing permission model.

• Audit event inventory and required before/after detail.

• Audit retention duration.

• Tamper-resistance architecture for audit records.

• Versioned record types and version-retention duration.

• Deleted-item recovery duration by record type.

• Permanent-deletion workflow and confirmation requirements.

• Legal-hold requirements.

• Backup frequency and retention.

• Recovery Point Objective.

• Recovery Time Objective.

• Disaster-recovery test cadence.

• Key-management provider and rotation policy.

• Customer-managed encryption-key roadmap.

• File-malware scanning provider and workflow.

• Rate-limit thresholds.

• Security-event detection thresholds.

• New-device and new-location detection behavior.

• Geographic location collection and privacy policy.

• Which security events trigger immediate notifications.

• Export reauthentication and MFA requirements.

• Secure-sharing expiration and revocation defaults.

• Password-protected report implementation.

• Redaction and watermarking scope.

• AI-provider security and data-retention requirements.

• Compliance targets and sequencing.

• Security incident response process.

• Vulnerability-management and penetration-testing cadence.

• Security log access rules for administrators.

• User-facing security-center scope.

• Whether end-to-end encrypted sharing or immutable evidence storage enters future roadmap.

15. Administration


## 15.1 Purpose


### 15.1.1 Overview

The Administration module shall provide a centralized management area for user account maintenance, subscriptions, billing, storage, AI usage, data imports and exports, application settings, and administrative oversight. The module shall support both individual-user administration and elevated system-administrator capabilities while enforcing least-privilege access.


### 15.1.2 Objectives

The Administration module shall:

• Provide centralized account and application management.

• Manage subscriptions, trials, renewals, and billing state.

• Expose storage usage and configurable quotas.

• Track AI usage and quotas.

• Support account-level imports and exports.

• Manage users, roles, permissions, and administrative account state.

• Provide configurable application settings.

• Provide system-health visibility for authorized administrators.

• Audit significant administrative actions.

• Support future organization and enterprise administration.


## 15.2 Functional Scope

The Administration module shall include:

• Subscription Management

• Billing Management

• Storage Management

• AI Usage Management

• Data Export Management

• Data Import Entry Points

• User Management

• Role and Permission Management

• System Settings

• System Monitoring

• Administrative Audit History

• Security Controls

• Administrative APIs and operational tooling


## 15.3 Administration Data Model


### 15.3.1 Administrative Account Context

Administrative functions shall operate within an authenticated user context and, where applicable, an administrative role context.


### 15.3.2 Subscription Record

A Subscription Record should support:

• Subscription ID

• User or Organization ID

• Plan ID

• Status

• Billing Cycle

• Trial Start/End

• Renewal Date

• Auto-Renew State

• Cancellation State

• Pause State

• Created Date

• Updated Date


### 15.3.3 Billing Profile

A Billing Profile should support:

• Billing Profile ID

• User or Organization ID

• Payment Provider Reference

• Billing Address

• Tax Information

• Default Payment Method Reference

• Invoice History References

• Refund History References


### 15.3.4 Storage Allocation

A Storage Allocation should support:

• User or Organization ID

• Total Quota

• Used Storage

• Available Storage

• Storage by Module

• Upload Limits

• Retention Policy Reference

• Storage Alert Thresholds


### 15.3.5 AI Usage Ledger

An AI Usage Ledger should support:

• User or Organization ID

• Usage Period

• Credits Allocated

• Credits Used

• Credits Remaining

• Requests by Feature

• Provider Usage Metadata

• Reset Date


## 15.4 Subscription Management


### 15.4.1 User Capabilities

Users shall be able to:

• View Current Plan

• Upgrade Subscription

• Downgrade Subscription

• Cancel Subscription

• Reactivate Subscription

• Pause Subscription where supported

• Change Billing Cycle where supported

• Manage Trial Period where allowed

• View Subscription Status

• View Renewal Date

• Manage Automatic Renewal


### 15.4.2 Administrative Capabilities

Authorized administrators may:

• View subscriptions

• Modify subscription plans

• Extend trial periods

• Grant complimentary subscriptions

• Suspend subscriptions

• Restore subscriptions


### 15.4.3 Subscription State Integrity

Subscription state changes shall be persisted transactionally and audited. Exact billing-provider behavior, proration, refund handling, pause semantics, and grace periods are Product Decisions.


## 15.5 Billing


### 15.5.1 User Billing Capabilities

Users shall be able to:

• View Billing History

• View Invoices

• Download Receipts

• Update Payment Method

• Change Billing Address

• View Upcoming Charges

• View Payment Status

• View Refund History

• Manage Tax Information


### 15.5.2 Administrative Billing Capabilities

Authorized administrators may:

• Issue Refunds

• View Billing Analytics

• Manage Subscription Pricing

• Review Failed Payments

• Manage Promotional Codes

• Generate Billing Reports


### 15.5.3 Billing Boundaries

The application shall not store raw payment-card data unless explicitly required by the selected payment architecture. Payment-provider tokens or references should be preferred.


## 15.6 Storage Management


### 15.6.1 Storage Visibility

Users shall be able to view:

• Total Storage Used

• Available Storage

• Storage by Module

• Evidence Storage

• Document Storage

• Photo Storage

• Video Storage

• Audio Storage

• Report Storage

• Backup Storage where user-visible


### 15.6.2 Storage Actions

Users may:

• Delete unused files subject to retention rules

• Archive records where supported

• Recover storage space

• Purchase additional storage in Future Scope


### 15.6.3 Administrative Storage Controls

Authorized administrators may configure:

• Storage quotas

• Maximum upload sizes

• Retention policies

• Storage alerts


## 15.7 AI Usage Administration


### 15.7.1 User AI Usage

Users shall be able to view:

• AI Credits Remaining

• AI Credits Used

• Monthly Usage

• Daily Usage

• Usage History

• AI Requests by Feature

• AI Response History where policy permits


### 15.7.2 Administrative AI Controls

Authorized administrators may:

• Allocate AI credits

• Increase usage limits

• Reset quotas

• Monitor AI utilization

• Configure AI service providers where supported


### 15.7.3 AI Usage Boundaries

Usage-based billing for AI features is Future Scope unless separately approved.


## 15.8 Data Export Administration


### 15.8.1 Supported Export Scope

Users shall be able to export selected records or a complete account export including authorized data from:

• Parenting Journal

• Parenting Time

• Timeline

• Calendar

• Communications

• Evidence

• Expenses

• Contacts

• Reports

• Audit History where user-accessible

• AI Conversations


### 15.8.2 Supported Export Formats

Supported formats may include:

• PDF

• Microsoft Word

• CSV

• Excel

• JSON

• ZIP Archive


### 15.8.3 Export Security

Exports shall be authorization-aware, auditable, and generated without altering canonical source records.


## 15.9 Data Import Administration

The Administration area may provide account-level access to Data Import workflows. Import execution and detailed processing rules are defined by the Data Import module.


## 15.10 User Management


### 15.10.1 User Self-Service

Users shall be able to:

• Edit Profile

• Update Contact Information

• Change Password

• Configure Multi-Factor Authentication

• Manage Devices

• Review Login History

• Configure Notifications

• Manage Privacy Settings

• Delete Account

• Export Personal Data


### 15.10.2 Administrative User Management

Authorized administrators may:

• View Users

• Search Users

• Suspend Accounts

• Restore Accounts

• Reset Passwords through approved workflows

• Assign Roles

• Manage Permissions

• View Account Activity

• Disable Accounts


### 15.10.3 Account State

Administrative suspension, disablement, restoration, and deletion shall preserve auditability and shall not silently erase records unless retention policy explicitly requires destruction.


## 15.11 Role Management


### 15.11.1 Supported Roles

The role model may include:

• Parent

• Attorney

• Parenting Coach

• Therapist

• Guardian ad Litem

• Mediator

• Read-Only User

• Organization Administrator

• System Administrator


### 15.11.2 Permission Model

Permissions shall determine which modules, actions, and records each user may access. Exact role defaults and custom-role capability are Product Decisions.


## 15.12 System Settings

Users shall be able to configure supported settings including:

• Theme

• Language

• Time Zone

• Date Format

• Default Dashboard

• Default Report Format

• Notification Preferences

• AI Preferences

• Privacy Settings

• Data Retention Preferences where supported


## 15.13 System Monitoring


### 15.13.1 Administrative Health Metrics

Authorized administrators should be able to view:

• Active Users

• Storage Utilization

• AI Utilization

• Failed Imports

• Failed Exports

• Backup Status

• System Notifications

• Application Version

• Service Status


### 15.13.2 Operational Boundaries

System Monitoring shall expose operational health without bypassing user-data authorization. Enterprise-wide analytics beyond core health monitoring are Future Scope.


## 15.14 Audit Logging


### 15.14.1 Auditable Administrative Actions

The system shall record significant actions including:

• Subscription Changes

• Billing Updates

• User Role Changes

• Permission Changes

• Account Suspension

• Account Restoration

• Data Export

• Data Import

• Storage Changes

• AI Credit Modifications

• Configuration Changes


### 15.14.2 Audit Fields

Each administrative audit record should include:

• Timestamp

• User

• Device where available

• IP Address where available

• Action Performed

• Previous Value where applicable

• New Value where applicable

• Target Record

• Result


## 15.15 Security and Authorization

Administrative functions shall require appropriate authorization and follow least privilege. Sensitive operations such as billing changes, role assignments, account deletion, privilege escalation, and full-account exports should require re-authentication and/or MFA according to security policy.


## 15.16 Error Handling

The Administration module shall:

• Prevent partial subscription-state corruption.

• Surface failed payment operations without inventing success.

• Preserve account settings after recoverable failures.

• Prevent unauthorized administrative actions.

• Provide actionable errors for failed imports, exports, storage operations, and provider failures.


## 15.17 Performance and Scalability

Administrative list views should support pagination and filtering. Long-running export, import, billing-report, or storage-analysis actions may execute asynchronously.


## 15.18 Suggested API Surface


### 15.18.1 Subscription Operations

• GET /admin/subscription

• PATCH /admin/subscription

• POST /admin/subscription/cancel

• POST /admin/subscription/reactivate


### 15.18.2 Billing Operations

• GET /admin/billing/history

• GET /admin/billing/invoices

• PATCH /admin/billing/profile


### 15.18.3 Storage Operations

• GET /admin/storage

• GET /admin/storage/by-module


### 15.18.4 AI Usage Operations

• GET /admin/ai-usage

• GET /admin/ai-usage/history


### 15.18.5 User and Role Operations

• GET /admin/users

• GET /admin/users/{id}

• PATCH /admin/users/{id}

• POST /admin/users/{id}/suspend

• POST /admin/users/{id}/restore

• PATCH /admin/users/{id}/roles


### 15.18.6 Settings Operations

• GET /admin/settings

• PATCH /admin/settings


## 15.19 Proposed Domain Relationships

Administration Context

├── User

├── Roles

├── Permissions

├── Subscription

├── Billing Profile

├── Storage Allocation

├── AI Usage Ledger

├── Import Jobs

├── Export Jobs

├── User Settings

└── Audit Records


## 15.20 MVP Scope

The Administration MVP should include:

• User account settings

• Subscription status and plan management

• Billing history and payment-method management

• Storage usage visibility

• AI usage visibility

• Basic export controls

• Entry points to Data Import

• User self-service security settings

• Core administrative user search and suspension

• Role assignment for supported roles

• System settings

• Administrative audit history

• Basic system-health indicators


## 15.21 Future Enhancements

Future versions may include:

• Multi-organization management

• White-label configuration

• Custom branding

• Enterprise licensing

• API key management

• Webhook management

• Usage analytics

• Feature flag management

• Team administration

• Custom user roles

• Organization billing

• Third-party integration management


## 15.22 Acceptance Criteria

The Administration module shall be considered functionally complete for the approved release scope when:

1. Users can view and manage supported account settings.

2. Users can view current subscription status and supported subscription actions.

3. Users can view billing history and update supported billing information.

4. Users can view storage usage and module breakdowns.

5. Users can view AI usage and remaining quota where applicable.

6. Users can initiate authorized exports.

7. Users can access Data Import from Administration.

8. Users can manage supported security settings.

9. Authorized administrators can search and manage supported user account states.

10. Authorized administrators can assign supported roles and permissions.

11. Sensitive administrative operations enforce configured re-authentication/MFA policy.

12. Significant administrative actions are auditable.

13. Failed operations do not silently report success.

14. Unauthorized users cannot access administrative actions outside their scope.


## 15.23 Product Decisions Still Required

Before implementation is locked, Product, Engineering, Security, and Finance shall define:

• Subscription provider

• Plan catalog and pricing

• Proration rules

• Pause and cancellation semantics

• Trial length and trial-extension policy

• Refund rules

• Promotional-code behavior

• Tax-calculation provider

• Storage quota values

• Maximum upload sizes

• Storage-retention policies

• Storage-alert thresholds

• AI quota model

• AI credit reset cadence

• AI provider configuration boundaries

• Whether AI response history is user-visible

• Full-account export packaging and retention

• User-accessible audit scope

• Account deletion retention/destruction policy

• Default roles and permissions

• Whether custom roles are MVP or Future Scope

• Administrative MFA/re-authentication triggers

• System-monitoring retention

• Enterprise organization model


# 16. Data Import


## 16.1 Purpose


### 16.1.1 Overview

The Data Import module shall enable users to migrate historical parenting records, communications, evidence, documents, calendar data, contacts, and financial information into ClearParent Log while preserving original source files and metadata.


### 16.1.2 Objectives

The Data Import module shall:

• Import historical records from multiple external sources.

• Preserve original files and source metadata.

• Extract searchable content.

• Suggest categories, tags, children, contacts, and related records.

• Detect duplicates before finalization.

• Minimize manual organization.

• Provide review and approval before records become canonical.

• Maintain import history and failure recovery.


## 16.2 Functional Scope

The Data Import module shall include:

• File and archive upload

• Bulk import

• Source validation

• Security scanning

• Metadata extraction

• OCR

• Speech-to-text where applicable

• AI-assisted classification

• Duplicate detection

• Mapping and preview

• User approval

• Import execution

• Partial-failure handling

• Import history

• Reprocessing

• Search indexing

• Timeline creation where configured


## 16.3 Import Data Model


### 16.3.1 Import Job

An Import Job should support:

• Import Job ID

• User ID

• Source Type

• Source Name

• Created Date

• Started Date

• Completed Date

• Status

• Total Items

• Processed Items

• Successful Items

• Failed Items

• Duplicate Items

• Processing Time

• Review State

• Audit Metadata


### 16.3.2 Import Item

An Import Item should support:

• Import Item ID

• Import Job ID

• Original File Reference

• Original File Name

• MIME Type

• File Size

• File Hash

• Source Metadata

• Extracted Text

• OCR Text

• Transcript where applicable

• Suggested Destination Module

• Suggested Category

• Suggested Tags

• Suggested Children

• Suggested Contacts

• Suggested Date

• Duplicate Candidates

• Processing Status

• Validation Errors

• User Mapping Overrides

• Final Record Reference


### 16.3.3 Canonical Source Behavior

Original imported files shall be preserved without modification. Derived OCR, transcript, AI summary, and mapping data shall be stored separately from the original source artifact.


## 16.4 Supported Import Sources


### 16.4.1 Baseline Sources

Supported import sources include:

• OurFamilyWizard exports

• EML and MSG email files

• PDF Documents

• Microsoft Word Documents

• Microsoft Excel

• CSV

• JSON

• ZIP Archives

• Photos

• Videos

• Audio Recordings

• Screenshots

• Text Message Exports

• Call Logs

• ICS Calendar Files

• Medical Records

• School Records

• Court Orders

• Parenting Plans

• Financial Records

• Receipts


### 16.4.2 Connected Sources

The source requirements mention Gmail, Outlook, Google Calendar, Apple Calendar, Outlook Calendar, cloud storage, and connected-account synchronization. Exact MVP support for direct connected-service import is a Product Decision; direct synchronization remains Future Scope unless separately approved.


## 16.5 Import Methods

Users shall be able to use supported methods including:

• Drag and Drop

• File Upload

• Folder Upload where platform support exists

• ZIP Archive Import

• Bulk Import

• Email Import

• Mobile Upload

• Cloud Storage Import where enabled

• Connected Account Synchronization in Future Scope or where approved


## 16.6 Import Processing Pipeline


### 16.6.1 Processing Stages

Each Import Item may pass through:

• File Validation

• Virus/Malware Scanning

• Hash Calculation

• Metadata Extraction

• OCR Processing

• Speech-to-Text

• AI Analysis

• Duplicate Detection

• Category Assignment

• Tag Generation

• Contact Recognition

• Child Recognition

• Date Extraction

• Relationship Discovery

• Timeline Placement Proposal

• Search Index Preparation

• User Review

• Finalization


### 16.6.2 Long-Running Processing

Users should receive progress updates during long-running imports.


### 16.6.3 Isolation

Failure of one Import Item shall not necessarily fail the entire Import Job.


## 16.7 Security Validation

All files shall undergo security validation before downstream processing. Files failing security checks shall be quarantined or rejected according to policy and shall not be processed into application records.


## 16.8 Metadata Extraction

The system shall extract available metadata including:

• Original File Name

• Creation Date

• Modified Date

• Author

• GPS Location

• Camera Information

• File Type

• File Size

• Email Sender

• Email Recipient

• Subject

• Calendar Event Details

Metadata shall remain searchable where permitted.


## 16.9 OCR and Speech-to-Text


### 16.9.1 OCR

OCR may extract text from scanned documents, PDFs, screenshots, receipts, school documents, court documents, and medical records.


### 16.9.2 Speech-to-Text

Audio and supported video may produce derived transcripts.


### 16.9.3 Derived Content

OCR and transcript output shall not modify original files and should retain provenance to the source item.


## 16.10 AI Analysis

AI-assisted processing may provide:

• Document Summarization

• Keyword Extraction

• Name Recognition

• Date Recognition

• Organization Recognition

• Suggested Categories

• Suggested Tags

• Duplicate Detection Assistance

• Related Record Identification

• Timeline Generation Suggestions

• Missing Information Detection

AI output shall be treated as derived suggestions until user-approved where approval is required.


## 16.11 Automatic Categorization

The system should suggest mappings such as:

• Emails → Communications

• OFW Messages → Communications

• Photos → Evidence

• Receipts → Expenses

• Court Orders → Legal Documents

• Parenting Plans → Parenting Plan

• Medical Records → Medical

• School Documents → School

• Calendar Files → Calendar

• Videos → Evidence

• Voice Recordings → Evidence

Users shall be able to modify suggested categories before finalization.


## 16.12 Duplicate Detection


### 16.12.1 Duplicate Signals

Duplicate detection may compare:

• File Hash

• File Name

• Creation Date

• OCR Text

• AI Summary

• Metadata

• Image similarity where supported

• Email identifiers/content

• Communication identifiers/content


### 16.12.2 User Actions

Users should be able to:

• Keep Both

• Skip Duplicate

• Replace Existing where safely supported

• Merge Records where supported


### 16.12.3 Duplicate Safety

The system shall not automatically delete, merge, or replace canonical records based solely on a duplicate suggestion.


## 16.13 Import Mapping

The import wizard should display:

• Source Data

• Destination Module

• Suggested Category

• Suggested Child

• Suggested Contacts

• Tags

• Potential Duplicates

Users shall be able to edit mappings before finalization.


## 16.14 Import Validation

Validation shall include:

• Supported File Types

• File Size Limits

• Required Fields

• Corrupted Files

• Duplicate Records

• Invalid Dates

• Invalid Metadata

• Security validation results

Validation errors shall provide clear resolution guidance.


## 16.15 Import Preview and Approval


### 16.15.1 Preview

Before finalization, the user should be able to review:

• Files to be Imported

• Categories

• Suggested Tags

• AI Summary

• Duplicate Warnings

• Metadata

• Timeline Placement

• Related Records


### 16.15.2 Approval

Records shall not become permanent canonical application records until the import reaches an approved finalization state.


### 16.15.3 Partial Approval

Whether users may approve only selected items within a job is a Product Decision.


## 16.16 Import Finalization

Finalization shall create or link the appropriate canonical records in target modules while preserving references back to the Import Job and source files.


## 16.17 Import History

Import history shall include:

• Import Date

• User

• Source

• Files Imported

• Records Created

• Duplicates Found

• Errors

• Processing Time

• Import Status

Users shall be able to review previous imports and reprocess supported failed items.


## 16.18 Error Handling

The import system shall:

• Continue processing independent items after item-level failures.

• Provide detailed error messages.

• Provide failed-item reports.

• Support retry of failed imports.

• Support partial-import recovery.

• Support import cancellation where technically feasible.

• Never claim an item was imported when finalization failed.


## 16.19 Integration


### 16.19.1 Parenting Journal

Imported journal records may create canonical Journal entries after approval.


### 16.19.2 Communications

Imported OFW messages, emails, texts, call logs, and related records may populate Communications.


### 16.19.3 Evidence Management

Imported photos, videos, audio, documents, screenshots, and source files may create or reference Evidence Records.


### 16.19.4 Timeline

Finalized imported records may create Timeline events according to Timeline rules.


### 16.19.5 Calendar

ICS and supported calendar imports may create Calendar records after review.


### 16.19.6 Expense Tracking

Receipts and financial records may populate Expense Tracking after review.


### 16.19.7 Contacts

Recognized contacts may link to existing Contact records or propose new ones.


### 16.19.8 Search

Finalized imported records and approved searchable derived content shall become searchable.


### 16.19.9 AI Assistant

AI may use finalized authorized imported records and derived artifacts according to AI rules.


### 16.19.10 Parenting Time

Imported parenting-time data may populate Parenting Time when mapping support exists.


## 16.20 Audit History

Auditable import actions should include:

• Import created

• File uploaded

• Validation completed

• Duplicate decision made

• Mapping changed

• AI suggestion accepted/rejected

• Import approved

• Record finalized

• Import cancelled

• Import retried

• Import item failed


## 16.21 Performance and Scalability

Bulk imports may execute asynchronously. Processing shall support resumable job state where feasible and shall avoid loading entire large archives into memory unnecessarily.


## 16.22 Suggested API Surface


### 16.22.1 Import Job Operations

• POST /imports

• GET /imports

• GET /imports/{id}

• POST /imports/{id}/cancel

• POST /imports/{id}/finalize


### 16.22.2 Import Item Operations

• GET /imports/{id}/items

• GET /imports/{id}/items/{itemId}

• PATCH /imports/{id}/items/{itemId}/mapping

• POST /imports/{id}/items/{itemId}/retry


### 16.22.3 Review Operations

• GET /imports/{id}/preview

• POST /imports/{id}/approve


## 16.23 Proposed Domain Relationships

Import Job

├── User

├── Import Items

├── Source Files

├── Duplicate Decisions

├── Mapping Overrides

├── Finalized Record References

└── Audit Metadata

Import Item

├── Import Job

├── Original File

├── Extracted Metadata

├── OCR/Transcript

├── AI Suggestions

├── Duplicate Candidates

├── Destination Mapping

└── Final Record Reference


## 16.24 MVP Scope

The Data Import MVP should include:

• File upload

• Multiple-file import

• ZIP import

• OFW export import

• EML/MSG import where supported

• PDF/document import

• Image/video/audio import

• CSV/Excel import for supported schemas

• ICS import

• Security scanning

• Metadata extraction

• OCR

• Basic duplicate detection

• Suggested categorization

• Import preview

• User approval

• Import history

• Retry failed items

• Finalized integration with core modules

• Search indexing


## 16.25 Future Enhancements

Future versions may include:

• Direct OFW synchronization

• Gmail synchronization

• Outlook synchronization

• Dropbox integration

• Google Drive integration

• OneDrive integration

• iCloud Drive integration

• Automatic scheduled imports

• Mobile device synchronization

• Social media imports

• AI-powered historical timeline reconstruction

• Incremental synchronization

• Background imports with real-time progress notifications


## 16.26 Acceptance Criteria

The Data Import module shall be considered functionally complete for the approved release scope when:

1. Users can create an Import Job from a supported source.

2. Supported files are validated and security scanned before processing.

3. Original files remain preserved without modification.

4. Metadata is extracted where available.

5. OCR is produced for supported documents and images.

6. Duplicate candidates are shown before finalization.

7. Users can review and edit destination mappings.

8. Users can preview import results before approval.

9. Finalization creates records in appropriate target modules.

10. Failed items do not necessarily prevent other items from completing.

11. Users can review Import History.

12. Users can retry supported failed items.

13. Imported records become searchable after finalization.

14. Finalized records preserve traceability to the import source.

15. AI suggestions do not modify original source files.

16. Unauthorized imports or target-record creation are blocked.


## 16.27 Product Decisions Still Required

Before implementation is locked, Product, Engineering, Security, and Data teams shall define:

• Exact supported MIME types

• Maximum file size

• Maximum archive size

• Maximum files per import

• Maximum concurrent imports

• Virus/malware scanning provider

• OCR provider

• Speech-to-text provider

• AI classification provider/model

• Import job retention

• Original-file retention

• Quarantine retention

• Connected-service MVP scope

• OFW export formats supported

• Excel/CSV schema-mapping behavior

• ICS recurrence handling

• Duplicate confidence thresholds

• Merge/replace semantics

• Image-similarity support

• Whether partial approval is supported

• Timeline creation rules for imported records

• Contact auto-creation rules

• Import rollback behavior

• Import cancellation behavior after partial finalization

• Performance targets


# 17. Analytics


## 17.1 Purpose


### 17.1.1 Overview

The Analytics module shall transform authorized parenting records into interactive metrics, charts, comparisons, and trends without becoming the canonical owner of source data.


### 17.1.2 Objectives

The Analytics module shall:

• Provide visual insights into parenting-related data.

• Measure configured parenting-plan and parenting-time metrics.

• Identify trends over time.

• Compare historical periods.

• Generate interactive charts and dashboards.

• Support data-driven review.

• Provide exportable analytics for reports and presentations.


## 17.2 Functional Scope

The Analytics module shall include:

• Executive analytics dashboard

• Parenting Time analytics

• Expense analytics

• Communication analytics

• School analytics

• Therapy analytics

• Compliance analytics

• Evidence analytics

• Journal analytics

• AI usage analytics

• Custom analytics

• Filtering

• Period comparison

• Visualization selection

• Drill-through where supported

• Export

• AI-generated insights

• Security-aware aggregation


## 17.3 Analytics Data Model


### 17.3.1 Analytics Query

An Analytics Query should support:

• User ID

• Metric Set

• Date Range

• Comparison Date Range

• Child Filters

• Parent Filters

• Category Filters

• Event Type Filters

• Contact Filters

• Tags

• Location

• Module

• Parenting Plan Phase

• Grouping

• Aggregation Interval


### 17.3.2 Saved Analytics View

A Saved Analytics View should support:

• View ID

• User ID

• Name

• Metric Selection

• Filter Configuration

• Visualization Configuration

• Sort/Grouping Configuration

• Created Date

• Updated Date


### 17.3.3 Canonical Metric Ownership

Where a source module defines a metric, Analytics shall use the canonical source-module calculation rather than reimplementing a conflicting formula.


## 17.4 Executive Analytics Dashboard

The Analytics dashboard may display:

• Parenting Time Summary

• Parenting Plan Compliance

• Upcoming Events

• Recent Activity

• Expense Summary

• Communication Trends

• Therapy Attendance

• School Events

• AI Insights

• Outstanding Tasks

Users should be able to configure visible analytics.


## 17.5 Parenting Time Analytics


### 17.5.1 Metrics

Metrics may include:

• Scheduled Parenting Time

• Actual Parenting Time

• Parenting Time Percentage

• Missed Parenting Time

• Denied Parenting Time

• Make-Up Parenting Time

• Late Exchanges

• Early Pickups

• Exchange Success Rate

• Parenting Time by Child

• Parenting Time by Month

• Parenting Time by Year


### 17.5.2 Visualizations

Available charts may include:

• Monthly Parenting Time

• Parenting Time Trend

• Parenting Time by Child

• Parenting Time Compliance

• Parenting Schedule Distribution


## 17.6 Expense Analytics

Metrics may include:

• Total Expenses

• Monthly Spending

• Annual Spending

• Expenses by Child

• Expenses by Category

• Medical Expenses

• School Expenses

• Activity Expenses

• Outstanding Reimbursements

• Average Monthly Spending

Charts may include monthly trend, category breakdown, child breakdown, reimbursement status, and year-over-year comparison.


## 17.7 Communication Analytics

Metrics may include:

• Messages Sent

• Messages Received

• Communication Frequency

• OFW Messages

• Emails

• Text Messages

• Phone Calls

• Average Response Time where computable

• Communication by Contact

• Communication by Month


## 17.8 School Analytics

Metrics may include:

• School Events

• Parent-Teacher Conferences

• Attendance Records

• School Meetings

• Activities

• Academic Milestones

• Extracurricular Participation


## 17.9 Therapy Analytics

Metrics may include:

• Therapy Sessions

• Missed Sessions

• Family Therapy

• Individual Therapy

• Parenting Coach Meetings

• Psychological Evaluations

• Session Frequency


## 17.10 Compliance Analytics


### 17.10.1 Supported Metrics

Metrics may include:

• Parenting Time Compliance

• Exchange Compliance

• Missed Parenting Time

• Parenting Time Denials

• Make-Up Time Completed

• Schedule Changes

• Court-Ordered Requirement Completion where source data supports it


### 17.10.2 Calculation Boundaries

Analytics shall not independently determine legal compliance. It shall display configured calculations and source-module outcomes and may provide clearly labeled AI observations.


## 17.11 Evidence Analytics

Metrics may include:

• Total Evidence Items

• Photos

• Videos

• Documents

• Audio Files

• OCR Documents

• Evidence by Category

• Evidence by Child


## 17.12 Journal Analytics

Metrics may include:

• Total Journal Entries

• Entries by Month

• Entries by Child

• Entries by Category

• Positive Memories

• Behavioral Observations

• Medical Notes

• School Notes


## 17.13 AI Analytics

Metrics may include:

• AI Requests

• AI Credits Used

• Reports Generated

• Summaries Created

• Searches Performed

• AI Suggestions Accepted

• AI Suggestions Rejected


## 17.14 Custom Analytics

Users shall be able to:

• Select Metrics

• Choose Date Ranges

• Group Data

• Filter Results

• Save Dashboard Views

• Export Analytics


## 17.15 Filtering

All analytics should support supported combinations of:

• Date Range

• Child

• Parent

• Category

• Event Type

• Contact

• Tags

• Location

• Module

• Parenting Plan Phase


## 17.16 Period Comparison

Users should be able to compare periods such as:

• This Month vs Last Month

• This Year vs Last Year

• Summer vs School Year

• Custom Range vs Custom Range

Comparison calculations shall use equivalent metric definitions in both periods.


## 17.17 Visualizations

Supported visualization types may include:

• Line Charts

• Bar Charts

• Pie Charts

• Stacked Bar Charts

• Area Charts

• Trend Lines

• Heat Maps

• Summary Cards

• Tables

Users may switch visualization types only where the selected metric structure supports the target visualization.


## 17.18 Drill-Down and Source Navigation

Where supported, users should be able to select a metric or chart element and navigate to the filtered set of source records contributing to the value.


## 17.19 AI Insights


### 17.19.1 Supported Insight Types

AI may generate observations such as:

• Parenting Time changes over time

• Expense trends

• Communication changes around events

• Therapy attendance patterns

• Documentation gaps

• Exchange-delay patterns


### 17.19.2 AI Boundaries

AI insights shall:

• Be clearly labeled as AI-generated.

• Reference supporting metrics or source records where feasible.

• Not alter canonical data.

• Not present inferred motives, diagnoses, or legal conclusions as established facts.


## 17.20 Integration


### 17.20.1 Parenting Journal

Journal metrics shall use authorized Journal records.


### 17.20.2 Parenting Time

Parenting Time metrics shall use canonical Parenting Time calculations.


### 17.20.3 Calendar

Calendar may contribute event counts and scheduling trends.


### 17.20.4 Timeline

Timeline may provide unified chronological aggregation for supported analytics.


### 17.20.5 Evidence Management

Evidence counts and growth metrics shall use authorized Evidence Records.


### 17.20.6 Communications

Communication metrics shall use canonical Communication records.


### 17.20.7 Expense Tracking

Financial metrics shall use canonical Expense and Reimbursement calculations.


### 17.20.8 Contacts

Contacts may provide grouping dimensions.


### 17.20.9 Reports

Analytics may be embedded or exported into reports.


### 17.20.10 Dashboard

Dashboard may surface selected Analytics widgets and summaries.


### 17.20.11 AI Assistant

AI may query authorized analytics results and source data under AI security rules.


## 17.21 Security and Permissions

Analytics shall include only authorized source data. Aggregated metrics shall not leak restricted records through counts, labels, drill-downs, or comparisons.


## 17.22 Audit History

Saved analytics configuration changes, sensitive exports, administrative metric changes, and other significant analytics actions may be audited according to policy.


## 17.23 Performance and Scalability


### 17.23.1 Query Performance

Analytics should return common metrics responsively for normal account sizes.


### 17.23.2 Pre-Aggregation

The system may use caches, materialized views, or pre-aggregations where appropriate, provided authorization remains correct.


### 17.23.3 Long-Running Queries

Large custom analytics or exports may run asynchronously.


## 17.24 Error Handling

The Analytics module shall:

• Identify unavailable source modules.

• Omit unauthorized records.

• Identify incomplete data where feasible.

• Avoid presenting partial metrics as complete.

• Preserve saved view configuration after recoverable failures.


## 17.25 Suggested API Surface


### 17.25.1 Analytics Queries

• POST /analytics/query

• GET /analytics/summary

• GET /analytics/parenting-time

• GET /analytics/expenses

• GET /analytics/communications

• GET /analytics/school

• GET /analytics/therapy

• GET /analytics/compliance

• GET /analytics/evidence

• GET /analytics/journal

• GET /analytics/ai-usage


### 17.25.2 Saved Views

• GET /analytics/views

• POST /analytics/views

• GET /analytics/views/{id}

• PATCH /analytics/views/{id}

• DELETE /analytics/views/{id}


### 17.25.3 Export

• POST /analytics/exports


## 17.26 Proposed Domain Relationships

Analytics View

├── User

├── Metrics

├── Filters

├── Comparison Period

├── Grouping

├── Visualization Configuration

└── Export References

Analytics Result

├── Metric Definition

├── Aggregated Values

├── Source Module References

├── Date Range

├── Comparison Range

└── Authorization Context


## 17.27 MVP Scope

The Analytics MVP should include:

• Executive analytics dashboard

• Parenting Time analytics

• Expense analytics

• Communication analytics

• School analytics

• Therapy analytics

• Compliance analytics

• Evidence analytics

• Journal analytics

• Core filtering

• Date-range selection

• Basic period comparison

• Line/bar/pie/table/summary-card visualizations

• Saved analytics views

• Export

• Security-aware aggregation

• Basic AI insights where AI is enabled


## 17.28 Future Enhancements

Future versions may include:

• Predictive analytics

• Parenting plan forecasting

• AI-generated trend reports

• Goal tracking

• Benchmark comparisons

• Interactive drill-down dashboards

• Geographic visualizations

• Data anomaly detection

• Custom KPI creation

• Scheduled analytics reports

• Organization-wide analytics for enterprise customers


## 17.29 Acceptance Criteria

The Analytics module shall be considered functionally complete for the approved release scope when:

1. Users can view authorized aggregate metrics from supported modules.

2. Parenting Time analytics use canonical Parenting Time calculations.

3. Financial analytics use canonical Expense calculations.

4. Users can filter analytics by supported dimensions.

5. Users can compare supported date ranges.

6. Users can view supported charts and summary cards.

7. Users can save analytics views.

8. Users can export supported analytics.

9. Aggregated results do not expose unauthorized data.

10. Users can drill through to source records where supported.

11. AI insights are clearly labeled and grounded in underlying data.

12. Analytics updates as source data changes according to refresh policy.

13. Partial or unavailable data is identified rather than silently treated as complete.


## 17.30 Product Decisions Still Required

Before implementation is locked, Product and Engineering shall define:

• Exact metric catalog

• Metric formulas not owned by source modules

• Exchange Success Rate formula

• Average Response Time formula

• Compliance metric definitions

• Court-ordered requirement data source

• Default date ranges

• Comparison-period rules

• Supported chart types per metric

• Chart library

• Drill-down MVP scope

• Saved-view limits

• Custom dashboard limits

• Export formats

• Cache strategy

• Cache invalidation

• Pre-aggregation strategy

• Performance targets

• AI insight generation cadence

• Whether AI insights are proactive or user-requested

• Analytics audit events

• Enterprise analytics roadmap


# 18. Mobile Application


## 18.1 Purpose


### 18.1.1 Overview

The Mobile Application shall provide secure iOS and Android access to ClearParent Log, optimized for rapid documentation, evidence capture, offline operation, notifications, and access to core platform functionality.


### 18.1.2 Objectives

The Mobile Application shall:

• Allow users to document events from anywhere.

• Capture evidence in real time.

• Support offline operation for approved workflows.

• Synchronize automatically when connectivity returns.

• Deliver timely notifications.

• Provide a mobile experience optimized for speed and ease of use.

• Preserve source data and synchronization integrity across devices.


## 18.2 Functional Scope

The Mobile Application shall include:

• Authentication

• Home Screen

• Dashboard

• Quick Journal Entry

• Voice Journal

• Evidence Capture

• Camera Integration

• Document Scanning

• Parenting Time

• Calendar

• Timeline

• Search

• AI Assistant

• Notifications

• Offline Mode

• Synchronization

• Secure local storage

• Accessibility

• Mobile performance controls


## 18.3 Mobile Application Data Model


### 18.3.1 Local Record Cache

The mobile client may maintain encrypted local representations of authorized records needed for offline or fast access.


### 18.3.2 Sync Queue

A Sync Queue should support:

• Queue Item ID

• User ID

• Device ID

• Record Type

• Record ID or Temporary Local ID

• Operation Type

• Local Version

• Server Version where known

• Created Date

• Retry Count

• Sync Status

• Error State


### 18.3.3 Device Registration

A Device Registration should support:

• Device ID

• User ID

• Platform

• App Version

• Push Token Reference

• Last Active Date

• Device Security State where available

• Remote Logout State


## 18.4 Home Screen

The mobile Home Screen shall provide quick access to:

• Dashboard

• Quick Journal Entry

• Camera Upload

• Voice Journal

• Calendar

• Parenting Time

• Timeline

• Search

• AI Assistant

• Recent Activity

Users should be able to customize supported home-screen shortcuts.


## 18.5 Offline Mode


### 18.5.1 Supported Offline Actions

Offline capabilities may include:

• Create Journal Entries

• Edit previously synced entries where allowed

• Capture Photos

• Record Videos

• Record Audio

• Create Calendar Events

• Log Parenting Time

• Add Expenses

• Scan Documents

• View Previously Synced Data


### 18.5.2 Offline Boundaries

Features requiring current server-side authorization, AI inference, real-time search, remote records not previously synced, or server-only processing may be unavailable offline.


### 18.5.3 Offline Persistence

Offline changes shall be stored securely until synchronization succeeds or the user resolves a conflict.


## 18.6 Synchronization


### 18.6.1 Sync Scope

Synchronization shall include supported changes to:

• Journal Entries

• Evidence

• Calendar

• Parenting Time

• Expenses

• Reports where editable state is supported

• Contacts

• Settings

• AI Conversations where policy permits


### 18.6.2 Automatic Sync

The client should synchronize automatically when connectivity returns.


### 18.6.3 Conflict Detection

The system shall detect conflicting local/server edits using version or change-tracking metadata.


### 18.6.4 Conflict Resolution

Conflict handling shall preserve user data and require manual review when automatic resolution is unsafe.


### 18.6.5 Retry

Failed sync operations shall be retried according to policy without creating uncontrolled duplicates.


## 18.7 Camera Integration

Users shall be able to:

• Take Photos

• Record Videos

• Scan Documents

• Capture Receipts

• Capture Medical Documents

• Capture Court Documents

• Perform Multi-Page Document Scanning

• Crop Images

• Rotate Images

• Queue Automatic Upload


## 18.8 Evidence Capture


### 18.8.1 Supported Capture Types

The mobile application shall support:

• Photo Upload

• Video Upload

• Audio Recording

• Document Scanning

• Receipt Capture

• Screenshot Upload

• File Upload


### 18.8.2 Evidence Integration

Captured media shall create or reference canonical Evidence Records after successful synchronization/finalization.


### 18.8.3 Original Preservation

Original captured media shall be preserved according to Evidence Management rules. Editing for display, crop, OCR, or scan enhancement should create derived artifacts where required rather than silently replacing originals.


## 18.9 Voice Journal


### 18.9.1 Capture

Users shall be able to record voice for Journal creation.


### 18.9.2 Processing

Supported features may include:

• Voice Recording

• Speech-to-Text Transcription

• Save Original Audio

• Edit Transcript

• AI Summary

• AI Neutral Rewrite

• Automatic Categorization

• Link to Child

• Attach Evidence


### 18.9.3 Approval

AI summaries and rewrites shall remain proposed content until accepted by the user.


## 18.10 Quick Journal Entry

A simplified workflow should support:

• Title

• Date and Time

• Child

• Category

• Notes

• Photos

• Voice Note

• Location

• Tags

The source requirement states a goal of creating an entry in less than one minute. Exact performance/usability acceptance criteria are a Product Decision.


## 18.11 Parenting Time

Mobile Parenting Time shall support:

• View Parenting Schedule

• Log Parenting Exchanges

• Record Pickup Time

• Record Drop-off Time

• Add Notes

• Capture Photos

• Attach Evidence

• View Parenting Statistics

All calculations shall use canonical Parenting Time rules.


## 18.12 Calendar

Users shall be able to:

• View Upcoming Events

• Create Events

• Edit Events

• Receive Reminders

• Navigate to Event Locations where supported

• Mark Events Complete where the Calendar event model supports completion


## 18.13 Notifications


### 18.13.1 Push Notifications

The mobile application shall support push notifications for configured categories including parenting exchanges, court reminders, therapy, school, medication reminders where enabled, calendar events, communications, AI recommendations, report completion, and tasks.


### 18.13.2 Deep Links

Tapping a supported notification shall navigate to the associated authorized record.


### 18.13.3 Lock-Screen Privacy

Sensitive information shall not be exposed on the lock screen unless explicitly enabled and permitted by platform policy.


## 18.14 AI Assistant

The mobile application shall support approved AI capabilities including:

• Ask Questions

• Search Records

• Summarize Events

• Rewrite Journal Entries

• Generate Reports

• Find Related Records

Voice AI interaction is Future Scope unless separately approved.


## 18.15 Search

Mobile Search shall support authorized data from:

• Journal Entries

• Evidence

• Calendar

• Parenting Time

• Communications

• Reports

• Expenses

• Contacts

Voice Search is Future Scope.


## 18.16 Dashboard

The mobile Dashboard may display:

• Upcoming Parenting Time

• Today's Tasks

• Recent Activity

• Notifications

• AI Insights

• Parenting Statistics

• Calendar Events

• Recent Evidence


## 18.17 Timeline

The mobile Timeline shall allow users to review authorized chronological events and navigate to underlying records.


## 18.18 Security


### 18.18.1 Authentication

Supported controls may include:

• Biometric Authentication

• PIN Protection

• Multi-Factor Authentication

• Automatic Session Timeout


### 18.18.2 Device Security

The app shall use platform secure storage and encryption capabilities for local secrets and cached data.


### 18.18.3 Offline Encryption

Offline data shall be encrypted at rest on the device according to platform security requirements.


### 18.18.4 Remote Logout

Authorized users shall be able to invalidate mobile sessions or registered devices according to account security policy.


### 18.18.5 Screenshot and Screen-Capture Policy

Whether highly sensitive screens should restrict screenshots or app-switcher previews is a Security/Product Decision.


## 18.19 Local Data Management

The mobile client should minimize locally retained data and remove or invalidate cached data after logout, remote revocation, or policy-triggered expiration where technically possible.


## 18.20 Performance

The mobile application should prioritize:

• Fast application startup

• Responsive navigation

• Background synchronization

• Efficient battery usage

• Optimized image uploads

• Efficient storage management

Exact performance targets are Product and Engineering decisions.


## 18.21 Background Processing

Supported background operations may include:

• Upload retries

• Sync retries

• Media preprocessing

• Notification token refresh

• Deferred OCR/upload initiation

Background behavior shall comply with iOS and Android operating-system restrictions.


## 18.22 Accessibility

The mobile application shall support accessibility capabilities including:

• Screen Reader Support

• Dynamic Text Sizing

• High Contrast Mode

• Voice Control where supported by the OS

• Keyboard Navigation where applicable

• Color Accessibility

• Haptic Feedback where appropriate


## 18.23 Integration


### 18.23.1 User Management

Mobile authentication and device management shall integrate with User Management.


### 18.23.2 Parenting Journal

Mobile journal creation shall use canonical Journal APIs and models.


### 18.23.3 Parenting Time

Mobile Parenting Time shall use canonical Parenting Time records and calculations.


### 18.23.4 Calendar

Mobile Calendar shall use canonical Calendar records.


### 18.23.5 Timeline

Mobile Timeline shall consume Timeline records.


### 18.23.6 Evidence Management

Captured media shall integrate with Evidence Management.


### 18.23.7 Expense Tracking

Mobile expense creation and receipt capture shall integrate with Expense Tracking.


### 18.23.8 Communications

Mobile Communications shall respect canonical Communication data and permissions.


### 18.23.9 Contacts

Mobile Contacts features shall use application Contacts rather than device contacts unless a separate permissioned integration is implemented.


### 18.23.10 Reports

Users may view and initiate supported report workflows on mobile.


### 18.23.11 AI Assistant

Mobile AI shall use the same authorization and provenance rules as web AI.


### 18.23.12 Search

Mobile Search shall use the centralized Search service.


### 18.23.13 Dashboard

Mobile Dashboard shall consume Dashboard services with mobile-specific presentation.


### 18.23.14 Notifications

Mobile push and notification actions shall integrate with Notifications.


## 18.24 Audit and Telemetry

Security-relevant mobile actions should be auditable according to policy. Operational telemetry shall minimize sensitive content and shall not log protected user data unnecessarily.


## 18.25 Error Handling

The Mobile Application shall:

• Preserve unsynced user data across recoverable network failures.

• Show clear sync errors.

• Prevent duplicate record creation from retries where feasible.

• Avoid reporting successful uploads before server acknowledgment.

• Provide conflict-resolution states when needed.

• Fail closed for authorization errors.


## 18.26 Suggested API Surface


### 18.26.1 Sync Operations

• GET /mobile/sync

• POST /mobile/sync

• POST /mobile/sync/resolve-conflict


### 18.26.2 Device Operations

• POST /devices

• GET /devices

• DELETE /devices/{id}

• POST /devices/{id}/logout


### 18.26.3 Media Operations

• POST /mobile/uploads

• GET /mobile/uploads/{id}/status


## 18.27 Proposed Domain Relationships

Mobile Device

├── User

├── Device Registration

├── Push Token

├── Session

├── Local Cache

└── Sync Queue

Sync Queue Item

├── Device

├── Source Module

├── Local Record

├── Server Record

├── Conflict Metadata

└── Retry Metadata


## 18.28 MVP Scope

The Mobile MVP should include:

• iOS and Android support

• Authentication

• Dashboard

• Quick Journal Entry

• Camera/photo capture

• Document scanning

• Evidence upload

• Parenting Time viewing/logging

• Calendar viewing/editing

• Timeline

• Search

• AI Assistant

• Push notifications

• Offline Journal creation

• Offline evidence capture

• Offline expense capture

• Automatic synchronization

• Conflict detection

• Secure local storage

• Biometric authentication where supported

• Remote logout

• Accessibility baseline


## 18.29 Future Enhancements

Future versions may include:

• Apple Watch and Wear OS support

• Live Activities

• Android Widgets

• Home Screen Widgets

• Siri/Google Assistant integration

• GPS-based exchange check-ins

• Geofenced reminders

• Automatic mileage tracking

• Background document scanning

• AI-powered photo organization

• Real-time collaboration

• Offline AI assistance

• Digital document signing


## 18.30 Acceptance Criteria

The Mobile Application shall be considered functionally complete for the approved release scope when:

1. Users can authenticate securely on supported iOS and Android versions.

2. Users can access core authorized records from mobile.

3. Users can create Journal entries from mobile.

4. Users can capture and upload Evidence.

5. Users can log Parenting Time and exchanges.

6. Users can view and edit supported Calendar events.

7. Users can use Search from mobile.

8. Users can use approved AI Assistant capabilities.

9. Users receive configured push notifications.

10. Notification deep links revalidate authorization.

11. Supported offline actions persist securely without connectivity.

12. Offline changes synchronize when connectivity returns.

13. Conflicts are detected and do not silently overwrite data.

14. Failed uploads do not cause uncontrolled duplicate records.

15. Sensitive lock-screen content follows privacy settings.

16. Local cached data is encrypted.

17. Remote logout invalidates the supported device session.

18. Mobile changes appear in the web application after synchronization.

19. Accessibility baseline requirements are supported.

20. Recoverable network failures do not cause user-created offline data loss.


## 18.31 Product Decisions Still Required

Before implementation is locked, Product, Engineering, Security, and Mobile teams shall define:

• Minimum supported iOS version

• Minimum supported Android version

• Native vs cross-platform framework

19. Parenting Plan Management


## 19.1 Purpose

The Parenting Plan Management module shall enable users to upload, preserve, interpret, review, activate, and operationalize parenting plans and related court orders throughout ClearParent Log.

The module shall convert approved parenting-plan provisions into structured rules that can be consumed by Calendar, Parenting Time, Notifications, Timeline, Search, Analytics, Reports, and the AI Assistant.

The module shall preserve a clear distinction between the original legal document, extracted source language, structured system rules, user-confirmed interpretations, system calculations, and AI-generated interpretations.

The module shall not present an extracted or inferred rule as legally authoritative unless the user has explicitly reviewed and activated that rule.


### 19.1.1 Objectives

• Preserve original parenting-plan and court-order documents without modification.

• Extract relevant text and provisions from uploaded documents.

• Convert provisions into structured, traceable rules.

• Require user review before extracted rules become active.

• Support phased parenting plans and conditional progression.

• Generate parenting schedules, deadlines, reminders, and related system behavior from activated rules.

• Maintain complete version and modification history.

• Provide direct traceability from every structured rule to its source provision.

• Enable reliable use of parenting-plan data across the platform.

• Support AI-assisted questions while clearly distinguishing source language, system calculations, and interpretation.


## 19.2 Functional Scope

The Parenting Plan Management module shall support:

• Document upload and preservation.

• OCR and text extraction.

• Document classification and section detection.

• Parenting-plan rule extraction.

• User review, confirmation, editing, rejection, and ambiguity handling.

• Parenting phases.

• Regular residential schedules.

• Holiday and school-break schedules.

• Exchange and transportation rules.

• Vacation provisions.

• Parent-child communication provisions.

• Decision-making authority.

• Deadlines and notice requirements.

• Restrictions and special conditions.

• Rule precedence and overrides.

• Amendments and modified orders.

• Parenting Time integration.

• Make-up parenting-time calculations where supported.

• Calendar event generation.

• Notification generation.

• Search indexing.

• AI Assistant integration.

• Reports and analytics.

• Timeline integration.

• Security, auditing, and version history.


## 19.3 Parenting Plan Document Model


### 19.3.1 Supported Document Types

The system shall support parenting-related documents including:

• Final Parenting Plans.

• Temporary Parenting Plans.

• Modified Parenting Plans.

• Court Orders.

• Agreed Orders.

• Arbitration Decisions.

• Mediation Agreements.

• Supplemental Orders.

• Parenting-related rulings and amendments.


### 19.3.2 Supported File Types

The system should support, at minimum:

• PDF.

• Microsoft Word documents.

• Scanned documents.

• Images.

Additional supported formats are a Product Decision.


### 19.3.3 Original Document Preservation

The system shall preserve the original uploaded document without modification.

The original file shall be retained as the canonical source artifact for document-level provenance.

Any OCR output, extracted text, parsed sections, AI summaries, or structured rules shall be stored as derived artifacts and shall not overwrite the original document.


### 19.3.4 Core Document Fields

Each Parenting Plan Document should include:

• Parenting Plan Document ID.

• Document Type.

• Document Title.

• Effective Date.

• Expiration Date, when applicable.

• Filing or Order Date, when available.

• Court or Jurisdiction, when available.

• Case Number, when available.

• Parties.

• Children.

• Source File Reference.

• OCR Status.

• Processing Status.

• Review Status.

• Activation Status.

• Supersedes Document ID, when applicable.

• Superseded By Document ID, when applicable.

• Created Date.

• Updated Date.

• Created By.

• Activated Date.

• Activated By.


## 19.4 Parenting Plan Processing


### 19.4.1 Processing Workflow

After upload, the system shall create a processing job for the Parenting Plan Document.

Processing may include:

• OCR.

• Text extraction.

• Section detection.

• Heading detection.

• Date extraction.

• Name recognition.

• Child recognition.

• Location recognition.

• Schedule recognition.

• Holiday recognition.

• Deadline extraction.

• Restriction extraction.

• Parenting-phase detection.

• AI-generated summaries.

• Structured rule generation.


### 19.4.2 Processing Status

The system should support processing statuses such as:

• Uploaded.

• Processing.

• Processing Complete.

• Review Required.

• Processing Failed.

• Activated.

• Superseded.

• Archived.

Exact status taxonomy is a Product Decision.


### 19.4.3 Extraction Provenance

Every extracted provision or structured rule shall retain enough provenance to identify:

• Source document.

• Source page, when available.

• Source section or heading, when available.

• Source text range or excerpt.

• Extraction method.

• Extraction timestamp.

• AI model or processing version, when applicable.


### 19.4.4 Processing Failure

If OCR, extraction, or AI processing fails, the original uploaded document shall remain intact and accessible.

A processing failure shall not cause partially extracted rules to become active automatically.


## 19.5 Parenting Plan Rule Model


### 19.5.1 Structured Rule Record

Each extracted or manually created Parenting Plan Rule shall have a unique immutable Rule ID.

A rule shall represent a discrete parenting-plan provision that can be reviewed, activated, versioned, linked, calculated, or consumed by other modules.


### 19.5.2 Rule Fields

Each rule should include:

• Rule ID.

• Parenting Plan Document ID.

• Rule Type.

• Rule Title.

• Rule Summary.

• Structured Rule Data.

• Source Language.

• Source Page.

• Source Section.

• Effective Date.

• Expiration Date.

• Applicable Child or Children.

• Applicable Parent or Party.

• Status.

• Interpretation Status.

• Confidence Indicator, if AI extracted.

• Created By Method.

• Created Date.

• Updated Date.

• Confirmed By.

• Confirmed Date.


### 19.5.3 Rule Origin

The system shall identify whether a rule is:

• AI Extracted.

• User Confirmed.

• User Modified.

• Manually Entered.


### 19.5.4 Rule Activation

AI-extracted rules shall never become active automatically.

A rule shall become operational only after the required review and activation workflow is completed.


## 19.6 Parenting Plan Review


### 19.6.1 Review Actions

Users shall be able to review each extracted rule and:

• Accept it.

• Edit it.

• Reject it.

• Mark it as ambiguous.

• Add missing information.

• Link it to exact source language.

• Request AI re-analysis.


### 19.6.2 Review State

The system should support rule review states such as:

• Pending Review.

• Accepted.

• Modified.

• Rejected.

• Ambiguous.

• Needs Re-analysis.

• Activated.


### 19.6.3 Source and Interpretation Separation

The user interface shall visually distinguish:

• Direct source language.

• Extracted structured values.

• User edits.

• System calculations.

• AI interpretation.

The system shall not silently modify source language to match a structured interpretation.


### 19.6.4 Ambiguity Handling

If the source document is ambiguous or conflicting, the system shall allow the user to mark the rule as ambiguous.

Ambiguous rules shall not be used for definitive automated answers unless an explicit user-approved interpretation has been configured.


## 19.7 Parenting Phases


### 19.7.1 Phase Model

The system shall support phased parenting plans, including examples such as:

• Phase 1.

• Phase 2.

• Phase 3.

• Phase 4.

• Final Parenting Schedule.


### 19.7.2 Phase Fields

Each phase may include:

• Phase ID.

• Phase Name.

• Start Date.

• End Date.

• Trigger Condition.

• Parenting Schedule.

• Overnight Rules.

• Exchange Rules.

• Supervision Requirements.

• Therapy Requirements.

• Treatment Requirements.

• Required Classes.

• Professional Recommendations.

• Conditions for Progression.

• Phase Status.


### 19.7.3 Phase Trigger Types

The system shall distinguish between automatically measurable triggers and triggers requiring human confirmation.

Examples of automatically measurable triggers may include:

• Completion of a specified number of parenting periods.

• Passage of a defined number of days.

• Completion of a required class where completion evidence is recorded.

Examples requiring human confirmation may include:

• Therapist recommendation.

• Parenting Coach recommendation.

• Guardian ad Litem recommendation.

• Court approval.

• Professional discretion.


### 19.7.4 Automatic Phase Evaluation

Where a phase condition can be objectively calculated from authorized structured data, the system may evaluate phase eligibility automatically.

Automatic evaluation shall identify the calculation basis and source records used.


### 19.7.5 Human Confirmation

Where a phase condition depends on a professional recommendation or other subjective condition, the system shall require confirmation before changing phase status.


## 19.8 Regular Parenting Schedule


### 19.8.1 Supported Schedule Types

The system shall support residential schedule patterns including:

• Alternating Weekends.

• Week-on / Week-off.

• 2-2-3.

• 2-2-5-5.

• Specific Weekdays.

• Every Other Weekend.

• School-Year Schedule.

• Summer Schedule.

• Custom Schedule.


### 19.8.2 Schedule Rule Fields

Each schedule rule may include:

• Parent.

• Child or Children.

• Start Day.

• Start Time.

• End Day.

• End Time.

• Recurrence Pattern.

• Exchange Location.

• Transportation Responsibility.

• Effective Date.

• Expiration Date.

• Source Rule ID.


### 19.8.3 Schedule Generation

Activated parenting schedule rules shall be available to Calendar and Parenting Time for generation of scheduled parenting periods.

Generated schedule records shall maintain a link to the Parenting Plan Rule that created them.


### 19.8.4 Schedule Exceptions

The system shall support exceptions created by higher-precedence rules, approved modifications, holidays, vacations, school breaks, or temporary orders.


## 19.9 Holiday and School-Break Schedule


### 19.9.1 Supported Holiday Types

The system may support:

• Christmas.

• Christmas Eve.

• Thanksgiving.

• Easter.

• Fourth of July.

• Memorial Day.

• Labor Day.

• Mother's Day.

• Father's Day.

• Birthdays.

• Spring Break.

• Winter Break.

• Mid-Winter Break.

• School Holidays.

• Religious Holidays.

• Custom Holidays.


### 19.9.2 Rotation Rules

Holiday assignments may rotate by:

• Odd Years.

• Even Years.

• Parent.

• Child.

• Custom Rotation.


### 19.9.3 Holiday Overrides

The system shall support holiday rules that override the regular parenting schedule.

The precedence relationship between a holiday rule and regular schedule shall be traceable to the activated Parenting Plan rules.


### 19.9.4 Holiday Questions

The AI Assistant and relevant application views should be able to answer supported questions such as:

• Who has a specified holiday in a given year?

• What time does holiday parenting time begin or end?

• Does the holiday schedule override a regular weekend?

Answers shall be based on activated rules and clearly identify ambiguity when the source does not provide a definitive answer.


## 19.10 Exchange Locations and Transportation


### 19.10.1 Exchange Rules

The system shall support exchange provisions including:

• Pickup Location.

• Drop-off Location.

• School Exchange.

• Daycare Exchange.

• Neutral Exchange Location.

• Parent Responsible for Transportation.

• Third-Party Transportation.

• Supervised Exchange.

• Exchange Time.

• Waiting Period.

• Late Arrival Rules.

• Special Exchange Restrictions.


### 19.10.2 Exchange Integration

Activated exchange rules shall be available to Parenting Time and Calendar.

Generated exchange events should include source Rule ID, location, assigned responsibility, time, and relevant restrictions.


## 19.11 Vacation Rules


### 19.11.1 Vacation Rule Types

The system shall support vacation provisions including:

• Number of Vacation Days.

• Number of Vacation Weeks.

• Consecutive-Day Limits.

• Summer Vacation Rules.

• Domestic Travel Rules.

• International Travel Rules.

• Notice Requirements.

• Required Itinerary.

• Contact Information Requirements.

• Passport Requirements.

• Priority Selection Rules.

• Vacation Selection Deadlines.

• Blackout Dates.

• Conflict Resolution Rules.


### 19.11.2 Vacation Entitlement Calculations

Where a rule provides sufficient structured information, the system may calculate vacation entitlement and remaining availability.

Calculations shall identify the source rule and applicable usage records.


### 19.11.3 Vacation Questions

The system should support questions such as:

• How much vacation time is available?

• How much notice is required?

• When is the summer vacation selection deadline?


## 19.12 Phone and Electronic Communication


### 19.12.1 Communication Rule Types

The system shall support provisions including:

• Scheduled Phone Calls.

• Video Calls.

• Daily Calls.

• Weekly Calls.

• Reasonable Contact.

• Calling Hours.

• Maximum Call Duration.

• Minimum Call Duration.

• Parent Responsible for Initiating.

• Child Device Requirements.

• Privacy Requirements.

• Communication Restrictions.


### 19.12.2 Communication Schedule Generation

Structured communication schedules shall be available to Calendar and Notifications.

Generated events and reminders shall maintain a reference to the Parenting Plan Rule that created them.


## 19.13 Decision-Making Authority


### 19.13.1 Decision Categories

The system shall support decision-making categories including:

• Education.

• Medical.

• Dental.

• Mental Health.

• Therapy.

• Medication.

• Extracurricular Activities.

• Religion.

• Travel.

• School Enrollment.

• Major Activities.


### 19.13.2 Authority Types

Authority may include:

• Parent A.

• Parent B.

• Joint Decision-Making.

• Consultation Required.

• Mutual Agreement Required.

• Professional Recommendation Required.

• Court Determination.


### 19.13.3 Decision-Making Queries

The AI Assistant should be able to answer supported questions regarding authority, consultation, and agreement requirements using activated rules.


## 19.14 Deadlines and Notice Requirements


### 19.14.1 Deadline Types

The system shall support deadline rules including:

• Vacation Notice.

• Travel Notice.

• Holiday Selection.

• Summer Schedule Selection.

• Activity Registration.

• Expense Reimbursement.

• Address Changes.

• School Decisions.

• Medical Notification.

• Schedule Change Requests.


### 19.14.2 Deadline Rule Fields

Each deadline rule may include:

• Trigger Event.

• Number of Days.

• Calendar Days or Business Days.

• Responsible Party.

• Required Action.

• Required Delivery Method.

• Source Provision.

• Source Rule ID.


### 19.14.3 Deadline Calculation

Where sufficient structured data exists, the system shall calculate applicable deadline dates.

The system shall preserve the calculation basis, triggering date, and source rule used.


### 19.14.4 Deadline Integration

Calculated deadlines shall be available to Notifications and Calendar.


## 19.15 Restrictions and Special Conditions


### 19.15.1 Restriction Types

The system shall support provisions including:

• Supervised Parenting Time.

• No Alcohol.

• No Drug Use.

• Driving Restrictions.

• Geographic Restrictions.

• Travel Restrictions.

• Overnight Restrictions.

• Third-Party Restrictions.

• Communication Restrictions.

• Treatment Requirements.

• Evaluation Requirements.

• Therapy Requirements.

• School Attendance Requirements.

• Contact Restrictions.


### 19.15.2 Neutral Presentation

Restrictions shall be presented neutrally and tied directly to source language.

The system shall avoid converting ambiguous language into a categorical restriction without user confirmation.


## 19.16 Rule Precedence and Overrides


### 19.16.1 Override Sources

The system shall support rule precedence involving sources such as:

• Temporary Court Orders.

• Modified Orders.

• Holiday Schedules.

• Vacation Schedules.

• School Break Schedules.

• Regular Parenting Schedules.


### 19.16.2 Precedence Model

The system shall maintain an explicit hierarchy or override relationship between active rules where such precedence is supported by the source documents.


### 19.16.3 Conflict Detection

If two active rules conflict and the controlling rule cannot be determined from approved structured data, the system shall flag the conflict.

The system shall not invent a precedence rule merely to produce an answer.


## 19.17 Amendments and Modified Orders


### 19.17.1 New Document Evaluation

When a later parenting-related document is uploaded, the system should identify whether it:

• Replaces the entire parenting plan.

• Modifies specific sections.

• Temporarily overrides existing rules.

• Adds new requirements.

• Removes restrictions.

• Extends an existing order.

• Changes parenting phases.


### 19.17.2 Version History

The system shall maintain complete version history for Parenting Plan Documents and structured rules.


### 19.17.3 Rule History

Users shall be able to view, where applicable:

• Current Rule.

• Previous Rule.

• Effective Date.

• Source Document.

• Rule History.

• Superseding Rule.


### 19.17.4 No Silent Replacement

Uploading a new order shall not silently delete prior source documents or historical rules.

Historical rules shall remain available for period-specific calculations and audit history according to retention policy.


## 19.18 Parenting Time Integration


### 19.18.1 Scheduled Baseline

Parenting Time shall use activated Parenting Plan rules as the scheduled baseline when those rules are configured as authoritative for scheduling.


### 19.18.2 Scheduled Versus Actual Comparison

The system shall support comparison of:

• Scheduled Parenting Time.

• Actual Parenting Time.


### 19.18.3 Derived Metrics

Supported calculations may include:

• Scheduled Parenting Hours.

• Actual Parenting Hours.

• Parenting Time Percentage.

• Missed Parenting Time.

• Denied Parenting Time.

• Make-Up Parenting Time.

• Late Exchanges.

• Parenting Plan Deviations.

• Parenting Time Compliance Metrics.

Exact calculation definitions and denominator rules shall be defined by the Parenting Time module and Product Decisions.


## 19.19 Make-Up Parenting Time


### 19.19.1 Make-Up Rule Tracking

Where an activated Parenting Plan contains make-up time rules, the system shall be able to associate those rules with Parenting Time records.


### 19.19.2 Make-Up Fields

Information may include:

• Original Missed Parenting Event.

• Scheduled Duration.

• Reason.

• Responsible Party.

• Make-Up Requirement.

• Hours Owed.

• Make-Up Deadline.

• Make-Up Date.

• Hours Completed.

• Remaining Balance.


### 19.19.3 Make-Up Calculations

The system may answer whether make-up time appears owed only when the applicable Parenting Plan rules and Parenting Time records provide sufficient information.

Where the source is ambiguous or required records are incomplete, the system shall state that the calculation cannot be determined definitively.


## 19.20 Calendar Integration


### 19.20.1 Generated Calendar Events

Once applicable Parenting Plan rules are activated, the system shall be able to generate Calendar events including:

• Parenting Time.

• Parenting Exchanges.

• Holidays.

• Vacation Periods.

• Phone Calls.

• Parenting Phase Changes.

• Notice Deadlines.

• Court-Ordered Requirements.

• School Break Parenting Time.


### 19.20.2 Source Traceability

Every generated Calendar event shall retain a link to the Parenting Plan Rule that created it.


### 19.20.3 Regeneration and Updates

When an activated rule changes, the system shall update future generated events according to the Calendar integration contract without silently rewriting completed historical events.


## 19.21 Notifications


### 19.21.1 Generated Notifications

Parenting Plan rules should create notifications where appropriate, including:

• Upcoming Parenting Exchange.

• Vacation Selection Deadline.

• Travel Notice Deadline.

• Holiday Selection Deadline.

• Phase Eligibility Date.

• Phone Call Schedule.

• Upcoming Parenting Time.

• Make-Up Time Deadline.

• Required Documentation Deadline.


### 19.21.2 Notification Source

Each generated notification should retain a reference to its source Parenting Plan Rule.


## 19.22 Search Integration


### 19.22.1 Searchable Content

Parenting Plan information shall be searchable through Global Search, subject to user permissions.

Searchable content may include:

• Raw Parenting Plan Text.

• OCR Text.

• Structured Rules.

• Parenting Phases.

• Holiday Rules.

• Exchange Locations.

• Vacation Rules.

• Phone Rules.

• Decision-Making Authority.

• Restrictions.

• Deadlines.


### 19.22.2 Search Result Navigation

Search results shall provide navigation to the applicable source document, source provision, or structured rule where authorized.


## 19.23 AI Assistant Integration


### 19.23.1 Parenting Plan Knowledge Source

Activated Parenting Plan rules shall be available as a primary knowledge source for the AI Assistant.


### 19.23.2 Supported Questions

The AI Assistant should support questions such as:

• Whose weekend is next?

• What time is the exchange Saturday?

• Where is the exchange?

• How many overnights have I had this year?

• When does the next parenting phase begin?

• What conditions must occur before a phase begins?

• What does the Parenting Plan say about holidays?

• Who has a particular holiday this year?

• How much vacation time is allowed?

• Who makes medical decisions?

• What does the Parenting Plan say about phone calls?

• How much notice is required for vacation?

• Is make-up parenting time owed?

• What Parenting Plan deadlines are approaching?


### 19.23.3 AI Answer Provenance

AI responses should reference the underlying Parenting Plan provision whenever possible.

The system shall clearly distinguish among:

• Direct Parenting Plan Language.

• Structured System Calculation.

• AI Interpretation.


### 19.23.4 Ambiguous Source Language

If the Parenting Plan is ambiguous, the AI Assistant shall identify the ambiguity and present the relevant source language rather than presenting an unsupported interpretation as fact.


### 19.23.5 Authorization

AI retrieval and answers shall respect the user's authorization to the underlying Parenting Plan documents and related records.


## 19.24 Reports and Analytics


### 19.24.1 Supported Reports

Parenting Plan information shall be available to the Report Builder.

Reports may include:

• Parenting Plan Summary.

• Parenting Schedule Summary.

• Parenting Phase History.

• Holiday Schedule.

• Parenting Time Compliance.

• Missed Parenting Time.

• Make-Up Parenting Time.

• Exchange History.

• Parenting Plan Deadlines.

• Parenting Plan Deviations.

• Current Restrictions.

• Decision-Making Authority Summary.


### 19.24.2 Source Integrity

Reports shall distinguish between source language, user-confirmed structured rules, calculated values, and AI-generated summaries.


### 19.24.3 Analytics

Analytics may use activated Parenting Plan rules to compare scheduled expectations with recorded events, subject to the calculation definitions of the applicable source modules.


## 19.25 Timeline Integration


### 19.25.1 Timeline Events

Significant Parenting Plan events should automatically appear in Timeline, including:

• Parenting Plan Uploaded.

• Parenting Plan Activated.

• Modified Order Uploaded.

• Parenting Phase Started.

• Parenting Phase Completed.

• Holiday Parenting Period.

• Parenting Plan Deadline.

• Parenting Plan Modification.

• Restriction Added or Removed.


### 19.25.2 Canonical References

Timeline events shall reference the canonical Parenting Plan Document or Parenting Plan Rule rather than duplicating the underlying legal information.


## 19.26 Security and Permissions


### 19.26.1 Sensitive Information

Parenting Plan documents and structured rules shall be treated as sensitive legal and family information.


### 19.26.2 Encryption

The system shall encrypt Parenting Plan data in transit and at rest using platform security standards.


### 19.26.3 Permission Enforcement

Document, rule, Search, Timeline, Report, Analytics, and AI access shall honor the same underlying authorization rules.


### 19.26.4 Export Authorization

Users shall only be able to export Parenting Plan content they are authorized to access.


## 19.27 Audit History


### 19.27.1 Auditable Actions

The system shall record audit events for significant Parenting Plan actions including:

• Document upload.

• Processing activity.

• OCR and extraction activity.

• AI extraction activity.

• Rule creation.

• Rule acceptance.

• Rule modification.

• Rule rejection.

• Ambiguity marking.

• Rule activation.

• Plan activation.

• Plan deactivation.

• Modification or supersession.

• Access and exports where required by platform policy.


### 19.27.2 Audit Fields

Audit records should include:

• Audit ID.

• Document ID.

• Rule ID, when applicable.

• Timestamp.

• User or System Process.

• Action.

• Previous Value, when applicable.

• New Value, when applicable.

• Processing or Request ID, when applicable.

• Result.


### 19.27.3 Audit Immutability

Audit records shall not be editable by end users.


## 19.28 Deletion, Archiving, and Recovery


### 19.28.1 Original Document Retention

Deletion behavior for original Parenting Plan documents shall follow platform retention and legal-document policies.


### 19.28.2 Soft Deletion

Where deletion is permitted, the system should use soft deletion or archival where appropriate so historical rule traceability is preserved.


### 19.28.3 Historical Calculations

Historical rules that were active during a prior date range shall remain available to period-specific reporting and audit logic according to retention policy.


### 19.28.4 Recovery

Where supported, archived or soft-deleted documents and rules may be restored without creating duplicate records.


## 19.29 Performance and Scalability

The module shall support multiple Parenting Plan documents, amendments, structured rules, generated schedules, and historical versions without requiring the full data set to be loaded into the client.

Processing of large documents, OCR, AI extraction, rule generation, schedule generation, and broad recalculation may be performed asynchronously.

Search and rule evaluation shall use indexed or otherwise scalable retrieval patterns.


## 19.30 Error Handling

The module shall handle failures including:

• Unsupported file type.

• Corrupt upload.

• OCR failure.

• Text extraction failure.

• AI extraction failure.

• Rule validation failure.

• Conflicting rules.

• Ambiguous provisions.

• Schedule-generation failure.

• Notification-generation failure.

• Calendar-generation failure.

• Unauthorized access.

• Report-generation failure.

Failures in derived processing shall not alter or damage the original Parenting Plan document.


## 19.31 Suggested API Surface


### 19.31.1 Document Operations

Conceptual operations may include:

• POST /parenting-plans

• GET /parenting-plans

• GET /parenting-plans/{id}

• POST /parenting-plans/{id}/process

• POST /parenting-plans/{id}/activate

• POST /parenting-plans/{id}/archive


### 19.31.2 Rule Operations

Conceptual operations may include:

• GET /parenting-plans/{id}/rules

• GET /parenting-plan-rules/{id}

• PUT /parenting-plan-rules/{id}

• POST /parenting-plan-rules/{id}/accept

• POST /parenting-plan-rules/{id}/reject

• POST /parenting-plan-rules/{id}/mark-ambiguous

• POST /parenting-plan-rules/{id}/reanalyze

• POST /parenting-plan-rules/{id}/activate


### 19.31.3 Schedule and Calculation Operations

Conceptual operations may include:

• GET /parenting-plans/{id}/schedule

• GET /parenting-plans/{id}/holidays

• GET /parenting-plans/{id}/deadlines

• GET /parenting-plans/{id}/phases

• POST /parenting-plans/{id}/evaluate-phases

• POST /parenting-plans/{id}/generate-calendar


### 19.31.4 Version and History Operations

Conceptual operations may include:

• GET /parenting-plans/{id}/versions

• GET /parenting-plan-rules/{id}/history

• GET /parenting-plans/{id}/audit

The exact external API architecture is a Product and Engineering Decision.


## 19.32 Proposed Domain Relationships

Parenting Plan Document

├── Original Evidence/File Reference

├── Extracted Text

├── Document Sections

├── Parenting Plan Rules

│   ├── Schedule Rules

│   ├── Holiday Rules

│   ├── Exchange Rules

│   ├── Vacation Rules

│   ├── Communication Rules

│   ├── Decision-Making Rules

│   ├── Deadline Rules

│   ├── Restriction Rules

│   └── Phase Rules

├── Rule Versions

├── Child Associations

├── Party Associations

├── Calendar References

├── Parenting Time References

├── Notification References

├── Timeline References

├── Report References

└── Audit History

The Parenting Plan Document remains the preserved source artifact, while structured rules serve as reviewed operational representations of the provisions within that source.


## 19.33 MVP Scope

The MVP should include:

• Parenting Plan and related-order upload.

• Original file preservation.

• PDF, Word, scanned-document, and image processing where technically feasible.

• OCR and text extraction.

• Section and provision identification.

• Structured rule extraction.

• Rule review workflow.

• Accept, edit, reject, and ambiguous states.

• Source-language traceability.

• Rule activation.

• Regular Parenting Schedule rules.

• Holiday rules.

• Exchange rules.

• Vacation rules.

• Communication rules.

• Decision-making rules.

• Deadline rules.

• Restriction rules.

• Parenting phases.

• Modified-order/version history.

• Calendar integration.

• Parenting Time integration.

• Notification integration.

• Search integration.

• Timeline integration.

• Report integration.

• Basic AI Parenting Plan questions.

• Security and audit history.


## 19.34 Future Enhancements

Future versions may include:

• Automatic parenting-plan compliance monitoring.

• Automatic court-order modification detection.

• Expanded AI-generated Parenting Plan summaries.

• Parenting-plan comparison tools.

• Side-by-side comparison of old and new plans.

• Interactive visual parenting-plan calendars.

• Automatic vacation entitlement tracking.

• Automatic phase progression monitoring.

• Professional review workflows.

• Attorney verification of extracted rules.

• Parenting Coach verification.

• Automated court-order deadline tracking.

• Advanced conflict detection between multiple orders.

• Jurisdiction-specific Parenting Plan templates.

• Shared Parenting Plan access with authorized professionals.


## 19.35 Acceptance Criteria

The Parenting Plan Management module shall be considered functionally complete for MVP when:

1. A user can upload a supported Parenting Plan or related order.

2. The original uploaded document is preserved without modification.

3. The system can extract readable text from supported digital or scanned documents where technically possible.

4. Extracted rules are linked to source language.

5. AI-extracted rules do not become active automatically.

6. A user can accept, edit, reject, or mark extracted rules ambiguous.

7. Rule origin and review status are visible.

8. Activated regular Parenting Schedule rules can generate scheduled parenting periods.

9. Holiday rules can override regular schedules according to approved precedence rules.

10. Parenting phases can represent both measurable and human-confirmed trigger conditions.

11. Exchange requirements can be represented structurally.

12. Vacation provisions can be represented structurally.

13. Communication provisions can generate supported Calendar and Notification records.

14. Decision-making authority can be represented by category.

15. Deadlines can be calculated where source rules provide sufficient information.

16. Restrictions remain linked to their source provisions.

17. Conflicting or ambiguous rules are flagged rather than guessed.

18. Modified orders preserve prior versions and effective-date history.

19. Parenting Time can consume activated schedule rules as its scheduled baseline.

20. Generated Calendar events retain links to source Parenting Plan rules.

21. Search can retrieve authorized Parenting Plan text and structured rules.

22. AI answers distinguish direct language, system calculation, and AI interpretation.

23. Reports can use activated Parenting Plan data while preserving source traceability.

24. Significant Parenting Plan lifecycle events can appear in Timeline.

25. Permissions are enforced consistently across document, Search, Timeline, Reports, Analytics, and AI.

26. Significant rule and document changes are auditable.

27. Derived processing failures do not damage the original source document.

28. Historical rule versions remain available according to retention policy.


## 19.36 Product Decisions Still Required

• Exact supported file types and file-size limits.

• OCR provider and fallback behavior.

• AI extraction provider and model-version retention.

• Rule confidence thresholds.

• Whether low-confidence rules require special review treatment.

• Exact document and rule status taxonomy.

• Whether activation occurs per rule, per section, or per document.

• Whether multiple Parenting Plans may be active for different children simultaneously.

• How joint cases with different child-specific schedules are represented.

• Default rule-precedence hierarchy.

• Whether users may manually override calculated precedence.

• How conflicting active orders are handled.

• Business-day calculation rules and holiday calendars.

• Timezone and daylight-saving behavior.

• School-calendar source and school-break resolution.

• Exact holiday taxonomy and custom-holiday behavior.

• Schedule-generation horizon.

• Regeneration behavior after plan changes.

• Whether generated Calendar events are canonical or projections.

• Whether users may manually edit generated events and how those overrides are preserved.

• Phase eligibility calculation rules.

• Whether phase progression can ever be activated automatically.

• Parenting Time compliance definitions.

• Make-up parenting-time calculation rules.

• Vacation entitlement calculation rules.

• Notification defaults and reminder timing.

• Search indexing behavior for raw legal text.

• Retention policy for superseded plans and extracted artifacts.

• Whether original Parenting Plan files are stored directly in this module or referenced through Evidence Management.

• Export formats and export-size limits.

• Professional sharing and verification workflows.

• Which Future Enhancements remain outside MVP.

Change Log

