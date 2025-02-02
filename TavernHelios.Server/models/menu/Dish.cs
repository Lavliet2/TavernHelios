namespace TavernHelios.Server.models.menu
{
    public class Dish
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int DishType { get; set; }
        public string ImageBase64 { get; set; }
    }
}
