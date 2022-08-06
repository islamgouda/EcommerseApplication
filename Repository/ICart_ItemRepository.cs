using EcommerseApplication.Models;

namespace EcommerseApplication.Repository
{
    public interface ICart_ItemRepository
    {
        int AddCart_Item(Cart_Item NewCart_Item);
        int DeleteCart_ItemById(int Id);
        List<Cart_Item> GetAllCart_Items();
        Cart_Item GetCart_ItemById(int Id);
        int UpdateCart_Item(int Id, Cart_Item NewCart_Item);
    }
}