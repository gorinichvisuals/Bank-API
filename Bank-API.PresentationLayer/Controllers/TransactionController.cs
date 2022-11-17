using Bank_API.BusinessLogicLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bank_API.PresentationLayer.Controllers
{
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
        [Route("{id:int}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetTransactionDetails(int id)
        {
            var transactionResponce = await transactionService.GetTransactionById(id);

            if(transactionResponce != null)
            {
                return StatusCode(200,new
                {
                    amount = transactionResponce.Amount,
                    message = transactionResponce.Message,
                    type = transactionResponce.Type,
                    peer = transactionResponce.Peer,
                    resultingBalance = transactionResponce.ResultingBalance,
                    date = transactionResponce.Date
                });
            }

            return StatusCode(404, new { error = "Transaction not found or unavaulable" });
        }
    }
}
