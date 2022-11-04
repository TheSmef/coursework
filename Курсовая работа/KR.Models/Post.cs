using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

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
        [Range(0.01, Double.MaxValue, ErrorMessage = "Зарплата не может быть 0 и меньше")]
        public decimal Salary { get; set; }
        [JsonIgnore]
        public virtual ICollection<UserPost>? UserPosts { get; set; }
    }
}
