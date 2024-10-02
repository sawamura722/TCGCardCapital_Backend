using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TCGCardCapital.DTOs;
using TCGCardCapital.Models;
using TCGCardCapital.Services.IService;
using TCGCardCapital.Services.ServiceImpl;

namespace TCGCardCapital.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemsController : ControllerBase
    {
        private readonly ICartItemService _cartitemService;

        public CartItemsController(ICartItemService cartitemService)
        {
            _cartitemService = cartitemService;
        }

        [Authorize(Roles = "USER")]
        [HttpGet("{userId}")] 
        public async Task<ActionResult<IEnumerable<CartItemDTO>>> GetCartItems(int userId)
        {
            var cartitems = await _cartitemService.GetCartItemsByUserIdAsync(userId);
            return Ok(cartitems);
        }

        [Authorize(Roles = "USER")]
        [HttpGet("{userId}/{productId}")]
        public async Task<ActionResult<IEnumerable<CartItemDTO>>> GetCartItemEachUser(int userId, int productId)
        {
            var cartitem = await _cartitemService.GetCartItemsByUserIdProductIdAsync(userId, productId);
            return Ok(cartitem);
        }

        [Authorize(Roles = "USER")]
        [HttpPost]
        public async Task<ActionResult<CartItem>> PostCartItem([FromForm] CartItemDTO cartItemDTO)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get user ID from claims
            cartItemDTO.UserId = int.Parse(userId);

            var createdCartItem = await _cartitemService.CreateCartItemAsync(cartItemDTO);
            return CreatedAtAction(nameof(GetCartItems), new { userId = cartItemDTO.UserId }, createdCartItem);
        }


        [Authorize(Roles = "USER")]
        [HttpPut("{userId}/{productId}")]
        public async Task<IActionResult> PutCartItem(int userId, int productId, [FromForm] CartItemDTO cartItemDTO)
        {
            if (userId != cartItemDTO.UserId || productId != cartItemDTO.ProductId)
            {
                return BadRequest("User ID or Product ID mismatch.");
            }

            if (await _cartitemService.UpdateCartItemAsync(userId, productId, cartItemDTO))
            {
                return NoContent();
            }
            return NotFound();
        }

        [Authorize(Roles = "USER")]
        [HttpDelete("{userId}/{productId}")]
        public async Task<IActionResult> DeleteCartItem(int userId, int productId)
        {
            if (await _cartitemService.DeleteCartItemAsync(userId, productId))
            {
                return NoContent();
            }
            return NotFound();
        }

        [Authorize(Roles = "USER")]
        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteAllCartItem(int userId)
        {
            if (await _cartitemService.DeleteAllCartItemsAsync(userId))
            {
                return NoContent();
            }
            return NotFound();
        }

    }
}
