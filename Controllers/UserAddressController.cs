using EcommerseApplication.DTO;
using EcommerseApplication.Models;
using EcommerseApplication.Repository;
using EcommerseApplication.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EcommerseApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAddressController : ControllerBase
    {
        IUserAddress userAddressrepo;
        private readonly ConsumerRespons Respons;
        private readonly IUserRepository userRepo;
        private readonly string NotFoundMSG = "Data Not Found";
        private readonly string BadRequistMSG = "Invalid Input Data";
        private readonly string SuccessMSG = "Data Found Successfuly";
        private string baseUrl2;
        public UserAddressController(IUserAddress _userAddress,ConsumerRespons response, IUserRepository _userRepo)
        {
            userAddressrepo = _userAddress;
            this.Respons = response;
            this.userRepo = _userRepo;
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
        [HttpGet("GetUserAdress")]
        public IActionResult GetAddress()
        {
            User user = userRepo.GetUserByIdentityId(User?.FindFirstValue("UserId"));
            if (user == null)
                return BadRequest(new { Success = false, Message = BadRequistMSG ,Data="not LoggedIn"});
            List<User_address> GetAll;
            Console.WriteLine(user.Id);
            try
            {
                GetAll = userAddressrepo.GetAllAddress(2);
                if (GetAll == null) {
                    return Ok(new { Success = false, Message = BadRequistMSG, Data = "not Found" });
                }
                //Ahmed
            }
            catch
            {
                return BadRequest(new { Success = false, Message = BadRequistMSG, Data = "not Found" });
            }

            return Ok(new { Success = true, Message = SuccessMSG, Data =GetAll  });
        }
    }
}
