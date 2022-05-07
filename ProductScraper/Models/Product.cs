﻿using System.ComponentModel.DataAnnotations;

namespace ProductScraper.Models
{
    public class Product
    {
        [Required]
        [StringLength(250)]
        public string Name { get; set; } = null!;

        public string? Price { get; set; } 

        public string? PictureUri { get; set; }

        [Required]
        [StringLength(10)]
        public string ASIN { get; set; } = null!;
    }
}
