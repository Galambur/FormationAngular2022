using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DutchTreat.Data
{
    public class DutchRepository : IDutchRepository
    {
        private readonly DutchContext _context;
        private readonly ILogger<DutchRepository> _logger;

        public DutchRepository(DutchContext context, ILogger<DutchRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            try
            {
                _logger.LogInformation("GetAllProducts");
                return _context.Products.OrderBy(p => p.Title).ToList();
            } catch (Exception e)
            {
                _logger.LogError($"Failed to get all products : {e}");
                return null;
            }
        }

        public IEnumerable<Product> GetProductByCategory(string category)
        {
            return _context.Products.Where(p => p.Category == category).ToList();
        }

        public IEnumerable<Order> GetAllOrders()
        {
            try
            {
                _logger.LogInformation("GetAllOrders");
                return _context.Orders
                    .Include(o => o.Items)
                    .ThenInclude(i => i.Product)
                    .ToList();
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to get all orders : {e}");
                return null;
            }
        }

        public Order GetOrderById(int id)
        {
            try
            {
                _logger.LogInformation("GetAllOrders");
                return _context.Orders
                    .Include(o => o.Items)
                    .ThenInclude(i => i.Product)
                    .SingleOrDefault(o => o.Id == id);
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to get all orders : {e}");
                return null;
            }
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }

        public void AddEntity(object model)
        {
            _context.Add(model);
        }
    }
}
