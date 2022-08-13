using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerseApplication.Models
{
    public class Partener
    {
        public int Id { get; set; }
        [ForeignKey("User")]
        public int? userID { get; set; }
        public string Name { get; set; }
        public string? Type { get; set; }
        public int? numberOfBranches { get; set; }
        
        public int? addressID;
        public string? UserIDentityID { get; set; }
       // public User? User { get; set; }
        public List<Product>? Products { get; set; }
    }
}
