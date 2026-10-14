# UNITE repository instructions

Before changing this repository, read the shared UNITE instructions and
relevant project documentation maintained in
[dkfz-unite/.github](https://github.com/dkfz-unite/.github/blob/main/README.md).

1. Prefer the local shared checkout: `../unite-meta/AGENTS.md`, relative to this
   repository root. A sibling `.github/AGENTS.md` or a user-provided path is also
   valid. Resolve its document links relative to that shared checkout.
2. Read the shared `AGENTS.md`, project overview, repository map, and topic
   documents relevant to the task, following the shared README's task index.
3. If no shared checkout is accessible, use the GitHub versions beginning with
   [shared AGENTS.md](https://github.com/dkfz-unite/.github/blob/main/AGENTS.md).
   If those are unavailable too, report the missing context and proceed only
   with what local documentation/code establishes; do not invent shared rules.
4. Read this repository's README and relevant local docs; inspect Git status.
   More specific instructions here supplement the shared guidance.

Keep common rules in `.github`; keep component details in this repository.

## Identity

Status: reviewed with the owner (2026-10-12).

`unite-identity` authenticates users (standard and LDAP providers), manages
accounts, sessions, the access list, permissions and worker tokens, issues JWTs
signed with the shared `UNITE_API_KEY`, and deletes inactive accounts with their
saved data. Responsibilities, instance modes, permissions, token and lifecycle
rules are described in
[Identity and lifecycle](../unite-meta/docs/identity-and-lifecycle.md); read it
first. Configuration is in the [README](README.md), the API in [Docs/](Docs/api.md).

### Projects

| Project | Contents |
| --- | --- |
| `Unite.Identity` | Services: accounts (`AccountService`), users, sessions, tokens, providers, the identity services per provider (`DefaultIdentityService`, `LdapIdentityService` with `Ldap/LdapService`), saved-data cleanup (`UserDataService`, Mongo repositories); `PasswordHelper`; constants (permissions, providers) |
| `Unite.Identity.Web` | Controllers, options, JWT and claims helpers (`Helpers/`), session cookie, validators, workers (`RootWorker`, `AccountWorker`) |
| `Unite.Post` | SMTP mail client and the password-reset mail |
| `Unite.Tests` | MSTest tests for `Unite.Post` only |

Entities and the database context are in the `Unite.Identity.Data` package
(`unite-identity-data`), migrations in `unite-identity-data-migrations`.
Identity also uses `Unite.Data.Context` (domain database) to delete analysis
tasks, and `Unite.Cache` for the Mongo user data.

### Build and run

```sh
dotnet build Unite.Identity.sln
dotnet test Unite.Identity.sln
dotnet run --project Unite.Identity.Web --launch-profile Unite.Identity.Web
```

- Local URL `http://localhost:5000`; Docker `identity.unite.net`. Needs the
  identity PostgreSQL database, the domain database and MongoDB (for account
  deletion). To test against the running platform, stop `unite.identity` first;
  see [Running a service locally](../unite-meta/docs/development-validation.md#running-a-service-locally-instead-of-its-container).
  `AccountWorker` starts too and deletes expired sessions and inactive accounts
  of the database it connects to.
- The portal calls Identity through `/api/identity`; login sets the
  `unite_session` cookie (HttpOnly, Secure, SameSite Lax) used for token refresh.

### Authentication flow

- `api/realm/{default|ldap}/login` checks credentials, creates a session
  (30 days, `IdentityController.SESSION_EXPIRY_DAYS`), sets the session cookie
  and returns a JWT (15 minutes, `TokenHelper`).
- `api/realm/{provider}/token?email=` returns a new JWT for a valid session
  cookie, rotates the session key and updates the user's `LastActive`, which
  drives inactivity cleanup. Login should update it as well (Jira UND-1067).
  Access-list entries start with `LastActive` at creation, so an entry not
  registered within the retention period is removed; that is intended.
- Claims (`ClaimsHelper`): users get `NameIdentifier` (integer user ID), actor
  `User`, email, provider (`AuthenticationMethod`), role `Root` for the root
  user and one `permission` claim per permission. Worker tokens get actor
  `Worker`, name, key (`Sid`) and permissions. Other services read these claims;
  changing a claim type or value is a cross-service change (Composer and
  Analysis personal criteria, the portal's worker-token check, every service's
  permission policies).
- Administration endpoints (`api/user`, `api/users`, `api/token`, `api/tokens`)
  require role `Root`: only the root user is an administrator; `api/account` requires actor `User`. The token activity
  check `api/token/{key}/active` is anonymous; the portal calls it.

### Conventions and pitfalls

- Emails are normalized to lowercase in the request models (`AccountModel`,
  `IdentityModel`, `UserModel`); compare normalized values.
- Passwords are hashed with ASP.NET `PasswordHasher` (salted). Use it
  everywhere; `PasswordHelper` rejects hashes in another format, and old MD5
  hashes are not migrated (owner, 2026-10-12: no existing installation needs
  it). The old MD5 method is kept only for reference.
- Password reset works only when SMTP is configured; otherwise the portal tells
  users it is unavailable and an administrator recreates the account. Other
  reset mechanisms are out of scope for now (owner, 2026-10-12).
- Only the standard provider has passwords in UNITE: password change and reset
  apply to `default` users only. LDAP users authenticate against the directory
  and must already exist in UNITE (added by an administrator).
- Password change should end the user's other sessions as reset does (Jira
  UND-1071).
- `RootWorker` creates the root user once on startup, always in the standard
  (`default`) provider; it never resets an existing root account's password.
  The standard provider is therefore always active and visible
  (`DefaultProviderOptions.Active` is `true`); LDAP runs alongside it. Do not
  make it disableable.
- Deleting a user must remove their datasets and analyses: use
  `AccountService.Delete`, not `UserService.Delete` (the admin endpoint still
  uses the latter, Jira UND-1068). Deletion should also remove the user's
  project memberships (`DataUser`/`ProjectUser` in the domain database, then
  reindex projects); not implemented yet (Jira UND-1069).
- `UserDataService` matches saved datasets and analyses by **email** (the
  portal stores the email as their `UserId`), while other services identify
  users by the integer ID.

### Shared libraries

`Unite.Identity.Data` 4.3.0, `Unite.Data.Context` 12.10.20 (latest 12.10.33),
`Unite.Cache` 1.3.0, `Unite.Essentials` 1.6.0 (`Unite.Post`).

### CI

`docker-images.yml` builds amd64 and arm64 images under one manifest on pushes
to `main`.

### Known issues

Defects are tracked in the private Jira; see the shared
[Track defects](../unite-meta/AGENTS.md#track-defects).
