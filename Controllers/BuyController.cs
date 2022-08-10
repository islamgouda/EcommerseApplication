using EcommerseApplication.DTO;
using EcommerseApplication.Models;
using EcommerseApplication.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerseApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuyController : ControllerBase
    {
        private readonly string NotFoundMSG = "Data Not Found";
        private readonly string BadRequistMSG = "Invalid Input Data";
        private readonly string SuccessMSG = "Data Found Successfuly";

        private readonly IShopping_SessionRepository shopping_SessionRrpo;
        private readonly ICart_ItemRepository cart_ItemRepo;
        private readonly IOrder_DetailsRepository order_DetailsRepo;
        private readonly IOrder_ItemsRepository order_ItemsRepo;
        private readonly IUserPayement userPayementRepo;
        private readonly IPayment_DetailsRepository payment_DetailsRepo;
        private readonly IUserAddress userAddress;
        private readonly IshippingDetails shippingDetailsRepo;
        private readonly IUserRepository userRepo;

        public BuyController(IShopping_SessionRepository _shopping_SessionRrpo,
                             ICart_ItemRepository _cart_ItemRepo, IOrder_DetailsRepository _order_DetailsRepo,
                             IOrder_ItemsRepository _order_ItemsRepo, IUserPayement _userPayementRepo,
                             IPayment_DetailsRepository _payment_DetailsRepo, IUserRepository _userRepo,
                             IUserAddress _userAddress, IshippingDetails _shippingDetailsRepo)
        {
            shopping_SessionRrpo = _shopping_SessionRrpo;
            cart_ItemRepo = _cart_ItemRepo;
            order_DetailsRepo = _order_DetailsRepo;
            order_ItemsRepo = _order_ItemsRepo;
            userPayementRepo = _userPayementRepo;
            payment_DetailsRepo = _payment_DetailsRepo;
            userAddress = _userAddress;
            shippingDetailsRepo = _shippingDetailsRepo;
            userRepo = _userRepo;
        }


        [HttpPost("BuyNow")]
        public IActionResult BuyProducts(BuyDTO buyDTO)
        {
            try
            {
                int UserID = buyDTO.UserID;
                int PaymentID = buyDTO.PaymentID;
                int ShipperID = buyDTO.ShipperID;
                int AddressID = buyDTO.AddressID;

                int Shopping_SessionID;
                int TotalPrice = 0;
                List<Shopping_Session> UserSessions = shopping_SessionRrpo.GetByUserId(UserID);
                if (UserSessions.Count == 0 || UserSessions == null)
                {
                    return BadRequest(new { Success = false, Message = BadRequistMSG });
                }
                else { Shopping_SessionID = UserSessions[UserSessions.Count - 1].Id; }

                List<Cart_Item> cart_Items = cart_ItemRepo.GetAllBySessionID(Shopping_SessionID);

                if (cart_Items.Count == 0 || cart_Items == null)
                {
                    return NotFound(new { Success = false, Message = "No Items In Cart" });
                }

                foreach (var item in cart_Items)
                {
                    if (item.Quantity <= item.product.Product_Inventory.Quantity)
                    {
                        TotalPrice += item.product.Price;
                    }
                    else
                    {
                        return BadRequest(new { Success = false, Message = $"{item.product.Name} Quantity Is Not Enough" });
                    }
                }
                //Payment Chech
                bool PaymentStutus = true;

                if (PaymentStutus)
                {
                    //Order Details
                    User_Payement user_Payement = userPayementRepo.GetUserPayment(PaymentID);

                    Order_Details newOrder = new Order_Details();
                    newOrder.Total = TotalPrice;
                    newOrder.UserID = UserID;

                    Payment_Details newPayment = new Payment_Details();
                    newPayment.Provider = user_Payement.Provider;
                    newPayment.Amount = TotalPrice;
                    payment_DetailsRepo.AddPayment_Details(newPayment);

                    newOrder.Payment_ID = newPayment.ID;
                    order_DetailsRepo.Create(newOrder);

                    //Order Items
                    Order_Items Neworder_Item;
                    for (int i = 0; i < cart_Items.Count; i++)
                    {
                        Neworder_Item = new Order_Items();

                        Neworder_Item.OrderID = newOrder.Id;
                        Neworder_Item.ProductID = cart_Items[i].ProductId;
                        Neworder_Item.Quantity = cart_Items[i].Quantity;

                        order_ItemsRepo.Create(Neworder_Item);
                    }

                    for (int i = 0; i < cart_Items.Count; i++)
                    {
                        //Reduce Quantity From Order
                        //Remove Orders from Cart_Item

                    }

                    shopping_SessionRrpo.ClearTotal(Shopping_SessionID);
                    //Add Shipping Details
                    User user = userRepo.GetUserById(UserID);
                    User_address userAddresss = userAddress.GetAddress(AddressID);
                    shippingDetails shippingDetails = new shippingDetails();
                    shippingDetails.shipperID = ShipperID;
                    shippingDetails.shipName = user.FirstName + user.LastName;
                    shippingDetails.userID = UserID;
                    shippingDetails.addressID = AddressID;
                    shippingDetails.orderDetailsID = newOrder.Id;
                    shippingDetails.shippingstate = "Pick Up";
                    shippingDetails.arabicshippingstate = "استلام شركة الشحن";
                    shippingDetails.ALLaddress = userAddresss.Country + " - " + userAddresss.City + " - " + userAddresss.PostalCode;
                    shippingDetails.ALLaddress_Ar = userAddresss.arabicCountry + " - " + userAddresss.arabicCity + " - " + userAddresss.PostalCode;
                    shippingDetails.CustomerMobile = userAddresss.mobile;

                    shippingDetailsRepo.insert(shippingDetails);
                    return Ok(new { Success = true, Message = "Order Purched Successfuly" });
                }
                else { return BadRequest(new { Success = false, Message = "Money Is Not Enough" }); }
            }
            catch (Exception ex)
            {

                return BadRequest(new { Success = false, Message = ex.Message });
            }
        }
    }
}
