using EcommerseApplication.DTO;
using EcommerseApplication.Models;
using EcommerseApplication.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerseApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubCategoryController : ControllerBase
    {
        private readonly string NotFoundMSG = "Data Not Found";
        private readonly string BadRequistMSG = "Invalid Input Data";
        private readonly string SuccessMSG = "Data Found Successfuly";

        private readonly IsubCategory subCategoryRepo;

        public SubCategoryController(IsubCategory _subCategoryRepo)
        {
            subCategoryRepo = _subCategoryRepo;
        }

        [HttpGet("SubCategorysByCategoryID/{CategoryID:int}")]
        public IActionResult GetAllByCategoryID(int CategoryID)
        {
            try
            {
                List<SubCategoryResponseDTO> AllSubCategoryDTOs = new List<SubCategoryResponseDTO>();
                if (!ModelState.IsValid)
                    return BadRequest(new
                    {
                        Success = false,
                        Message = String.Join("; ", ModelState.Values.SelectMany(n => n.Errors)
                                            .Select(m => m.ErrorMessage)),
                        Data = new List<SubCategoryResponseDTO>()
                    });

                List<subCategory> AllSubCategorys = subCategoryRepo.GetAllWithIncludeByCategoryID(CategoryID);
                if (AllSubCategorys.Count == 0)
                    return NotFound(new { Success = true, Message = NotFoundMSG, Data = AllSubCategoryDTOs });

                if (AllSubCategorys != null)
                {
                    for (int i = 0; i < AllSubCategorys.Count; i++)
                    {
                        AllSubCategoryDTOs.Add(new SubCategoryResponseDTO());
                        AllSubCategoryDTOs[i].Id = AllSubCategorys[i].Id;
                        AllSubCategoryDTOs[i].Name = AllSubCategorys[i].Name;
                        AllSubCategoryDTOs[i].Description = AllSubCategorys[i].Description;
                        AllSubCategoryDTOs[i].Image = AllSubCategorys[i].image;
                        AllSubCategoryDTOs[i].CategoryName = AllSubCategorys[i].category.Name;
                    }
                    return Ok(new { Success = true, Message = SuccessMSG, Data = AllSubCategoryDTOs });
                }
                else
                {
                    return NotFound(new { Success = true, Message = NotFoundMSG, Data = AllSubCategoryDTOs });
                }
            }
            catch (Exception ex)
            {

                return BadRequest(new { Success = false, Message = ex.Message });
            }
        }
    }
}
