using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace DutchTreat.Data
{
    public class DutchSeeder
    {
        private readonly DutchContext _context;
        private readonly IWebHostEnvironment _environment;

        public DutchSeeder(DutchContext ctx, IWebHostEnvironment env)
        {
            _context = ctx;
            _environment = env;
        }

        public void Seed()
        {
            _context.Database.EnsureCreated();

            if (!_context.Products.Any())
            {
                var filePath = Path.Combine(_environment.ContentRootPath, "Data/art.json");
                var json = File.ReadAllText(filePath);
                var products = JsonSerializer.Deserialize<IEnumerable<Product>>(json);

                _context.Products.AddRange(products);

                var order = new Order
                {
                    OrderDate = System.DateTime.Now,
                    OrderNumber = "1000",
                    Items = new List<OrderItem>()
                    {
                        new OrderItem
                        {
                            Product = products.First(),
                            Quantity = 5,
                            UnitPrice =products.First().Price
                        }
                    }
                };
                _context.Add(order);

                _context.SaveChanges();
            }
        }
    }
}
