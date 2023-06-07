﻿namespace ApiMinimalCatalog.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? ImgUrl { get; set; }
        public DateTime BoughtDate { get; set; }
        public int Stock { get; set; }

        public int CategoryId { get; set; }
        public Category? category { get; set; }
    }
}