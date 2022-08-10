using EcommerseApplication.DTO;
using EcommerseApplication.Models;
using EcommerseApplication.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerseApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemController : ControllerBase
    {
        private readonly string NotFoundMSG = "Data Not Found";
        private readonly string BadRequistMSG = "Invalid Input Data";
        private readonly string SuccessMSG = "Data Found Successfuly";
        private readonly ICart_ItemRepository cart_ItemRepo;
        private readonly IShopping_SessionRepository shopping_SessionRrpo;
        private readonly IWebHostEnvironment environment;

        public CartItemController(ICart_ItemRepository _cart_ItemRepo, IShopping_SessionRepository _shopping_SessionRrpo, IWebHostEnvironment _environment)
        {
            cart_ItemRepo = _cart_ItemRepo;
            shopping_SessionRrpo = _shopping_SessionRrpo;
            environment = _environment;
        }


        [HttpPost("AddToCart")]
        public IActionResult AddToCart(CartItemRequestDTO NewCartItemDTO)
        {
            try
            {
                if (ModelState.IsValid && NewCartItemDTO != null)
                {
                    int Shopping_SessionID;
                    List<Shopping_Session> UserSessions = shopping_SessionRrpo.GetByUserId((int)NewCartItemDTO.UserId);
                    if (UserSessions.Count == 0 || UserSessions == null)
                    {
                        Shopping_Session NewShopping_Session = new Shopping_Session();
                        NewShopping_Session.UserID = NewCartItemDTO.UserId;
                        NewShopping_Session.Total = 0;
                        shopping_SessionRrpo.AddShopping_Session(NewShopping_Session);
                        Shopping_SessionID = NewShopping_Session.Id;
                    }
                    else { Shopping_SessionID = UserSessions[UserSessions.Count - 1].Id; }

                    Cart_Item cart_Item = new Cart_Item();
                    cart_Item.SessionId = Shopping_SessionID;
                    cart_Item.ProductId = NewCartItemDTO.ProductId;
                    cart_Item.Quantity = NewCartItemDTO.Quantity;

                    cart_ItemRepo.AddCart_Item(cart_Item);

                    return Ok(new { Success = true, Message = "Data Added Successfuly" });
                }
                else
                {
                    return BadRequest(new { Success = false, Message = BadRequistMSG });
                }
            }
            catch (Exception ex)
            {

                return BadRequest(new { Success = false, Message = ex.Message });
            }
        }

        [HttpGet("CartItemsByUser/{UserID:int}")]
        public IActionResult GetCartItemsByUserID(int UserID)
        {
            try
            {
                if (ModelState.IsValid && UserID != 0)
                {
                    List<CartItemResponseDTO> cartItemsDTO = new List<CartItemResponseDTO>();
                    int Shopping_SessionID;
                    List<Shopping_Session> UserSessions = shopping_SessionRrpo.GetByUserId((int)UserID);
                    if (UserSessions.Count == 0 || UserSessions == null)
                    {
                        return Ok(new { Success = true, Message = NotFoundMSG, Data = new List<CartItemResponseDTO>() });
                    }
                    else { Shopping_SessionID = UserSessions[UserSessions.Count - 1].Id; }

                    List<Cart_Item> cart_Items = cart_ItemRepo.GetAllBySessionID(Shopping_SessionID);


                    for (int i = 0; i < cart_Items.Count; i++)
                    {
                        cartItemsDTO.Add(new CartItemResponseDTO());
                        cartItemsDTO[i].ProductId = cart_Items[i].ProductId;
                        cartItemsDTO[i].QuantityOrdered = cart_Items[i].Quantity;
                        cartItemsDTO[i].ProductName = cart_Items[i].product.Name;

                        cartItemsDTO[i].ProductImage = Path.Combine(environment.WebRootPath, "Images/Product", cart_Items[i].product.Product_Images.FirstOrDefault().ImageFileName);
                        cartItemsDTO[i].ProductDiscription = cart_Items[i].product.Description;
                        cartItemsDTO[i].QuantityAvailable = cart_Items[i].product.Product_Inventory.Quantity;

                    }

                    return Ok(new { Success = true, Message = SuccessMSG, Data = cartItemsDTO });
                }
                else
                {
                    return BadRequest(new { Success = false, Message = BadRequistMSG });
                }
            }
            catch (Exception ex)
            {

                return BadRequest(new { Success = false, Message = ex.Message });
            }
        }
    }
}
