# Account API

## POST: [api/account](http://localhost:5004/api/account)
Creates new account.

### Body - application/json
```jsonc
{
    "Email": "test@mail.com",
    "Password": "Long-Pa55w0rd",
    "PasswordRepeat": "Long-Pa55w0rd",
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
    "Email": "test@mail.com", // email
    "Provider": "Default", // identity provider
    "Permissions": ["Data.Read"], // permissions
    "Devices": [] // active devices
}
```


## PUT: [api/account/password](http://localhost:5004/api/account/password)
Changes account password.

### Headers
- `Authorization: Bearer [token]` - JWT token

### Body - application/json
```jsonc
{
    "OldPassword": "Long-Pa55w0rd", // old password
    "NewPassword": "Long-Pa55w0rd", // new password
    "NewPasswordRepeat": "Long-Pa55w0rd", // new password repeat
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
