using EcommerseApplication.Repository;
using EcommerseApplication.DDO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EcommerseApplication.Models;

namespace EcommerseApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository productrepository;
        private readonly ConsumerRespons Respons;

        public ProductController(IProductRepository productrepository, ConsumerRespons _Response)
        {
            this.productrepository = productrepository;
            Respons = _Response;
        }
        [HttpPost]
        public IActionResult AddNewProduct(ProductCetegorySubcategoryDTO NewProduct)
        {
           
            if (ModelState.IsValid == true)
            {
                Product product = new Product();
                product.CategoryID = NewProduct.CategoryID;
                product.CreatedAt = DateTime.Now;
                product.DiscountID = NewProduct.DiscountID;
                product.Description = NewProduct.Description;
                product.Name = NewProduct.Name;
                product.Price = NewProduct.Price;
                product.InventoryID = NewProduct.InventoryID;
                product.subcategoryID = NewProduct.subcategoryID;
                product.PartenerID = NewProduct.PartenerID;
                product.Description_Ar = "ssssss";
                product.Name_Ar = "kkkkk";

                try
                {
                    productrepository.Create(product);
                    Respons.succcess = true;
                    Respons.Message = "product Added successfuly";
                    Respons.Data = "";
                    return Ok(Respons);
                }
                catch (Exception ex)
                {
                    Respons.Message = ex.InnerException.Message;
                    Respons.succcess = false;
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
        [HttpDelete("DeleteProductById/{Id:int}")]
        public IActionResult DeleteProduct(int Id)
        {
            try
            {
                int result = productrepository.Deletee(Id);
                if (result == 1)
                {
                    Respons.succcess = true;
                    Respons.Message = "Product Deleted successfuly";
                    Respons.Data = "";
                    return Ok(Respons);
                }
                else
                {
                    Respons.succcess = false;
                    Respons.Message = "not found this product";
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
        [HttpPut("updateProduct/{Id:int}")]
        public IActionResult UpdateProduct(int Id,ProductCetegorySubcategoryDTO NewProduct)
        {
            
            if (ModelState.IsValid == true)
            {
                Product oldproduct = productrepository.Get(Id);
                if (oldproduct != null)
                {
                    oldproduct.CategoryID = NewProduct.CategoryID;
                    oldproduct.CreatedAt = DateTime.Now;
                    oldproduct.DiscountID = NewProduct.DiscountID;
                    oldproduct.Description = NewProduct.Description;
                    oldproduct.Name = NewProduct.Name;
                    oldproduct.Price = NewProduct.Price;
                    oldproduct.InventoryID = NewProduct.InventoryID;
                    oldproduct.subcategoryID = NewProduct.subcategoryID;
                    oldproduct.PartenerID = NewProduct.PartenerID;
                    oldproduct.UpdatedAt = DateTime.Now;
                    try
                    {
                         productrepository.Update(Id, oldproduct);
                        Respons.succcess = true;
                        Respons.Message = "product updated successfuly";
                        Respons.Data = "";
                        return Ok(Respons);
                          
                    }
                    catch (Exception ex)
                    {
                        Respons.Message = ex.InnerException.Message;
                        Respons.succcess = false;
                        Respons.Data = "";
                        return BadRequest(Respons);

                    }
                }
                else
                {
                    Respons.Message = "product Not Found";
                    Respons.succcess = false;
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
