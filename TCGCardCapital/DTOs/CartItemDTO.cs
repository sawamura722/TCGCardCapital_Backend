namespace TCGCardCapital.DTOs
{
    public class CartItemDTO
    {
        public int? CartItemId { get; set; }

        public int UserId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }
    }
}
