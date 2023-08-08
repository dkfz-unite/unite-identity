# Account API

## POST: [api/account](http://localhost:5004/api/account)
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
- `200` - successful request
- `400` - invalid request data


## GET: [api/account](http://localhost:5004/api/account)
Loads account data.

### Headers
- `Authorization: Bearer [token]` - JWT token

### Responses
- `200` - successful request
- `401` - missing token
- `403` - invalid token

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
- `200` - successful request
- `400` - invalid request data
- `401` - missing token
- `403` - invalid token
