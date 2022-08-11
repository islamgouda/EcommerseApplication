using EcommerseApplication.DTO;
using EcommerseApplication.Models;
using EcommerseApplication.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EcommerseApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly string NotFoundMSG = "Data Not Found";
        private readonly string BadRequistMSG = "Invalid Input Data";
        private readonly string SuccessMSG = "Data Found Successfuly";

        private readonly IOrder_DetailsRepository order_DetailsRepo;

        public OrdersController(IOrder_DetailsRepository _order_DetailsRepo)
        {
            order_DetailsRepo = _order_DetailsRepo;
        }
        [HttpGet("GetAll")]
        public IActionResult GetAllByUserID()
        {
            try
            {

                List<Order_Details> Orders = order_DetailsRepo.GetAllByUserID(int.Parse(User?.FindFirstValue("UserId")));
                //List<Order_Details> Orders = order_DetailsRepo.GetAllByUserID(5);
                if (Orders.Count == 0)
                    return NotFound(new { Success = true, Message = NotFoundMSG, Data = new List<OrderDetailsDTO>() });

                if (Orders != null)
                {
                    List<OrderDetailsDTO> OrdersDTO = new List<OrderDetailsDTO>();
                    for (int i = 0; i < Orders.Count; i++)
                    {
                        OrdersDTO.Add(new OrderDetailsDTO());

                        OrdersDTO[i].OrderId = Orders[i].Id;
                        OrdersDTO[i].TotalPrice = Orders[i].Total;
                        OrdersDTO[i].CreatedAt = Orders[i].CreatedAt;

                        List<Order_Items> Items = Orders[i].Order_Items.ToList();
                        OrdersDTO[i].OrderItems = new List<OrderItemsDTO>();
                        for (int j = 0; j < Items.Count; j++)
                        {
                            OrdersDTO[i].OrderItems.Add(new OrderItemsDTO());

                            OrdersDTO[i].OrderItems[j].ItemID = Items[j].ID;
                            OrdersDTO[i].OrderItems[j].Quantity = Items[j].Quantity;
                            OrdersDTO[i].OrderItems[j].CreatedAt = Items[j].CreatedAt;
                            OrdersDTO[i].OrderItems[j].ProductID = Items[j].ProductID;
                            OrdersDTO[i].OrderItems[j].ProductName = Items[j].Product.Name;

                            if(Items[j].Product.Product_Images.Count > 0 && Items[j].Product.Product_Images != null)
                                OrdersDTO[i].OrderItems[j].ProductImage = Items[j].Product.Product_Images.FirstOrDefault().ImageFileName;
                        }
                    }
                    return Ok(new { Success = true, Message = SuccessMSG, Data = OrdersDTO });
                }
                else
                {
                    return NotFound(new { Success = false, Message = NotFoundMSG , Data = new List<OrderDetailsDTO>() });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
        }
    }
}
