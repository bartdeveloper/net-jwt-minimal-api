using JWT_Minimal_API;
using JWT_Minimal_API.Models;
using JWT_Minimal_API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthenticationWithJWT(builder.Configuration);
builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerWithJWTAuth();

builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.UseAuthentication();
app.UseHttpsRedirection();

app.MapGet("/", [AllowAnonymous] () => "Hello World!")
    .ExcludeFromDescription();

app.MapPost("/login", [AllowAnonymous]
(UserLogin user, IUserService service) => Login(user, service))
    .Accepts<UserLogin>("application/json")
    .Produces<string>(statusCode: 200, contentType: "application/json");

app.MapGet("/user", [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
(HttpContext httpContext, IUserService service) => GetUserClaims(httpContext, service))
    .Produces<User>(statusCode: 200, contentType: "application/json");

IResult Login(UserLogin user, IUserService service)
{

    if (!string.IsNullOrEmpty(user.Username) && !string.IsNullOrEmpty(user.Password))
    {

        var loggedInUser = service.GetUser(user);

        if (loggedInUser is null) return Results.NotFound("User not found");

        var tokenString = service.GenerateToken(loggedInUser);

        return Results.Ok(tokenString);

    }

    return Results.BadRequest("Invalid user credentials");

}

IResult GetUserClaims(HttpContext httpContext, IUserService service)
{

    var userInfo = service.GetUserClaims(httpContext);
   
    if (userInfo is not null)
    {
        return Results.Ok(userInfo);
    }

    return Results.BadRequest();

}

app.Run();
