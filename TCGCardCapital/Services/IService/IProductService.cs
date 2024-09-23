using TCGCardCapital.DTOs;

namespace TCGCardCapital.Services.IService
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDTO>> GetProductsAsync();
        Task<ProductDTO> GetProductByIdAsync(int id);
        Task<ProductDTO> CreateProductAsync(ProductDTO productDTO);
        Task<bool> UpdateProductAsync(int id, ProductUpdateDTO productUpdateDTO);
        Task<bool> DeleteProductAsync(int id);
    }
}
