# Availability API

## GET: api/availability/password-reset

Returns a JSON boolean indicating whether password reset is enabled. No authentication is required.

- `true` — an SMTP host is configured.
- `false` — no SMTP host is configured; password reset is disabled.

This checks configuration only and does not connect to the SMTP server. No SMTP settings or credentials are returned.

The portal calls this endpoint when displaying login, registration, and password reset pages. When disabled, it hides the reset link and warns registering users that they will lose access if they forget their password. Both reset API methods also reject requests with HTTP `503` when SMTP is not configured.
