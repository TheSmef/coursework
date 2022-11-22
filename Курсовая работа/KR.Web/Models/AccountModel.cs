using System.ComponentModel.DataAnnotations;

namespace Kr.Web.Models
{
    public class AccountModel
    {
        [Required(ErrorMessage = "Логин/Электронная почта - необходимое поле")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Пароль - необходимое поле")]
        public string Password { get; set; }
    }
}
