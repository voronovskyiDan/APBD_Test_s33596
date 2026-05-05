using APBD_Test_s33596.DTOs.Request;
using APBD_Test_s33596.DTOs.Response;
using APBD_Test_s33596.Repository;

namespace APBD_Test_s33596.Services
{
    public class MarkersService : IMarkersService
    {
        private readonly IMarkersRepository _markersRepository;
        public MarkersService(IMarkersRepository markersRepository)
        {
            _markersRepository = markersRepository;
        }

        public async Task<int> AddMarker(AddMarketDto dto)
        {
            return await _markersRepository.AddMarker(dto);
        }

        public async Task<MakersDetailsDto?> GetMarkersDetails(int id)
        {
            return await _markersRepository.GetMarkersDetails(id);
        }
    }
}
