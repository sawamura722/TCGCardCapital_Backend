using TCGCardCapital.DTOs;

namespace TCGCardCapital.Services.IService
{
    public interface IOrderDetailService
    {
        Task<IEnumerable<OrderDetailDTO>> GetOrderDetailsAsync();
        Task<OrderDetailDTO> GetOrderDetailByIdAsync(int id);
        Task<OrderDetailDTO> CreateOrderDetailAsync(OrderDetailDTO orderDetailDTO);
        Task<bool> UpdateOrderDetailAsync(int id, OrderDetailDTO orderDetailDTO);
     
    }
}
