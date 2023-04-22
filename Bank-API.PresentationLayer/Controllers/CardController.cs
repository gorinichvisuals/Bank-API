namespace Bank_API.PresentationLayer.Controllers;

[Route("cards")]
[ApiController]
public class CardController : ControllerBase
{
    private readonly ICardService cardService;
    private readonly ITransactionService transactionService;
    private readonly ISessionProvider sesstionProvider;

    public CardController(ICardService cardService,
                          ITransactionService transactionService,
                          ISessionProvider sesstionProvider)
    {
        this.cardService = cardService;
        this.transactionService = transactionService;
        this.sesstionProvider = sesstionProvider;
    }

    /// <summary>
    /// Create a new user card.
    /// </summary>
    /// <returns>Return card id</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /Bank API
    ///     {
    ///        "currency": "string"(UAH/USD/EUR),
    ///     }
    /// </remarks>
    /// <response code="201">Returns the newly created item by id</response>
    /// <response code="400">If no card has been created</response>
    [HttpPost]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> CreateCard([FromBody] CardCreateDTO request)
    {
        var userId = sesstionProvider.GetUserId();
        var response = await cardService.CreateCard(request, userId);

        if (response != null)
        {
            return StatusCode(201, new { id = response.Result });
        }

        return StatusCode(400, new { error = response!.ErrorMessage });
    }

    /// <summary>
    /// Get array of non closed user cards.
    /// </summary>
    /// <remarks>
    /// Sample responce:
    ///
    ///     GET/List of cards/Bank API
    ///     "cards": [
    ///         {
    ///             "id": "int",
    ///             "number": "number",
    ///             "exp":"string"(format'MM/YY'),
    ///             "cvv": "number",
    ///             "currency": "string",
    ///             "balance":"number",
    ///             "status":"string",
    ///         }
    ///     ]
    /// </remarks>
    /// <response code="200">Returns information about user cards</response>
    [HttpGet]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> GetCardList(CancellationToken cancellationToken)
    {
        var userId = sesstionProvider.GetUserId();
        var cardArray = await cardService.GetUserCards(userId, cancellationToken);

        return StatusCode(200, new
        {
            cards = cardArray
        });
    }

    /// <summary>
    /// Change card status.
    /// </summary>
    /// <returns>None</returns>
    /// <remarks>
    /// Sample responce:
    ///
    ///     PUT /Bank API()
    ///     {
    ///         "freezeCard": (true/false value),
    ///         "id": "number of card id"
    ///     }
    /// </remarks>
    /// <response code="201">If card status change to freeze/unfreeze.</response>
    /// <response code="401">If card status already have this status.</response>
    /// <response code="404">If card not found or unavailable.</response>
    [HttpPut]
    [Route("{cardId:int}/status")]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> ChangeCardStatus([FromBody] CardStatusDTO request, int cardId)
    {
        var response = await cardService.ChangeCardStatus(request.FreezeCard, cardId);

        if(!response.Result)
        {
            return StatusCode(400, new { error = response.ErrorMessage });
        }

        return StatusCode(201, "Created");
    }

    /// <summary>
    /// Transfer from card to card.
    /// </summary>
    /// <remarks>
    /// Sample response:
    ///
    ///     POST /Bank API
    ///     {
    ///         "id": "number"(transaction id)
    ///     }
    /// </remarks>
    /// <response code="201">If transfer succsessfull.</response>
    /// <response code="404">If card not found or unavailable.</response>
    /// <response code="401">If any other problem.</response>
    [HttpPost]
    [Route("{transactionId:int}/p2p")]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> TransferCardToCard([FromBody] CardTransferDTO request, int transactionId)
    {
        var response = await transactionService.TransferCardToCard(request, transactionId);

        if (response.Result == default)
        {
            return StatusCode(401, new 
            { 
                error = response!.ErrorMessage 
            });
        }

        return StatusCode(201, new
        {
            id = response.Result
        });
    }
}