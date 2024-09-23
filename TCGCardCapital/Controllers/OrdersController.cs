using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TCGCardCapital.DTOs;
using TCGCardCapital.Models;
using TCGCardCapital.Services.IService;
using TCGCardCapital.Services.ServiceImpl;

namespace TCGCardCapital.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetOrders()
        {
            var orders = await _orderService.GetOrdersAsync();
            return Ok(orders);
        }

        [Authorize(Roles = "USER")]
        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetOrdersByUserId(int userId)
        {
            var cartitems = await _orderService.GetOrdersByUserIdAsync(userId);
            return Ok(cartitems);
        }

        [Authorize(Roles = "USER")]
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder([FromBody] OrderDTO orderDTO)
        {
            var createdOrder = await _orderService.CreateOrderAsync(orderDTO);
            return CreatedAtAction(nameof(GetOrders), new { id = createdOrder.OrderId }, createdOrder);
        }

        [Authorize(Roles = "USER")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, [FromBody] OrderDTO orderDTO)
        {
            if (await _orderService.UpdateOrderAsync(id, orderDTO))
            {
                return NoContent();
            }
            return BadRequest();
        }

        [Authorize(Roles = "USER")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            if (await _orderService.DeleteOrderAsync(id))
            {
                return NoContent();
            }
            return BadRequest();
        }
    }
}
