using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UmojaCampus.API.Controllers.Base;
using UmojaCampus.Business.Services.Contracts;
using UmojaCampus.Shared.DTO.Account;

namespace UmojaCampus.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController(IAuthService authService) : BaseAuthController
    {
        private readonly IAuthService _authService = authService;
        
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var result = await _authService.LoginAsync(dto);
            
            if(!result.Succeeded) 
                return BadRequest(result.Messages);

            return Ok(result.Data);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto dto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RefreshTokenAsync(dto);
            if(!result.Succeeded)
                return BadRequest(result.Messages);

            return Ok(result.Data);
        }

        [HttpPost("revoke-token")]
        [Authorize]
        public async Task<IActionResult> RevokeToken()
        {
            var result = await _authService.RevokeTokenAsync(GetUser(User.Identity));
            
            if (!result.Succeeded)
                return BadRequest();

            return NoContent();
        }
    }
}
