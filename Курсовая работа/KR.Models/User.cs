using CsvHelper.Configuration.Attributes;
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
        [Ignore]
        public Guid Id_User { get; set; } = new Guid();
        [Name("Фамилия")]
        [Index(0)]
        [Required(ErrorMessage = "Фамилия - необходимое поле")]
        [StringLength(50, ErrorMessage = "Фамилия не может быть более 50 символов")]
        [MinLength(3, ErrorMessage = "Фамилия не может быть менее 3 символов")]
        public string Last_name { get; set; }
        [Name("Имя")]
        [Index(1)]
        [Required(ErrorMessage = "Имя - необходимое поле")]
        [StringLength(50, ErrorMessage = "Имя не может быть более 50 символов")]
        [MinLength(3, ErrorMessage = "Имя не может быть менее 3 символов")]
        public string First_name { get; set; }
        [Index(2)]
        [Name("Отчество")]
        [AllowNull]
        [StringLength(50, ErrorMessage = "Отчество не может быть более 50 символов")]
        [MinLength(3, ErrorMessage = "Отчество не может быть менее 3 символов")]
        public string? Otch { get; set; }
        [Index(3)]
        [Name("Дата рождения")]
        [Required(ErrorMessage = "Дата рождения - необходимое поле")]
        [DateAttribute(ErrorMessage = "Дата рождения должна быть между {1} и {2}")]
        public DateTime BirthDate { get; set; }
        [AllowNull]
        public virtual Account? Account { get; set; } 
    }

    public class DateAttribute : RangeAttribute
    {
        public DateAttribute()
          : base(typeof(DateTime), DateTime.Now.AddYears(-18).ToShortDateString(), DateTime.Now.AddYears(-80).ToShortDateString()) { }
    }
}
