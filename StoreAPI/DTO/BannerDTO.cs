namespace StoreAPI.DTO
{
    public class BannerDTO
    {
        public int Id { get; set; }

        public int storeId { get; set; }
        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
