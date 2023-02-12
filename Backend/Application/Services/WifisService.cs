using Application.Dtos.WifiDtos;
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
    public class WifisService : IWifisService
    {
        private readonly IWifisRepository _wifisRepository;
        private readonly IMapper _mapper;
        public WifisService(IWifisRepository wifisRepository, IMapper mapper)
        {
            _wifisRepository = wifisRepository;
            _mapper = mapper;
        }
        public async Task<AddWifiResponseDto> AddWifi(AddWifiDto wifiDto)
        {
            var wifi = await _wifisRepository.GetWifi(wifiDto.Name);
            if(wifi == null)
            {
                var newWifi = _mapper.Map<Wifi>(wifiDto);
                await _wifisRepository.AddWifi(newWifi);
                return _mapper.Map<AddWifiResponseDto>(newWifi);
            }

            return _mapper.Map<AddWifiResponseDto>(wifi);
        }
    }
}
