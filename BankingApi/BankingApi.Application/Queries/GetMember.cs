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

namespace BankingApi.Application.Queries
{
    public class GetMemberRequest : IRequest<CommandResponse<Member>>
    {
        public int MemberId { get; set; }
    }

    public class GetMemberRequestHandler : IRequestHandler<GetMemberRequest, CommandResponse<Member>>
    {
        private readonly ILogger<GetMemberRequestHandler> _logger;
        private readonly IMemberRepository _memberRepository;

        public GetMemberRequestHandler(ILogger<GetMemberRequestHandler> logger, IMemberRepository memberRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _memberRepository = memberRepository ?? throw new ArgumentNullException(nameof(memberRepository));
        }

        public async Task<CommandResponse<Member>> Handle(GetMemberRequest request, CancellationToken cancellationToken = default)
        {
            if (request.MemberId == 0)
            {
                var message = $"No MemberId provided.";
                _logger.LogWarning(message);
                return new CommandResponse<Member>
                {
                    isSuccess = false,
                    Message = message
                };
            }

            try
            {
                var memberEntity = await _memberRepository.FindByIdAsync(request.MemberId, cancellationToken);
                var member = AutoMapper.MapTo<MemberEntity, Member>(memberEntity);
                return new CommandResponse<Member>
                {
                    isSuccess = true,
                    Value = member
                };
            }
            catch (Exception ex)
            {
                var message = $"An error occurred GetMemberById request";
                _logger.LogError(message, ex);
                return new CommandResponse<Member>
                {
                    isSuccess = false,
                    Message = message
                };
            }
            
        }
    }
}
