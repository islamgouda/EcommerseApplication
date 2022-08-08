using EcommerseApplication.Models;
using EcommerseApplication.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EcommerseApplication.Controllers.AdminScope
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public AdminController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        [HttpPost]
        [Route("AddRoleToUser")]
        public async Task<IActionResult> AddUSerToSpecificRole([FromBody] AssignRolesByEmail model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Email Does not Exist" });
                }
                if (!await _roleManager.RoleExistsAsync(model.RoleName))
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Role Does not Exist" });
                }
                if (await _userManager.IsInRoleAsync(user, model.RoleName))
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = $"User already assigned to {model.RoleName} Role" });

                }
                await _userManager.AddToRoleAsync(user, model.RoleName);

            }
            else
            {
                var message = string.Join(" | ", ModelState.Values
               .SelectMany(v => v.Errors)
               .Select(e => e.ErrorMessage));
                return StatusCode(StatusCodes.Status500InternalServerError, message);
                //{
                //    "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                //   "title": "One or more validation errors occurred.",
                //   "status": 400,
                //   "traceId": "00-3bd746565e6013fab2c615e972c050d0-3670e1a7eeef39c4-00",
                //    "errors": {
                //        "Email": [
                //          "Email is required"
                //       ],
                //      "RoleName": [
                //     "Role Name is required"
                //         ]
                //            }
                //}
            }

            return Ok(new Response { Status = "Ok", Message = "Created Successfuly" });
        }
        [HttpPost]
        [Route("DeleteSubAdmin")]
        public async Task<IActionResult> DeleteSubAdmin([FromBody] string Email)
        {
            var user=await _userManager.FindByEmailAsync(Email);
            if(user == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Email is incorrect" });
            }
            if (!await _userManager.IsInRoleAsync(user, "SubAdmin"))
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User not assigned to A subAdmin Role" });

            }
            await _userManager.RemoveFromRoleAsync(user, "SubAdmin");
            return Ok(new Response { Status="Done",Message="Removed Successfuly"});
        }

        [HttpPost]
        [Route("CreateShiper")]
        public async Task<IActionResult> CreateShipper([FromBody] string Email)
        {
            var user=await _userManager.FindByEmailAsync(Email);
            if(user==null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User not Exists" });
            }
            else
            {

                await _userManager.AddToRoleAsync(user, "Shiper");
            }
            return Ok(new Response { Status = "Ok", Message = "Assigned Successfuly" });
        }

    }
}
