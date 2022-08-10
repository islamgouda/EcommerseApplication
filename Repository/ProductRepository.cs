using EcommerseApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerseApplication.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly Context context;

        public ProductRepository(Context _context)
        {
            context = _context;
        }
        public List<Product> GetAll()
        {
            return context.Products.Where(p => p.DeletedAt == null).ToList();
        }

        public List<Product> GetAllByCategoryID(int id)
        {
            return context.Products.Include(p => p.Discount)
                .Include(p => p.Partener)
                .Include(p => p.Product_Images)
                .Include(p => p.subcategory)
                .Include(p => p.Product_Category)
                .Include(p => p.Product_Inventory)
                .Where(p => p.CategoryID == id)
                .Where(p => p.DeletedAt == null).ToList();
        }
        
        public List<Product> GetAllBySubCategoryID(int id)
        {
            return context.Products.Include(p => p.Discount)
                .Include(p => p.Partener)
                .Include(p => p.Product_Images)
                .Include(p => p.subcategory)
                .Include(p => p.Product_Category)
                .Include(p => p.Product_Inventory)
                .Where(p => p.subcategoryID == id)
                .Where(p => p.DeletedAt == null).ToList();
        }
        public List<Product> GetPartnerProducts(int PartnerID)
        {
            return context.Products.Include(p => p.Discount)
                .Include(p => p.Partener)
                .Include(p => p.Product_Images)
                .Include(p => p.subcategory)
                .Include(p => p.Product_Category)
                .Include(p => p.Product_Inventory)
                .Where(p => p.PartenerID == PartnerID)
                .Where(p => p.DeletedAt == null).ToList();
        }
        public List<Product> GetPartnerProductsByCategoryID(int PartnerID, int CategoryID)
        {
            return context.Products.Include(p => p.Discount)
                .Include(p => p.Partener)
                .Include(p => p.Product_Images)
                .Include(p => p.subcategory)
                .Include(p => p.Product_Category)
                .Include(p => p.Product_Inventory)
                .Where(p => p.PartenerID == PartnerID)
                .Where(p => p.CategoryID == CategoryID)
                .Where(p => p.DeletedAt == null).ToList();
        }
        public List<Product> GetPartnerProductsBySubCategoryID(int PartnerID, int SubCategoryID)
        {
            return context.Products.Include(p => p.Discount)
                .Include(p => p.Partener)
                .Include(p => p.Product_Images)
                .Include(p => p.subcategory)
                .Include(p => p.Product_Category)
                .Include(p => p.Product_Inventory)
                .Where(p => p.PartenerID == PartnerID)
                .Where(p => p.subcategoryID == SubCategoryID)
                .Where(p => p.DeletedAt == null).ToList();
        }
        public List<Product> GetAllWithInclude()
        {
            return context.Products.Include(p=>p.Discount)
                .Include(p => p.Partener)
                .Include(p => p.Product_Images)
                .Include(p => p.subcategory)
                .Include(p => p.Product_Category)
                .Include(p => p.Product_Inventory)
                .Where(p => p.DeletedAt == null).ToList();
        }
        public Product Get(int Id)
        {
            return context.Products.Where(p => p.DeletedAt == null).FirstOrDefault(p=>p.ID == Id);
        } 
        public void Create(Product Product)
        {
            context.Products.Add(Product);
            context.SaveChanges();
        }
        public void Delete(int Id)
        {
            context.Products.Remove(Get(Id));
            context.SaveChanges();
        }
        public int Deletee(int Id)
        {
            Product product = Get(Id);
            if (product != null)
            {
                context.Products.Remove(product);
                return context.SaveChanges();
            }
            return 0;
           
        }
        public void Update(int Id, Product Product)
        {
            context.SaveChanges();
        }
        public void IsDiscountFinish()
        {
            List<Discount> discounts = context.Discounts.ToList();
            foreach (Discount discount in discounts)
            {
                if (discount.EndTime < discount.StartTime)
                {
                    List<Product> products = context.Products.Where(p => p.DiscountID == discount.ID).ToList();
                    foreach (Product product in products)
                    {
                        product.DiscountID = null;
                        context.SaveChanges();
                    }
                    context.Discounts.Remove(discount);
                    context.SaveChanges();
                }
            }
        }
    }
}
