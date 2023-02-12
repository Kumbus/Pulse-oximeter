using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUsersDevicesRepository
    {
        public Task<List<Guid>> GetUserDevices(Guid userId);
        public Task AddDeviceToUser(UserDevice userDevice);
        public Task<UserDevice> GetUserDevice(Guid UserId, Guid DeviceId);

    }
}
