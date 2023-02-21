using Domain.QueryParameters;
using Domain.Entities;
using PagedList;

namespace Domain.Interfaces
{
    public  interface IMeasurementsRepository
    {
        public Task AddMeasurement(Measurement measurement);
        public Task<List<Measurement>> GetMeasurementsByDevice(Guid deviceId);
        public Task<Measurement> ReadMeasurement(Guid measurementId, Guid userId);

        public PagedList<Measurement> GetMeasurementsByUser(Guid userId, UserResultsParameters parameters);
        public Task<List<Measurement>> GetAllMeasurementsByUser(Guid userId);
        public Task<List<Guid>> GetDevicesIdsFromWifi(Guid wifiId);
    }
}
