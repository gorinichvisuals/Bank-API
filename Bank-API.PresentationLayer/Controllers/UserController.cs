using Bank_API.BusinessLogicLayer.Interfaces;
using Bank_API.DataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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

        [HttpGet]
        [Route("personalInfo")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetPersonalInfo()
        {            
            var currentUser = await userService.GetUser();

            if (currentUser != null)
            {
                return StatusCode(200, new
                {
                    email = currentUser.Email,
                    phone = currentUser.Phone,
                    firstName = currentUser.FirstName,
                    lastName = currentUser.LastName,
                    birthDate = currentUser.BirthDate
                });
            }

            return StatusCode(401, new { error = "Unauthorize" });
        }
    }
}
