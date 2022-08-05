using EcommerseApplication.Models;

namespace EcommerseApplication.Repository
{
    public class UserAddressRepository : IUserAddress
    {
        private readonly Context context;

        public UserAddressRepository(Context _con)
        {
            context = _con;
        }

        public User_address  GetAddress(int id)
        {
            return context.User_Addresses.FirstOrDefault(i => i.Id == id);
        }
        public void AddNewAddress(User_address user_Address)
        {
            context.User_Addresses.Add(user_Address);
            context.SaveChanges();
        }

        public void DeleteAddress(int id)
        {
           context.User_Addresses.Remove(GetAddress(id));
            context.SaveChanges();
        }

        public void UpdateAddress(int id, User_address user_Address)
        {
            User_address user_Addressold=GetAddress(id);
            user_Addressold.AddressLine1=user_Address.AddressLine1;
            user_Addressold.AddressLine2=user_Address.AddressLine2;
            user_Addressold.City=user_Address.City;
            user_Addressold.PostalCode=user_Address.PostalCode;
            user_Addressold.Country=user_Address.Country;
            user_Addressold.telephone = user_Address.telephone;
            user_Addressold.mobile=user_Address.mobile;
            context.SaveChanges();
        }
    }
}
