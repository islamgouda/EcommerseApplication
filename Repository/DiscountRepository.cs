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
        public Discount getDiscountById(int Id)
        {
            Discount discount = context.Discounts.FirstOrDefault(d => d.ID == Id);
            return discount;

        }

        public void AssignDiscount(DiscountIDPartnerIDProductIDDTO AssignNewDiscount)
        {

        }
       
        public int  AsssignDiscount(DiscountIDPartnerIDProductIDDTO AssignNewDiscount)
        {

            Product product = context.Products.FirstOrDefault(p => p.ID == AssignNewDiscount.ProductID && p.PartenerID == 1);
            if (product.DiscountID == null)
            {
                product.DiscountID = AssignNewDiscount.DiscountId;
                int price = product.Price;
                Discount discount = context.Discounts.FirstOrDefault(d => d.ID == AssignNewDiscount.DiscountId);
                decimal DiscountPersent = discount.Descount_Persent;
                if (DiscountPersent > 0)
                {
                    int priceAfetrDiscount = price - (int)((price * DiscountPersent) / 100);
                    product.Price = priceAfetrDiscount;
                    context.SaveChanges();
                    return 1;
                }
                context.SaveChanges();
                return 2; 
            }
            return 0; 
        }
        public int DeleteDiscount(int Id)
        {
            Discount discount=context.Discounts.FirstOrDefault(p=>p.ID==Id);
            if(discount!=null)
            {
                context.Discounts.Remove(discount);
                return  context.SaveChanges();
                
            }
            return 0;
        }
        public int UpdateDiscount(int Id , DiscountDTO NewDiscount)
        {

            Discount discount = context.Discounts.FirstOrDefault(d => d.ID == Id);
            if (discount != null)
            {
                discount.Name = NewDiscount.Name;
                discount.Description = NewDiscount.Description;
                discount.Descount_Persent = NewDiscount.Descount_Persent;
                discount.UpdatedAt = DateTime.Now;
                discount.EndTime = NewDiscount.EndTime;
                discount.StartTime = NewDiscount.StartTime;
                discount.Active = NewDiscount.Active;
                return context.SaveChanges();
            }
            return 0;
        }
    }
}
