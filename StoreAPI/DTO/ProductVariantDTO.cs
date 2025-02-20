namespace StoreAPI.DTO
{
    public class ProductVariantDTO
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public int VariantId { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
