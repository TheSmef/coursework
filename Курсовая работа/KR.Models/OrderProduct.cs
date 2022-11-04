using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.ConstrainedExecution;
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
        [Required(ErrorMessage = "Количество товара - необходимое поле")]
        [Range(1, int.MaxValue, ErrorMessage = "Количество товара не может быть 0 и меньше")]
        public int Amount { get; set; }
        [Required(ErrorMessage = "Цена - необходимое поле")]
        [Range(0.01, Double.MaxValue, ErrorMessage = "Цена не может быть 0 и меньше")]
        public decimal Price { get; set; }
    }
}
