using Application.Dtos.DevicesDtos;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PulseOximeterAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevicesController : ControllerBase
    {
        private readonly IDevicesService _devicesService;

        public DevicesController(IDevicesService devicesService)
        {
            _devicesService = devicesService;
        }

        [HttpPost]
        public async Task<IActionResult> AddDevice(AddDeviceDto addDeviceDto)
        {
            var response = await _devicesService.AddDevice(addDeviceDto);
            return Ok(response);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserDevices(Guid userId)
        {
            var response = await _devicesService.GetUserDevices(userId);
            return Ok(response);
        }

        [HttpGet("{ssid}/{password}/{userId}")]
        public async Task<IActionResult> GetDevicesFromWifi(string ssid, string password, Guid userId)
        {
            var response = await _devicesService.GetDevicesFromWifi(ssid, password, userId);
            return Ok(response);
        }



    }
}
