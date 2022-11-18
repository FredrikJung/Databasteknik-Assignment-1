using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OrderManagerApp.WebApi.Models.Entities
{
    public class OrderRowEntity
    {
        public int Quantity { get; set; }
        [Required]
        public decimal UnitPrice { get; set; }
        public int OrderId { get; set; }
        public OrderEntity Order { get; set; } = null!;
        public int ProductId { get; set; }
        public ProductEntity Product { get; set; } = null!;
    }
}
