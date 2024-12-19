namespace TulWebBack.Entities
{
    public class Item
    {
        public Guid Id { get; set; }
        public virtual ItemImage Image { get; set; }
        public virtual ItemProperties Props { get; set; }
        public virtual List<User> Customers { get; set; } = [];



    }
}
