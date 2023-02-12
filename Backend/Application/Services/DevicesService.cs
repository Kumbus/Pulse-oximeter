using Application.Dtos.DevicesDtos;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public  class DevicesService : IDevicesService
    {
        private readonly IDevicesRepository _devicesRepository;
        private readonly IUsersDevicesRepository _usersDevicesRepository;
        private readonly IWifisRepository _wifisRepository;
        private readonly IMeasurementsRepository _measurementsRepository;
        private readonly IMapper _mapper;

        public DevicesService(IDevicesRepository devicesRepository, IUsersDevicesRepository usersDevicesRepository, IMapper mapper, IWifisRepository wifisRepository, IMeasurementsRepository measurementsRepository)
        {
            _devicesRepository = devicesRepository;
            _usersDevicesRepository = usersDevicesRepository;
            _mapper = mapper;
            _measurementsRepository= measurementsRepository;
            _wifisRepository= wifisRepository;
        }

        public async Task<AddDeviceResponseDto> AddDevice(AddDeviceDto addDeviceDto)
        {
            if(addDeviceDto is null)
                throw new ArgumentNullException(nameof(addDeviceDto));

            var device = await _devicesRepository.GetDevice(addDeviceDto.MacAddress);
            if(device == null)
            {
                var newDevice = _mapper.Map<Device>(addDeviceDto);
                await _devicesRepository.AddDevice(newDevice);
                return _mapper.Map<AddDeviceResponseDto>(newDevice);
            }

            return _mapper.Map<AddDeviceResponseDto>(device);

            
        }

        public async Task<List<GetDevicesFromWifiResponseDto>> GetDevicesFromWifi(string ssid, string password, Guid userId)
        {
            var wifi = await _wifisRepository.CheckWifi(ssid, password);
            if(wifi is null)
            {
                return new List<GetDevicesFromWifiResponseDto>();
            }

            var devicesIds = await _measurementsRepository.GetDevicesIdsFromWifi(wifi.Id);
            List<Guid> filteredIds= new List<Guid>();

            foreach(var deviceId in devicesIds)
            {
                var userDevice = await _usersDevicesRepository.GetUserDevice(userId, deviceId);
                if(userDevice == null)
                {
                    filteredIds.Add(deviceId);
                }
                
            }

            var devices = await _devicesRepository.GetDevices(filteredIds);

            return _mapper.Map<List<GetDevicesFromWifiResponseDto>>(devices);
        }

        public async Task<List<GetUserDevicesResponseDto>> GetUserDevices(Guid userId)
        {
            var deviceIds = await _usersDevicesRepository.GetUserDevices(userId);

            var devices = await _devicesRepository.GetDevices(deviceIds);

            return _mapper.Map<List<GetUserDevicesResponseDto>>(devices);
        }
    }
}
