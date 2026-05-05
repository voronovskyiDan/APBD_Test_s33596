using APBD_Test_s33596.DTOs.Request;
using APBD_Test_s33596.DTOs.Response;
using Microsoft.AspNetCore.Mvc;

namespace APBD_Test_s33596.Services
{
    public interface IMarkersService
    {
        public Task<MakersDetailsDto?> GetMarkersDetails(int id);
        public Task<int> AddMarker(AddMarketDto dto);

    }
}
