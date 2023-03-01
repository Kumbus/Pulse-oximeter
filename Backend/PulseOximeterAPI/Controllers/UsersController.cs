using Application.Dtos;
using Application.Dtos.UserDtos;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PulseOximeterAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;
        
        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;   
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegistrationUserDto userDto)
        {
            var result = await _usersService.Register(userDto);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);

                return BadRequest(new RegistrationResponseDto { Errors = errors });
            }

            return Ok(new RegistrationResponseDto { IsSuccessfulRegistration = true }); 
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserDto userDto)
        {
            var response = await _usersService.Login(userDto);

            return Ok(response);
        }

        [HttpPost("googleLogin")]
        public async Task<IActionResult> ExternalLogin(GoogleLoginDto googleLoginDto)
        {
            var response = await _usersService.GoogleLogin(googleLoginDto);

            return Ok(response);
        }
    }
}
