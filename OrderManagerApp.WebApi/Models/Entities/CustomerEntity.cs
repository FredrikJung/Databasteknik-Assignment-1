using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OrderManagerApp.WebApi.Models.Entities
{
    public class CustomerEntity
    {
        [Key]
        public int CustomerId { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; } = null!;
        public ICollection<OrderEntity>? Orders { get; set; } 
    }
}
