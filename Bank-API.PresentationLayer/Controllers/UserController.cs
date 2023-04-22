namespace Bank_API.PresentationLayer.Controllers;

[Route("user")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService userService;
    private readonly ISessionProvider sessionProvider;

    public UserController(IUserService userService, 
                          ISessionProvider sessionProvider)
    {
        this.userService = userService;
        this.sessionProvider = sessionProvider;
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
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> GetPersonalInfo(CancellationToken cancellationToken)
    {            
        var userId = sessionProvider.GetUserId();
        var response = await userService.GetCurrentUser(userId, cancellationToken);
        if (response.Result != null)
        {
            return StatusCode(200, new
            {
                user = response.Result,
            });
        }

        return StatusCode(401, new { error = response.ErrorMessage });
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
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> UpdateUser([FromBody] UserUpdateDTO updateUserRequest)
    {
        var userId = sessionProvider.GetUserId();
        await userService.UpdateUser(updateUserRequest, userId);
        return StatusCode(201);
    }
}
