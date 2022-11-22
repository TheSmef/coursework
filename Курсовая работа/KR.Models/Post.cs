using System.ComponentModel.DataAnnotations;

namespace Kr.Models
{
    public class Post
    {
        [Key]
        public Guid Id_Post { get; set; } = new Guid();
        [Required(ErrorMessage = "Название должности - необходимое поле")]
        [StringLength(50, ErrorMessage = "Название должности должно быть не более 50 символов")]
        [MinLength(3, ErrorMessage = "Название должности должно быть не менее 3 символов")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Зарплата - необходимое поле")]
        [Range(0.01, 999999999999.99, ErrorMessage = "Значение зарплаты должно находиться между 0 и 1000000000000.00")]
        public decimal Salary { get; set; }
    }
}
