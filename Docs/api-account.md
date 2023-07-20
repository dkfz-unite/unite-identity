# Account API

## POST: [api/account](http://localhost:5004/api/account)
Creates new account.

### Body - application/json
```jsonc
{
    "email": "test@mail.com",
    "password": "Long-Pa55w0rd",
    "passwordRepeat": "Long-Pa55w0rd",
}
```

**Password requirements**
- Minimum length is 8 characters
- Should have at least one letter
- Should have at least one number
- Passwords should match

### Responses
- `400` - invalid request data
- `200` - successful request


## GET: [api/account](http://localhost:5004/api/account)
Loads account data.

### Headers
- `Authorization: Bearer [token]` - JWT token

### Responses
- `403` - anouthorized
- `200` - successful request

### Resources
```jsonc
{
    "email": "test@mail.com", // email
    "provider": "Default", // identity provider
    "permissions": ["Data.Read"], // permissions
    "devices": [] // active devices
}
```


## PUT: [api/account/password](http://localhost:5004/api/account/password)
Changes account password.

### Headers
- `Authorization: Bearer [token]` - JWT token

### Body - application/json
```jsonc
{
    "oldPassword": "Long-Pa55w0rd", // old password
    "newPassword": "Long-Pa55w0rd", // new password
    "newPasswordRepeat": "Long-Pa55w0rd", // new password repeat
}
```

**Password requirements**
- Minimum length is 8 characters
- Should have at least one letter
- Should have at least one number
- Passwords should match

### Responses
- `400` - invalid request data
- `403` - anouthorized
- `200` - successful request
