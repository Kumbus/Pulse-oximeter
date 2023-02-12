using Application.Dtos.DevicesDtos;
using Application.Dtos.UsersDevicesDtos;
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
    public class UsersDevicesService : IUsersDevicesService
    {
        private readonly IUsersDevicesRepository _usersDevicesRepository;
        private readonly IMapper _mapper;
        public UsersDevicesService(IUsersDevicesRepository usersDevicesRepository, IMapper mapper) 
        {
            _usersDevicesRepository = usersDevicesRepository;
            _mapper = mapper;
        }
        public async Task<AddDeviceToUserResponseDto> AddDeviceToUser(AddDeviceToUserDto addDeviceToUserDto)
        {
            if(addDeviceToUserDto is null)
                throw new ArgumentNullException(nameof(addDeviceToUserDto));

            var existingUserDevice = await _usersDevicesRepository.GetUserDevice(addDeviceToUserDto.UserId, addDeviceToUserDto.DeviceId);
            if(existingUserDevice is null)
            {
                var userDevice = _mapper.Map<UserDevice>(addDeviceToUserDto);
                await _usersDevicesRepository.AddDeviceToUser(userDevice);

                return _mapper.Map<AddDeviceToUserResponseDto>(userDevice);
            }

            return _mapper.Map<AddDeviceToUserResponseDto>(existingUserDevice);


        }
    }
}
