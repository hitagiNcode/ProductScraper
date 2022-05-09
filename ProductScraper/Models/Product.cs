using System.ComponentModel.DataAnnotations;

namespace ProductScraper.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; } 

        [Required]
        [StringLength(10)]
        public string ASIN { get; set; } = null!;

        [Required]
        [StringLength(250)]
        public string Name { get; set; } = null!;

        public string? Price { get; set; } 

        public string? PictureUri { get; set; }

        public DateTime LastSyncTime { get; set; } = DateTime.Now;
    }

    public class ProductLink
    {
        public string URL { get; set; } = null!;
    }
}
