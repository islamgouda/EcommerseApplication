using EcommerseApplication.Models;
using EcommerseApplication.Respository;
using EcommerseApplication.ViewModels;
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
        public void AddnewCategory(ProductCategoryViewModel model)
        {
            Product_Category newCat = new Product_Category();
            newCat.Name = model.Name;   
            newCat.Description = model.Description;
            newCat.CreatedAt= DateTime.Now;
            productCategoryRespository.AddCategory(newCat);
        }


    }
}
