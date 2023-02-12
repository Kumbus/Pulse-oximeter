using Application.Dtos.WifiDtos;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace PulseOximeterAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WifisController : ControllerBase
    {
        private readonly IWifisService _wifisService;

        public WifisController(IWifisService wifisService)
        {
            _wifisService = wifisService;
        }

        [HttpPost]
        public async Task<IActionResult> AddWifi(AddWifiDto wifiDto)
        {
            var response = await _wifisService.AddWifi(wifiDto);
            return Ok(response);
 

        }
    }
}
