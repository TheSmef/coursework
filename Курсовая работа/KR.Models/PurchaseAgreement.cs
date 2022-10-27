using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Kr.Models
{
    public class PurchaseAgreement
    {
        [Key]
        public Guid Id_Purchase_Agreement { get; set; } = new Guid();
        [Required]
        public DateTime Date_Of_Purchase { get; set; }
        [Required]
        [StringLength(50)]
        [MinLength(3)]
        public string Provider { get; set; }
        [AllowNull]
        [StringLength(20)]
        [MinLength(3)]
        public string Agreement_Code { get; set; }
        public virtual ICollection<Purchase>? Purchases { get; set; } 

    }
}
