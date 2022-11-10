﻿using Bank_API.BusinessLogicLayer.Interfaces;
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
    }
}
