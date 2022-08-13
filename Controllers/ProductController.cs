
using EcommerseApplication.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EcommerseApplication.Models;
using EcommerseApplication.DTO;
using System.Security.Claims;

namespace EcommerseApplication.Controllers
{
    

    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly string NotFoundMSG = "Data Not Found";
        private readonly string BadRequistMSG = "Invalid Input Data";
        private readonly string SuccessMSG = "Data Found Successfuly";
        private string baseUrl2;

        private readonly IProductRepository productRepo;

        private readonly IProductRepository productrepository;
        private readonly IWebHostEnvironment environment;
        private readonly ConsumerRespons Respons;
        private readonly IProduct_InventoryRepository inventproductRepo;
        private readonly IUserRepository userRepo;
        private readonly Ipartener partenerRepo;
        private readonly IHttpContextAccessor baseUrl;
        private readonly IProduct_ImageRepository productImageRepo;

        public ProductController(IProductRepository _productRepo, IWebHostEnvironment _environment ,
                                 IProductRepository productrepository, ConsumerRespons _Response,
                                 IProduct_InventoryRepository _inventproductRepo, IUserRepository _userRepo,
                                 Ipartener _partenerRepo, IHttpContextAccessor _baseUrl, 
                                 IProduct_ImageRepository _productImageRepo)
        {
           this. productRepo = _productRepo;
           this.environment = _environment;
           this.productrepository = productrepository;
           this. Respons = _Response;
           this.inventproductRepo= _inventproductRepo;
            userRepo = _userRepo;
            partenerRepo = _partenerRepo;
            this.baseUrl = _baseUrl;
            productImageRepo = _productImageRepo;
            baseUrl2 = string.Format("{0}://{1}//", baseUrl.HttpContext.Request.Scheme, baseUrl.HttpContext.Request.Host.Value);
        }
        [HttpGet]
        
        public IActionResult Index()
        {
            List<ProductResponseDTO> ProductDTO = new List<ProductResponseDTO>();
            try
            {
              // productRepo.IsDiscountFinish();
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
                    //string wwwrootPath = environment.WebRootPath;

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

                        if(AllProducts[i].Name_Ar != null)
                            ProductDTO[i].Name_Ar = AllProducts[i].Name_Ar;
                        if (AllProducts[i].Description_Ar != null)
                            ProductDTO[i].Description_Ar = AllProducts[i].Description_Ar;

                        ProductDTO[i].Images = new List<string>();
                        foreach (var item in AllProducts[i].Product_Images)
                        {
                            string ImageFullPath = Path.Combine(baseUrl2, "Images/Product", item.ImageFileName);
                            //byte[] imgByte;
                            if (System.IO.File.Exists(ImageFullPath))
                            {
                                //imgByte = System.IO.File.ReadAllBytes(ImageFullPath);
                                //ProductDTO[i].Images.Add(Convert.ToBase64String(imgByte));
                                ProductDTO[i].Images.Add(ImageFullPath);
                            }
                        }
                    }
                    return Ok(new { Success= true, Message = SuccessMSG, Data = ProductDTO });
                }
                else
                {
                    return NotFound(new { Success = true, Message = NotFoundMSG, Data = ProductDTO });
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
                    //string wwwrootPath = environment.WebRootPath;

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


                        if (AllProducts[i].Name_Ar != null)
                            ProductDTO[i].Name_Ar = AllProducts[i].Name_Ar;
                        if (AllProducts[i].Description_Ar != null)
                            ProductDTO[i].Description_Ar = AllProducts[i].Description_Ar;

                        ProductDTO[i].Images = new List<string>();
                        foreach (var item in AllProducts[i].Product_Images)
                        {
                            string ImageFullPath = Path.Combine(baseUrl2, "Images/Product", item.ImageFileName);
                            //byte[] imgByte;
                            if (System.IO.File.Exists(ImageFullPath))
                            {
                                //imgByte = System.IO.File.ReadAllBytes(ImageFullPath);
                                //ProductDTO[i].Images.Add(Convert.ToBase64String(imgByte));
                                ProductDTO[i].Images.Add(ImageFullPath);
                            }
                        }
                    }
                    return Ok(new { Success = true, Message = SuccessMSG, Data = ProductDTO });
                }
                else
                {
                    return NotFound(new { Success = true, Message = NotFoundMSG, Data = ProductDTO });
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
                    //string wwwrootPath = environment.WebRootPath;

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

                        if (AllProducts[i].Name_Ar != null)
                            ProductDTO[i].Name_Ar = AllProducts[i].Name_Ar;
                        if (AllProducts[i].Description_Ar != null)
                            ProductDTO[i].Description_Ar = AllProducts[i].Description_Ar;

                        ProductDTO[i].Images = new List<string>();
                        foreach (var item in AllProducts[i].Product_Images)
                        {
                            string ImageFullPath = Path.Combine(baseUrl2, "Images/Product", item.ImageFileName);
                            //byte[] imgByte;
                            if (System.IO.File.Exists(ImageFullPath))
                            {
                                //imgByte = System.IO.File.ReadAllBytes(ImageFullPath);
                                //ProductDTO[i].Images.Add(Convert.ToBase64String(imgByte));
                                ProductDTO[i].Images.Add(ImageFullPath);
                            }
                        }
                    }
                    return Ok(new { Success = true, Message = SuccessMSG, Data = ProductDTO });
                }
                else
                {
                    return NotFound(new { Success = true, Message = NotFoundMSG, Data = ProductDTO });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message, Data = new List<Product>() });
            }
        }

        [HttpGet("PartnerProducts")]
        public IActionResult GetPartnerProducts()
        {
            try
            {
                List<ProductResponseDTO> ProductDTO = new List<ProductResponseDTO>();
                if (!ModelState.IsValid)
                    return BadRequest(new
                    {
                        Success = false,
                        Message = String.Join("; ", ModelState.Values.SelectMany(n => n.Errors)
                                            .Select(m => m.ErrorMessage)),
                        Data = new List<ProductResponseDTO>()
                    });

                User user = userRepo.GetUserByIdentityId(User?.FindFirstValue("UserId"));
                if (user == null)
                    return BadRequest(new { Success = false, Message = BadRequistMSG });

                Partener partener = partenerRepo.getByUserID(user.Id);
                if (partener == null)
                    return BadRequest(new { Success = false, Message = BadRequistMSG });

                int PartnerID = partener.Id;

                List<Product> AllProducts = productRepo.GetPartnerProducts(PartnerID);

                if (AllProducts.Count == 0)
                    return NotFound(new { Success = true, Message = NotFoundMSG, Data = ProductDTO });

                if (AllProducts.Count != 0 && AllProducts != null)
                {
                    //string wwwrootPath = environment.WebRootPath;

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

                        if (AllProducts[i].Name_Ar != null)
                            ProductDTO[i].Name_Ar = AllProducts[i].Name_Ar;
                        if (AllProducts[i].Description_Ar != null)
                            ProductDTO[i].Description_Ar = AllProducts[i].Description_Ar;

                        ProductDTO[i].Images = new List<string>();
                        foreach (var item in AllProducts[i].Product_Images)
                        {
                            string ImageFullPath = Path.Combine(baseUrl2, "Images/Product", item.ImageFileName);
                            //byte[] imgByte;
                            if (System.IO.File.Exists(ImageFullPath))
                            {
                                //imgByte = System.IO.File.ReadAllBytes(ImageFullPath);
                                //ProductDTO[i].Images.Add(Convert.ToBase64String(imgByte));
                                ProductDTO[i].Images.Add(ImageFullPath);
                            }
                        }
                    }
                    return Ok(new { Success = true, Message = SuccessMSG, Data = ProductDTO });
                }
                else
                {
                    return NotFound(new { Success = true, Message = NotFoundMSG, Data = ProductDTO });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message, Data = new List<Product>() });
            }
        }

        [HttpGet("PartnerProductsByCategory/{CategoryID:int}")]
        public IActionResult GetPartnerProductsByCategory(int CategoryID)
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

                User user = userRepo.GetUserByIdentityId(User?.FindFirstValue("UserId"));
                if (user == null)
                    return BadRequest(new { Success = false, Message = BadRequistMSG });

                Partener partener = partenerRepo.getByUserID(user.Id);
                if (partener == null)
                    return BadRequest(new { Success = false, Message = BadRequistMSG });

                int PartnerID = partener.Id;

                List<Product> AllProducts = productRepo.GetPartnerProductsByCategoryID(PartnerID, CategoryID);
                if (AllProducts.Count == 0)
                    return NotFound(new { Success = true, Message = NotFoundMSG, Data = ProductDTO });

                if (AllProducts.Count != 0 && AllProducts != null)
                {
                    //string wwwrootPath = environment.WebRootPath;

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

                        if (AllProducts[i].Name_Ar != null)
                            ProductDTO[i].Name_Ar = AllProducts[i].Name_Ar;
                        if (AllProducts[i].Description_Ar != null)
                            ProductDTO[i].Description_Ar = AllProducts[i].Description_Ar;

                        ProductDTO[i].Images = new List<string>();
                        foreach (var item in AllProducts[i].Product_Images)
                        {
                            string ImageFullPath = Path.Combine(baseUrl2, "Images/Product", item.ImageFileName);
                            //byte[] imgByte;
                            if (System.IO.File.Exists(ImageFullPath))
                            {
                                //imgByte = System.IO.File.ReadAllBytes(ImageFullPath);
                                //ProductDTO[i].Images.Add(Convert.ToBase64String(imgByte));
                                ProductDTO[i].Images.Add(ImageFullPath);
                            }
                        }
                    }
                    return Ok(new { Success = true, Message = SuccessMSG, Data = ProductDTO });
                }
                else
                {
                    return NotFound(new { Success = true, Message = NotFoundMSG, Data = ProductDTO });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message, Data = new List<Product>() });
            }
        }

        [HttpGet("PartnerProductsBySubCategory/{SubCategoryID:int}")]
        public IActionResult GetPartnerProductsBySubCategory(int SubCategoryID)
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

                User user = userRepo.GetUserByIdentityId(User?.FindFirstValue("UserId"));
                if (user == null)
                    return BadRequest(new { Success = false, Message = BadRequistMSG });

                Partener partener = partenerRepo.getByUserID(user.Id);
                if (partener == null)
                    return BadRequest(new { Success = false, Message = BadRequistMSG });

                int PartnerID = partener.Id;

                List<Product> AllProducts = productRepo.GetPartnerProductsBySubCategoryID(PartnerID, SubCategoryID);
                if (AllProducts.Count == 0)
                    return NotFound(new { Success = true, Message = NotFoundMSG, Data = ProductDTO });

                if (AllProducts.Count != 0 && AllProducts != null)
                {
                    for (int i = 0; i < AllProducts.Count; i++)
                    {
                        //string wwwrootPath = environment.WebRootPath;

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

                        if (AllProducts[i].Name_Ar != null)
                            ProductDTO[i].Name_Ar = AllProducts[i].Name_Ar;
                        if (AllProducts[i].Description_Ar != null)
                            ProductDTO[i].Description_Ar = AllProducts[i].Description_Ar;

                        ProductDTO[i].Images = new List<string>();
                        foreach (var item in AllProducts[i].Product_Images)
                        {
                            string ImageFullPath = Path.Combine(baseUrl2, "Images/Product", item.ImageFileName);
                            //byte[] imgByte;
                            if (System.IO.File.Exists(ImageFullPath))
                            {
                                //imgByte = System.IO.File.ReadAllBytes(ImageFullPath);
                                //ProductDTO[i].Images.Add(Convert.ToBase64String(imgByte));
                                ProductDTO[i].Images.Add(ImageFullPath);
                            }
                        }

                    }
                    return Ok(new { Success = true, Message = SuccessMSG, Data = ProductDTO });
                }
                else
                {
                    return NotFound(new { Success = true, Message = NotFoundMSG, Data = ProductDTO });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message, Data = new List<Product>() });
            }
        }

        [HttpPost]
        public IActionResult AddNewProduct([FromForm] ProductCetegorySubcategoryDTO NewProduct)
        {

            if (ModelState.IsValid == true)
            {
                Product product = new Product();
                product.CategoryID = NewProduct.CategoryID;
                product.CreatedAt = DateTime.Now;
                product.Name_Ar = NewProduct.Name_Ar;
                product.Description_Ar = NewProduct.Description_Ar;
                product.Description = NewProduct.Description;
                product.Name = NewProduct.Name;
                product.Price = NewProduct.Price;
                product.IsAvailable=NewProduct.IsAvailable;
                product.subcategoryID = NewProduct.subcategoryID;
                product.PartenerID = 1;//from parteenerID
                int ress = inventproductRepo.AddproductInventory(NewProduct.Quantity);
                if (ress != 0)
                {

                    try
                    {
                        product.InventoryID = ress;
                        productRepo.Create(product);
                        Respons.succcess = true;
                        Respons.Message = "product Added successfuly";
                        Respons.Data = "";
                        return Ok(Respons);
                    }
                    catch (Exception ex)
                    {
                        inventproductRepo.Delete(ress);
                        Respons.Message = ex.InnerException.Message;
                        Respons.succcess = false;
                        Respons.Data = "";
                        return BadRequest(Respons);
                    }
                }
                else
                {
                    Respons.Message = "error when add Quenitiy";
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
        public IActionResult UpdateProduct(int Id,[FromForm] ProductCetegorySubcategoryDTO NewProduct)
        {
            try
            {
                if (ModelState.IsValid == true)
                {
                    User user = userRepo.GetUserByIdentityId(User?.FindFirstValue("UserId"));
                    if(user == null)
                        return BadRequest(new { Success = false, Message = BadRequistMSG });

                    Partener partener = partenerRepo.getByUserID(user.Id);
                    if (partener == null)
                        return BadRequest(new { Success = false, Message = BadRequistMSG });

                    int PartnerID = partener.Id;

                
                    Product oldproduct = productrepository.Get(Id);
                    if (oldproduct != null)
                    {
                        oldproduct.CategoryID = NewProduct.CategoryID;
                        oldproduct.Name_Ar = NewProduct.Name_Ar;
                        oldproduct.Description_Ar = NewProduct.Description_Ar;
                        oldproduct.Description = NewProduct.Description;
                        oldproduct.Name = NewProduct.Name;
                        oldproduct.Price = NewProduct.Price;
                        oldproduct.IsAvailable = NewProduct.IsAvailable;
                        oldproduct.subcategoryID = NewProduct.subcategoryID;
                        oldproduct.PartenerID = PartnerID;

                        if (NewProduct.ImageFiles.Count > 0 && NewProduct.ImageFiles != null)
                        {
                            string wwwrootPath = environment.WebRootPath;
                            foreach (var item in NewProduct.ImageFiles)
                            {
                                string ImageName = Guid.NewGuid() + "_" + item.FileName;
                                string path = Path.Combine(wwwrootPath, "Images/Product");
                                string fileNameWithPath = Path.Combine(path, ImageName);
                                if (!Directory.Exists(path))
                                {
                                    Directory.CreateDirectory(path);
                                }
                                using (FileStream stream = new FileStream(fileNameWithPath, FileMode.Create))
                                {
                                    item.CopyTo(stream);
                                }
                                Product_Images NewImage = new Product_Images();
                                NewImage.ProductID = Id;
                                NewImage.ImageFileName = ImageName;
                                productImageRepo.Create(NewImage);
                            }
                            List<string> OldImages = productRepo.GetImages(Id);
                            foreach (var image in OldImages)
                            {
                                System.IO.File.Delete(Path.Combine("wwwroot", "Images", "SubCategory", image));
                            }
                        }
                        
                        try
                        {
                            inventproductRepo.updateproductInventory((int)oldproduct.InventoryID, NewProduct.Quantity);
                            productrepository.Update(Id, oldproduct);

                            Respons.succcess = true;
                            Respons.Message = "product updated successfuly";
                            Respons.Data = oldproduct;
                            return Ok(Respons);

                        }
                        catch (Exception ex)
                        {
                            Respons.Message = ex.InnerException.Message;
                            Respons.succcess = false;
                            Respons.Data = new List<string>();
                            return BadRequest(Respons);
                        }
                    }
                    else
                    {
                        Respons.Message = "product Not Found";
                        Respons.succcess = false;
                        Respons.Data = new List<string>();
                        return BadRequest(Respons);
                    }
                }
                Respons.Message = String.Join("; ", ModelState.Values.SelectMany(n => n.Errors)
                                             .Select(m => m.ErrorMessage));
                Respons.succcess = false;
                Respons.Data = NewProduct;
                return BadRequest(Respons);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message});
            }
            
        }
    }
}

 


