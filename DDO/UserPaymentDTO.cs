using System.ComponentModel.DataAnnotations;

namespace EcommerseApplication.DDO
{
    public class UserPaymentDTO
    {
        [Required]
        public string PayementType { get; set; }
        [Required]
        public string arabicPayementType { get; set; }
        [Required]
        public string Provider { get; set; }
        [Required]
        public string arabicProvider { get; set; }
        [Required]
        public int AccountNo { get; set; }
       
    }
}
