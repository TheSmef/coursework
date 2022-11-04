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
        [Required(ErrorMessage = "Электронная почта - необходимое поле")]
        [EmailAddress(ErrorMessage = "Неправильный формат электронной почты", ErrorMessageResourceName = null)]
        [StringLength(255, ErrorMessage = "Длинна электронной почты не может превышать 255 символов")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Пароль - необходимое поле")]
        [RegularExpression(pattern: "^(?=.*[0-9])(?=.*[!@#$%^&*])(?=.*[a-z])(?=.*[A-Z])[0-9a-zA-Z!@#$%^&*]{8,30}$", ErrorMessage = "Пароль должен быть 8-30 символов, содержать в себе как минимум одну букву, как минимум 1 цифру и как минимум 1 знак (!@#$%^&*)")]
        [StringLength(128)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Логин - необходимое поле")]
        [MaxLength(40, ErrorMessage = "Логин должен быть не более 40 символов"), MinLength(4, ErrorMessage = "Логин должен быть не менее 4 символов")]
        public string Login { get; set; }
        public virtual ICollection<Role>? Roles { get; set; }
    }
}
