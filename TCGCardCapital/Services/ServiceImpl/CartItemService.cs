using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TCGCardCapital.DTOs;
using TCGCardCapital.Models;
using TCGCardCapital.Services.IService;

namespace TCGCardCapital.Services.ServiceImpl
{
    public class CartItemService : ICartItemService
    {
        private readonly TcgcardCapitalContext _context;
        private readonly IMapper _mapper;

        public CartItemService(TcgcardCapitalContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CartItemDTO>> GetCartItemsByUserIdAsync(int userId)
        {
            var cartitems = await _context.CartItems
                .Where(c => c.UserId == userId)
                .ToListAsync();
            return _mapper.Map<IEnumerable<CartItemDTO>>(cartitems);
        }

        public async Task<IEnumerable<CartItemDTO>> GetCartItemsByUserIdProductIdAsync(int userId, int productId)
        {
            var cartItems = await _context.CartItems
                .Where(c => c.UserId == userId && c.ProductId == productId)
                .ToListAsync();

            return _mapper.Map<IEnumerable<CartItemDTO>>(cartItems);
        }

        public async Task<IEnumerable<CartItemDTO>> GetCartItemsAsync()
        {
            var cartitems = await _context.CartItems.ToListAsync();
            return _mapper.Map<IEnumerable<CartItemDTO>>(cartitems);
        }

        public async Task<CartItemDTO> CreateCartItemAsync(CartItemDTO cartItemDTO)
        {
            // Check if the cart item already exists for the user and product
            var existingCartItem = await _context.CartItems
                .FirstOrDefaultAsync(c => c.UserId == cartItemDTO.UserId && c.ProductId == cartItemDTO.ProductId);

            if (existingCartItem != null)
            {
                // If it exists, increment the quantity
                existingCartItem.Quantity += cartItemDTO.Quantity;

                // Update the existing cart item
                _context.Entry(existingCartItem).State = EntityState.Modified;
            }
            else
            {
                // If it does not exist, create a new cart item
                var newCartItem = _mapper.Map<CartItem>(cartItemDTO);
                _context.CartItems.Add(newCartItem);

                existingCartItem = newCartItem; // Set the existingCartItem to the newly created one for return purposes
            }

            // Save changes to the database
            await _context.SaveChangesAsync();

            // Return the updated or newly created cart item as a DTO
            return _mapper.Map<CartItemDTO>(existingCartItem);
        }


        public async Task<bool> UpdateCartItemAsync(int userId, int productId, CartItemDTO cartItemDTO)
        {
            if (userId != cartItemDTO.UserId || productId != cartItemDTO.ProductId) return false;

            var cartitem = await _context.CartItems
                .FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId);

            if (cartitem == null) return false;

            cartitem.Quantity = cartItemDTO.Quantity;

            try
            {
                _context.Entry(cartitem).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                // Log the concurrency issue
                return false;
            }
        }



        public async Task<bool> DeleteCartItemAsync(int userId, int productId)
        {
            var cartitem = await _context.CartItems
                .FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId);

            if (cartitem == null) return false;

            _context.CartItems.Remove(cartitem);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
