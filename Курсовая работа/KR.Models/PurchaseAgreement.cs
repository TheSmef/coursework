using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Kr.Models
{
    public class PurchaseAgreement
    {
        [Key]
        public Guid Id_Purchase_Agreement { get; set; } = new Guid();
        [Required(ErrorMessage = "Дата рождения - необходимое поле")]
        [DateAttribute(ErrorMessage = "Дата договора поставки должна быть между {1} и {2}")]
        public DateTime Date_Of_Purchase { get; set; }
        [Required(ErrorMessage = "Название организации-поставщика - необходимое поле")]
        [StringLength(50, ErrorMessage = "Название организации-поставщика должно быть не более 50 символов")]
        [MinLength(3, ErrorMessage = "Название организации-поставщика должно быть не менее 3 символов")]
        public string Provider { get; set; }
        [Required(ErrorMessage = "Код договора - необходимое поле")]
        [StringLength(20, ErrorMessage = "Код договора должен быть не более 20 символов")]
        [MinLength(3, ErrorMessage = "Код договора должен быть не менее 3 символов")]
        public string Agreement_Code { get; set; }
        public virtual ICollection<Purchase>? Purchases { get; set; }

        public class DateAttribute : RangeAttribute
        {
            public DateAttribute()
              : base(typeof(DateTime), DateTime.Now.AddYears(-50).ToShortDateString(), DateTime.Now.ToShortDateString()) { }
        }

    }
}
