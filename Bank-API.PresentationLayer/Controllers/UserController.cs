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
        public async Task<IActionResult> CreateUser([FromBody] RegistrationRequest userRequest)
        {
            var token = await authService.CreateUser(userRequest);

            if (token == null)
            {
                return StatusCode(403, "User already exists");
            }

            return StatusCode(201, token);
        }
    }
}
