namespace EcommerseApplication.DTO
{
    public class SubCategoryRequestDTO
    {
        public string Name { get; set; }
        public string? arabicName { get; set; }
        public string Description { get; set; }
        public string? arabicDescription { get; set; }
        public IFormFile image { get; set; }
        public int CategoryId { get; set; }
    }
}
