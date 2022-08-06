using EcommerseApplication.Models;

namespace EcommerseApplication.Repository
{
    public interface IUserRepository
    {
        int AddUser(User NewUser);
        int DeleteUser(int Id);
        List<User> GetAllUsers();
        User GetUserById(int Id);
        int UpdateUser(int Id, User NewUser);
    }
}