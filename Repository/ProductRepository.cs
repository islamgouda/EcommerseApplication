using EcommerseApplication.Models;

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
        public Product Get(int Id)
        {
            return context.Products.Where(p => p.DeletedAt == null).FirstOrDefault(p=>p.ID == Id);
        } 
        public void Create(Product Product)
        {
            Product.CreatedAt = DateTime.Now;
            context.Products.Add(Product);
            context.SaveChanges();
        }
        public void Delete(int Id)
        {
            context.Products.Remove(Get(Id));
            context.SaveChanges();
        }
        public void Update(int Id, Product Product)
        {
            Product oldproduct = Get(Id);
            oldproduct.Name = Product.Name;
            oldproduct.Description = Product.Description;
            oldproduct.Price = Product.Price;
            oldproduct.IsAvailable = Product.IsAvailable;

            if(Product.Description_Ar != null && Product.Description_Ar != String.Empty)
                oldproduct.Description_Ar = Product.Description_Ar;
            if(Product.Name_Ar != null && Product.Name_Ar != String.Empty)
                oldproduct.Name_Ar = Product.Name_Ar;

            oldproduct.UpdatedAt = DateTime.Now;

            oldproduct.CategoryID = Product.CategoryID;
            oldproduct.DiscountID = Product.DiscountID;
            oldproduct.InventoryID = Product.InventoryID;
            oldproduct.PartenerID = Product.PartenerID;
            oldproduct.subcategoryID = Product.subcategoryID;

            context.SaveChanges();
        }
    }
}
