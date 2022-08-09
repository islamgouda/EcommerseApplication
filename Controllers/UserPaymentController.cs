using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EcommerseApplication.Repository;
using EcommerseApplication.DTO;

namespace EcommerseApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserPaymentController : ControllerBase
    {
        private readonly IUserPayement userpaymentrepo;
        private readonly ConsumerRespons Respons;

        public UserPaymentController(IUserPayement _userpaymentrepo,ConsumerRespons Respons)
        {
            userpaymentrepo = _userpaymentrepo;
            this.Respons = Respons;
        }
        [HttpPost]
        public IActionResult AddNewUserPayment(UserPaymentDTO NewUserPayment)
        {
            if(ModelState.IsValid==true)
            {
                try
                {
                    userpaymentrepo.AddUSerPaymentt(NewUserPayment);
                    Respons.succcess=true;
                    Respons.Message = "User Payment Added";
                    Respons.Data = "";
                    return Ok(Respons);
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError("",ex.InnerException.Message);
                    Respons.succcess = false;
                    Respons.Message = ex.InnerException.Message;
                    Respons.Data = "";
                    return BadRequest(Respons);
                }

               
            }
            Respons.succcess = false;
            Respons.Message = String.Join("; ", ModelState.Values.SelectMany(n => n.Errors)
                                            .Select(m => m.ErrorMessage));
            Respons.Data = "";
            return BadRequest(Respons);
        }
    }
}
