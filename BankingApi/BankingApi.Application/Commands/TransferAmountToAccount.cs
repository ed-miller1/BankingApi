using System;
using System.Threading;
using System.Threading.Tasks;
using BankingApi.Application.DTOs;
using BankingApi.Domain.Models;
using BankingApi.Domain.Repositories;
using BankingApi.Infrastructure.Database;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BankingApi.Application.Commands
{
    public class TransferAmountToAccountRequest : IRequest<CommandResponse<bool>>
    {
        public int FromAccountId { get; set; }
        public int ToAccountId { get; set; }
        public double Amount { get; set; }
    }

    public class TransferAmountToAccountRequestHandler : IRequestHandler<TransferAmountToAccountRequest, CommandResponse<bool>>
    {
        private readonly ILogger<TransferAmountToAccountRequestHandler> _logger;
        private readonly IAccountRepository _accountRepository;
        private readonly ITransactionFactory _transactionFactory;

        public TransferAmountToAccountRequestHandler(ILogger<TransferAmountToAccountRequestHandler> logger, IAccountRepository accountRepository, ITransactionFactory transactionFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
            _transactionFactory = transactionFactory ?? throw new ArgumentNullException(nameof(transactionFactory));
        }

        public async Task<CommandResponse<bool>> Handle(TransferAmountToAccountRequest request, CancellationToken cancellationToken = default)
        {
            var fromAccount = await _accountRepository.FindByIdAsync(request.FromAccountId, cancellationToken);
            if (fromAccount == null)
                return GenerateFailedCommandResponse($"{nameof(fromAccount)} Id is invalid.");
            
            var toAccount = await _accountRepository.FindByIdAsync(request.ToAccountId, cancellationToken);
            if (toAccount == null)
                return GenerateFailedCommandResponse($"{nameof(toAccount)} Id is invalid.");

            //check if enough funds in "fromAccount"
            if ((fromAccount.Balance - request.Amount) < 0)
            {
                var message =
                    $"Not enough funds in Account {fromAccount.AccountId} for member {fromAccount.MemberId} to transfer. Requested to transfer: {request.Amount}. Current balance: {fromAccount.Balance}";
                var response = new CommandResponse<bool>();
                _logger.LogWarning(message);
                response.Message = message;
                response.Value = false;
                response.isSuccess = true;
                return response;
            }

            try
            {
                using var transaction = _transactionFactory.BeginTransaction();

                fromAccount.Balance -= request.Amount;
                _accountRepository.Update(fromAccount);

                toAccount.Balance += request.Amount;
                _accountRepository.Update(toAccount);

                await _accountRepository.SaveChangesAsync(cancellationToken);
                transaction.Commit();

                return new CommandResponse<bool>
                {
                    isSuccess = true,
                    Value = true
                };
            }
            catch (Exception ex)
            {
                var message = $"An error has occurred processing transfer transaction.";
                _logger.LogError(message, ex);
                return GenerateFailedCommandResponse(message);
            }
        }

        private CommandResponse<bool> GenerateFailedCommandResponse(string message)
        {
            var response = new CommandResponse<bool>();
            _logger.LogWarning(message);
            response.Message = message;
            response.Value = false;
            response.isSuccess = false;
            return response;
        }
    }
}
