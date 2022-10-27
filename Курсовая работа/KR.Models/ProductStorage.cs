using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Kr.Models
{
    public class ProductStorage
    {
        [Key]
        public Guid Id_Product_Storage { get; set; } = new Guid();
        [Required]
        [JsonIgnore]
 
        public virtual Category Category { get; set; }
        [Required]
        public decimal Cost { get; set; }
        [Required]
        [StringLength(50)]
        [MinLength(3)]
        public string Name { get; set; }
        [Required]
        public int Amount { get; set; }
        [Required]
        public int Exipiration_Date { get; set; }

    }
}
