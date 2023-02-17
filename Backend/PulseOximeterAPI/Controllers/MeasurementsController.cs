using Application.Dtos.MeasurementDtos;
using Application.Dtos.WifiDtos;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace PulseOximeterAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeasurementsController : ControllerBase
    {
        private readonly IMeasurementsService _measurementsService;

        public MeasurementsController(IMeasurementsService measurementsService)
        {
            _measurementsService = measurementsService;         
        }

        [HttpPost]
        public async Task<IActionResult> AddMeasurement(AddMeasurementDto measurementDto)
        {
            var response = await _measurementsService.AddMeasurement(measurementDto);
            return Ok(response);
        }

        [HttpGet("device/{id}")]
        public async Task<IActionResult> GetMeasurementsByDevice(Guid id)
        {
            var response  = await _measurementsService.GetMeasurementsByDevice(id);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> ReadMeasurement(ReadMeasurementDto readMeasurementDto)
        {
            await _measurementsService.ReadMeasurement(readMeasurementDto);
            return Ok();
        }

        [HttpGet("user")]
        public async Task<IActionResult> GetMeasurementsByUser([FromQuery] UserResultsParameters parameters)
        {
            var response = await _measurementsService.GetMeasurementsByUser(parameters);
            var metadata = new
            {
                response.TotalItemCount,
                response.PageSize,
                response.PageNumber,
                response.PageCount,
                response.IsFirstPage,
                response.IsLastPage
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            Response.Headers.Add("Access-Control-Expose-Headers", "X-Pagination");
            return Ok(response);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetAllMeasurementsByUser(Guid userId)
        {
            var response = await _measurementsService.GetAllMeasurementsByUser(userId);
            return Ok(response);
        }
    }
}
