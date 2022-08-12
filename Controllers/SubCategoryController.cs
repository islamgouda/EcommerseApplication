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
        private readonly ConsumerRespons Respons;

        public SubCategoryController(IsubCategory _subCategoryRepo, IWebHostEnvironment _environment, ConsumerRespons _Response)
        {
            subCategoryRepo = _subCategoryRepo;
            environment = _environment;
            Respons = _Response;
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
                    string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/SubCategory");

                    for (int i = 0; i < AllSubCategorys.Count; i++)
                    {
                        AllSubCategoryDTOs.Add(new SubCategoryResponseDTO());
                        AllSubCategoryDTOs[i].Id = AllSubCategorys[i].Id;
                        AllSubCategoryDTOs[i].Name = AllSubCategorys[i].Name;
                        AllSubCategoryDTOs[i].Description = AllSubCategorys[i].Description;
                        AllSubCategoryDTOs[i].arabicName = AllSubCategorys[i].arabicName;
                        AllSubCategoryDTOs[i].arabicDescription = AllSubCategorys[i].arabicDescription;

                        string fileNameWithPath = Path.Combine(path, AllSubCategorys[i].image);
                        if (System.IO.File.Exists(fileNameWithPath))
                        {
                            //byte[] imgByte = System.IO.File.ReadAllBytes(fileNameWithPath);
                            //AllSubCategoryDTOs[i].Image = Convert.ToBase64String(imgByte);
                            AllSubCategoryDTOs[i].Image = fileNameWithPath;
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
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/SubCategory");
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
                    arabicName = subCategoryDTO.arabicName,
                    Description = subCategoryDTO.Description,
                    arabicDescription = subCategoryDTO.arabicDescription,
                    image = ImageName,
                    CategoryId = subCategoryDTO.CategoryId
                };
                subCategoryRepo.insert(NewSubCategory);
                return Ok(new { Success = true, Message = "Data Inserted Successfuly", Data = NewSubCategory });
            }
            catch (Exception ex)
            {

                return BadRequest(new { Success = false, Message = ex.Message });
            }
        }

        [HttpDelete("DeleteSubCategory")]
        public IActionResult DeletesubCategory(int Id)
        {
            try
            {
                int result = subCategoryRepo.DeletesubCategory(Id);
                if (result == 1)
                {
                    Respons.succcess = true;
                    Respons.Message = "  subcategoryCategory Deleted successfuly";
                    Respons.Data = "";
                    return Ok(Respons);
                }
                else
                {
                    Respons.succcess = false;
                    Respons.Message = "not found this  subcategoryCategory";
                    Respons.Data = "";
                    return Ok(Respons);
                    return BadRequest(Respons);
                }
            }
            catch (Exception ex)
            {
                Respons.Message = ex.InnerException.Message;
                Respons.succcess = false;
                Respons.Data = "";
                return BadRequest(Respons);
            }
        }



        [HttpPut("updateSubcategory")]
        public IActionResult updateSubCategoryByCategoryID(int Id, [FromForm] SubCategoryRequestDTO subCategoryDTO)
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
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/SubCategory");
                string fileNameWithPath = Path.Combine(path, ImageName);

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                using (FileStream stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    subCategoryDTO.image.CopyTo(stream);
                }

                subCategory OldSubCategory = subCategoryRepo.getByID(Id);

                OldSubCategory.Name = subCategoryDTO.Name;
                OldSubCategory.arabicName = subCategoryDTO.arabicName;
                OldSubCategory.Description = subCategoryDTO.Description;
                OldSubCategory.arabicDescription = subCategoryDTO.arabicDescription;
                OldSubCategory.image = ImageName;
                OldSubCategory.CategoryId = subCategoryDTO.CategoryId;
                
                subCategoryRepo.updateSubCategory(OldSubCategory);
                return Ok(new { Success = true, Message = "Data updated Successfuly", Data = "" });
            }
            catch (Exception ex)
            {

                return BadRequest(new { Success = false, Message = ex.Message, Data= subCategoryDTO });
            }
        }
    }
}
