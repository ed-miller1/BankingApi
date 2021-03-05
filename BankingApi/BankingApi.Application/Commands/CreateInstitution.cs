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
    public class CreateInstitutionRequest : IRequest<CommandResponse<Institution>>
    {
        public Institution Institution { get; set; }
    }

    public class CreateInstitutionRequestHandler : IRequestHandler<CreateInstitutionRequest, CommandResponse<Institution>>
    {
        private readonly ILogger<CreateInstitutionRequestHandler> _logger;
        private readonly IInstitutionRepository _institutionRepository;

        public CreateInstitutionRequestHandler(ILogger<CreateInstitutionRequestHandler> logger, IInstitutionRepository institutionRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _institutionRepository = institutionRepository ?? throw new ArgumentNullException(nameof(institutionRepository));
        }


        public async Task<CommandResponse<Institution>> Handle(CreateInstitutionRequest request, CancellationToken cancellationToken)
        {
            if (request.Institution == null)
            {
                var message = $"Request Value is null";
                _logger.LogWarning(message);
                return new CommandResponse<Institution>
                {
                    isSuccess = false,
                    Message = message
                };
            }

            var validationMessage = string.Empty;
            var response = new CommandResponse<Institution> { isSuccess = false };
            if (string.IsNullOrWhiteSpace(request.Institution.InstitutionName))
                validationMessage = $"{nameof(request.Institution.InstitutionName)} is required;";
            if (!string.IsNullOrWhiteSpace(validationMessage))
            {
                _logger.LogWarning(validationMessage);
                response.Message = validationMessage;
                return response;
            }

            try
            {
                var institution = await _institutionRepository.AddAsync(AutoMapper.MapTo<Institution, InstitutionEntity>(request.Institution),
                    cancellationToken);
                await _institutionRepository.SaveChangesAsync(cancellationToken);

                response.isSuccess = true;
                response.Value = AutoMapper.MapTo<InstitutionEntity, Institution>(institution);
                return response;
            }
            catch (Exception ex)
            {
                var errorMessage = "An error occurred creating a new institution.";
                _logger.LogError(errorMessage, ex);
                response.Message = errorMessage;
                return response;
            }
        }
    }
}
