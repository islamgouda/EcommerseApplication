using EcommerseApplication.DTO;
using EcommerseApplication.Models;
using EcommerseApplication.Respository;
using EcommerseApplication.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerseApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IProductCategory productCategoryRespository;
        private readonly ConsumerRespons Respons;

        public CategoryController(IProductCategory productCategory, ConsumerRespons _Response)
        {
            productCategoryRespository = productCategory;
            Respons = _Response;
        }
       
        [HttpGet]
        [Route("GetallCategories")]
        public List<Product_Category> GetallCategories()
        {
            return productCategoryRespository.GetAllCategories();
        }
        [HttpPost]
        [Route("AddnewCategory")]
        public IActionResult AddnewCategory(ProductCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Product_Category newCat = new Product_Category();
                    newCat.Name = model.Name;
                    newCat.Description = model.Description;
                    newCat.Name_Ar = model.Name_Ar;
                    newCat.Description_Ar = model.Description_Ar;
                    newCat.CreatedAt = DateTime.Now;
                    productCategoryRespository.AddCategory(newCat);
                    return Ok(new { Success = true, Message = "succeded", Data = "saved" });
                }
                catch (Exception ex)
                {
                    return BadRequest(new { Success = false, Message = ex.Message, Data = "Dontsaved" });
                }
            }
            return BadRequest(new
            {
                Success = false,
                Message = String.Join("; ", ModelState.Values.SelectMany(n => n.Errors)
                .Select(m => m.ErrorMessage)),
                Data = ""
            });
        }
        [HttpPut("updateCategory")]
        public IActionResult UpdateCategory(int Id,ProductCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    Product_Category oldCat = productCategoryRespository.GetCategoryByID(Id);
                    oldCat.Name = model.Name;
                    oldCat.Description = model.Description;
                    oldCat.Name_Ar = model.Name_Ar;
                    oldCat.Description_Ar = model.Description_Ar;
                    oldCat.UpdatedAt = DateTime.Now;
                    productCategoryRespository.UpdateOldCategory(oldCat);
                    return Ok(new { Success = true, Message = "succeded", Data = "Updated" });
                }
                catch (Exception ex)
                {
                    return Ok(new { Success = false, Message = ex.Message, Data = "Dontupdated" });
                }
            }
            return BadRequest(new
            {
                Success = false,
                Message = String.Join("; ", ModelState.Values.SelectMany(n => n.Errors)
                .Select(m => m.ErrorMessage)),
                Data = model
            });

        }
        [HttpDelete("DeleteCategory")]
        public IActionResult DeleteCategory(int Id)
        {
            try
            {
                int result = productCategoryRespository.DeleteACategory(Id);
                if (result == 1)
                {
                    Respons.succcess = true;
                    Respons.Message = "Product Category Deleted successfuly";
                    Respons.Data = "";
                    return Ok(Respons);
                }
                else
                {
                    Respons.succcess = false;
                    Respons.Message = "not found this product Category";
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

    }
}
