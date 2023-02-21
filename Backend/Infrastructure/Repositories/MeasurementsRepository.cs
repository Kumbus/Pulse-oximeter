using Domain.Entities;
using Domain.Interfaces;
using Domain.QueryParameters;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using PagedList;

namespace Infrastructure.Repositories
{
    public class MeasurementsRepository : IMeasurementsRepository
    {
        private readonly DatabaseContext _databaseContext;
        public MeasurementsRepository(DatabaseContext databaseContext) 
        { 
            _databaseContext = databaseContext;
        }
        public async Task AddMeasurement(Measurement measurement)
        {
            await _databaseContext.Measurements.AddAsync(measurement);
            await _databaseContext.SaveChangesAsync();

        }

        public async Task<List<Measurement>> GetAllMeasurementsByUser(Guid userId)
        {
            return await _databaseContext.Measurements.Where(m => m.UserId == userId).OrderBy(m => m.Date).ToListAsync();
        }

        public PagedList<Measurement> GetMeasurementsByUser(Guid userId, UserResultsParameters parameters)
        { 
            return new PagedList<Measurement>(_databaseContext.Measurements.Where(m => m.UserId == userId && m.Date >= parameters.MinDate && m.Date <= parameters.MaxDate)
                .OrderBy(m => m.Date), parameters.PageNumber, parameters.PageSize);
        }

        public async Task<List<Measurement>> GetMeasurementsByDevice(Guid deviceId)
        {
            return await _databaseContext.Measurements.Where(m => m.DeviceId == deviceId && m.IsRead == false).OrderBy(m => m.Date).ToListAsync();
        }

        public async Task<Measurement> ReadMeasurement(Guid measurementId, Guid userId)
        {
            var measurement = await  _databaseContext.Measurements.SingleOrDefaultAsync(m => m.Id == measurementId);
            measurement.UserId = userId;
            measurement.IsRead = !measurement.IsRead;
            await _databaseContext.SaveChangesAsync();

            return measurement;
        }

        public async Task<List<Guid>> GetDevicesIdsFromWifi(Guid wifiId)
        {
            return await _databaseContext.Measurements.Where(m => m.WifiId == wifiId).GroupBy(m => m.DeviceId).Select(m => m.Key).ToListAsync();
        }
    }
}
