using Domain.Entities;
using PagedList;
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

        public PagedList<Measurement> GetMeasurementsByUser(Guid userId, int pageNumber, int pageSize);
        public Task<List<Measurement>> GetAllMeasurementsByUser(Guid userId);
        public Task<List<Guid>> GetDevicesIdsFromWifi(Guid wifiId);
    }
}
