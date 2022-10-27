using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Kr.Models
{
    public class OrderProduct
    {
        [Key]
        public Guid Id_order_product { get; set; } = new Guid();
        [Required]
        [JsonIgnore]
        public virtual Order Order { get; set; }
        public virtual ProductStorage Product { get; set; }
        [Required]
        public int Amount { get; set; }
        [Required]
        public decimal Price { get; set; }
    }
}
