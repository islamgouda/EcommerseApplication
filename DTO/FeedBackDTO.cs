namespace EcommerseApplication.DTO
{
    public class FeedBackDTO
    {
        public int UserID { get; set; }
        public int productID { get; set; }
        public int OrderID { get; set; }
        public string Comment { get; set; }
        public decimal Rate { get; set; }
    }
}
