using Application.Dtos.DevicesDtos;

namespace Application.Interfaces
{
    public interface IDevicesService
    {
        public Task<List<GetUserDevicesResponseDto>> GetUserDevices(Guid userId);
        public Task<AddDeviceResponseDto> AddDevice(AddDeviceDto addDeviceDto);

        public Task<List<GetDevicesFromWifiResponseDto>> GetDevicesFromWifi(string ssid, string password, Guid userId);


    }
}
