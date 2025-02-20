namespace StoreAPI.DTO
{
    public class CollectionProductDTO
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public int CollectionId { get; set; }

        public DateOnly CreatedAt { get; set; }
    }
}
