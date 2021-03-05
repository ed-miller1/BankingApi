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
    public class UpdateMemberRequest : IRequest<CommandResponse<bool>>
    {
        public Member Member { get; set; }
    }

    public class UpdateMemberRequestHandler : IRequestHandler<UpdateMemberRequest, CommandResponse<bool>>
    {
        private readonly ILogger<UpdateMemberRequestHandler> _logger;
        private readonly IMemberRepository _memberRepository;

        public UpdateMemberRequestHandler(ILogger<UpdateMemberRequestHandler> logger, IMemberRepository memberRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _memberRepository = memberRepository ?? throw new ArgumentNullException(nameof(memberRepository));
        }

        public async Task<CommandResponse<bool>> Handle(UpdateMemberRequest request, CancellationToken cancellationToken = default)
        {
            var memberEntity = AutoMapper.MapTo<Member, MemberEntity>(request.Member);
            try
            {
                _memberRepository.Update(memberEntity);
                await _memberRepository.SaveChangesAsync(cancellationToken);
                return new CommandResponse<bool>
                {
                    isSuccess = true,
                    Value = true
                };
            }
            catch (Exception ex)
            {
                var message = $"An error occurred processing update for member {request.Member.MemberId}.";
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
