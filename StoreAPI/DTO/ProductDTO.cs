using StoreAPI.Models;

namespace StoreAPI.DTO
{
    public class ProductDTO
    {
        public int Id { get; set; }

        public string Slug { get; set; } = null!;

        public string Name { get; set; } = null!;

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public string Description { get; set; }

        public DateTime CreatedAt { get; set; }

    }
}
