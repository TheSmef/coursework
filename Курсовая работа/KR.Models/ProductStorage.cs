using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Kr.Models
{
    public class ProductStorage
    {
        [Key]
        public Guid Id_Product_Storage { get; set; } = new Guid();
        [Required]
        [JsonIgnore]
        public virtual Category Category { get; set; }
        [Required(ErrorMessage = "Стоимость товара - необходимое поле")]
        [Range(0.01, Double.MaxValue, ErrorMessage = "Стоимость товара не может быть 0 и меньше")]
        public decimal Cost { get; set; }
        [Required(ErrorMessage = "Название продукта - необходимое поле")]
        [StringLength(50, ErrorMessage = "Длинна названия продукта не может быть больше 50 символов")]
        [MinLength(3, ErrorMessage ="Длинна названия продукта не может быть меньше 3 символов")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Количество товара - необходимое поле")]
        [Range(0, int.MaxValue, ErrorMessage = "Количество товара не может быть 0 и меньше")]
        public int Amount { get; set; }
        [Required(ErrorMessage = "Срок годности (в днях) - необходимое поле")]
        [Range(0, int.MaxValue, ErrorMessage = "Срок годности товара (в днях) не может быть 0 меньше")]
        public int Exipiration_Date { get; set; }

    }
}
