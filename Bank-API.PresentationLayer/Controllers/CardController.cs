using Bank_API.BusinessLogicLayer.Interfaces;
using Bank_API.BusinessLogicLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bank_API.PresentationLayer.Controllers
{
    [Route("cards")]
    [ApiController]
    public class CardController : ControllerBase
    {
        private readonly ICardService cardService;
        private readonly ITransactionService transactionService;

        public CardController(ICardService cardService,
                              ITransactionService transactionService)
        {
            this.cardService = cardService;
            this.transactionService = transactionService;
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
        [Authorize(Roles = "User")]
        public async Task<IActionResult> CreateCard([FromBody] CardCreateRequest request)
        {
            var cardId = await cardService.CreateCard(request);

            if (cardId != null)
            {
                return StatusCode(201, new { id = cardId });
            }

            return StatusCode(400, new { error = "Card not created. You can have only 2 cards of same currency." });
        }

        /// <summary>
        /// Get array of non closed user cards.
        /// </summary>
        /// <remarks>
        /// Sample response:
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
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetCardList()
        {
            var cardArray = await cardService.GetUserCards();

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
        /// Sample response:
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
        [Route("{id:int}/status")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> ChangeCardStatus([FromBody] CardStatusRequest request, int id)
        {
            var result = await cardService.ChangeCardStatus(request.FreezeCard, id);

            if(result == null)
            {
                return StatusCode(404, new { error = "Card not found or unavailable" });
            }

            if (result != true)
            {
                return StatusCode(401, new { error = "Card Is already frozen/unfrozen" });
            }

            return StatusCode(201, "Created");
        }

        /// <summary>
        /// Get array of all card transactions.
        /// </summary>
        /// <remarks>
        /// Sample response:
        ///
        ///     GET/List of transactions/Bank API
        ///     "transactions": [
        ///         {
        ///         "id": "number"
        ///         "amount": "number"
        ///         "message": "string",
        ///         "type":"string",
        ///         "peer": "string",
        ///         "date": "date"
        ///         }
        ///     ]
        /// </remarks>
        /// <response code="200">Returns information about card transactions</response>
        /// <response code="404">If cardn not found or unavailable</response>
        [HttpGet]
        [Route("{id:int}/transactions")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetTransactionsList([FromQuery] TransactionQueryParams requestParams, int cardId)
        {
            var transactions = await transactionService.GetTransactionsList(requestParams, cardId);

            if(transactions != null)
            {
                return StatusCode(200, new
                {
                    transactions = transactions,
                    total = transactions.Length
                });
            }

            return StatusCode(404, new { error = "Card not found or unavailable" });
        }
    }
}
