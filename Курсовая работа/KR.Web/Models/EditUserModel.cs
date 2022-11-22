using CsvHelper.Configuration.Attributes;
using Kr.Models;
using KR.Models.Attributes;
using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace KR.Web.Models
{
    public class EditUserModel
    {
        [Required(ErrorMessage = "Электронная почта - необходимое поле")]
        [EmailAddress(ErrorMessage = "Неправильный формат электронной почты", ErrorMessageResourceName = null)]
        public string Email { get; set; }
        [AllowNull]
        [RegularExpression(pattern: "^(?=.*[0-9])(?=.*[!@#$%^&*])(?=.*[a-z])(?=.*[A-Z])[0-9a-zA-Z!@#$%^&*]{8,30}$", ErrorMessage = "Пароль должен быть 8-30 символов, содержать в себе как минимум одну букву, как минимум 1 цифру и как минимум 1 знак (!@#$%^&*)")]
        [StringLength(128)]
        public string Password { get; set; }
        [Compare(nameof(Password), ErrorMessage = "Введённые пароли не совпадают")]
        public string PasswordConfirm { get; set; }
        [Required(ErrorMessage = "Логин - необходимое поле")]
        [MaxLength(40, ErrorMessage = "Логин должен быть не более 40 символов"), MinLength(4, ErrorMessage = "Логин должен быть не менее 4 символов")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Фамилия - необходимое поле")]
        [StringLength(50, ErrorMessage = "Фамилия не может быть более 50 символов")]
        [MinLength(3, ErrorMessage = "Фамилия не может быть менее 3 символов")]
        public string Last_name { get; set; }
        [Required(ErrorMessage = "Имя - необходимое поле")]
        [StringLength(50, ErrorMessage = "Имя не может быть более 50 символов")]
        [MinLength(3, ErrorMessage = "Имя не может быть менее 3 символов")]
        public string First_name { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = true, ApplyFormatInEditMode = true)]
        [AllowNull]
        [StringLength(50, ErrorMessage = "Отчество не может быть более 50 символов")]
        [Nullable(3, ErrorMessage = "Отчество не может быть менее 3 символов")]
        public string? Otch { get; set; }
        [Required(ErrorMessage = "Дата рождения - необходимое поле")]
        [DataType(DataType.Date, ErrorMessage = "Неправильный формат даты")]
        [DateAttribute(18, 80, ErrorMessage = "Дата рождения должна быть между {1} и {2}")]
        public DateTime BirthDate { get; set; }
        public virtual List<Role>? Roles { get; set; }

    }
}
