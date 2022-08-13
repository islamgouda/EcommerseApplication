using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EcommerseApplication.Repository;
using EcommerseApplication.DTO;
using EcommerseApplication.Models;
using System.Security.Claims;

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
                    int UserID = int.Parse(User?.FindFirstValue("UserId"));
                    //int UserID =5;

                    User_Payement user_Payement = new User_Payement();
                    user_Payement.PayementType = NewUserPayment.PayementType;
                    user_Payement.Provider = NewUserPayment.Provider;

                    user_Payement.HolderName = NewUserPayment.HolderName;
                    user_Payement.CardNumber = NewUserPayment.CardNumber;
                    user_Payement.ExpYear = NewUserPayment.ExpYear;
                    user_Payement.ExpMonth = NewUserPayment.ExpMonth;
                    user_Payement.Cvc = NewUserPayment.Cvc;
                    user_Payement.UserId = UserID;


                    if (NewUserPayment.AccountNo != 0)
                        user_Payement.AccountNo = NewUserPayment.AccountNo;
                    if (NewUserPayment.Expiry != null)
                        user_Payement.Expiry = NewUserPayment.Expiry;

                    ///
                    userpaymentrepo.AddUSerPayment(user_Payement);
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
