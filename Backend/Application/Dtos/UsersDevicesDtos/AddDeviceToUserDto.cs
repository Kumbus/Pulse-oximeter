using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.UsersDevicesDtos
{
    public class AddDeviceToUserDto
    {
        public Guid UserId { get; set; }
        public Guid DeviceId { get; set; }
    }
}
