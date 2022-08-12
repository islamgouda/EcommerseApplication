using EcommerseApplication.DTO;
using EcommerseApplication.Models;
using EcommerseApplication.Repository;
using EcommerseApplication.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EcommerseApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestTobeAPartner : ControllerBase
    {
        private readonly IRequest request;
        private readonly UserManager<AppUser> userManager;

        public RequestTobeAPartner(IRequest _request,UserManager<AppUser> _userManager)
        {
            request = _request;
            userManager = _userManager;
        }
        [HttpPost]
        [Route("BePartner")]
        public async Task<IActionResult> BePartner([FromBody] RequestDto requestmodel)
        {
            Requests model = new Requests();
            var UserId = User?.FindFirstValue(ClaimTypes.Sid);
            if (UserId == null)
            {
               
                return Ok(new Response { Status = "Error", Message = "Login first" });
            }
            model.IdentityId = UserId;
            model.Name = requestmodel.Name;
            model.numberOfBranches = requestmodel.numberOfBranches;
            model.RequestType = "Partner";
            request.RequestTobePartner(model);
            return Ok(new Response { Status = "Ok", Message = "Request under Review" });
        }
        [HttpGet]
        [Authorize(Roles ="Admin")]
        [Route("AllParteners")]
        public List<Requests> AllPartenersRequests()
        {
            List<Requests> Allrequests = request.GetAllRequests();
            return Allrequests;
        }
    }
}
