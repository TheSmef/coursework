using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kr.Models
{
    public class Order
    {
        [Key]
        public Guid Id_Order { get; set; } = new Guid();
        [Required]
        public virtual UserPost UserPost { get; set; }
        [Required]
        public DateTime Date_Order  { get; set; }
        public virtual ICollection<OrderProduct>? OrderProducts { get; set; }
    }
}
