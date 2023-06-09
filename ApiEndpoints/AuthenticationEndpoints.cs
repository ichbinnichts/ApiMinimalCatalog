using ApiMinimalCatalog.Models;
using ApiMinimalCatalog.Services;
using Microsoft.AspNetCore.Authorization;

namespace ApiMinimalCatalog.ApiEndpoints
{
    public static class AuthenticationEndpoints
    {
        public static void MapAuthentication(this WebApplication app)
        {
            // ---------- Login Endpoint ----------

            app.MapPost("/login", [AllowAnonymous] (UserModel user, ITokenService tokenService) =>
            {
                if (user is null) return Results.BadRequest();
                if (user.Username == "nathanfaria" && user.Password == "mysecretpassword")
                {
                    var tokenString = tokenService.GenerateToken(app.Configuration["Jwt:Key"],
                        app.Configuration["Jwt:Issuer"],
                        user);
                    return Results.Ok(new { token = tokenString });
                }
                else
                {
                    return Results.BadRequest();
                }
            });
        }
    }
}
