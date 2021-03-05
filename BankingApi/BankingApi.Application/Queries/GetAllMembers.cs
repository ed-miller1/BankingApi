using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    public class GetAllMembersRequest : IRequest<CommandResponse<IEnumerable<Member>>>
    {
    }

    public class GetAllMembersRequestHandler : IRequestHandler<GetAllMembersRequest, CommandResponse<IEnumerable<Member>>>
    {
        private readonly IMemberRepository _memberRepository;
        private readonly ILogger<GetAllMembersRequestHandler> _logger;

        public GetAllMembersRequestHandler(IMemberRepository memberRepository, ILogger<GetAllMembersRequestHandler> logger)
        {
            _memberRepository = memberRepository ?? throw new ArgumentNullException(nameof(memberRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<CommandResponse<IEnumerable<Member>>> Handle(GetAllMembersRequest request, CancellationToken cancellationToken)
        {
            IEnumerable<Member> results = null;
            try
            {
                results = (await _memberRepository.ListAsync(cancellationToken)).Select(x => AutoMapper.MapTo<MemberEntity, Member>(x));
            }
            catch (Exception ex)
            {
                var message = $"An error occurred getting institutions.";
                _logger.LogError($"An error occurred getting institutions.", ex);
                return new CommandResponse<IEnumerable<Member>>
                {
                    isSuccess = false,
                    Message = message
                };
            }

            return new CommandResponse<IEnumerable<Member>>
            {
                isSuccess = true,
                Value = results
            };
        }
    }
}
