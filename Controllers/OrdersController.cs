using AutoMapper;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace DutchTreat.Controllers
{
    [Route("api/[Controller]")]
    public class OrdersController : Controller
    {
        private readonly ILogger<OrdersController> _logger;
        private readonly IMapper _mapper;
        private readonly IDutchRepository _repository;

        public OrdersController(IDutchRepository repository, 
            ILogger<OrdersController> logger,
            IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get(bool includeItems = true)
        {
            try
            {
                var result = _repository.GetAllOrders(includeItems);
                return Ok(_mapper.Map<IEnumerable<OrderViewModel>>(result));
            } catch (Exception e)
            {
                _logger.LogError($"Failed : {e}");
                return BadRequest();
            }
        }

        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            try
            {
                var order = _repository.GetOrderById(id);
                if (order != null)
                    return Ok(_mapper.Map<Order, OrderViewModel>(order));
                else
                    return NotFound();
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed : {e}");
                return BadRequest();
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] OrderViewModel order)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var newOrder = _mapper.Map<Order>(order);

            if (newOrder.OrderDate == DateTime.MinValue)
                newOrder.OrderDate = DateTime.Now;

            try
            {
                _repository.AddEntity(newOrder);
                if (_repository.SaveAll())
                {
                    var vm = _mapper.Map<OrderViewModel>(newOrder);
                    return Created($"/api/orders/{vm.OrderId}", order);
                }
                return BadRequest("Failed to save order");
            }
            catch (Exception e)
            {
                _logger.LogError($"{e}");
            }
            
            return BadRequest("Something went wrong");
        }
    }
}
