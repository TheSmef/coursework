using CsvHelper.Configuration.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Kr.Models
{
    public class ProductStorage
    {
        [Key]
        [Ignore]
        public Guid Id_Product_Storage { get; set; } = new Guid();
        [Required(ErrorMessage = "Категория - необходимое поле")]
        public virtual Category Category { get; set; }
        [Required(ErrorMessage = "Стоимость товара - необходимое поле")]
        [Range(0.01, 999999999999.99, ErrorMessage = "Значение цены должно находиться между 0 и 1000000000000.00")]
        [Name("Цена продукта")]
        [Index(0)]
        public decimal Cost { get; set; }
        [Required(ErrorMessage = "Название продукта - необходимое поле")]
        [StringLength(50, ErrorMessage = "Длинна названия продукта не может быть больше 50 символов")]
        [MinLength(3, ErrorMessage ="Длинна названия продукта не может быть меньше 3 символов")]
        [Name("Название продукта")]
        [Index(1)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Количество товара - необходимое поле")]
        [Range(0, int.MaxValue, ErrorMessage = "Количество товара не может быть 0 и меньше, и не превышать 2147483647")]
        [Name("Количество продукта")]
        [Index(2)]
        public int Amount { get; set; }
        [Required(ErrorMessage = "Срок годности (в днях) - необходимое поле")]
        [Name("Срок годности продукта")]
        [Index(3)]
        [Range(0, int.MaxValue, ErrorMessage = "Срок годности товара (в днях) не может быть 0 меньше")]
        public int Exipiration_Date { get; set; }

    }
}
