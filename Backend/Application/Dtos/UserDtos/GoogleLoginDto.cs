﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.UserDtos
{
    public class GoogleLoginDto
    {
        public string? Provider { get; set; }
        public string? IdToken { get; set; }
    }
}
