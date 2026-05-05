namespace APBD_Test_s33596.DTOs.Response
{
    public class VendorResponseDto
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Amount { get; set; }
        public decimal PricePerUnit { get; set; }
    }
}
