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

        public CategoryController(IProductCategory productCategory)
        {
            productCategoryRespository = productCategory;
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
            try
            {
                Product_Category newCat = new Product_Category();
                newCat.Name = model.Name;
                newCat.Description = model.Description;
                newCat.Name_Ar = model.Name_Ar;
                newCat.Description_Ar = model.Description_Ar;
                newCat.CreatedAt = DateTime.Now;
                productCategoryRespository.AddCategory(newCat);
            }
            catch (Exception ex) {
                return Ok(new { Success = false, Message = "Failed", Data = "Dontsaved" });
            }
            return Ok(new { Success = true, Message = "succeded", Data = "saved" });
        }


    }
}
