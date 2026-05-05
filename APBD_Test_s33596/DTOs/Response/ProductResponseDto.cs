namespace APBD_Test_s33596.DTOs.Response
{
    public class ProductResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal StrickerPrice { get; set; }
        public TypeResponseDto ProductType { get; set; } = new TypeResponseDto();
        public List<VendorResponseDto> Vendors { get; set; } = new List<VendorResponseDto>();
    }
}
