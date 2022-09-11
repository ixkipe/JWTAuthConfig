# JWTAuthConfig

A humble class library which allows you to add a simple authorization feature (login/password) to your ASP.NET Core Web API with the help of JSON Web Tokens (JWT).

A huge shoutout to @PatrickGod and all his videos on ASP.NET Core Web API; this classlib relies heavily on two of his videos, check them out:

.NET 6 Web API 🔒 Create JSON Web Tokens (JWT) - User Registration / Login / Authentication: https://www.youtube.com/watch?v=v7q3pEK1EA0

.NET 6 Web API 🔒 Role-Based Authorization with JSON Web Tokens (JWT): https://www.youtube.com/watch?v=TDY_DtTEkes

To configure login feature with this library, follow these steps:

1. Configure Swagger options in ``builder.Services.AddSwaggerGen()`` method:

    ``builder.Services.AddSwaggerGen(options => options.ConfigureSwaggerOptions());``
    
2. Add a token to your configuration file (appsettings.json):

    ![token](https://user-images.githubusercontent.com/99867292/189518684-5758399d-60fd-4bfe-a81c-5c0c8a8b4660.PNG)

    **NOTE:** Your token must be at least 16 characters long.

3. Configure authentication and reference your JWT as a method parameter:

    ``builder.ConfigureJWTAuthentication("AppSettings:Token");``
    
4. Use ``AuthenticationMiddleware`` for your app:

    ``app.UseAuthentication();``
    
5. Set up an authorization controller that will have ``Register()`` and ``Login()`` functions.
   I have it like so:
   
   ![authcontroller](https://user-images.githubusercontent.com/99867292/189518993-24948303-e369-4b0a-9987-4e62773f4825.PNG)

6. Set ``[Authorize]`` and ``[AllowAnonymous]`` attributes next to classes and functions which will require the user to authorize so as to be accessed.

    ![authorize](https://user-images.githubusercontent.com/99867292/189519152-74ac2e78-4164-4b3c-b617-994d7f7e6cc6.PNG)

**That's it!** Storing users and retrieving them from the database is up to you.
