namespace Bank_API.PresentationLayer.Controllers;

[Route("transaction")]
[ApiController]
public class TransactionController : ControllerBase
{
    private readonly ITransactionService transactionService;

    public TransactionController(ITransactionService transactionService)
    {
        this.transactionService = transactionService;
    }

    /// <summary>
    /// Get transaction details by id.
    /// </summary>
    /// <remarks>
    /// Sample responce:
    ///
    ///     GET /Bank API
    ///     {
    ///         "amount": "number"
    ///         "message": "string",
    ///         "type":"string",
    ///         "peer": "string",
    ///         "resultingBalance": "number",
    ///         "date": "date"
    ///     }
    /// </remarks>
    /// <response code="200">Returns information details about transaction</response>
    /// <response code="404">If transaction not found or unavailable</response>
    [HttpGet]
    [Route("{transactionId:int}")]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> GetTransactionDetails(int transactionId, CancellationToken cancellationToken)
    {
        var response = await transactionService.GetTransactionById(transactionId, cancellationToken);

        if(response.Result != null)
        {
            return StatusCode(200,new
            {
                transaction = response.Result,
            });
        }

        return StatusCode(404, new { error =  response.ErrorMessage});
    }
}