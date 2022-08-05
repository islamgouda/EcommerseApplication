using EcommerseApplication.Models;

namespace EcommerseApplication.Respository
{
    public class DiscountRepository : IDiscount

    {
        private readonly Context context;

        public DiscountRepository(Context _context)
        {
            context = _context;
        }


        public void AddnewDiscount(Discount newdiscount)
        {
            context.Add(newdiscount);
            context.SaveChanges();
        }
    }
}
