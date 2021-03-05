using System;
using System.Threading;
using System.Threading.Tasks;
using BankingApi.Application.DTOs;
using BankingApi.Domain.Entities;
using BankingApi.Domain.Models;
using BankingApi.Domain.Repositories;
using BankingApi.Infrastructure.Database;
using BankingApi.Utilities.AutoMapping;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BankingApi.Application.Commands
{
    public class AddMemberRequest : IRequest<CommandResponse<Member>> 
    {
        public Member Member { get; set; }
    }

    public class AddMemberRequestHandler : IRequestHandler<AddMemberRequest, CommandResponse<Member>>
    {
        private readonly ILogger<AddMemberRequestHandler> _logger;
        private readonly IMemberRepository _memberRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly ITransactionFactory _transactionFactory;

        public AddMemberRequestHandler(ILogger<AddMemberRequestHandler> logger, IMemberRepository memberRepository, ITransactionFactory transactionFactory, IAccountRepository accountRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _memberRepository = memberRepository ?? throw new ArgumentNullException(nameof(memberRepository));
            _transactionFactory = transactionFactory ?? throw new ArgumentNullException(nameof(transactionFactory));
            _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        }


        public async Task<CommandResponse<Member>> Handle(AddMemberRequest request, CancellationToken cancellationToken)
        {
            if (request.Member == null)
            {
                var message = $"Request is null";
                _logger.LogWarning($"Request is null");
                return new CommandResponse<Member>
                {
                    isSuccess = false,
                    Message = message
                };
            }

            var validationMessage = string.Empty;
            var response = new CommandResponse<Member>{isSuccess = false};
            if (string.IsNullOrWhiteSpace(request.Member.GivenName))
                validationMessage = $"{nameof(request.Member.GivenName)} is required;";
            if(string.IsNullOrWhiteSpace(request.Member.Surname))
                validationMessage = $"{validationMessage} {nameof(request.Member.Surname)} is required;";
            if (request.Member.InstitutionId < 1)
                validationMessage = $"{validationMessage} {nameof(request.Member.InstitutionId)} is required;";
            if (!string.IsNullOrWhiteSpace(validationMessage))
            {
                _logger.LogWarning(validationMessage);
                response.Message = validationMessage;
                return response;
            }

            //i would think that some sort of check to make sure that there are no duplicate members but we would need something like SSN here for that
            //so we're just assuming that every new member is unique.

            try
            {
                using var transaction = _transactionFactory.BeginTransaction();

                var member = await _memberRepository.AddAsync(AutoMapper.MapTo<Member, MemberEntity>(request.Member),
                    cancellationToken);
                await _memberRepository.SaveChangesAsync(cancellationToken);

                //create an account for new member with balance of $0
                await _accountRepository.AddAsync(new AccountEntity {Balance = 0.0, MemberId = member.MemberId},
                    cancellationToken);

                await _memberRepository.SaveChangesAsync(cancellationToken);
                transaction.Commit();

                response.isSuccess = true;
                response.Value = AutoMapper.MapTo<MemberEntity, Member>(member);
                return response;
            }
            catch (Exception ex)
            {
                var errorMessage = "An error occurred creating a new member.";
                _logger.LogError(errorMessage, ex);
                response.Message = errorMessage;
                return response;
            }
        }
    }
}
