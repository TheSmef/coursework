using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;

namespace Kr.Models
{
    public class Category
    {
        [Key]
        public Guid Id_Category { get; set; } = new Guid();
        [Required]
        [StringLength(30)]
        [MinLength(3)]
        public string Name { get; set; }
        public virtual ICollection<ProductStorage>? ProductStorages { get; set; } 
    }
}
