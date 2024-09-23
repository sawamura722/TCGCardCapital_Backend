namespace TCGCardCapital.DTOs
{
    public class ProductUpdateDTO
    {
        public string ProductName { get; set; } = null!;

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public int Stock { get; set; }

        public int CategoryId { get; set; }

        public string? ImageUrl { get; set; }
        public IFormFile? Image { get; set; }
    }
}
