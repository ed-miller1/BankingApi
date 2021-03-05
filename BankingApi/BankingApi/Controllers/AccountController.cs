using System;
using System.Threading;
using System.Threading.Tasks;
using BankingApi.Application.Commands;
using BankingApi.Application.DTOs;
using BankingApi.Utilities.AutoMapping;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BankingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IMediator _mediator;

        public AccountController(ILogger<AccountController> logger, IMediator mediator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <summary>
        /// Update an account's balance.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAccountBalance([FromBody] Application.DTOs.UpdateAccountBalanceRequest request, int id, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid) return BadRequest();

            _logger.LogInformation($"Updating account balance for {id}.");
            var response = await _mediator.Send(new Application.Commands.UpdateAccountBalanceRequest
            {
                AccountId = id,
                newBalance = request.NewBalance
            }, cancellationToken);

            if (!response.isSuccess)
                return StatusCode(StatusCodes.Status500InternalServerError);

            return Accepted();
        }

        /// <summary>
        /// Transfer money from one account to another.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("transfer")]
        public async Task<ActionResult> TransferAmountToAccount([FromBody] TransferRequest request, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid) return BadRequest();

            _logger.LogInformation($"Transferring {request.Amount} from account {request.FromAccountId} to account {request.ToAccountId}.");
            var response = await _mediator.Send(AutoMapper.MapTo<TransferRequest, TransferAmountToAccountRequest>(request), cancellationToken);

            if (response.isSuccess && !response.Value)
                return BadRequest(response.Message);

            if (!response.isSuccess)
                return StatusCode(StatusCodes.Status500InternalServerError);

            return Accepted();
        }
    }
}
