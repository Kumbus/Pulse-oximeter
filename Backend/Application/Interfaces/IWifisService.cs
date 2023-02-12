using Application.Dtos.WifiDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IWifisService
    {
        public Task<AddWifiResponseDto> AddWifi(AddWifiDto wifiDto);
    }
}
