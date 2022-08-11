using EcommerseApplication.Models;

namespace EcommerseApplication.Repository
{
    public class ShipperRequestRepository : IshipperRequest
    {
        Context context;
        public ShipperRequestRepository(Context _context)
        {
            this.context = _context;
        }
        public void Add(ShipperRequest shipperRequest)
        {
            context.shipperRequests.Add(shipperRequest);
            context.SaveChanges();
        }

        public List<ShipperRequest> GetAll()
        {
            return context.shipperRequests.ToList();
        }
    }
}
