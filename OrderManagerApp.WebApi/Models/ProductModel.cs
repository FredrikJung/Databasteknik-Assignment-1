namespace OrderManagerApp.WebApi.Models
{
    public class ProductModel
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string ProductInfo
        {
            get
            {
                return $"{Name} - {Price} SEK";
            }

        }
    }
}
