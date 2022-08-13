using System.ComponentModel.DataAnnotations;

namespace EcommerseApplication.DTO
{
    public class CheckoutDTO
    {
        public int PaymentID { get; set; }
        
        public string? Currency { get; set; } = "USD";
        [Range(maximum:double.MaxValue,minimum: 1.0)]
        public decimal Amount { get; set; }

        //[StringLength(16)]
        //[RegularExpression("^[0-9]{16,16}$")]
        //public string CardNumber { get; set; }

        //[StringLength(3)]
        //[RegularExpression("^[0-9]{3,3}$")]
        //public string Cvc { get; set; }

        //[StringLength(2)]
        //public string ExpMonth { get; set; }

        //[StringLength(maximumLength:4,MinimumLength =2)]
        //public string ExpYear { get; set; }


        //public string Email { get; set; }
        //public string? CustomerName { get; set; } = string.Empty;
        //public string? Phone { get; set; } = string.Empty;
    }
}
