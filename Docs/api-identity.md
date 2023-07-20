# Identity API

The following identity providers are supported:
- `default` - Default UNITE identity provider
- `ldap` - LDAP identity provider (requires configuration)


## POST: [api/realm/[provider]/login](http://localhost:5004/api/realm/default/login)
Logs user in.

### Body - application/json
```jsonc
{
    "Email": "test@mail.com",
    "Password": "Long-Pa55w0rd",
}
```

### Responses
- `400` - invalid request data
- `200` - successful request

### Resources
- `unite_token` - JWT token as response body
- `unite_session` - session as response cookie (HttpOnly, Secure)


## POST: [api/realm/[provider]/logout](http://localhost:5004/api/realm/default/logout)
Logs user out.

### Headers
- `Authorization: Bearer [token]` - JWT token

### Responses
- `403` - invalid JWT token
- `200` - successful request


## POST: [api/realm/[provider]/token](http://localhost:5004/api/realm/default/token)
Refreshes JWT token.

### Cookies
- `unite_session` - session cookie

### Responses
- `403` - missing session cookie
- `400` - invalid session cookie
- `200` - successful request

### Resources
- `unite_token` - JWT token as response body
