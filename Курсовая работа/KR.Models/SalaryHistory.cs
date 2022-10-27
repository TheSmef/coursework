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
        [Required]
        public decimal Payment { get; set; }
        [Required]
        public bool Premium { get; set; }
    }
}
