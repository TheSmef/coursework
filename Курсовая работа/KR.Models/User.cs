using KR.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Kr.Models
{
    public class User
    {
        [Key]
        public Guid Id_User { get; set; } = new Guid();
        [Required]
        [StringLength(50)]
        [MinLength(3)]
        public string Last_name { get; set; }
        [Required]
        [StringLength(50)]
        [MinLength(3)]
        public string First_name { get; set; }
        [AllowNull]
        [StringLength(50)]
        [MinLength(3)]
        public string? Otch { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
        [AllowNull]
        public virtual Account? Account { get; set; } 
    }
}
