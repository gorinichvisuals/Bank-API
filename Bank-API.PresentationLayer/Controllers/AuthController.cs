using Bank_API.BusinessLogicLayer.Interfaces;
using Bank_API.BusinessLogicLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bank_API.PresentationLayer.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> CreateUser([FromBody] UserRegistrationRequest userRequest)
        {
            var token = await authService.CreateUser(userRequest);

            if(token == null)
            {
                return StatusCode(403, new { error = "User already exists"});
            }

            return StatusCode(201, new { token = token });
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest userRequest)
        {
            var token = await authService.Login(userRequest);

            if (token == null)
            {
                return StatusCode(403, new { error = "Incorrect credentials" });
            }

            return StatusCode(201, new { token = token });
        }
    }
}
