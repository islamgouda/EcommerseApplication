using System.ComponentModel.DataAnnotations;

namespace EcommerseApplication.DTO
{
    public class DiscountDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public decimal Descount_Persent { get; set; }
        [Required]
        public bool Active { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }





    }
}
