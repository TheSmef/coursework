using KR.Models.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Kr.Models
{
    public class Order
    {
        [Key]
        public Guid Id_Order { get; set; } = new Guid();
        [StringLength(36)]
        public string Order_Number { get; set; }
        [Required(ErrorMessage = "Исполнитель - необходимое поле")]
        public virtual UserPost UserPost { get; set; }
        [Required(ErrorMessage = "Дата заказа - необходимое поле")]
        [DateAttribute(50, 0, ErrorMessage = "Дата заказа должна быть между {1} и {2}")]
        public DateTime Date_Order  { get; set; }
        [JsonIgnore]
        public virtual ICollection<OrderProduct>? OrderProducts { get; set; }
    }
}
