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
        [Required]
        [StringLength(50)]
        [MinLength(3)]
        public string Name { get; set; }
        [Required]
        public decimal Salary { get; set; }
        [JsonIgnore]
        public virtual ICollection<UserPost>? UserPosts { get; set; }
    }
}
