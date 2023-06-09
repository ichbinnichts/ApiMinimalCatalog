using ApiMinimalCatalog.Context;
using ApiMinimalCatalog.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiMinimalCatalog.ApiEndpoints
{
    public static class CategoryEndpoints
    {
        public static void MapCategory(this WebApplication app)
        {
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
            }).RequireAuthorization();

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
                return Results.NoContent();
            });
        }
    }
}
