using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TCGCardCapital.DTOs;
using TCGCardCapital.Models;
using TCGCardCapital.Services.IService;

namespace TCGCardCapital.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts()
        {
            var products = await _productService.GetProductsAsync();
            return  Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProduct(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public async Task<ActionResult<ProductDTO>> PostProduct([FromForm] ProductDTO productDTO)
        {
            try
            {
                // The image handling is now managed in the service method.
                var createdProduct = await _productService.CreateProductAsync(productDTO);
                return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.ProductName }, createdProduct);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }



        [Authorize(Roles = "ADMIN")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, [FromForm] ProductUpdateDTO productUpdateDTO)
        {
            try
            {
                // Handle image upload
                if (productUpdateDTO.Image != null && productUpdateDTO.Image.Length > 0)
                {
                    var uploadFolder = @"D:\Coding File\CS\TCGCardCapital\TCGCardCapital\Uploads";
                    if (!Directory.Exists(uploadFolder))
                    {
                        Directory.CreateDirectory(uploadFolder);
                    }

                    // Generate a unique file name
                    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(productUpdateDTO.Image.FileName);
                    var filePath = Path.Combine(uploadFolder, uniqueFileName);

                    // Save the file to disk
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await productUpdateDTO.Image.CopyToAsync(stream);
                    }

                    // Set ImageUrl to only the filename
                    productUpdateDTO.ImageUrl = uniqueFileName;
                }
                else if (productUpdateDTO.ImageUrl != null)
                {
                    // If no new image is uploaded, keep the existing ImageUrl
                    productUpdateDTO.ImageUrl = productUpdateDTO.ImageUrl;
                }

                // Call the service to update the product
                if (await _productService.UpdateProductAsync(id, productUpdateDTO))
                {
                    return NoContent();
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }




        [Authorize(Roles = "ADMIN")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if(await _productService.DeleteProductAsync(id))
            {
                return NoContent();
            }
            return BadRequest();
        }


    }
}
