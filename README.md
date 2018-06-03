# Angular Front End with ASP .NET Core 2.0 API using JWT Access/Refresh Token authentication

## Features:
* Custom User Identity Setup
* Controller acces using JWT Access token
* Auto JWT Access token refresh using Refresh token
* Angular Web App with HttpClient wrapper for JWT token managment

## First time setup:
1. Open **appsettings.json**
2. Set default **Admin** account(this will be created on first start)
    * Only **UserName** and **Password** fields are required
3. Set 256-bit **SecretKey** inside **JwtIssuerOptions**
