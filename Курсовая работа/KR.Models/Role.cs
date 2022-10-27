using KR.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Kr.Models
{
    public class Role
    {
        [Key]
        public Guid Id_Role { get; set; } = new Guid();
        [Required]
        [StringLength(30)]
        [MinLength(3)]
        public string Name { get; set; }
        [Required]
        [JsonIgnore]
        public virtual Account AccountUser { get; set; }

        enum NameRole
        {
            [Description("Администратор")]
            ADMIN,
            [Description("Бухгалтер")]
            ACCOUNTANT,
            [Description("Отдел кадров")]
            CHAR,
            [Description("Отдел продаж")]
            SALESDEPARTMENT,
            [Description("Работник склада")]
            STORAGE,
        }

    }
}
