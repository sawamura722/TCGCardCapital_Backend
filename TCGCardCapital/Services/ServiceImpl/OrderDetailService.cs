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

            var product = await _context.Products.FindAsync(orderDetail.ProductId);
            if (product == null)
            {
                throw new Exception("Product not found");
            }

            // Check if the product has enough stock
            if (product.Stock < orderDetail.Quantity)
            {
                throw new Exception("Not enough stock for this product");
            }

            // Decrease the product's stock by the order quantity
            product.Stock -= orderDetail.Quantity;

            // Add the order detail to the context
            _context.OrderDetails.Add(orderDetail);

            // Save changes for both the order detail and the product
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

        
    }
}
