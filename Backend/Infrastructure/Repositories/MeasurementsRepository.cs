using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public PagedList<Measurement> GetMeasurementsByUser(Guid userId, int pageNumber, int pageSize)
        {
            return new PagedList<Measurement>(_databaseContext.Measurements.Where(m => m.UserId == userId).OrderBy(m => m.Date), pageNumber, pageSize);
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
