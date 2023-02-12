using Application.Dtos.MeasurementDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IMeasurementsService
    {
        public Task<AddMeasurementResponseDto> AddMeasurement(AddMeasurementDto measurementDto);
        public Task<List<GetDeviceMeasurementsResponseDto>> GetMeasurementsByDevice(Guid id);

        public Task ReadMeasurement(ReadMeasurementDto readMeasurementDto);

        public Task<List<GetUserMeasurementsResponseDto>> GetMeasurementsByUser(Guid userId);
    }
}
