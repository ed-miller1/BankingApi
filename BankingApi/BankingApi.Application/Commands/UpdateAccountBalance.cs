using System;
using System.Threading;
using System.Threading.Tasks;
using BankingApi.Application.DTOs;
using BankingApi.Domain.Entities;
using BankingApi.Domain.Models;
using BankingApi.Domain.Repositories;
using BankingApi.Utilities.AutoMapping;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BankingApi.Application.Commands
{
    public class UpdateAccountBalanceRequest : IRequest<CommandResponse<bool>>
    {
        public int AccountId { get; set; }
        public double newBalance { get; set; }
    }

    public class UpdateAccountBalanceRequestHandler : IRequestHandler<UpdateAccountBalanceRequest, CommandResponse<bool>>
    {
        private readonly ILogger<UpdateAccountBalanceRequestHandler> _logger;
        private readonly IAccountRepository _accountRepository;

        public UpdateAccountBalanceRequestHandler(ILogger<UpdateAccountBalanceRequestHandler> logger, IAccountRepository accountRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        }

        public async Task<CommandResponse<bool>> Handle(UpdateAccountBalanceRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                var accountEntity = await _accountRepository.FindByIdAsync(request.AccountId, cancellationToken);
                accountEntity.Balance = request.newBalance;

                _accountRepository.Update(accountEntity);
                await _accountRepository.SaveChangesAsync(cancellationToken);
                return new CommandResponse<bool>
                {
                    isSuccess = true,
                    Value = true
                };
            }
            catch (Exception ex)
            {
                var message = $"An error occurred processing update for account {request.AccountId}.";
                _logger.LogError(message, ex);
                return new CommandResponse<bool>
                {
                    isSuccess = false,
                    Message = message,
                    Value = false
                };
            }
        }
    }
}
