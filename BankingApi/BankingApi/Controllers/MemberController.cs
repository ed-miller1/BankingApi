using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BankingApi.Application.Commands;
using BankingApi.Application.DTOs;
using BankingApi.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BankingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly ILogger<MemberController> _logger;
        private readonly IMediator _mediator;

        public MemberController(ILogger<MemberController> logger, IMediator mediator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <summary>
        /// Get all members in system.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Member>>> GetAllMembers(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation($"Getting all members.");
            var response = await _mediator.Send(new GetAllMembersRequest(), cancellationToken);
            if (!response.isSuccess)
            {
                _logger.LogWarning(response.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok(response.Value);
        }

        /// <summary>
        /// Get a member's information.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Member>> GetMember(int id, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation($"Getting member {id}.");

            if (!ModelState.IsValid) return BadRequest();

            var response = await _mediator.Send(new GetMemberRequest
            {
                MemberId = id
            }, cancellationToken);

            if (!response.isSuccess)
            {
                _logger.LogWarning(response.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok(response.Value);
        }

        /// <summary>
        /// Add a new member.
        /// </summary>
        /// <param name="member"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Member>> AddMember([FromBody] Member member, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid) return BadRequest();

            _logger.LogInformation($"Creating new member: {member.GivenName} {member.Surname}");

            var response = await _mediator.Send(new AddMemberRequest
            {
                Member = member
            }, cancellationToken);

            if (!response.isSuccess)
            {
                _logger.LogWarning(response.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return StatusCode(StatusCodes.Status201Created, response.Value);
        }

        /// <summary>
        /// Update an existing member.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="member"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateMember(int id, [FromBody] Member member, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid) return BadRequest();

            var response = await _mediator.Send(new UpdateMemberRequest
            {
                Member = member
            }, cancellationToken);

            if (!response.isSuccess)
            {
                _logger.LogWarning(response.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Accepted();
        }

        /// <summary>
        /// Remove a member.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMember(int id, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid) return BadRequest();

            _logger.LogInformation($"Deleting member {id}.");

            var response = await _mediator.Send(new DeleteMemberRequest
            {
                MemberId = id
            }, cancellationToken);

            if (!response.isSuccess)
            {
                _logger.LogWarning(response.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Accepted();
        }
    }
}
