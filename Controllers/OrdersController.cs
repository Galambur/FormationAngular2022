using AutoMapper;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DutchTreat.Controllers
{
    [Route("api/[Controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OrdersController : Controller
    {
        private readonly ILogger<OrdersController> _logger;
        private readonly IMapper _mapper;
        private readonly IDutchRepository _repository;
        private readonly UserManager<StoreUser> _userManager;

        public OrdersController(IDutchRepository repository, 
            ILogger<OrdersController> logger,
            IMapper mapper,
            UserManager<StoreUser> userManager)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Get(bool includeItems = true)
        {
            try
            {
                var username = User.Identity.Name;
                var result = _repository.GetAllOrdersByUser(username, includeItems);
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
                var order = _repository.GetOrderById(User.Identity.Name, id);
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
        public async Task<IActionResult> Post([FromBody] OrderViewModel order)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var newOrder = _mapper.Map<Order>(order);

            if (newOrder.OrderDate == DateTime.MinValue)
                newOrder.OrderDate = DateTime.Now;

            try
            {
                var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
                newOrder.User = currentUser;

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
