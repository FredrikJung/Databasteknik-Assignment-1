using System.Collections.ObjectModel;

namespace OrderManagerApp.WebApi.Models
{
    public class OrderRequest
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime DueDate { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
        public ObservableCollection<ProductModel> Products { get; set; } = null!;
    }
}
