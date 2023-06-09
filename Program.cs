using ApiMinimalCatalog.Context;
using ApiMinimalCatalog.Models;
using Microsoft.AspNetCore.Mvc;
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

// ---------- Category Endpoints ----------

app.MapPost("/categories", async (Category category, AppDbContext db) =>
{
    db.Categories.Add(category);
    await db.SaveChangesAsync();
    return Results.Created($"/categories/{category.Id}", category);
});

app.MapGet("/categories", async (AppDbContext db) =>
{
    return await db.Categories.ToListAsync();
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

app.MapDelete("/categories/{id:int}", async (int id, AppDbContext db) =>
{
    var category = await db.Categories.FindAsync(id);
    if (category is null) return Results.NotFound();
    db.Remove(category);
    await db.SaveChangesAsync();
    return Results.Ok();
});

// ---------- Product Endpoints ----------

app.MapGet("/products", async (AppDbContext db) =>
{
    return await db.Products.ToListAsync();
});

app.MapGet("/products/{id:int}", async (int id, AppDbContext db) =>
{
    var product = await db.Products.FindAsync(id);
    if (product is null) return Results.NotFound();
    return Results.Ok(product);
});

app.MapPost("/products", async (Product product, AppDbContext db) =>
{
    db.Products.Add(product);
    await db.SaveChangesAsync();
    return Results.Ok(product);
});

app.MapPut("/products/{id:int}", async (int id, Product product, AppDbContext db) =>
{
    if (id != product.Id) return Results.BadRequest();
    var productDB = await db.Products.FindAsync(id);
    if (productDB is null) return Results.NotFound();

    productDB.Name = product.Name;
    productDB.Description = product.Description;
    await db.SaveChangesAsync();
    return Results.Ok(productDB);
});


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.Run();
