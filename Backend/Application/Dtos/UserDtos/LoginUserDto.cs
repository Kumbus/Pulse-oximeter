using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.UserDtos
{
    public class LoginUserDto
    {
        [Required]
        public string UserName { get; set;}
        [Required]
        public string Password { get; set;}
    }
}
