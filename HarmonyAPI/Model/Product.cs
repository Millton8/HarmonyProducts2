namespace HarmonyAPI.Model
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? ManufacturerId { get; set; }
        public Manufacturer? Manufacturer { get; set; }
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
        public float Price { get; set; }
        public float? ConditionPrice { get; set; } = null;
        public short Bonus { get; set; } = 0;
        public bool isRecept { get; set; } = false;
        public string[] Images { get; set; } = ["PICTURE.jpg"];
        public int RetailCount { get; set; } = 0;
        public string? Description { get; set; } = null;


    }

}
