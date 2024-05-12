
namespace AdministratorClient.Model
{
    public class Product
    {
        public Guid Id { get; set; }=Guid.Empty;
        public string Name { get; set; }
        public Guid? ManufacturerId { get; set; }
        public Manufacturer? Manufacturer { get; set; }
        public Guid CategoryId { get; set; }
        public Category Category { get; set; } = new();
        private float price;
        public float Price 
        { 
            get { return price; }
            set 
            {
                if (value < 0)
                    price = 0;
                price = value;
            }
        }
        public float? ConditionPrice { get; set; } = null;
        public short Bonus { get; set; } = 0;
        public bool isRecept { get; set; } = false;
        public string[] Images { get; set; } = ["PICTURE.jpg"];
        private int retailCount;
        public int RetailCount
        {
            get { return retailCount; }
            set
            {
                if (value < 0)
                    retailCount = 0;
                retailCount = value;
            }
        }

        public string? Description { get; set; } = null;

        





    }
}
