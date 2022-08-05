using EcommerseApplication.Models;

namespace EcommerseApplication.Repository
{
    public interface IProductRepository
    {
        List<Product> GetAll();
        Product Get(int Id);
        void Create(Product Product);
        void Update(int Id, Product Product);
        void Delete(int Id);
    }
}
