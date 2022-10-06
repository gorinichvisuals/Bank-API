using Bank_API.BusinessLogicLayer.Interfaces;
using Bank_API.BusinessLogicLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bank_API.PresentationLayer.Controllers
{
    [Route("auth")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IAuthService authService;

        public UserController(IAuthService authService)
        {
            this.authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> CreateUser([FromBody] RegistrationRequest registrationRequest)
        {
            var token = await authService.CreateUser(registrationRequest);

            if (token != null)
            {
                return StatusCode(201, token);
            }

            return StatusCode(403, "User already exist");
        }
    }
}
