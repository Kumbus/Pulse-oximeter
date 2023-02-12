using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public record Device
    {
        public Guid Id { get; set; }
        public string MacAddress { get; set; }
    }
}
