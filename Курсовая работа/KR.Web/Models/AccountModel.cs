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
