namespace TulWebBack.Entities
{
    public class ItemProperties
    {
        public Guid Id { get; set; }
        public Guid ItemId { get; set; }
        public virtual Item Item { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Upakovka { get; set; }
        public string GroupUpakovka { get; set; }
        public string Evropallet { get; set; }
        public string ShtrihCode { get; set; }
        public string Brand { get; set; }
        
    }
}
