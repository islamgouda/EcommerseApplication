using EcommerseApplication.Models;

namespace EcommerseApplication.Repository
{
    public interface IUserPayement
    {
        public void AddUSerPayment(User_Payement newPaement);
        public void DeleteUserPayment(int id);
        public User_Payement GetUserPayment(int id);
        public void updateUserPayement(int id,User_Payement newPaement);
    }
}
