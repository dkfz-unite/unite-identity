# Account API

## POST: [api/account](http://localhost:5000/api/account) - [api/identity/account](https://localhost/api/identity/account)
Creates new account.

### Body - application/json
```jsonc
{
    "email": "test@mail.com", // email (required, unique, max length 256)
    "password": "Long-Pa55w0rd", // password (required)
    "passwordRepeat": "Long-Pa55w0rd", // password repeat (required)
}
```

**Password requirements**
- Minimum length is 8 characters
- Should have at least one letter
- Should have at least one number
- Passwords should match

### Responses
- `200` - request was processed successfully
- `400` - request data didn't pass validation


## GET: [api/account](http://localhost:5000/api/account) - [api/identity/account](https://localhost/api/identity/account)
Loads account data.

### Headers
- `Authorization: Bearer [token]` - JWT token

### Responses
- `200` - request was processed successfully
- `401` - missing JWT token
- `403` - missing required permissions


### Resources
```jsonc
{
    "email": "test@mail.com", // email
    "provider": "Default", // identity provider
    "permissions": ["Data.Read"], // permissions
    "devices": [] // active devices
}
```


## PUT: [api/account/password](http://localhost:5000/api/account/password) - [api/identity/account/password](https://localhost/api/identity/account/password)
Changes account password.

### Headers
- `Authorization: Bearer [token]` - JWT token

### Body - application/json
```jsonc
{
    "oldPassword": "Long-Pa55w0rd", // old password (required)
    "newPassword": "Long-Pa55w0rd", // new password (required)
    "newPasswordRepeat": "Long-Pa55w0rd", // new password repeat (required)
}
```

**Password requirements**
- Minimum length is 8 characters
- Should have at least one letter
- Should have at least one number
- Passwords should match

### Responses
- `200` - request was processed successfully
- `400` - request data didn't pass validation
- `401` - missing JWT token
- `403` - missing required permissions


## POST: api/account/password-reset

Requests a password reset email. No authentication is required.

The reset token expires after `UNITE_RESET_TTL` minutes (default: 30).

### Body - application/json
```json
{
    "email": "test@mail.com"
}
```

### Responses
- `200` - request was processed; the same response is returned whether the account exists or not
- `400` - request data didn't pass validation
- `503` - password reset is disabled because no SMTP host is configured; no token is generated


## POST: api/account/password-reset-confirm

Consumes a valid, unexpired reset token and sets a new password. No authentication is required. Successful reset removes the user's existing sessions.

### Body - application/json
```json
{
    "token": "password-reset-token",
    "password": "Long-Pa55w0rd",
    "passwordRepeat": "Long-Pa55w0rd"
}
```

The new password must satisfy the password requirements above, and both password fields must match.

### Responses
- `200` - password was reset successfully
- `400` - request data or reset token is invalid, or the token has expired
- `503` - password reset is disabled because no SMTP host is configured; the password is not changed
