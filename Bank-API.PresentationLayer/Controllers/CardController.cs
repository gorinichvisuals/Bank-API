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

        public CardController(ICardService cardService)
        {
            this.cardService = cardService;
        }

        /// <summary>
        /// Create a new user card.
        /// </summary>
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
        /// Get list of non closed user cards.
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
        /// <remarks>
        /// Sample responce:
        ///
        ///     PUT /Bank API()
        ///     {
        ///     }
        /// </remarks>
        /// <response code="201">If card status change to freeze.</response>
        /// <response code="401">If card status already frozen.</response>
        /// <response code="404">If card status change to freeze.</response>
        [HttpPut]
        [Route("{id:int}/status")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> ChangeCardStatus(int id)
        {
            var result = await cardService.ChangeCardStatus(id);

            if (result == true)
            {
                return StatusCode(201, "Created");
            }
            if(result == false)
            {
                return StatusCode(401, new { error = "Card Is already frozen/unfrozen" });
            }

            return StatusCode(404, new {error = "Card not found or unavailable" });
        }
    }
}
