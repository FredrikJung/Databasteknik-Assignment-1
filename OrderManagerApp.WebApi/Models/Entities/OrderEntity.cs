using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OrderManagerApp.WebApi.Models.Entities
{
    public class OrderEntity
    {
        [Key]
        public int OrderId { get; set; }
        [Required]
        [Column(TypeName = "datetime2")]
        public DateTime OrderDate { get; set; }

        [Required]
        [Column(TypeName = "datetime2")]
        public DateTime DueDate { get; set; }
        public decimal TotalPrice { get; set; }
        public int CustomerId { get; set; }
        public CustomerEntity Customer { get; set; } = null!;
        public ICollection<OrderRowEntity>? OrderRows { get; set; }

    }
}
