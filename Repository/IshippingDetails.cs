using EcommerseApplication.Models;

namespace EcommerseApplication.Repository
{
    public interface IshippingDetails
    {
        public void insert(shippingDetails shippingDetails);
        public void update(int id, shippingDetails shippingDetails);
        public shippingDetails getByID(int id);
        public shippingDetails getByName(string name);
        public List<shippingDetails> getAll();
        public void delete(int id);
        public void updateState(int id, string shippingstate);

    }
}
