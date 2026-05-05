using APBD_Test_s33596.DTOs.Request;
using APBD_Test_s33596.DTOs.Response;

namespace APBD_Test_s33596.Repository
{
    public interface IMarkersRepository
    {
        public Task<MakersDetailsDto?> GetMarkersDetails(int id);
        public Task<int> AddMarker(AddMarketDto dto);
    }
}
