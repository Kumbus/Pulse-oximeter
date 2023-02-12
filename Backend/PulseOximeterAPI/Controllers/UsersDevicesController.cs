using Application.Dtos.UsersDevicesDtos;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PulseOximeterAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersDevicesController : ControllerBase
    {
        private readonly IUsersDevicesService _usersDevicesService;

        public UsersDevicesController(IUsersDevicesService usersDevicesService)
        {
            _usersDevicesService = usersDevicesService;
        }

        [HttpPost]
        public async Task<IActionResult> AddDeviceToUser(AddDeviceToUserDto addDeviceToUserDto)
        {
            var response = await _usersDevicesService.AddDeviceToUser(addDeviceToUserDto);
            return Ok(response);
        }
    }
}
