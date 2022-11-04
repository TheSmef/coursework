using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Kr.Models
{
    public class Purchase
    {
        [Key]
        public Guid Id_Puchase { get; set; } = new Guid();
        public virtual ProductStorage ProductStorage { get; set; }
        [Required(ErrorMessage = "Количество товара - необходимое поле")]
        [Range(1, int.MaxValue, ErrorMessage = "Количество товара не может быть 0 и меньше")]
        public int Amount { get; set; }
        [JsonIgnore]
        public virtual PurchaseAgreement PurchaseAgreement { get; set; }
        [Required(ErrorMessage = "Цена - необходимое поле")]
        [Range(0.01, Double.MaxValue, ErrorMessage = "Количество товара не может быть 0 и меньше")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Дата закупки необходма для ввода")]
        public DateTime Date_Purchase { get; set; }
        [Required]
        public DateTime Date_Creation { get; set; }
    }
}
