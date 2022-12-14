using System.ComponentModel.DataAnnotations;

namespace Kr.Models
{
    public class StorageHistory
    {
        [Key]
        public Guid Id_StorageHistory { get; set; } = new Guid();
        [Required]
        public virtual ProductStorage ProductStorage { get; set; }
        [Required]
        [StringLength(100)]
        public string Summary { get; set; }
        [Required]
        public DateTime Date { get; set; }
    }
}
