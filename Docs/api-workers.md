# Workers API
Api is protected and requires the following headers to be set:
- `Authorization: Bearer [token]` - JWT token with `Root` role

## GET: [api/workers](http://localhost:5000/api/workers) - [api/identity/workers](https://localhost/api/identity/workers)
Returns list of existing workers.

### Responses
- `200` - request was processed successfully
- `401` - missing JWT token
- `403` - missing required permissions

### Resources
- [Worker](#worker)[] - list of workers


## GET: [api/worker](http://localhost:5000/api/worker) - [api/identity/worker](https://localhost/api/identity/worker)
Checks if worker exists.

### Parameters
- `name` - worker name

### Responses
- `200` - request was processed successfully
- `401` - missing JWT token
- `403` - missing required permissions


## GET: [api/worker/{id}](http://localhost:5000/api/worker/1) - [api/identity/worker/{id}](https://localhost/api/identity/worker/1)
Returns worker by id.

### Responses
- `200` - request was processed successfully
- `401` - missing JWT token
- `403` - missing required permissions
- `404` - worker wasn't found

### Resources
- [Worker](#worker) - worker


## POST: [api/worker](http://localhost:5000/api/worker) - [api/identity/worker](https://localhost/api/identity/worker)
Creates new worker.

### Body - application/json
```jsonc
{
    "name": "Worker-1", // worker name (required, unique, max length: 100)
    "description": "Worker-1 description", // worker description
    "permissions": ["Data.Read"] // list of worker permissions
}
```

### Responses
- `200` - request was processed successfully
- `400` - request data didn't pass validation
- `401` - missing JWT token
- `403` - missing required permissions


## PUT: [api/worker/{id}](http://localhost:5000/api/worker/1) - [api/identity/worker/{id}](https://localhost/api/identity/worker/1)
Updates worker.

### Body - application/json
```jsonc
{
    "name": "Worker 1", // worker name
    "description": "Worker 1 description", // worker description
    "permissions": ["Data.Read"] // list of worker permissions
}
```

### Responses
- `200` - request was processed successfully
- `400` - request data didn't pass validation
- `401` - missing JWT token
- `403` - missing required permissions
- `404` - worker wasn't found


## DELETE: [api/worker/{id}](http://localhost:5000/api/worker/1) - [api/identity/worker/{id}](https://localhost/api/identity/worker/1)
Deletes worker.

### Responses
- `200` - request was processed successfully
- `401` - missing JWT token
- `403` - missing required permissions
- `404` - worker wasn't found


## GET: [api/worker/{id}/token](http://localhost:5000/api/worker/1/token) - [api/identity/worker/{id}/token](https://localhost/api/identity/worker/1/token)
Returns worker token.

### Responses
- `200` - request was processed successfully
- `401` - missing JWT token
- `403` - missing required permissions
- `404` - worker wasn't found

### Resources
- `unite_token` - JWT token as response body


## POST: [api/worker/{id}/token](http://localhost:5000/api/worker/1/token) - [api/identity/worker/{id}/token](https://localhost/api/identity/worker/1/token)
Creates new worker token.

**NOTE**: Worker token is not revokable. It's valid until it expires. Deletion of the worker will not revoke the token. Share the token only with trusted applications. Choose token expiry time wisely.

### Body - application/json
```jsonc
{
    // Summ of these values will be used as token expiry time (Max: 365d 23h 59m)
    "expiryMinutes": 15, // token expiry time in minutes (1..59)
    "expiryHours": 1, // token expiry time in hours (1..23)
    "expiryDays": 1, // token expiry time in days (1..365)
}
```

### Responses
- `200` - request was processed successfully
- `400` - request data didn't pass validation
- `401` - missing JWT token
- `403` - missing required permissions
- `404` - worker wasn't found

### Resources
- `unite_token` - JWT token as response body


## PUT: [api/worker/{id}/token](http://localhost:5000/api/worker/1/token) - [api/identity/worker/{id}/token](https://localhost/api/identity/worker/1/token)
Updates worker token.

**NOTE**: Worker token is not revokable. It's valid until it expires. Deletion of the worker will not revoke the token. Share the token only with trusted applications. Choose token expiry time wisely.

### Body - application/json
```jsonc
{
    // Summ of these values will be used as token expiry time (Max: 365d 23h 59m)
    "expiryMinutes": 15, // token expiry time in minutes (1..59)
    "expiryHours": 1, // token expiry time in hours (1..23)
    "expiryDays": 1, // token expiry time in days (1..365)
}
```

### Responses
- `200` - request was processed successfully
- `400` - request data didn't pass validation
- `401` - missing JWT token
- `403` - missing required permissions
- `404` - worker wasn't found


## Models

### Worker
```jsonc
{
    "id": 1, // worker id
    "name": "Worker 1", // worker name
    "description": "Worker 1 description", // worker description
    "permissions": ["Data.Read", "Data.Write"], // list of worker permissions
    "toekExpiryDate": "2024-03-17T09:45:10.9359202Z", // worker token expiry date
}
```
