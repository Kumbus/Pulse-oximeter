using Application.Dtos.UsersDevicesDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUsersDevicesService
    {
        public Task<AddDeviceToUserResponseDto> AddDeviceToUser(AddDeviceToUserDto addDeviceToUserDto);
    }
}
