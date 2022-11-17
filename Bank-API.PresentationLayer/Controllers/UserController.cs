using Bank_API.BusinessLogicLayer.Interfaces;
using Bank_API.BusinessLogicLayer.Models;
using Bank_API.DataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Bank_API.PresentationLayer.Controllers
{
    [Route("user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IAuthService authService;

        public UserController(IUserService userService, 
                              IAuthService authService)
        {
            this.userService = userService;
            this.authService = authService;
        }

        /// <summary>
        /// Get personal info for user.
        /// </summary>
        /// <remarks>
        /// Sample responce:
        ///
        ///     GET /Bank API
        ///     {
        ///         "email": "user@example.com"
        ///         "phone": "+380505553535"(in string format),
        ///         "FirstName":"String",
        ///         "LastName": "String",
        ///         "BirthDate": "2020-10-19"
        ///     }
        /// </remarks>
        /// <response code="200">Returns information about the user</response>
        /// <response code="401">If the user is not authorized</response>
        [HttpGet]
        [Route("personalInfo")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetPersonalInfo()
        {            
            var currentUser = await authService.GetUser();

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

        /// <summary>
        /// Update personal info for user.
        /// </summary>
        /// <remarks>
        /// Sample responce:
        ///
        ///     PUT /Bank API(All parameters are optional so if parameter is not provided then it should not be changed in the database)
        ///     {
        ///         "phone": "+380505553535"(in string format),
        ///         "FirstName":"String",
        ///         "LastName": "String",
        ///         "BirthDate": "2020-10-19",
        ///         "Password": "string"
        ///     }
        /// </remarks>
        /// <response code="201">The request has succeeded and has led to the update of a resource.</response>
        [HttpPut]
        [Route("personalInfo")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> UpdateUser([FromBody] UserUpdateRequest updateUserRequest)
        {
            await userService.UpdateUser(updateUserRequest);
            return StatusCode(201);
        }
    }
}
