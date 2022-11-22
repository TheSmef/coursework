using KR.Models;
using KR.Models.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Kr.Models
{
    public class Purchase
    {
        [Key]
        public Guid Id_Puchase { get; set; } = new Guid();
        [Required(ErrorMessage = "Продукт - необходимое поле")]
        public virtual ProductStorage ProductStorage { get; set; }
        [Required(ErrorMessage = "Количество товара - необходимое поле")]
        [Range(1, int.MaxValue, ErrorMessage = "Количество товара не может быть 0 и меньше, и не превышать 2147483647")]
        public int Amount { get; set; }
        [Required(ErrorMessage = "Договор закупки - необходимое поле")]
        public virtual PurchaseAgreement PurchaseAgreement { get; set; }
        [Required(ErrorMessage = "Цена - необходимое поле")]
        [Range(0.01, 999999999999.99, ErrorMessage = "Значение стоимости должно находиться между 0 и 1000000000000.00")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Дата поставки - необходимое поле")]
        [DateAttribute(50, 2, ErrorMessage = "Дата поставки должна быть между {1} и {2}")]
        public DateTime Date_Purchase { get; set; }
        public DateTime Date_Creation { get; set; } = DateTime.Now;
    }
}
