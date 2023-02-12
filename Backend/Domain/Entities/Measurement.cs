using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public record Measurement
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public int AverageHeartRate { get; set; }
        public int MaximumHeartRate { get; set; }
        public int MinimumHeartRate { get; set; }
        public int AverageSpO2 { get; set; }
        public int MaximumSpO2 { get; set; }
        public int MinimumSpO2 { get; set; }
        public bool IsRead { get; set; }
        public Guid WifiId { get; set; }
        public Guid DeviceId { get; set; }
        public Guid? UserId { get; set; }
        
 
    }
}
