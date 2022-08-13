using EcommerseApplication.Models;
namespace EcommerseApplication.Repository

{
    public interface IRequest
    {
        public void RequestTobePartner(Requests request);
        public List<Requests> GetAllRequests();
        public Requests GetPartnerById(int id);
    }
}
