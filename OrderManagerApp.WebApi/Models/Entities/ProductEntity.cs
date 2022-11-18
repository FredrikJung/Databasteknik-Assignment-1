using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OrderManagerApp.WebApi.Models.Entities
{
    public class ProductEntity
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; } = null!;

        [Required]
        public decimal Price { get; set; }
        public ICollection<OrderRowEntity>? OrderRows { get; set; } 
    }
}
