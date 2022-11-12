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
        [Required(ErrorMessage = "Заказ - необходимое поле")]
        [JsonIgnore]
        public virtual Order Order { get; set; }
        [Required(ErrorMessage = "Продукт - необходимое поле")]
        public virtual ProductStorage Product { get; set; }
        [Required(ErrorMessage = "Количество товара - необходимое поле")]
        [Range(1, int.MaxValue, ErrorMessage = "Количество товара не может быть 0 и меньше, и не превышать 2147483647")]
        public int Amount { get; set; }
        [Required(ErrorMessage = "Цена - необходимое поле")]
        [Range(0.01, 999999999999.99, ErrorMessage = "Значение цены должно находиться между 0 и 1000000000000.00")]
        public decimal Price { get; set; }
    }
}
