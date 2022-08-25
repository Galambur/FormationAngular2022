using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace DutchTreat.Data
{
    public class DutchSeeder
    {
        private readonly DutchContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly UserManager<StoreUser> _userManager;

        public DutchSeeder(DutchContext ctx, 
            IWebHostEnvironment env,
            UserManager<StoreUser> userManager)
        {
            _context = ctx;
            _environment = env;
            _userManager = userManager;
        }

        public async Task SeedAsync()
        {
            _context.Database.EnsureCreated();

            StoreUser user = await _userManager.FindByEmailAsync("gaelle@deram");
            if (user == null)
            {
                user = new StoreUser()
                {
                    FirstName = "Gaelle",
                    LastName = "Deram",
                    Email = "gaelle@deram",
                    UserName = "gaelle@deram"
                };

                var result = await _userManager.CreateAsync(user, "P@ssw0rd!");
                if (result != IdentityResult.Success)
                    throw new InvalidOperationException("Could not create new user");
            }

            if (!_context.Products.Any())
            {
                var filePath = Path.Combine(_environment.ContentRootPath, "Data/art.json");
                var json = File.ReadAllText(filePath);
                var products = JsonSerializer.Deserialize<IEnumerable<Product>>(json);

                _context.Products.AddRange(products);

                var order = _context.Orders.FirstOrDefault(o => o.Id == 1);
                if (order != null)
                {
                    order.User = user;
                    order.Items = new List<OrderItem>()
                    {
                        new OrderItem
                        {
                            Product = products.First(),
                            Quantity = 5,
                            UnitPrice = products.First().Price
                        }
                    };
                }

                _context.Add(order);
                _context.SaveChanges();
            }
        }
    }
}
