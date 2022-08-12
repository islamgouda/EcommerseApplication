using EcommerseApplication.Models;
using EcommerseApplication.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EcommerseApplication.DTO;
using System.Security.Claims;

namespace EcommerseApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShippingDetailsController : ControllerBase
    {
        private IshippingDetails shippingDetails;
        private readonly IOrder_DetailsRepository order_DetailsRepo;
        private readonly string NotFoundMSG = "Data Not Found";
        private readonly string BadRequistMSG = "Invalid Input Data";
        private readonly string SuccessMSG = "Data Found Successfuly";
        public ShippingDetailsController(IshippingDetails _shippingDetails ,IOrder_DetailsRepository _order_DetailsRepo)
        {
            this.shippingDetails = _shippingDetails;
            order_DetailsRepo = _order_DetailsRepo;
        }
        //ShippingDetails/1   --Get
        [HttpGet("{id:int}")]
        public IActionResult getAllShippingByUserID(int id)
        {
            List<shippingDetails> shippingDetailsList;
            try
            {
                shippingDetailsList = shippingDetails.getAllbyUserID(id);
            }
            catch
            {
                return BadRequest(new { Success = true, Message = NotFoundMSG, Data ="notfound" });
            }
            return Ok(new { Success = true, Message = SuccessMSG, Data = shippingDetailsList });
           
        }
        //ShippingDetails/1    --put
        //"shipName" ,"shippingstate","arabicshippingstate"userID": shipperID:

        [HttpPut("{id:int}")]
        public IActionResult updateShipingState(int id,[FromBody]shippingDetails shipping)
        {
            try
            {
                shippingDetails.updateShippingDetails(id, shipping);
            }
            catch
            {
                return BadRequest(new { Success = true, Message = BadRequistMSG, Data = "dontsaved" });
            }
            return Ok(new { Success = true, Message = SuccessMSG, Data = "saved" });
            //shippingDetails.updateState(id, shippingstate);
            
            
        }
        //ShippingDetails/shipper/1   --Get
        [HttpGet("shipper/{id}")]
        public IActionResult getbyShipperID(int id)
        {
            List<shippingDetails> shippingDetailsList;
            try
            {
                shippingDetailsList = shippingDetails.getAllbyShipperID(id);
            }
            catch
            {
                return BadRequest(new { Success = true, Message = NotFoundMSG, Data = "notfound" });
            }
            return Ok(new { Success = true, Message = SuccessMSG, Data = shippingDetailsList });
            
        }
        /* public IActionResult ADDNewShip([FromBody] AddShippingDTO shipping)
         {
             shippingDetails sh = new shippingDetails();
             sh.
         }*/


        [HttpGet("showShippingProgress/{Orderid:int}")]
        public IActionResult showShippingProgressState(int Orderid)
        {
            try
            {
                int userID = int.Parse(User?.FindFirstValue("UserId"));
                List<Order_Details> Orders = order_DetailsRepo.GetAllByUserID(userID);
                //int userID = 5;
                //List<Order_Details> Orders = order_DetailsRepo.GetAllByUserID(5);

                Order_Details CurntOrder = Orders.FirstOrDefault(o => o.Id == Orderid);

                if (Orders.Count == 0 || CurntOrder == null)
                    return NotFound(new { Success = true, Message = NotFoundMSG, Data = new List<string>() });

                shippingDetails shippingData = shippingDetails.getByUserAndOrder(userID, Orderid);

                if (shippingData != null)
                {
                    return Ok(new { Success = true, Message = SuccessMSG, Data = new { ShippingStatt = shippingData.shippingstate, ShippingStatt_Ar = shippingData.arabicshippingstate } });
                }
                else
                {
                    return NotFound(new { Success = true, Message = NotFoundMSG, Data = new List<string>() });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }

        }
    }
}
