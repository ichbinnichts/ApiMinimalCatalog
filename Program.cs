using ApiMinimalCatalog.Context;
using ApiMinimalCatalog.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
);

var app = builder.Build();

//Endpoints

app.MapPost("/categories", async (Category category, AppDbContext db) =>
{
    db.Categories.Add(category);
    await db.SaveChangesAsync();
    return Results.Created($"/categories/{category.Id}", category);
});

app.MapGet("/categories", async (AppDbContext db) =>
{
    await db.Categories.ToListAsync();
});

app.MapGet("/categories/{id:int}", async (int id, AppDbContext db) =>
{
    return await db.Categories.FindAsync(id)
        is Category category ? Results.Ok(category) : Results.NotFound();
});

app.MapPut("/categories/{id:int}", async (int id, Category category, AppDbContext db) =>
{
    if (id != category.Id) return Results.BadRequest();
    var categoryDB = await db.Categories.FindAsync(id);
    if (categoryDB is null) return Results.NotFound();

    categoryDB.Name = category.Name;
    categoryDB.Description = category.Description;
    await db.SaveChangesAsync();
    return Results.Ok(categoryDB);
});
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.Run();
