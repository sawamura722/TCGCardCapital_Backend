using TCGCardCapital.Models;
using TCGCardCapital.Services.IService;
using AutoMapper;
using TCGCardCapital.DTOs;
using Microsoft.EntityFrameworkCore;

namespace TCGCardCapital.Services.ServiceImpl
{
    public class ProductService : IProductService
    {
        private readonly TcgcardCapitalContext _context;
        private readonly IMapper _mapper;
        public ProductService(TcgcardCapitalContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDTO>> GetProductsAsync()
        {
            var products = await _context.Products.ToListAsync();
            return _mapper.Map<IEnumerable<ProductDTO>>(products);
        }

        public async Task<ProductDTO> GetProductByIdAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return null;
            return _mapper.Map<ProductDTO>(product);
        }

        public async Task<ProductDTO> CreateProductAsync(ProductDTO productDTO)
        {
            if (productDTO.Image != null)
            {
                // Generate a unique file name
                var fileName = Guid.NewGuid() + Path.GetExtension(productDTO.Image.FileName);
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
                var uniqueFilePath = Path.Combine(uploadsFolder, fileName);

                // Ensure the upload directory exists
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Save the file to disk
                using (var stream = new FileStream(uniqueFilePath, FileMode.Create))
                {
                    await productDTO.Image.CopyToAsync(stream);
                }

                // Save only the file name in the DTO
                productDTO.ImageUrl = fileName; // Save just the filename or a relative path
            }

            // Map DTO to Product entity
            var product = _mapper.Map<Product>(productDTO);
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            // Return the mapped ProductDTO
            return _mapper.Map<ProductDTO>(product);
        }



        public async Task<bool> UpdateProductAsync(int id, ProductUpdateDTO productUpdateDTO)
        {
            // Fetch the product based on the route id
            var product = await _context.Products.FindAsync(id);
            if (product == null) return false;

            // Update the necessary fields
            product.ProductName = productUpdateDTO.ProductName;
            product.Description = productUpdateDTO.Description;
            product.Price = productUpdateDTO.Price;
            product.Stock = productUpdateDTO.Stock;
            product.CategoryId = productUpdateDTO.CategoryId;

            if (!string.IsNullOrEmpty(productUpdateDTO.ImageUrl))
            {
                product.ImageUrl = productUpdateDTO.ImageUrl;
            }

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Products.Any(p => p.ProductId == id))
                    return false;
                throw;
            }
        }



        public async Task<bool> DeleteProductAsync(int id)
        {
            // First, remove related cart items
            var cartItems = await _context.CartItems.Where(ci => ci.ProductId == id).ToListAsync();
            _context.CartItems.RemoveRange(cartItems);

            // Then, remove the product
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
            return true;
        }
    }
}
