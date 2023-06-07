﻿using ApiMinimalCatalog.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiMinimalCatalog.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        //Overriding the defaul model creating, expliciting the properties of the models
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Category model config with db
            modelBuilder.Entity<Category>().HasKey(c => c.Id);
            modelBuilder.Entity<Category>().Property(c => c.Name)
                .HasMaxLength(100)
                .IsRequired();
            modelBuilder.Entity<Category>().Property(c => c.Description)
                .HasMaxLength(150)
                .IsRequired();

            //Product model config with db
            modelBuilder.Entity<Product>().HasKey(c => c.Id);
            modelBuilder.Entity<Product>().Property(p => p.Name)
                .HasMaxLength(100)
                .IsRequired();
            modelBuilder.Entity<Product>().Property(p => p.Description)
                .HasMaxLength(150)
                .IsRequired();
            modelBuilder.Entity<Product>().Property(p => p.ImgUrl).HasMaxLength(100);
            modelBuilder.Entity<Product>().Property(p => p.Price).HasPrecision(14, 2);

            //Relatioship
            modelBuilder.Entity<Product>().HasOne<Category>(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);
        }
    }
}
