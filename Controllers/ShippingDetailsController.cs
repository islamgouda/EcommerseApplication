﻿using EcommerseApplication.Models;
using EcommerseApplication.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EcommerseApplication.DTO;

namespace EcommerseApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShippingDetailsController : ControllerBase
    {
        private IshippingDetails shippingDetails;
        private readonly string NotFoundMSG = "Data Not Found";
        private readonly string BadRequistMSG = "Invalid Input Data";
        private readonly string SuccessMSG = "Data Found Successfuly";
        public ShippingDetailsController(IshippingDetails _shippingDetails)
        {
            this.shippingDetails = _shippingDetails;
        }
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
       
    }
}