using EcommerseApplication.DTO;
using EcommerseApplication.Models;

namespace EcommerseApplication.Respository
{
    public interface IDiscount
    {
        public void AddnewDiscount(Discount newdiscount);
        public void AddnewDiscountt(DiscountDTO NewDiscount);
        public List<Discount> getDiscount();
        public void AssignDiscount(DiscountIDPartnerIDProductIDDTO AssignNewDiscount);
        public int DeleteDiscount(int Id);
        public int UpdateDiscount(int Id, DiscountDTO NewDiscount);
        public Discount getDiscountById(int Id);

    }
}
