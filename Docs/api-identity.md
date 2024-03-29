# Identity API

The following identity providers are supported:
- `default` - Default UNITE identity provider
- `ldap` - LDAP identity provider (requires configuration)


## POST: [api/realm/[provider]/login](http://localhost:5000/api/realm/default/login) - [api/identity/realm/[provider]/login](https://localhost/api/identity/realm/default/login)
Logs user in.

### Body - application/json
```jsonc
{
    "Email": "test@mail.com",
    "Password": "Long-Pa55w0rd",
}
```

### Responses
- `200` - request was processed successfully
- `400` - request data didn't pass validation

### Resources
- `unite_token` - JWT token as response body
- `unite_session` - session as response cookie (HttpOnly, Secure)


## POST: [api/realm/[provider]/logout](http://localhost:5000/api/realm/default/logout) - [api/identity/realm/[provider]/logout](https://localhost/api/identity/realm/default/logout)
Logs user out.

### Headers
- `Authorization: Bearer [token]` - JWT token

### Responses
- `200` - request was processed successfully
- `401` - missing JWT token


## POST: [api/realm/[provider]/token](http://localhost:5000/api/realm/default/token) - [api/identity/realm/[provider]/token](https://localhost/api/identity/realm/default/token)
Refreshes JWT token.

### Cookies
- `unite_session` - session cookie

### Responses
- `200` - request was processed successfully
- `400` - invalid session cookie
- `401` - missing session cookie


### Resources
- `unite_token` - JWT token as response body
