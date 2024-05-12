namespace HarmonyAPI.Model
{
    public class StockDetail
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Adress { get; set; }
        public bool IsMainStock { get; set; } = false;
    }
}
