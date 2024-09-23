using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TCGCardCapital.DTOs;
using TCGCardCapital.Models;
using TCGCardCapital.Services.IService;

namespace TCGCardCapital.Services.ServiceImpl
{
    public class OrderDetailService : IOrderDetailService
    {
        private readonly TcgcardCapitalContext _context;
        private readonly IMapper _mapper;

        public OrderDetailService(TcgcardCapitalContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrderDetailDTO>> GetOrderDetailsAsync()
        {
            var orderDetails = await _context.OrderDetails.ToListAsync();
            return _mapper.Map<IEnumerable<OrderDetailDTO>>(orderDetails);
        }

        public async Task<OrderDetailDTO> GetOrderDetailByIdAsync(int id)
        {
            var orderDetail = await _context.OrderDetails.FindAsync(id);
            if (orderDetail == null) return null;
            return _mapper.Map<OrderDetailDTO>(orderDetail);
        }

        public async Task<OrderDetailDTO> CreateOrderDetailAsync(OrderDetailDTO orderDetailDTO)
        {
            var orderDetail = _mapper.Map<OrderDetail>(orderDetailDTO);
            _context.OrderDetails.Add(orderDetail);
            await _context.SaveChangesAsync();
            return _mapper.Map<OrderDetailDTO>(orderDetail);
        }

        public async Task<bool> UpdateOrderDetailAsync(int id, OrderDetailDTO orderDetailDTO)
        {
            if (id != orderDetailDTO.OrderDetailId) return false;

            var category = _mapper.Map<OrderDetail>(orderDetailDTO);
            _context.Entry(category).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.OrderDetails.Any(o => o.OrderDetailId == id))
                    return false;
                throw;
            }
        }

        public async Task<bool> DeleteOrderDetailAsync(int id)
        {
            var orderDetail = await _context.OrderDetails.FindAsync(id);
            if (orderDetail == null) return false;

            _context.OrderDetails.Remove(orderDetail);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
