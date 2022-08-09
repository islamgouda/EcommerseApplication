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
    }
}
