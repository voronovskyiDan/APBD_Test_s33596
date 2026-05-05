namespace APBD_Test_s33596.DTOs.Response
{
    public class MakersDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<ProductResponseDto> Products { get; set; } = new List<ProductResponseDto>();
    }
}
