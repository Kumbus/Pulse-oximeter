using Application.Dtos.MeasurementDtos;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
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

        public async Task<List<GetUserMeasurementsResponseDto>> GetMeasurementsByUser(Guid userId)
        {
            var measurements = await _measurementsRepository.GetMeasurementsByUser(userId);

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
