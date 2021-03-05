using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BankingApi.Application.Commands;
using BankingApi.Application.DTOs;
using BankingApi.Application.Queries;
using Microsoft.Extensions.Logging;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BankingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstitutionController : ControllerBase
    {
        private readonly ILogger<InstitutionController> _logger;
        private readonly IMediator _mediator;

        public InstitutionController(ILogger<InstitutionController> logger, IMediator mediator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <summary>
        /// Get all Institutions.
        /// </summary>
        /// <returns></returns>
        // GET: api/<InstitutionController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Institution>>> Get(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation($"Getting all institutions.");
            var institutions = await _mediator.Send(new GetAllInstitutionsRequest(), cancellationToken);
            if(institutions.isSuccess)
                return Ok(institutions.Value);

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        /// <summary>
        /// Add a new institution.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        // POST api/<InstitutionController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Institution value, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning($"Invalid Model State: {ModelState.ErrorCount}.");
                return BadRequest();
            }

            var response = await _mediator.Send(new CreateInstitutionRequest
            {
                Institution = value
            }, cancellationToken);

            if (!response.isSuccess)
            {
                _logger.LogWarning($"Error encountered add institution {value.InstitutionName}.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return StatusCode(StatusCodes.Status201Created, response.Value);
        }
    }
}
