using Application.Dtos.DevicesDtos;
using Application.Dtos.MeasurementDtos;
using Application.Dtos.UserDtos;
using Application.Dtos.UsersDevicesDtos;
using Application.Dtos.WifiDtos;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappings
{
    public static class AutoMapperConfig
    {
        public static IMapper Initialize()
            => new MapperConfiguration(cfg =>
            {
                //cfg.CreateMap<OutputFile, OutputFileDto>().ConstructUsing(f => new OutputFileDto(f.UserId, f.Name, f.Path, f.Size, f.LastModifiedAt.ToString()));
                cfg.CreateMap<RegistrationUserDto, User>();
                cfg.CreateMap<User, LoginResponseDto>();
                cfg.CreateMap<AddWifiDto, Wifi>();
                cfg.CreateMap<Wifi, AddWifiResponseDto>();
                cfg.CreateMap<AddMeasurementDto, Measurement>();
                cfg.CreateMap<Measurement, AddMeasurementResponseDto>();
                cfg.CreateMap<Measurement, GetUserMeasurementsResponseDto>();
                cfg.CreateMap<Measurement, GetDeviceMeasurementsResponseDto>();
                cfg.CreateMap<Device, GetUserDevicesResponseDto>();
                cfg.CreateMap<AddDeviceDto, Device>();
                cfg.CreateMap<Device, AddDeviceResponseDto>();
                cfg.CreateMap<Device, GetDevicesFromWifiResponseDto>();
                cfg.CreateMap<UserDevice, AddDeviceToUserResponseDto>();
                cfg.CreateMap<AddDeviceToUserDto, UserDevice>();

            }).CreateMapper();
    }
}
