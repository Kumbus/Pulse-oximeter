using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UsersDevicesRepository : IUsersDevicesRepository
    {
        private readonly DatabaseContext _databaseContext;
        public UsersDevicesRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task AddDeviceToUser(UserDevice userDevice)
        {
            await _databaseContext.UsersDevices.AddAsync(userDevice);
            await _databaseContext.SaveChangesAsync();
        }


        public async Task<UserDevice> GetUserDevice(Guid UserId, Guid DeviceId)
        {
            var userDevice = await _databaseContext.UsersDevices.SingleOrDefaultAsync(ud => ud.UserId == UserId && ud.DeviceId == DeviceId);
            return userDevice;
        }

        public async Task<List<Guid>> GetUserDevices(Guid userId)
        {
            var deviceIds = await _databaseContext.UsersDevices.Where(ud => ud.UserId == userId).Select(ud => ud.DeviceId).ToListAsync();
            return deviceIds;
        }
    }
}
