# unite-identity
Unite identity service.

## General
Identity service provides the following functionality:
- [Identity service web API](/Docs/api.md) - Identity service REST API.


Identity service is written in ASP.NET (.NET 7)

## Dependencies
- [SQL](https://github.com/dkfz-unite/unite-environment/tree/main/programs/postgresql) - SQL server with domain data and user identity data.

## Access
Environment|Address|Port
-----------|-------|----
Host|http://localhost:5004|5004
Docker|http://identity.unite.net|80

## Configuration
To configure the application, change environment variables in either docker or [launchSettings.json](/Unite.Identity.Web/Properties/launchSettings.json) file (if running locally):
Variable|Description|Default(Local)|Default(Docker)
--------|-----------|--------------|---------------
ASPNETCORE_ENVIRONMENT|ASP.NET environment|Debug|Release
UNITE_SQL_HOST|SQL server host|localhost|sql.unite.net
UNITE_SQL_PORT|SQL server port|5432|5432
UNITE_SQL_USER|SQL server user||
UNITE_SQL_PASSWORD|SQL server password||
UNITE_API_KEY|32 bit string API key||
UNITE_ADMIN_USER|Root user login||
UNITE_ADMIN_PASSWORD|Root user password||
UNITE_DEFAULT_LABEL|Default identity provider title|UNITE|UNITE
UNITE_DEFAULT_PRIORITY|Default identity provider priority|1|1
UNITE_LDAP_ACTIVE|LDAP identity provider activation status|false|false
UNITE_LDAP_LABEL|LDAP identity provider title||
UNITE_LDAP_PRIORITY|LDAP identity provider priority||
UNITE_LDAP_HOST|LDAP server host||
UNITE_LDAP_PORT|LDAP server port||
UNITE_LDAP_TARGET_OU|LDAP target OU||
UNITE_LDAP_SERVICE_USER|LDAP service user login||
UNITE_LDAP_SERVICE_PASSWORD|LDAP service user password||

## Installation

### Docker Compose
The easiest way to install the application is to use docker-compose:
- Environment configuration and installation scripts: https://github.com/dkfz-unite/unite-environment
- Identity service configuration and installation scripts: https://github.com/dkfz-unite/unite-environment/tree/main/applications/unite-identity

### Docker
[Dockerfile](/Dockerfile) is used to build an image of the application.
To build an image run the following command:
```
docker build -t unite.identity:latest .
```

All application components should run in the same docker network.
To create common docker network if not yet available run the following command:
```bash
docker network create unite
```

To run application in docker run the following command:
```bash
docker run \
--name unite.identity \
--restart unless-stopped \
--net unite \
--net-alias identity.unite.net \
-p 127.0.0.1:5004:80 \
-e ASPNETCORE_ENVIRONMENT=Release \
-e UNITE_SQL_HOST=sql.unite.net \
-e UNITE_SQL_PORT=5432 \
-e UNITE_SQL_USER=[sql_user] \
-e UNITE_SQL_PASSWORD=[sql_password] \
-e UNITE_API_KEY=[api_key] \
-e UNITE_ADMIN_USER=[admin_user] \
-e UNITE_ADMIN_PASSWORD=[admin_password] \
-e UNITE_DEFAULT_LABEL=UNITE \
-e UNITE_DEFAULT_PRIORITY=1 \
-e UNITE_LDAP_ACTIVE=false \
-e UNITE_LDAP_LABEL=LDAP \
-e UNITE_LDAP_PRIORITY=2 \
-e UNITE_LDAP_HOST=[ldap_host] \
-e UNITE_LDAP_PORT=[ldap_port] \
-e UNITE_LDAP_TARGET_OU=[ldap_target_ou] \
-e UNITE_LDAP_SERVICE_USER=[ldap_service_user] \
-e UNITE_LDAP_SERVICE_PASSWORD=[ldap_service_password] \
-d \
unite.identity:latest
```
