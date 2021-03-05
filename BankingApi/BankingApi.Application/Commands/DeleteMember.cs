using System;
using System.Threading;
using System.Threading.Tasks;
using BankingApi.Domain.Models;
using BankingApi.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BankingApi.Application.Commands
{
    public class DeleteMemberRequest : IRequest<CommandResponse<bool>>
    {
        public int MemberId { get; set; }
    }

    public class DeleteMemberRequestHandler : IRequestHandler<DeleteMemberRequest, CommandResponse<bool>>
    {
        private readonly ILogger<DeleteMemberRequestHandler> _logger;
        private readonly IMemberRepository _memberRepository;

        public DeleteMemberRequestHandler(ILogger<DeleteMemberRequestHandler> logger, IMemberRepository memberRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _memberRepository = memberRepository ?? throw new ArgumentNullException(nameof(memberRepository)); ;
        }

        public async Task<CommandResponse<bool>> Handle(DeleteMemberRequest request, CancellationToken cancellationToken = default)
        {
            var response = new CommandResponse<bool>();
            if (request.MemberId < 1)
            {
                var message = $"{nameof(request.MemberId)} is invalid.";
                _logger.LogWarning(message);
                response.isSuccess = false;
                response.Value = false;
                return response;
            }

            try
            {
                var member = await _memberRepository.FindByIdAsync(request.MemberId, cancellationToken);
                _memberRepository.Delete(member);
                await _memberRepository.SaveChangesAsync(cancellationToken);
                response.Value = true;
                response.isSuccess = true;
                return response;
            }
            catch (Exception ex)
            {
                var message = $"An error occurred deleting id {request.MemberId}";
                _logger.LogError(message, ex);
                response.isSuccess = false;
                response.Value = false;
                response.Message = message;
                return response;
            }
        }
    }
}
