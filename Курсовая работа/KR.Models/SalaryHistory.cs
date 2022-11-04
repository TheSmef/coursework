using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Kr.Models
{
    public class SalaryHistory
    {
        [Key]
        public Guid Id_SalaryHistory { get; set; } = new Guid();
        [Required]
        [JsonIgnore]
        public virtual UserPost UserPost { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required(ErrorMessage = "Фонд оплаты - необходимое поле")]
        [Range(0.01, Double.MaxValue, ErrorMessage = "Сумма, выданная сотруднику не может быть 0 и меньше")]
        public decimal Payment { get; set; }
        [Required]
        public bool Premium { get; set; }
    }
}
