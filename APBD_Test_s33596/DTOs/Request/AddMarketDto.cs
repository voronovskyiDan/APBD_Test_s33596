using System.ComponentModel.DataAnnotations;

namespace APBD_Test_s33596.DTOs.Request
{
    public class AddMarketDto 
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        public List<ProductRequestDto> Products { get; set; } = new List<ProductRequestDto>();
    }
}
