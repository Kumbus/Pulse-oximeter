using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.MeasurementDtos
{
    public class AddMeasurementDto
    {
        public DateTime? Date { get; set; }
        public int AverageHeartRate { get; set; }
        public int MaximumHeartRate { get; set; }
        public int MinimumHeartRate { get; set; }
        public int AverageSpO2 { get; set; }
        public int MaximumSpO2 { get; set; }
        public int MinimumSpO2 { get; set; }
        public Guid WifiId { get; set; }
        public Guid DeviceId { get; set; }
    }
}
