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
Host|http://localhost:5000|5000
Docker|http://identity.unite.net|80

## Configuration
To configure the application, change environment variables in either docker or [launchSettings.json](/Unite.Identity.Web/Properties/launchSettings.json) file (if running locally):
Variable|Description|Default(Local)|Default(Docker)
--------|-----------|--------------|---------------
ASPNETCORE_ENVIRONMENT|ASP.NET environment|Debug|Release
UNITE_INSTANCE_HOST|Public portal base URL, including scheme (e.g. `https://unite.example.org`); used in password reset email links|http://localhost|
UNITE_INSTANCE_PUBLIC|Allow registration without the access list (`true` or `false`)|false|false
UNITE_RETENTION_PERIOD|Days of inactivity before non-root accounts and their saved data are deleted|90|90
UNITE_SQL_HOST|SQL server host|localhost|sql.unite.net
UNITE_SQL_PORT|SQL server port|5432|5432
UNITE_SQL_USER|SQL server user||
UNITE_SQL_PASSWORD|SQL server password||
UNITE_MONGO_HOST|MongoDB server host; used to remove saved datasets and analyses when deleting accounts|localhost|mongo.unite.net
UNITE_MONGO_PORT|MongoDB server port|27017|27017
UNITE_MONGO_USER|MongoDB server user||
UNITE_MONGO_PASSWORD|MongoDB server password||
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
UNITE_SMTP_HOST|Optional SMTP server host; unset or blank disables email password reset||
UNITE_SMTP_PORT|SMTP server port; required when SMTP is configured|587|
UNITE_SMTP_SSL_ENABLE|Use STARTTLS (`true`) or an unencrypted connection (`false`); required when SMTP is configured|true|
UNITE_SMTP_AUTH_METHOD|SMTP authentication method: `login`, `plain`, or `ntlm`; required when SMTP is configured|login|
UNITE_SMTP_NTLM_DOMAIN|SMTP authentication domain; required only for `ntlm`||
UNITE_SMTP_FROM|Sender email address; required when SMTP is configured||
UNITE_SMTP_USER|SMTP authentication user; required when SMTP is configured||
UNITE_SMTP_PASSWORD|SMTP authentication password; required when SMTP is configured||

SMTP configuration is optional. To enable password reset emails, set `UNITE_SMTP_HOST`, the other required SMTP variables, and `UNITE_INSTANCE_HOST` to the portal's public base URL. SMTP credentials and the sender address must be supplied by the operator; the local port, STARTTLS, and authentication values above come from `launchSettings.json`.

Without an SMTP host, both password reset requests and confirmations return HTTP `503`. No reset token is generated or logged; only an availability warning is logged. The anonymous `GET /api/availability/password-reset` endpoint returns whether an SMTP host is configured. The portal hides the password reset link when unavailable and warns users during registration that forgetting their password means losing access to their account. Registration, login, and authenticated password changes remain available.

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
-p 127.0.0.1:5000:80 \
-e ASPNETCORE_ENVIRONMENT=Release \
-e UNITE_INSTANCE_HOST=https://unite.example.org \
-e UNITE_INSTANCE_PUBLIC=false \
-e UNITE_RETENTION_PERIOD=90 \
-e UNITE_SQL_HOST=sql.unite.net \
-e UNITE_SQL_PORT=5432 \
-e UNITE_SQL_USER=[sql_user] \
-e UNITE_SQL_PASSWORD=[sql_password] \
-e UNITE_MONGO_HOST=mongo.unite.net \
-e UNITE_MONGO_PORT=27017 \
-e UNITE_MONGO_USER=[mongo_user] \
-e UNITE_MONGO_PASSWORD=[mongo_password] \
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
