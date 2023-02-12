using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IDevicesRepository
    {
        public Task AddDevice(Device device);

        public Task<List<Device>> GetDevices(List<Guid> deviceIds);
        public Task<Device> GetDevice(string macAddress);

    }
}
