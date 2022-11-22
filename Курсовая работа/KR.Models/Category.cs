using CsvHelper.Configuration.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;

namespace Kr.Models
{
    public class Category
    {
        [Key]
        [Ignore]
        public Guid Id_Category { get; set; } = new Guid();
        [Required(ErrorMessage = "Название категории - необходимое поле")]
        [StringLength(30, ErrorMessage = "Назвение категории не может быть больше 30 символов")]
        [MinLength(3, ErrorMessage = "Название категории не может быть меннее 3 символов")]
        [Name("Название категории")]
        [Index(4)]
        public string Name { get; set; }

    }
}
