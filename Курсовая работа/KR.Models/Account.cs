using Kr.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KR.Models
{
    public class Account
    {
        [Key]
        public Guid UserId { get; set; }
        [JsonIgnore]
        [AllowNull]
        public User User { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(256)]
        public string Password { get; set; }
        [Required]
        [MaxLength(40), MinLength(4)]
        public string Login { get; set; }
        public virtual ICollection<Role>? Roles { get; set; }
    }
}
