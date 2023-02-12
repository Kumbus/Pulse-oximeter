using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.DevicesDtos
{
    public class GetDevicesFromWifiResponseDto
    {
        public Guid Id { get; set; }
        public string MacAddress { get; set; }
    }
}
