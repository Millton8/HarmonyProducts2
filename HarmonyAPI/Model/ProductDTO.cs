namespace HarmonyAPI.Model
{
    public class ProductDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ManufacturerName { get; set; }
        public string CategoryName { get; set; }
        public float Price { get; set; }
        public float? ConditionPrice { get; set; }
        public int? Bonus { get; set; }
        public bool IsRecept { get; set; }
        public string[] Images { get; set; } 
        public int RetailCount { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; } 
    }
}
