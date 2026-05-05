using System.ComponentModel.DataAnnotations;

namespace APBD_Test_s33596.DTOs.Request
{
    public class ProductRequestDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public decimal StickerPrice { get; set; }

        [Required]
        public string Type { get; set; } = string.Empty;
    }
}
