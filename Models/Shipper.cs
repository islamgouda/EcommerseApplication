using System.ComponentModel.DataAnnotations;

namespace EcommerseApplication.Models
{
    public class Shipper
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string? arabicName { get; set; }
        public string officePhone { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? deletedAt { get; set; }
        public List<shippingDetails>? shippingDetails { get; set; }

    }
}
