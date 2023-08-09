# Users API
Api is protected and requires the following headers to be set:
- `Authorization: Bearer [token]` - JWT tokenwith `Root` role

## GET: [api/users](http://localhost:5004/api/users)
Returns list of existing users.

### Responses
- `200` - request was processed successfully
- `401` - missing JWT token
- `403` - missing required permissions

### Resources
- [User](#user)[] - list of users


## GET: [api/user](http://localhost:5004/api/user)
Checks if user exists.

### Parameters
- `provider`- identity provider id
- `email` - user email

### Responses
- `200` - request was processed successfully
- `401` - missing JWT token
- `403` - missing required permissions


## GET: [api/user/{id}](http://localhost:5004/api/user/1)
Gets user data.

### Responses
- `200` - request was processed successfully
- `401` - missing JWT token
- `403` - missing required permissions
- `404` - user wasn't found

### Resources
- [User](#user) - user


## POST: [api/user](http://localhost:5004/api/user)
Creates new user.

### Body - application/json
```jsonc
{
    "providerId": 1, // identity provider id
    "email": "test@unite.net", // user email
    "permissions": ["Data.Read"] // list of user permissions
}
```

### Responses
- `200` - request was processed successfully
- `400` - request data didn't pass validation
- `401` - missing JWT token
- `403` - missing required permissions

### Resources
- [User](#user) - created user


## PUT: [api/user/{id}](http://localhost:5004/api/user/1)
Updates user.

### Body - application/json
```jsonc
{
    "providerId": 1, // identity provider id
    "permissions": ["Data.Read"] // list of user permissions
}
```

### Responses
- `200` - request was processed successfully
- `400` - request data didn't pass validation
- `401` - missing JWT token
- `403` - missing required permissions
- `404` - user wasn't found

### Resources
- [User](#user) - updated user


## DELETE: [api/user/{id}](http://localhost:5004/api/user/1)
Deletes user.

### Headers
- `Authorization: Bearer [token]` - JWT token

### Responses
- `200` - request was processed successfully
- `401` - missing JWT token
- `403` - missing required permissions
- `404` - user wasn't found


## Models

### User
```jsonc
{
    "id": 1, // user id
    "email": "test@unite.net", // user email
    "provider": "default", // user identity provider
    "permissions": ["Data.Read"] // list of user permissions
}
```
