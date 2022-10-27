using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Kr.Models
{
    public class Purchase
    {
        [Key]
        public Guid Id_Puchase { get; set; } = new Guid();
        public virtual ProductStorage ProductStorage { get; set; }
        [Required]
        public int Amount { get; set; }
        [JsonIgnore]
        public virtual PurchaseAgreement PurchaseAgreement { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public DateTime Date_Purchase { get; set; }
        [Required]
        public DateTime Date_Creation { get; set; }
    }
}
