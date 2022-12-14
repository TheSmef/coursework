using KR.Models.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Kr.Models
{
    public class PurchaseAgreement
    {
        [Key]
        public Guid Id_Purchase_Agreement { get; set; } = new Guid();
        [Required(ErrorMessage = "Дата договора - необходимое поле")]
        [DateAttribute(50, 0, ErrorMessage = "Дата договора поставки должна быть между {1} и {2}")]
        public DateTime Date_Of_Purchase { get; set; }
        [Required(ErrorMessage = "Название организации-поставщика - необходимое поле")]
        [StringLength(50, ErrorMessage = "Название организации-поставщика должно быть не более 50 символов")]
        [MinLength(3, ErrorMessage = "Название организации-поставщика должно быть не менее 3 символов")]
        public string Provider { get; set; }
        [Required(ErrorMessage = "Код договора - необходимое поле")]
        [StringLength(20, ErrorMessage = "Код договора должен быть не более 20 символов")]
        [MinLength(3, ErrorMessage = "Код договора должен быть не менее 3 символов")]
        public string Agreement_Code { get; set; }
        [JsonIgnore]
        public virtual ICollection<Purchase>? Purchases { get; set; }

    }
}
