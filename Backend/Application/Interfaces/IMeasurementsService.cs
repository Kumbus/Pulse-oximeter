using Application.Dtos.MeasurementDtos;
using Domain.QueryParameters;
using PagedList;

namespace Application.Interfaces
{
    public interface IMeasurementsService
    {
        public Task<AddMeasurementResponseDto> AddMeasurement(AddMeasurementDto measurementDto);
        public Task<List<GetDeviceMeasurementsResponseDto>> GetMeasurementsByDevice(Guid id);

        public Task ReadMeasurement(ReadMeasurementDto readMeasurementDto);

        public Task<IPagedList<GetUserMeasurementsResponseDto>> GetMeasurementsByUser(Guid userId, UserResultsParameters parameters);
        public Task<List<GetUserMeasurementsResponseDto>> GetAllMeasurementsByUser(Guid userId);
    }
}
