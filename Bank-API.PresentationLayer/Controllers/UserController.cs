using Bank_API.BusinessLogicLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Bank_API.PresentationLayer.Controllers
{
    [Route("user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet("personalInfo")]
        public async Task<IActionResult> GetPersonalInfo(int userId)
        {
            var user = await userService.GetUserById(userId);

            if (user != null) 
            {
                return StatusCode(200, new
                {
                    email = user.Email,
                    phone = user.Phone,
                    firstName = user.FirstName,
                    lastName = user.LastName,
                    birthDate = user.BirthDate
                });
            }

            return StatusCode(404, "User not found");
        }
    }
}
