﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.WifiDtos
{
    public class AddWifiResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }

    }
}
