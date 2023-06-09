using ApiMinimalCatalog.ApiEndpoints;
using ApiMinimalCatalog.AppServicesExtensions;
using ApiMinimalCatalog.Context;
using ApiMinimalCatalog.Models;
using ApiMinimalCatalog.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ApiCatalog", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = @"JWT Authorization header using the bearer scheme. 
            Enter 'Bearer'[space].Example: \ 'Bearer 1234Sabcdef\'",
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
);

// ---------- Register Token JWT ----------

builder.Services.AddSingleton<ITokenService>(new TokenService());

// ---------- Validation Token JWT ----------

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// ---------- Login Endpoint ----------

app.MapAuthentication();

// ---------- Category Endpoints ----------

app.MapCategory();

// ---------- Product Endpoints ----------

app.MapProduct();


// Configure the HTTP request pipeline.
var env = app.Environment;

app.UseExceptionHandling(env)
    .UseSwaggerMiddleware()
    .UseAppCors();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
