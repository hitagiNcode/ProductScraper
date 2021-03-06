using System.ComponentModel.DataAnnotations;

namespace ProductScraper.Domain
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(10)]
        public string ASIN { get; set; } = null!;

        [Required]
        [StringLength(450)]
        public string Name { get; set; } = null!;

        [Range(1,50000)]
        public int? Price { get; set; }

        [StringLength(1500)]
        public string? PictureUri { get; set; }

        [StringLength(1500)]
        public string URL { get; set; } = null!;

        public DateTime LastSyncTime { get; set; } = DateTime.Now;

        public ICollection<TrackingUser>? FollowingUsers { get; set; }

    }

    public class TrackingUser
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public Product Product { get; set; } = null!;
    }

}
