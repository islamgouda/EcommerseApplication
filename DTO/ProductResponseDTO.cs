﻿using System.ComponentModel.DataAnnotations;


namespace EcommerseApplication.DTO
{
    public class ProductResponseDTO
    {
        
        public int ID { get; set; }
        public string Name { get; set; }
        //public string? Name_Ar { get; set; }
        public string Description { get; set; }
        //public string? Description_Ar { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public decimal? Discount { get; set; }
        public bool IsAvailable { get; set; }

        //Images List
        public string PartenerName { get; set; }
        public string CategoryName { get; set; }
        public string subcategoryName { get; set; }
    }
}