using Application.Dtos.UserDtos;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUsersService
    {
        public Task<IdentityResult> Register(RegistrationUserDto user);
        public Task<LoginResponseDto> Login(LoginUserDto user);
        public Task<LoginResponseDto> GoogleLogin(GoogleLoginDto googleLoginDto);
    }
}
