using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.MeasurementDtos
{
    public class ReadMeasurementDto
    {
        public Guid MeasurementId { get; set; }
        public Guid UserId { get; set; }

    }
}
