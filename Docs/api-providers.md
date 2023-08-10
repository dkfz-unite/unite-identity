# Providers API

## GET: [api/providers](http://localhost:5000/api/providers) - [api/identity/providers](https://localhost/api/identity/providers)
Returns list of available identity providers.

### Responses
- `200` - request was processed successfully

### Resources
- [Provider](#provider)[] - list of identity providers


## Models

### Provider
```jsonc
{
    "id": 1, // identity provider id
    "name": "default", // identity provider name
    "label": "UNITE", // identity provider label
    "priority": 1 // identity provider priority
}
```
