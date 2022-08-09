using EcommerseApplication.DTO;
using EcommerseApplication.Models;
using EcommerseApplication.Respository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerseApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountController : ControllerBase
    {
        private readonly IDiscount discountrepository;
        private ConsumerRespons Response;
        public DiscountController(IDiscount discountrepository,ConsumerRespons _Response)
        {
            this.discountrepository = discountrepository;
            Response = _Response;
        }
        [HttpPost]
        public IActionResult AddNewDiscount(DiscountDTO NewDiscount)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    discountrepository.AddnewDiscountt(NewDiscount);
                    Response.succcess = true;
                    Response.Message = "Discount Added";
                    Response.Data = "";
                    return Ok(Response);
                }
                catch (Exception ex)
                {
                    Response.succcess = false;
                    Response.Message = ex.Message;
                    Response.Data = "";
                    return Ok(Response);
                }

            }
         
                Response.succcess = false;
            Response.Message = String.Join("; ", ModelState.Values.SelectMany(n => n.Errors)
                                            .Select(m => m.ErrorMessage));
            Response.Data = "";
                return Ok(Response);
            
        }
        [HttpGet("GetDiscount")]
        public IActionResult GetAllDiscounts()
        {
            try
            {
                List<Discount> ListDiscount = discountrepository.getDiscount();
                List<AssignDiscountToProduct> DiscountList=new List<AssignDiscountToProduct>();
               
                foreach(Discount discount in ListDiscount)
                {
                    AssignDiscountToProduct discountDTO = new AssignDiscountToProduct();
                    discountDTO.ID = discount.ID;
                    discountDTO.StartTime = discount.StartTime;
                    discountDTO.EndTime = discount.EndTime;
                    discountDTO.Descount_Persent = discount.Descount_Persent;
                    discountDTO.Name= discount.Name;
                    DiscountList.Add(discountDTO);
                }
                Response.Message = "git Discount Done";
                Response.succcess = true;
                Response.Data = DiscountList;
                return Ok(Response);

            }
            catch(Exception ex)
            {
                Response.Message = ex.InnerException.Message;
                Response.succcess = false;
                Response.Data = "";
                return BadRequest(Response);
            }
        }
        [HttpPost("DiscountAssign")]
        public IActionResult AssignDiscount(DiscountIDPartnerIDProductIDDTO Discount)
        {
            if (ModelState.IsValid == true)
            {
                try
                {
                    discountrepository.AssignDiscount(Discount);
                    Response.Message = "Assign discount Done";
                    Response.succcess = true;
                    Response.Data = "";
                    return Ok(Response);
                }
                catch (Exception ex)
                {
                    Response.succcess = true;
                    Response.Message = ex.InnerException.Message;
                    Response.Data = "";
                    return BadRequest(Response);
                }
            }

            Response.Message = String.Join("; ", ModelState.Values.SelectMany(n => n.Errors)
                                            .Select(m => m.ErrorMessage));
            Response.succcess = false;
            Response.Data = "";
            return BadRequest(Response);
        }
    }
}
