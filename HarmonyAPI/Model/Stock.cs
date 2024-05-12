namespace HarmonyAPI.Model
{
    public class Stock
    {
        public Guid Id { get; set; }
        public Guid DetailId { get; set; }
        public StockDetail Detail { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; } = 0;
        
    }
}
