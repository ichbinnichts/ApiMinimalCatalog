using ApiMinimalCatalog.Context;
using ApiMinimalCatalog.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiMinimalCatalog.ApiEndpoints
{
    public static class ProductEndpoints
    {
        public static void MapProduct(this WebApplication app)
        {
            // ---------- Product Endpoints ----------

            app.MapGet("/products", async (AppDbContext db) =>
            {
                return await db.Products.ToListAsync();
            }).RequireAuthorization();

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

            app.MapDelete("/products/{id:int}", async (int id, AppDbContext db) =>
            {
                var product = await db.Products.FindAsync(id);
                if (product is null) return Results.NotFound();
                db.Products.Remove(product);
                await db.SaveChangesAsync();
                return Results.NoContent();
            });
        }
    }
}
