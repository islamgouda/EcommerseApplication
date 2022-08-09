using EcommerseApplication.DTO;
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
        public void AddnewDiscountt(DiscountDTO NewDiscount)
        {
            Discount discount = new Discount();
            discount.Name = NewDiscount.Name;
            discount.Description = NewDiscount.Description;
            discount.Descount_Persent = NewDiscount.Descount_Persent;
            discount.CreatedAt = DateTime.Now;
            discount.EndTime = NewDiscount.EndTime;
            discount.StartTime = NewDiscount.StartTime;
            discount.Active = NewDiscount.Active;
            context.Add(discount);
            context.SaveChanges();
        }
        public List< Discount> getDiscount()
        {
            List<Discount> discounts = context.Discounts.ToList();
            return discounts;

        }
        public void AssignDiscount(DiscountIDPartnerIDProductIDDTO AssignNewDiscount)
        {
            Product product=context.Products.FirstOrDefault(p=>p.ID==AssignNewDiscount.ProductID && p.PartenerID==AssignNewDiscount.PartnerID);
            product.DiscountID = AssignNewDiscount.DiscountId;
            context.SaveChanges();
        }
    }
}
