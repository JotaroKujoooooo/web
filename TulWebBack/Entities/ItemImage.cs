namespace TulWebBack.Entities
{
    public class ItemImage
    {
        public Guid Id { get; set; }
        public Guid ItemId { get; set; }
        public virtual Item Item { get; set; }
        public string Image { get; set; }
    }
}
