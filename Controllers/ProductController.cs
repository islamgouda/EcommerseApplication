using EcommerseApplication.DTO;
using EcommerseApplication.Models;
using EcommerseApplication.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace EcommerseApplication.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly string NotFoundMSG = "Data Not Found";
        private readonly string BadRequistMSG = "Invalid Input Data";
        private readonly string SuccessMSG = "Data Found Successfuly";
        private readonly IProductRepository productRepo;

        public ProductController(IProductRepository _productRepo)
        {
            productRepo = _productRepo;
        }
        [HttpGet]
        public IActionResult Index()
        {
            List<ProductResponseDTO> AllProductDTO = new List<ProductResponseDTO>();
            ProductResponseDTO ProductDTO = new ProductResponseDTO();
            try
            {
                if(!ModelState.IsValid)
                    return BadRequest(new { Success = false,
                                            Message = String.Join("; ",ModelState.Values.SelectMany(n=>n.Errors)
                                            .Select(m=>m.ErrorMessage)),
                                            Data = new List<ProductResponseDTO>() });

                List<Product> AllProducts = productRepo.GetAllWithInclude();
                if (AllProducts.Count == 0)
                    return NotFound(new { Success = true, Message = NotFoundMSG, Data = AllProductDTO });
                if (AllProducts != null)
                {
                    for (int i = 0; i < AllProducts.Count; i++)
                    {
                        ProductDTO.ID = AllProducts[i].ID;
                        ProductDTO.Name = AllProducts[i].Name;
                        ProductDTO.Description = AllProducts[i].Description;
                        ProductDTO.Price = AllProducts[i].Price;
                        ProductDTO.IsAvailable = AllProducts[i].IsAvailable;
                        ProductDTO.PartenerName = AllProducts[i].Partener.Name;
                        ProductDTO.Quantity = AllProducts[i].Product_Inventory.Quantity;
                        ProductDTO.Discount = AllProducts[i].Discount.Descount_Persent == decimal.Zero ||
                                              DateTime.Compare((DateTime)AllProducts[i].Discount.EndTime,DateTime.Now) < 0 ||
                                              AllProducts[i].Discount.Active == false ?
                                                                0: 
                                                                AllProducts[i].Discount.Descount_Persent;
                        ProductDTO.PartenerName = AllProducts[i].Partener.Name;
                        ProductDTO.CategoryName = AllProducts[i].Product_Category.Name;
                        ProductDTO.subcategoryName = AllProducts[i].subcategory.Name;

                        AllProductDTO.Add(ProductDTO);
                    }
                    return Ok(new { Success= true, Message = SuccessMSG, Data = AllProductDTO });
                }
                else
                {
                    return NotFound(new { Success = false, Message = NotFoundMSG, Data = AllProductDTO });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message, Data = new List<ProductResponseDTO>() });
            }
        }

        [HttpGet("CategoryProducts/{id:int}")]
        public IActionResult GetAllByCategory(int Id)
        {
            List<ProductResponseDTO> ProductDTO = new List<ProductResponseDTO>();
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new
                    {
                        Success = false,
                        Message = String.Join("; ", ModelState.Values.SelectMany(n => n.Errors)
                                            .Select(m => m.ErrorMessage)),
                        Data = new List<ProductResponseDTO>()
                    });

                List<Product> AllProducts = productRepo.GetAllByCategoryID(Id);
                if (AllProducts.Count == 0)
                    return NotFound(new { Success = true, Message = NotFoundMSG, Data = ProductDTO });

                if (AllProducts.Count != 0 && AllProducts != null)
                {
                    for (int i = 0; i < AllProducts.Count; i++)
                    {
                        ProductDTO.Add(new ProductResponseDTO());
                        ProductDTO[i].ID = AllProducts[i].ID;
                        ProductDTO[i].Name = AllProducts[i].Name;
                        ProductDTO[i].Description = AllProducts[i].Description;
                        ProductDTO[i].Price = AllProducts[i].Price;
                        ProductDTO[i].IsAvailable = AllProducts[i].IsAvailable;
                        ProductDTO[i].PartenerName = AllProducts[i].Partener.Name;
                        ProductDTO[i].Quantity = AllProducts[i].Product_Inventory.Quantity;
                        ProductDTO[i].Discount = AllProducts[i].Discount.Descount_Persent == decimal.Zero ||
                                              DateTime.Compare((DateTime)AllProducts[i].Discount.EndTime, DateTime.Now) < 0 ||
                                              AllProducts[i].Discount.Active == false ?
                                                                0 :
                                                                AllProducts[i].Discount.Descount_Persent;
                        ProductDTO[i].PartenerName = AllProducts[i].Partener.Name;
                        ProductDTO[i].CategoryName = AllProducts[i].Product_Category.Name;
                        ProductDTO[i].subcategoryName = AllProducts[i].subcategory.Name;

                    }
                    return Ok(new { Success = true, Message = SuccessMSG, Data = ProductDTO });
                }
                else
                {
                    return NotFound(new { Success = false, Message = NotFoundMSG, Data = ProductDTO });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message, Data = new List<Product>() });
            }
        }

        [HttpGet("SubCategoryProducts/{id:int}")]
        public IActionResult GetAllBySubCategory(int Id)
        {
            List<ProductResponseDTO> ProductDTO = new List<ProductResponseDTO>();
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new
                    {
                        Success = false,
                        Message = String.Join("; ", ModelState.Values.SelectMany(n => n.Errors)
                                            .Select(m => m.ErrorMessage)),
                        Data = new List<ProductResponseDTO>()
                    });

                List<Product> AllProducts = productRepo.GetAllBySubCategoryID(Id);
                if (AllProducts.Count == 0)
                    return NotFound(new { Success = true, Message = NotFoundMSG, Data = ProductDTO });

                if (AllProducts.Count != 0 && AllProducts != null)
                {
                    for (int i = 0; i < AllProducts.Count; i++)
                    {
                        ProductDTO.Add(new ProductResponseDTO());
                        ProductDTO[i].ID = AllProducts[i].ID;
                        ProductDTO[i].Name = AllProducts[i].Name;
                        ProductDTO[i].Description = AllProducts[i].Description;
                        ProductDTO[i].Price = AllProducts[i].Price;
                        ProductDTO[i].IsAvailable = AllProducts[i].IsAvailable;
                        ProductDTO[i].PartenerName = AllProducts[i].Partener.Name;
                        ProductDTO[i].Quantity = AllProducts[i].Product_Inventory.Quantity;
                        ProductDTO[i].Discount = AllProducts[i].Discount.Descount_Persent == decimal.Zero ||
                                                 DateTime.Compare((DateTime)AllProducts[i].Discount.EndTime, DateTime.Now) < 0 ||
                                                 AllProducts[i].Discount.Active == false ?
                                                                0 :
                                                                AllProducts[i].Discount.Descount_Persent;
                        ProductDTO[i].PartenerName = AllProducts[i].Partener.Name;
                        ProductDTO[i].CategoryName = AllProducts[i].Product_Category.Name;
                        ProductDTO[i].subcategoryName = AllProducts[i].subcategory.Name;

                    }
                    return Ok(new { Success = true, Message = SuccessMSG, Data = ProductDTO });
                }
                else
                {
                    return NotFound(new { Success = false, Message = NotFoundMSG, Data = ProductDTO });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message, Data = new List<Product>() });
            }
        }

        [HttpGet("PartnerProducts/{id:int}")]
        public IActionResult GetPartnerProducts(int Id)
        {
            List<ProductResponseDTO> ProductDTO = new List<ProductResponseDTO>();
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new
                    {
                        Success = false,
                        Message = String.Join("; ", ModelState.Values.SelectMany(n => n.Errors)
                                            .Select(m => m.ErrorMessage)),
                        Data = new List<ProductResponseDTO>()
                    });

                List<Product> AllProducts = productRepo.GetPartnerProducts(Id);

                if (AllProducts.Count == 0)
                    return NotFound(new { Success = true, Message = NotFoundMSG, Data = ProductDTO });

                if (AllProducts.Count != 0 && AllProducts != null)
                {
                    for (int i = 0; i < AllProducts.Count; i++)
                    {
                        ProductDTO.Add(new ProductResponseDTO());
                        ProductDTO[i].ID = AllProducts[i].ID;
                        ProductDTO[i].Name = AllProducts[i].Name;
                        ProductDTO[i].Description = AllProducts[i].Description;
                        ProductDTO[i].Price = AllProducts[i].Price;
                        ProductDTO[i].IsAvailable = AllProducts[i].IsAvailable;
                        ProductDTO[i].PartenerName = AllProducts[i].Partener.Name;
                        ProductDTO[i].Quantity = AllProducts[i].Product_Inventory.Quantity;
                        ProductDTO[i].Discount = AllProducts[i].Discount.Descount_Persent == decimal.Zero ||
                                                 DateTime.Compare((DateTime)AllProducts[i].Discount.EndTime, DateTime.Now) < 0 ||
                                                 AllProducts[i].Discount.Active == false ?
                                                                0 :
                                                                AllProducts[i].Discount.Descount_Persent;
                        ProductDTO[i].PartenerName = AllProducts[i].Partener.Name;
                        ProductDTO[i].CategoryName = AllProducts[i].Product_Category.Name;
                        ProductDTO[i].subcategoryName = AllProducts[i].subcategory.Name;

                    }
                    return Ok(new { Success = true, Message = SuccessMSG, Data = ProductDTO });
                }
                else
                {
                    return NotFound(new { Success = false, Message = NotFoundMSG, Data = ProductDTO });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message, Data = new List<Product>() });
            }
        }

        [HttpGet("PartnerProductsByCategory/{PartnerID:int}/{CategoryID:int}")]
        public IActionResult GetPartnerProductsByCategory(int PartnerID, int CategoryID)
        {
            List<ProductResponseDTO> ProductDTO = new List<ProductResponseDTO>();
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new
                    {
                        Success = false,
                        Message = String.Join("; ", ModelState.Values.SelectMany(n => n.Errors)
                                            .Select(m => m.ErrorMessage)),
                        Data = new List<ProductResponseDTO>()
                    });

                List<Product> AllProducts = productRepo.GetPartnerProductsByCategoryID(PartnerID, CategoryID);
                if (AllProducts.Count == 0)
                    return NotFound(new { Success = true, Message = NotFoundMSG, Data = ProductDTO });

                if (AllProducts.Count != 0 && AllProducts != null)
                {
                    for (int i = 0; i < AllProducts.Count; i++)
                    {
                        ProductDTO.Add(new ProductResponseDTO());
                        ProductDTO[i].ID = AllProducts[i].ID;
                        ProductDTO[i].Name = AllProducts[i].Name;
                        ProductDTO[i].Description = AllProducts[i].Description;
                        ProductDTO[i].Price = AllProducts[i].Price;
                        ProductDTO[i].IsAvailable = AllProducts[i].IsAvailable;
                        ProductDTO[i].PartenerName = AllProducts[i].Partener.Name;
                        ProductDTO[i].Quantity = AllProducts[i].Product_Inventory.Quantity;
                        ProductDTO[i].Discount = AllProducts[i].Discount.Descount_Persent == decimal.Zero ||
                                                 DateTime.Compare((DateTime)AllProducts[i].Discount.EndTime, DateTime.Now) < 0 ||
                                                 AllProducts[i].Discount.Active == false ?
                                                                0 :
                                                                AllProducts[i].Discount.Descount_Persent;
                        ProductDTO[i].PartenerName = AllProducts[i].Partener.Name;
                        ProductDTO[i].CategoryName = AllProducts[i].Product_Category.Name;
                        ProductDTO[i].subcategoryName = AllProducts[i].subcategory.Name;

                    }
                    return Ok(new { Success = true, Message = SuccessMSG, Data = ProductDTO });
                }
                else
                {
                    return NotFound(new { Success = false, Message = NotFoundMSG, Data = ProductDTO });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message, Data = new List<Product>() });
            }
        }

        [HttpGet("PartnerProductsBySubCategory/{PartnerID:int}/{SubCategoryID:int}")]
        public IActionResult GetPartnerProductsBySubCategory(int PartnerID, int SubCategoryID)
        {
            List<ProductResponseDTO> ProductDTO = new List<ProductResponseDTO>();
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new
                    {
                        Success = false,
                        Message = String.Join("; ", ModelState.Values.SelectMany(n => n.Errors)
                                            .Select(m => m.ErrorMessage)),
                        Data = new List<ProductResponseDTO>()
                    });

                List<Product> AllProducts = productRepo.GetPartnerProductsBySubCategoryID(PartnerID, SubCategoryID);
                if (AllProducts.Count == 0)
                    return NotFound(new { Success = true, Message = NotFoundMSG, Data = ProductDTO });

                if (AllProducts.Count != 0 && AllProducts != null)
                {
                    for (int i = 0; i < AllProducts.Count; i++)
                    {
                        ProductDTO.Add(new ProductResponseDTO());
                        ProductDTO[i].ID = AllProducts[i].ID;
                        ProductDTO[i].Name = AllProducts[i].Name;
                        ProductDTO[i].Description = AllProducts[i].Description;
                        ProductDTO[i].Price = AllProducts[i].Price;
                        ProductDTO[i].IsAvailable = AllProducts[i].IsAvailable;
                        ProductDTO[i].PartenerName = AllProducts[i].Partener.Name;
                        ProductDTO[i].Quantity = AllProducts[i].Product_Inventory.Quantity;
                        ProductDTO[i].Discount = AllProducts[i].Discount.Descount_Persent == decimal.Zero ||
                                              DateTime.Compare((DateTime)AllProducts[i].Discount.EndTime, DateTime.Now) < 0 ||
                                              AllProducts[i].Discount.Active == false ?
                                                                0 :
                                                                AllProducts[i].Discount.Descount_Persent;
                        ProductDTO[i].PartenerName = AllProducts[i].Partener.Name;
                        ProductDTO[i].CategoryName = AllProducts[i].Product_Category.Name;
                        ProductDTO[i].subcategoryName = AllProducts[i].subcategory.Name;

                    }
                    return Ok(new { Success = true, Message = SuccessMSG, Data = ProductDTO });
                }
                else
                {
                    return NotFound(new { Success = false, Message = NotFoundMSG, Data = ProductDTO });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message, Data = new List<Product>() });
            }
        }
    }
}
