namespace StoreAPI.DTO
{
    public class BannerSlideDTO
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public string ImageUrl { get; set; } = null!;

        public string? Href { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public int BannerId { get; set; }
    }
}
