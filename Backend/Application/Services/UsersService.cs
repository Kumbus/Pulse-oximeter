using Application.Dtos.UserDtos;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static Domain.Exceptions.NotAuthorizedException;

namespace Application.Services
{
    public class UsersService : IUsersService
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly TokenService _jwtHandler;
        public UsersService(UserManager<User> userManager, IMapper mapper, TokenService jwtHandler)
        {
            _userManager = userManager;
            _mapper = mapper;
            _jwtHandler = jwtHandler;
        }

        public async Task<LoginResponseDto> GoogleLogin(GoogleLoginDto googleLoginDto)
        {
            var payload = await _jwtHandler.VerifyGoogleToken(googleLoginDto);
            if(payload  == null)
               throw new InvalidGoogleCredentialsException();

            var info = new UserLoginInfo(googleLoginDto.Provider, payload.Subject, googleLoginDto.Provider);

            var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(payload.Email);
                if (user == null)
                {
                    user = new User { Email = payload.Email, UserName = payload.Email, FirstName = payload.GivenName, LastName = payload.FamilyName };
                    await _userManager.CreateAsync(user);
                    //prepare and send an email for the email confirmation
                    //await _userManager.AddToRoleAsync(user, "Viewer");
                    await _userManager.AddLoginAsync(user, info);
                }
                else
                {
                    await _userManager.AddLoginAsync(user, info);
                }
            }

            if (user == null)
                throw new InvalidGoogleCredentialsException();
            //check for the Locked out account

            var signingCredentials = _jwtHandler.GetSigningCredentials();
            var claims = _jwtHandler.GetClaims(user);
            var tokenOptions = _jwtHandler.GenerateTokenOptions(signingCredentials, claims);
            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return new LoginResponseDto { IsAuthSuccessful = true, Token = token };

        }

        public async Task<LoginResponseDto> Login(LoginUserDto userDto)
        {
            var user = await _userManager.FindByNameAsync(userDto.UserName);
            if (user == null || !await _userManager.CheckPasswordAsync(user, userDto.Password))
                throw new InvalidLoginCredentialsException();
            

            var signingCredentials = _jwtHandler.GetSigningCredentials();
            var claims = _jwtHandler.GetClaims(user);
            var tokenOptions = _jwtHandler.GenerateTokenOptions(signingCredentials, claims);
            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return new LoginResponseDto { IsAuthSuccessful = true, Token = token };


        }

        public async Task<IdentityResult> Register(RegistrationUserDto userDto)
        {
            if (userDto == null)
                throw new InvalidCredentialsException();

            var user = _mapper.Map<User>(userDto);

            var result = await _userManager.CreateAsync(user, userDto.Password);

            return result;
        }
    }
}
