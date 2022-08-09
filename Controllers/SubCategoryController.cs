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
        private readonly IWebHostEnvironment environment;

        public SubCategoryController(IsubCategory _subCategoryRepo, IWebHostEnvironment _environment)
        {
            subCategoryRepo = _subCategoryRepo;
            environment = _environment;
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
                    string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images");
                    
                    for (int i = 0; i < AllSubCategorys.Count; i++)
                    {
                        AllSubCategoryDTOs.Add(new SubCategoryResponseDTO());
                        AllSubCategoryDTOs[i].Id = AllSubCategorys[i].Id;
                        AllSubCategoryDTOs[i].Name = AllSubCategorys[i].Name;
                        AllSubCategoryDTOs[i].Description = AllSubCategorys[i].Description;

                        string fileNameWithPath = Path.Combine(path, AllSubCategorys[i].image);
                        if (System.IO.File.Exists(fileNameWithPath))
                        {
                            byte[] imgByte = System.IO.File.ReadAllBytes(fileNameWithPath);
                            AllSubCategoryDTOs[i].Image = Convert.ToBase64String(imgByte);
                        }

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

        [HttpPost("CreateSubCategory")]
        public IActionResult CreateSubCategoryByCategoryID([FromForm] SubCategoryRequestDTO subCategoryDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new
                    {
                        Success = false,
                        Message = String.Join("; ", ModelState.Values.SelectMany(n => n.Errors)
                                            .Select(m => m.ErrorMessage)),
                        Data = subCategoryDTO
                    });

                if (subCategoryDTO == null)
                    return NotFound(new { Success = false, Message = BadRequistMSG });


                string ImageName = Guid.NewGuid() + "_" + subCategoryDTO.image.FileName;
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images");
                string fileNameWithPath = Path.Combine(path, ImageName);

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                using (FileStream stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    subCategoryDTO.image.CopyTo(stream);
                }

                subCategory NewSubCategory = new subCategory
                {
                    Name = subCategoryDTO.Name,
                    Description = subCategoryDTO.Description,
                    image = ImageName,
                    CategoryId = subCategoryDTO.CategoryId
                };
                subCategoryRepo.insert(NewSubCategory);
                return Created("Data Inserted Successfuly",NewSubCategory);
            }
            catch (Exception ex)
            {

                return BadRequest(new { Success = false, Message = ex.Message });
            }
        }
    }
}
