using Application.Dtos.MeasurementDtos;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Domain.Interfaces;
using Domain.QueryParameters;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class MeasurementsService : IMeasurementsService
    {
        private readonly IMeasurementsRepository _measurementsRepository;
        private readonly IWifisRepository _wifisRepository;
        private readonly IMapper _mapper;
        public MeasurementsService(IMeasurementsRepository measurementsRepository, IMapper mapper, IWifisRepository wifisRepository)
        {
            _measurementsRepository = measurementsRepository;
            _mapper = mapper;
            _wifisRepository= wifisRepository;

        }
        public async Task<AddMeasurementResponseDto> AddMeasurement(AddMeasurementDto measurementDto)
        {
            if (measurementDto == null) 
                throw new ArgumentNullException();

            measurementDto.Date = DateTime.Now;
            var measurement = _mapper.Map<Measurement>(measurementDto);
            await _measurementsRepository.AddMeasurement(measurement);

            return _mapper.Map<AddMeasurementResponseDto>(measurement);
        }

        public async Task<IPagedList<GetUserMeasurementsResponseDto>> GetMeasurementsByUser(Guid userId, UserResultsParameters parameters)
        {
            var measurements = _measurementsRepository.GetMeasurementsByUser(userId, parameters);
            
            var pagedMeasurementsDto = measurements.AsQueryable().ProjectTo<GetUserMeasurementsResponseDto>(_mapper.ConfigurationProvider);
           
            return new StaticPagedList<GetUserMeasurementsResponseDto>(pagedMeasurementsDto, measurements.PageNumber, measurements.PageSize, measurements.TotalItemCount);

        }

        public async Task<List<GetUserMeasurementsResponseDto>> GetAllMeasurementsByUser(Guid userId)
        {
            var measurements = await _measurementsRepository.GetAllMeasurementsByUser(userId);

            return _mapper.Map<List<GetUserMeasurementsResponseDto>>(measurements);
        }

        public async Task<List<GetDeviceMeasurementsResponseDto>> GetMeasurementsByDevice(Guid id)
        {
            var measurements = await _measurementsRepository.GetMeasurementsByDevice(id);

            return _mapper.Map<List<GetDeviceMeasurementsResponseDto>>(measurements);
        }

        public async Task ReadMeasurement(ReadMeasurementDto readMeasurementDto)
        {
            await _measurementsRepository.ReadMeasurement(readMeasurementDto.MeasurementId, readMeasurementDto.UserId);
            
        }
    }
}
