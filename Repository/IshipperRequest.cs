using EcommerseApplication.Models;

namespace EcommerseApplication.Repository
{
    public interface IshipperRequest
    {
        public void Add(ShipperRequest shipperRequest);
        public List<ShipperRequest> GetAll();
    }
}
