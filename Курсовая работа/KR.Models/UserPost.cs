using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Kr.Models
{
    public class UserPost
    {
        [Key]
        public Guid Id_User_Post { get; set; } = new Guid();
        [Required(ErrorMessage = "Должность - необходимое поле")]
        public virtual Post Post { get; set; }
        [Required(ErrorMessage = "Сотрудник - необходимое поле")]
        public virtual User User { get; set; }
        [Required(ErrorMessage = "Ставка - необходимое поле")]
        [Range(0.01, 1.00, ErrorMessage = "Ставка является значением от 0 (не включительно) до 1")]
        public decimal Share { get; set; }

        [NotMapped]
        [JsonIgnore]
        public double ActSalary { get { return Math.Round((Double)(this.Share * this.Post.Salary), 2); } }
    }
}
