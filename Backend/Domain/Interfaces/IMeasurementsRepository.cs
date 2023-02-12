using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public  interface IMeasurementsRepository
    {
        public Task AddMeasurement(Measurement measurement);
        public Task<List<Measurement>> GetMeasurementsByDevice(Guid deviceId);
        public Task<Measurement> ReadMeasurement(Guid measurementId, Guid userId);

        public Task<List<Measurement>> GetMeasurementsByUser(Guid userId);
        public Task<List<Guid>> GetDevicesIdsFromWifi(Guid wifiId);
    }
}
