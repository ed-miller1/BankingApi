using System;
using System.Collections.Generic;
using System.Linq;
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
    public class GetAllInstitutionsRequest : IRequest<CommandResponse<IEnumerable<Institution>>>
    {
    }

    public class GetAllInstitutionRequestHandler : IRequestHandler<GetAllInstitutionsRequest, CommandResponse<IEnumerable<Institution>>>
    {
        private readonly IInstitutionRepository _institutionRepository;
        private readonly ILogger<GetAllInstitutionRequestHandler> _logger;

        public GetAllInstitutionRequestHandler(IInstitutionRepository institutionRepository, ILogger<GetAllInstitutionRequestHandler> logger)
        {
            _institutionRepository = institutionRepository ?? throw new ArgumentNullException(nameof(institutionRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<CommandResponse<IEnumerable<Institution>>> Handle(GetAllInstitutionsRequest request, CancellationToken cancellationToken = default)
        {
            IEnumerable<Institution> results = null;
            try
            {
                results = (await _institutionRepository.ListAsync(cancellationToken)).Select(x => AutoMapper.MapTo<InstitutionEntity, Institution>(x));
            }
            catch (Exception ex)
            {
                var message = $"An error occurred getting institutions.";
                _logger.LogError($"An error occurred getting institutions.", ex);
                return new CommandResponse<IEnumerable<Institution>>
                {
                    isSuccess = false,
                    Message = message
                };
            }

            return new CommandResponse<IEnumerable<Institution>>
            {
                isSuccess = true,
                Value = results
            };
        }
    }
}
