using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.DTO;
using Contracts.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OrderManager.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private IOrderService _orderSvc;

        public OrdersController(IOrderService orderService)
        {
            _orderSvc = orderService;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<OrderDTO> Get()
        {
            return _orderSvc.GetOrders();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            var order = _orderSvc.GetOrder(id);
            if (order == null)
                return NotFound();

            return Ok(order);
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]OrderDTO order)
        {
            if (order == null)
                return BadRequest("Order is null");

            var orderId = _orderSvc.CreateOrder(order, out string errorMessage);
            if (errorMessage != null)
                return BadRequest(errorMessage);

            return Ok(new { id = orderId });
        }
    }
}
