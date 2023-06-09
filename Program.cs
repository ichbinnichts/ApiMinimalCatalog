using ApiMinimalCatalog.ApiEndpoints;
using ApiMinimalCatalog.AppServicesExtensions;
var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.AddApiSwagger();
builder.AddPersistence();
builder.Services.AddCors();
builder.AddAuthenticationJwt();

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
