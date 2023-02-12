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
    public class DevicesRepository : IDevicesRepository
    {
        private readonly DatabaseContext _databaseContext;
        public DevicesRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }
        public async Task AddDevice(Device device)
        {
            await _databaseContext.Devices.AddAsync(device);
            await _databaseContext.SaveChangesAsync();
        }

        public async Task<List<Device>> GetDevices(List<Guid> deviceIds)
        {
            return await _databaseContext.Devices.Where(d => deviceIds.Contains(d.Id)).ToListAsync();
        }

        public async Task<Device> GetDevice(string macAddress)
        {
            return await _databaseContext.Devices.SingleOrDefaultAsync(d => d.MacAddress == macAddress);
        }


    }
}
