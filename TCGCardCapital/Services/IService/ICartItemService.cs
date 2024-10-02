using System.Threading.Tasks;
using TCGCardCapital.DTOs;

namespace TCGCardCapital.Services.IService
{
    public interface ICartItemService
    {
        Task<IEnumerable<CartItemDTO>> GetCartItemsByUserIdAsync(int userId);
        Task<IEnumerable<CartItemDTO>> GetCartItemsByUserIdProductIdAsync(int userId, int productId);
        Task<IEnumerable<CartItemDTO>> GetCartItemsAsync();
        Task<CartItemDTO> CreateCartItemAsync(CartItemDTO cartItemDTO);
        Task<bool> UpdateCartItemAsync(int userId, int productId, CartItemDTO cartItemDTO);
        Task<bool> DeleteCartItemAsync(int userId, int productId);
        Task<bool> DeleteAllCartItemsAsync(int userId);
    }
}
