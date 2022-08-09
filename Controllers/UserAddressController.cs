using EcommerseApplication.DTO;
using EcommerseApplication.Models;
using EcommerseApplication.Repository;
using EcommerseApplication.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerseApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAddressController : ControllerBase
    {
        IUserAddress userAddressrepo;
        private readonly ConsumerRespons Respons;

        public UserAddressController(IUserAddress _userAddress,ConsumerRespons response)
        {
            userAddressrepo = _userAddress;
            this.Respons = response;
        }
        [HttpPost]
        public IActionResult AddNewAdress(UserAddressUSerIdDTO NewAdress)
        {
           if(ModelState.IsValid==true)
            {
                try
                {
                    userAddressrepo.AddNewAddresss(NewAdress);
                    Respons.succcess = true;
                    Respons.Message = "User Address Added";
                    Respons.Data = "";
                    return Ok(Respons);
                }
                catch(Exception ex)
                {
                    //ModelState.AddModelError("",ex.InnerException.Message);
                    Respons.Message = ex.InnerException.Message;
                    Respons.succcess=false;
                    Respons.Data = "";
                    return BadRequest(Respons);
                }
            }
            Respons.Message = String.Join("; ", ModelState.Values.SelectMany(n => n.Errors)
                                            .Select(m => m.ErrorMessage));
            Respons.succcess = false;
            Respons.Data = "";
            return BadRequest(Respons);
            
        }
    }
}
