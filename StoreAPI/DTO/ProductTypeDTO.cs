namespace StoreAPI.DTO
{
    public class ProductTypeDTO
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public int TypeId { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

    }
}
