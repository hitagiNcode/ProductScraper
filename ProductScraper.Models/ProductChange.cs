using System.ComponentModel.DataAnnotations;

namespace ProductScraper.Domain
{
    public class ProductChange
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(10)]
        public string ASIN { get; set; } = null!;

        [Required]
        [StringLength(450)]
        public string ChangedVar { get; set; } = null!;

        [Required]
        [StringLength(1450)]
        public string ChangeValue { get; set; } = null!;

        [Required]
        [StringLength(1450)]
        public string? NewValue { get; set; }
    }
}
