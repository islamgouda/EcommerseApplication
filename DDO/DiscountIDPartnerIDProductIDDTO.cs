using System.ComponentModel.DataAnnotations;

namespace EcommerseApplication.DDO
{
    public class DiscountIDPartnerIDProductIDDTO
    {
        [Required]
        public int DiscountId { get; set; }
        [Required]
        public int ProductID { get; set; }
        [Required]
        public int PartnerID { get; set; }
    }
}
