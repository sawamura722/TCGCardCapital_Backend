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
    public class OrderDetailsController : ControllerBase
    {
        private readonly IOrderDetailService _orderDetailService;

        public OrderDetailsController(IOrderDetailService orderDetailService)
        {
            _orderDetailService = orderDetailService;
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDetailDTO>>> GetOrderDetails()
        {
            var orderDetails = await _orderDetailService.GetOrderDetailsAsync();
            return Ok(orderDetails);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDetailDTO>> GetOrderDetail(int id)
        {
            var orderDetail = await _orderDetailService.GetOrderDetailByIdAsync(id);

            if (orderDetail == null)
            {
                return NotFound();
            }

            return Ok(orderDetail);
        }

        [Authorize(Roles = "USER")]
        [HttpPost]
        public async Task<ActionResult<OrderDetail>> PostOrderDetail([FromForm] OrderDetailDTO orderDetailDTO)
        {
            var createdOrderDetail = await _orderDetailService.CreateOrderDetailAsync(orderDetailDTO);
            return CreatedAtAction(nameof(GetOrderDetails), new { id = createdOrderDetail.OrderDetailId }, createdOrderDetail);
        }

        [Authorize(Roles = "USER")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrderDetail(int id, [FromForm] OrderDetailDTO orderDetailDTO)
        {
            if (await _orderDetailService.UpdateOrderDetailAsync(id, orderDetailDTO))
            {
                return NoContent();
            }
            return BadRequest();
        }

    }
}
