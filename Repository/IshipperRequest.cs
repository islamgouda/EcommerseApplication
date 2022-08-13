using EcommerseApplication.Models;

namespace EcommerseApplication.Repository
{
    public interface IshipperRequest
    {
        public void Add(ShipperRequest shipperRequest);
        public List<ShipperRequest> GetAll();
        public ShipperRequest Get(int id);
        public void remove(int id);
    }
}
