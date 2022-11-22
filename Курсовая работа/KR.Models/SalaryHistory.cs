using KR.Models;
using KR.Models.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Kr.Models
{
    public class SalaryHistory
    {
        [Key]
        public Guid Id_SalaryHistory { get; set; } = new Guid();
        [Required(ErrorMessage = "Сотрудник - необходимое поле")]
        public virtual UserPost UserPost { get; set; }
        [Required(ErrorMessage = "Дата - необходимое поле")]
        [DateAttribute(50, 0, ErrorMessage = "Дата выплаты должна быть между {1} и {2}")]
        public DateTime Date { get; set; }
        [Required(ErrorMessage = "Фонд оплаты - необходимое поле")]
        [Range(0.01, 999999999999.99, ErrorMessage = "Значение выплаты должно находиться между 0 и 1000000000000.00")]
        public decimal Payment { get; set; }
        [Required]
        public bool Premium { get; set; }

    }
}
