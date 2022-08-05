using EcommerseApplication.Models;

namespace EcommerseApplication.Repository
{
    public class Product_InventoryRepository : IProduct_InventoryRepository
    {
        private readonly Context context;

        public Product_InventoryRepository(Context _context)
        {
            context = _context;
        }

        public List<Product_Inventory> GetAll()
        {
            return context.Product_Inventorys.Where(p => p.DeletedAt == null).ToList();
        }
        public Product_Inventory Get(int Id)
        {
            return context.Product_Inventorys.Where(p => p.DeletedAt == null).FirstOrDefault(p => p.ID == Id);
        }
        
        public void Create(Product_Inventory Product_Inventory)
        {
            Product_Inventory.CreatedAt = DateTime.Now;
            context.Product_Inventorys.Add(Product_Inventory);
            context.SaveChanges();
        }

        public void Delete(int Id)
        {
            context.Product_Inventorys.Remove(Get(Id));
            context.SaveChanges();
        }

        public void Update(int Id, Product_Inventory Product_Inventory)
        {
            Product_Inventory OldProduct_Inventory = Get(Id);
            OldProduct_Inventory.Quantity = Product_Inventory.Quantity;

            OldProduct_Inventory.UpdatedAt = DateTime.Now;
        }
    }
}
