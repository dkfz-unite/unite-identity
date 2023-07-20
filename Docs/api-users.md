# Users API

## GET: [api/users](http://localhost:5004/api/users)
Returns list of available users.

### Headers
- `Authorization: Bearer [token]` - JWT token

### Responses
- `403` - anouthorized
- `200` - successful request

### Resources
- [User](#user)[] - list of users


## GET: [api/user](http://localhost:5004/api/user)
Checks if user exists.

### Headers
- `Authorization: Bearer [token]` - JWT token

### Parameters
- `provider`- identity provider id
- `email` - user email

### Responses
- `403` - anouthorized
- `200` - successful request


## GET: [api/user/{id}](http://localhost:5004/api/user/1)
Returns user by id.

### Headers
- `Authorization: Bearer [token]` - JWT token

### Responses
- `403` - anouthorized
- `200` - successful request

### Resources
- [User](#user) - user


## POST: [api/user](http://localhost:5004/api/user)
Creates new user.

### Headers
- `Authorization: Bearer [token]` - JWT token

### Body - application/json
```jsonc
{
    "providerId": 1, // identity provider id
    "email": "test@unite.net", // user email
    "permissions": ["Data.Read"] // list of user permissions
}
```

### Responses
- `403` - anouthorized
- `200` - successful request

### Resources
- [User](#user) - created user


## PUT: [api/user/{id}](http://localhost:5004/api/user/1)
Updates existing user.

### Headers
- `Authorization: Bearer [token]` - JWT token

### Body - application/json
```jsonc
{
    "providerId": 1, // identity provider id
    "permissions": ["Data.Read"] // list of user permissions
}
```

### Responses
- `403` - anouthorized
- `200` - successful request

### Resources
- [User](#user) - updated user


## DELETE: [api/user/{id}](http://localhost:5004/api/user/1)
Deletes existing user.

### Headers
- `Authorization: Bearer [token]` - JWT token

### Responses
- `403` - anouthorized
- `200` - successful request


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
