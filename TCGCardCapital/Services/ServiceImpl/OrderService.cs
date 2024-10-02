using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TCGCardCapital.DTOs;
using TCGCardCapital.Models;
using TCGCardCapital.Services.IService;

namespace TCGCardCapital.Services.ServiceImpl
{
    public class OrderService : IOrderService
    {
        private readonly TcgcardCapitalContext _context;
        private readonly IMapper _mapper;

        public OrderService(TcgcardCapitalContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrderDTO>> GetOrdersAsync()
        {
            var orders = await _context.Orders.ToListAsync();
            return _mapper.Map<IEnumerable<OrderDTO>>(orders);
        }

        public async Task<IEnumerable<OrderDTO>> GetOrdersByUserIdAsync(int userId)
        {
            var orders = await _context.Orders
                .Where(o => o.UserId == userId)
                .ToListAsync();
            return _mapper.Map<IEnumerable<OrderDTO>>(orders);
        }

        public async Task<OrderDTO> GetOrderByUserIdAsync(int userId, int orderId)
        {
            var order = await _context.Orders
                .Where(o => o.UserId == userId && o.OrderId == orderId)
                .FirstOrDefaultAsync();

            return _mapper.Map<OrderDTO>(order);
        }

        public async Task<OrderDTO> CreateOrderAsync(OrderDTO orderDTO)
        {
            var order = _mapper.Map<Order>(orderDTO);
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return _mapper.Map<OrderDTO>(order);
        }

        public async Task<bool> UpdateOrderAsync(int id, UpdateOrderDTO updateOrderDTO)
        {
            if (id != updateOrderDTO.OrderId) return false;

            var existingOrder = await _context.Orders.FindAsync(id);
            if (existingOrder == null) return false;

            existingOrder.Status = updateOrderDTO.Status;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Orders.Any(o => o.OrderId == id))
                    return false;
                throw;
            }
        }


        public async Task<bool> DeleteOrderAsync(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product) // Include the related products to update the stock
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null) return false;

            // Loop through each order detail and update the product stock
            foreach (var orderDetail in order.OrderDetails)
            {
                var product = orderDetail.Product;

                if (product != null)
                {
                    // Increase the product's stock by the order detail quantity
                    product.Stock += orderDetail.Quantity;
                }
            }

            // Remove all related order details first
            _context.OrderDetails.RemoveRange(order.OrderDetails);

            // Now remove the order
            _context.Orders.Remove(order);

            // Save changes to the database
            await _context.SaveChangesAsync();

            return true;
        }


    }
}
