using APBD_Test_s33596.DTOs.Request;
using APBD_Test_s33596.Excpetions;
using APBD_Test_s33596.Services;
using Microsoft.AspNetCore.Mvc;

namespace APBD_Test_s33596.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MarkersConrtoller : ControllerBase
    {
        private readonly IMarkersService _markersService;
        public MarkersConrtoller(IMarkersService markersService)
        {
            _markersService = markersService;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetMarkersDetails(int id)
        {
            var markers = await _markersService.GetMarkersDetails(id);
            if (markers == null)
                return NotFound();

            return Ok(markers);
        }

        [HttpPost]
        public async Task<IActionResult> AddMarker(AddMarketDto dto)
        {
            try
            {
                var res = await _markersService.AddMarker(dto);
                return CreatedAtAction(nameof(GetMarkersDetails), new { id = res }, null);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
