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
        private readonly IWebHostEnvironment environment;

        public ProductController(IProductRepository _productRepo,IWebHostEnvironment _environment)
        {
            productRepo = _productRepo;
            environment = _environment;
        }
        [HttpGet]
        public IActionResult Index()
        {
            List<ProductResponseDTO> ProductDTO = new List<ProductResponseDTO>();
            try
            {
                if(!ModelState.IsValid)
                    return BadRequest(new { Success = false,
                                            Message = String.Join("; ",ModelState.Values.SelectMany(n=>n.Errors)
                                            .Select(m=>m.ErrorMessage)),
                                            Data = new List<ProductResponseDTO>() });

                List<Product> AllProducts = productRepo.GetAllWithInclude();
                if (AllProducts.Count == 0)
                    return NotFound(new { Success = true, Message = NotFoundMSG, Data = ProductDTO });

                if (AllProducts != null)
                {
                    string wwwrootPath = environment.WebRootPath;

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
                                              DateTime.Compare((DateTime)AllProducts[i].Discount.EndTime,DateTime.Now) < 0 ||
                                              AllProducts[i].Discount.Active == false ?
                                                                0: 
                                                                AllProducts[i].Discount.Descount_Persent;
                        ProductDTO[i].PartenerName = AllProducts[i].Partener.Name;
                        ProductDTO[i].CategoryName = AllProducts[i].Product_Category.Name;
                        ProductDTO[i].subcategoryName = AllProducts[i].subcategory.Name;

                        ProductDTO[i].Images = new List<string>();
                        foreach (var item in AllProducts[i].Product_Images)
                        {
                            string ImageFullPath = Path.Combine(wwwrootPath, "Images", item.ImageFileName);
                            byte[] imgByte;
                            if (System.IO.File.Exists(ImageFullPath))
                            {
                                imgByte = System.IO.File.ReadAllBytes(ImageFullPath);
                                ProductDTO[i].Images.Add(Convert.ToBase64String(imgByte));
                            }
                        }
                    }
                    return Ok(new { Success= true, Message = SuccessMSG, Data = ProductDTO });
                }
                else
                {
                    return NotFound(new { Success = false, Message = NotFoundMSG, Data = ProductDTO });
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
                    string wwwrootPath = environment.WebRootPath;

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

                        ProductDTO[i].Images = new List<string>();
                        foreach (var item in AllProducts[i].Product_Images)
                        {
                            string ImageFullPath = Path.Combine(wwwrootPath, "Images", item.ImageFileName);
                            byte[] imgByte;
                            if (System.IO.File.Exists(ImageFullPath))
                            {
                                imgByte = System.IO.File.ReadAllBytes(ImageFullPath);
                                ProductDTO[i].Images.Add(Convert.ToBase64String(imgByte));
                            }
                        }
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
                    string wwwrootPath = environment.WebRootPath;

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

                        ProductDTO[i].Images = new List<string>();
                        foreach (var item in AllProducts[i].Product_Images)
                        {
                            string ImageFullPath = Path.Combine(wwwrootPath, "Images", item.ImageFileName);
                            byte[] imgByte;
                            if (System.IO.File.Exists(ImageFullPath))
                            {
                                imgByte = System.IO.File.ReadAllBytes(ImageFullPath);
                                ProductDTO[i].Images.Add(Convert.ToBase64String(imgByte));
                            }
                        }
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
                    string wwwrootPath = environment.WebRootPath;

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

                        ProductDTO[i].Images = new List<string>();
                        foreach (var item in AllProducts[i].Product_Images)
                        {
                            string ImageFullPath = Path.Combine(wwwrootPath, "Images", item.ImageFileName);
                            byte[] imgByte;
                            if (System.IO.File.Exists(ImageFullPath))
                            {
                                imgByte = System.IO.File.ReadAllBytes(ImageFullPath);
                                ProductDTO[i].Images.Add(Convert.ToBase64String(imgByte));
                            }
                        }
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
                    string wwwrootPath = environment.WebRootPath;

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

                        ProductDTO[i].Images = new List<string>();
                        foreach (var item in AllProducts[i].Product_Images)
                        {
                            string ImageFullPath = Path.Combine(wwwrootPath, "Images", item.ImageFileName);
                            byte[] imgByte;
                            if (System.IO.File.Exists(ImageFullPath))
                            {
                                imgByte = System.IO.File.ReadAllBytes(ImageFullPath);
                                ProductDTO[i].Images.Add(Convert.ToBase64String(imgByte));
                            }
                        }
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
                        string wwwrootPath = environment.WebRootPath;

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

                        ProductDTO[i].Images = new List<string>();
                        foreach (var item in AllProducts[i].Product_Images)
                        {
                            string ImageFullPath = Path.Combine(wwwrootPath, "Images", item.ImageFileName);
                            byte[] imgByte;
                            if (System.IO.File.Exists(ImageFullPath))
                            {
                                imgByte = System.IO.File.ReadAllBytes(ImageFullPath);
                                ProductDTO[i].Images.Add(Convert.ToBase64String(imgByte));
                            }
                        }

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
