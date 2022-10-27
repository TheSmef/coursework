using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kr.Models
{
    public class UserPost
    {
        [Key]
        public Guid Id_User_Post { get; set; } = new Guid();
        [Required]
        public virtual Post Post { get; set; }
        [Required]
        public virtual User User { get; set; }
        [Required]
        public decimal Share { get; set; }
        public virtual ICollection<SalaryHistory>? SalaryHistories { get; set; }
    }
}
