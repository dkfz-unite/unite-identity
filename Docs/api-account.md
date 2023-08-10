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
