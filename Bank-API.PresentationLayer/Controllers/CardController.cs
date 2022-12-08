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
        [Route("{id:int}/p2p")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> TransferCardToCard([FromBody] CardTransferRequest request, int? id)
        {
            var response = await transactionService.TransferCardToCard(request, (int)id!);

            if (id == null)
            {
                return StatusCode(404, "Card not found or unavailable");
            }

            if (response?.Result == null)
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
}