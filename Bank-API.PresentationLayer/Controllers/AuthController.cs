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

        /// <summary>
        /// Create a new user.
        /// </summary>
        /// <returns>A newly created user token</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Bank API
        ///     {
        ///        "email": "user@example.com",
        ///        "phone": "+380505553535"(in string format),
        ///        "password": "string",
        ///        "firstName": "String",
        ///        "lastName": "String",
        ///        "birthDate": "2022-11-10"
        ///     }
        /// </remarks>
        /// <response code="201">Returns the user token if the user was created</response>
        /// <response code="403">If user already exists</response>
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

        /// <summary>
        /// Login user.
        /// </summary>
        /// <returns>A newly created user token</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Bank API
        ///     {
        ///        "login": "+380505553535"(in string format),
        ///        "password": "string",
        ///     }
        /// </remarks>
        /// <response code="201">Returns the token if the user is logged in</response>
        /// <response code="403">If the data is incorrectly entered</response>
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
