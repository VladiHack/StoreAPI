namespace StoreAPI.DTO
{
    public class StoreOptionDTO
    {
        public int Id { get; set; }

        public string StoreName { get; set; } = null!;

        public string PrimaryColor { get; set; } = null!;

        public string SecondaryColor { get; set; } = null!;
    }
}
