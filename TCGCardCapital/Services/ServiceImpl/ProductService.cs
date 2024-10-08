﻿using TCGCardCapital.Models;
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

            // If a new image is provided, save it and delete the old one
            if (productUpdateDTO.Image != null)
            {
                // Delete the old image if it exists
                if (!string.IsNullOrEmpty(product.ImageUrl))
                {
                    var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", product.ImageUrl);
                    if (File.Exists(oldImagePath))
                    {
                        File.Delete(oldImagePath);
                    }
                }

                // Save the new image
                var fileName = Guid.NewGuid() + Path.GetExtension(productUpdateDTO.Image.FileName);
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
                var uniqueFilePath = Path.Combine(uploadsFolder, fileName);

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                using (var stream = new FileStream(uniqueFilePath, FileMode.Create))
                {
                    await productUpdateDTO.Image.CopyToAsync(stream);
                }

                // Update the product's ImageUrl with the new file name
                product.ImageUrl = fileName;
            }

            // Update the necessary fields
            product.ProductName = productUpdateDTO.ProductName;
            product.Description = productUpdateDTO.Description;
            product.Price = productUpdateDTO.Price;
            product.Stock = productUpdateDTO.Stock;
            product.CategoryId = productUpdateDTO.CategoryId;

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
            // First, delete all related order details
            var orderDetails = await _context.OrderDetails
                .Where(od => od.ProductId == id)
                .ToListAsync();

            if (orderDetails.Any())
            {
                _context.OrderDetails.RemoveRange(orderDetails);
                await _context.SaveChangesAsync(); // Save changes to remove order details
            }

            // Next, delete all related cart items
            var cartItems = await _context.CartItems
                .Where(ci => ci.ProductId == id)
                .ToListAsync();

            if (cartItems.Any())
            {
                _context.CartItems.RemoveRange(cartItems);
                await _context.SaveChangesAsync(); // Save changes to remove cart items
            }

            // Now delete the product
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return false; // Product not found
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync(); // Save changes to remove the product

            return true;
        }

    }
}
