using TCGCardCapital.DTOs;
using TCGCardCapital.Models;

namespace TCGCardCapital.Services.IService
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDTO>> GetOrdersAsync();
        Task<IEnumerable<OrderDTO>> GetOrdersByUserIdAsync(int userId);
        Task<OrderDTO> GetOrderByUserIdAsync(int userId, int orderId);
        Task<OrderDTO> CreateOrderAsync(OrderDTO orderDTO);
        Task<bool> UpdateOrderAsync(int id, UpdateOrderDTO updateOrderDTO);
        Task<bool> DeleteOrderAsync(int id);
    }
}
