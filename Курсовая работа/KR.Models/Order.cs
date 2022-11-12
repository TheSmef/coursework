using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kr.Models
{
    public class Order
    {
        [Key]
        public Guid Id_Order { get; set; } = new Guid();
        [Required(ErrorMessage = "Исполнитель - необходимое поле")]
        public virtual UserPost UserPost { get; set; }
        [Required(ErrorMessage = "Дата заказа - необходимое поле")]
        [DateAttribute(ErrorMessage = "Дата заказа должна быть между {1} и {2}")]
        public DateTime Date_Order  { get; set; }
        public virtual ICollection<OrderProduct>? OrderProducts { get; set; }

        public class DateAttribute : RangeAttribute
        {
            public DateAttribute()
              : base(typeof(DateTime), DateTime.Now.AddYears(-50).ToShortDateString(), DateTime.Now.ToShortDateString()) { }
        }
    }
}
