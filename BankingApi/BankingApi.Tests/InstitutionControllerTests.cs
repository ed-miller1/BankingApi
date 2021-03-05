using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BankingApi.Application.Commands;
using BankingApi.Application.DTOs;
using BankingApi.Application.Queries;
using BankingApi.Controllers;
using BankingApi.Domain.Entities;
using BankingApi.Domain.Models;
using BankingApi.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace BankingApi
{
    public class InstitutionTests
    {
        [Fact]
        public async Task Controller_Get_Success()
        {
            var returnList = new List<Institution>()
            {
                new Institution
                {
                    InstitutionName = "Test",
                    InstitutionId = 12456
                }
            };
            var commandResponse = new CommandResponse<IEnumerable<Institution>>
            {
                Value = returnList,
                isSuccess = true
            };
            Mock<IMediator> mockMediator = new Mock<IMediator>();
            Mock<ILogger<InstitutionController>> mockLogger = new Mock<ILogger<InstitutionController>>();
            mockMediator.Setup(x => x.Send(It.IsAny<GetAllInstitutionsRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(commandResponse);
            var controller = new InstitutionController(mockLogger.Object, mockMediator.Object);
            var result = await controller.Get(CancellationToken.None);
            Assert.IsType<ActionResult<IEnumerable<Institution>>>(result);
        }

        [Fact]
        public async Task Controller_Get_Fail()
        {
            Mock<IMediator> mockMediator = new Mock<IMediator>();
            Mock<ILogger<InstitutionController>> mockLogger = new Mock<ILogger<InstitutionController>>();
            mockMediator.Setup(x => x.Send(It.IsAny<GetAllInstitutionsRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(new CommandResponse<IEnumerable<Institution>>{isSuccess = false});
            var controller = new InstitutionController(mockLogger.Object, mockMediator.Object);
            var result = await controller.Get(CancellationToken.None);
            Assert.IsType<ActionResult<IEnumerable<Institution>>>(result);
        }

        [Fact]
        public async Task Controller_Post_Success()
        {
            Mock<IMediator> mockMediator = new Mock<IMediator>();
            Mock<ILogger<InstitutionController>> mockLogger = new Mock<ILogger<InstitutionController>>();
            mockMediator.Setup(x => x.Send(It.IsAny<CreateInstitutionRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(new CommandResponse<Institution> { isSuccess = true, Value = new Institution()});
            var controller = new InstitutionController(mockLogger.Object, mockMediator.Object);
            var result = await controller.Post(new Institution { InstitutionId = 123456, InstitutionName = "test" }, CancellationToken.None);
            Assert.IsType<ObjectResult>(result);
        }

        [Fact]
        public async Task Controller_Post_Fail()
        {
            Mock<IMediator> mockMediator = new Mock<IMediator>();
            Mock<ILogger<InstitutionController>> mockLogger = new Mock<ILogger<InstitutionController>>();
            mockMediator.Setup(x => x.Send(It.IsAny<CreateInstitutionRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(new CommandResponse<Institution> { isSuccess = false, Value = new Institution() });
            var controller = new InstitutionController(mockLogger.Object, mockMediator.Object);
            var result = await controller.Post(new Institution{InstitutionId = 123456, InstitutionName = "test"},CancellationToken.None);
            Assert.IsType<StatusCodeResult>(result);
        }

        [Fact]
        public async Task Handler_GetAll_Success()
        {
            Mock<ILogger<GetAllInstitutionRequestHandler>> mockLogger = new Mock<ILogger<GetAllInstitutionRequestHandler>>();
            Mock<IInstitutionRepository> mockRepo = new Mock<IInstitutionRepository>();
            mockRepo.Setup(x => x.ListAsync(It.IsAny<CancellationToken>())).ReturnsAsync(new List<InstitutionEntity>()
                {new InstitutionEntity {InstitutionName = "Test", InstitutionId = 123456}});
            var handler = new GetAllInstitutionRequestHandler(mockRepo.Object,mockLogger.Object);
            var result = await handler.Handle(new GetAllInstitutionsRequest(), CancellationToken.None);
            Assert.True(result.isSuccess);
        }

        [Fact]
        public async Task Handler_GetAll_Fail()
        {
            Mock<ILogger<GetAllInstitutionRequestHandler>> mockLogger = new Mock<ILogger<GetAllInstitutionRequestHandler>>();
            Mock<IInstitutionRepository> mockRepo = new Mock<IInstitutionRepository>();
            mockRepo.Setup(x => x.ListAsync(CancellationToken.None)).Throws<Exception>();
            var handler = new GetAllInstitutionRequestHandler(mockRepo.Object, mockLogger.Object);
            var result = await handler.Handle(new GetAllInstitutionsRequest(), CancellationToken.None);
            Assert.False(result.isSuccess);

        }

        [Fact]
        public async Task Handler_Create_Success()
        {
            var entity = new InstitutionEntity{InstitutionName = "test", InstitutionId = 123456};
            Mock<ILogger<CreateInstitutionRequestHandler>> mockLogger = new Mock<ILogger<CreateInstitutionRequestHandler>>();
            Mock<IInstitutionRepository> mockRepo = new Mock<IInstitutionRepository>();
            mockRepo.Setup(x => x.AddAsync(entity, CancellationToken.None)).ReturnsAsync(entity);
            var handler = new CreateInstitutionRequestHandler(mockLogger.Object, mockRepo.Object);
            var result = await handler.Handle(new CreateInstitutionRequest{Institution = new Institution{InstitutionId = 123456, InstitutionName = "test"}}, CancellationToken.None);
            Assert.True(result.isSuccess);

        }

        [Fact]
        public async Task Handler_Create_Fail()
        {
            var entity = new InstitutionEntity { InstitutionName = "test", InstitutionId = 123456 };
            Mock<ILogger<CreateInstitutionRequestHandler>> mockLogger = new Mock<ILogger<CreateInstitutionRequestHandler>>();
            Mock<IInstitutionRepository> mockRepo = new Mock<IInstitutionRepository>();
            mockRepo.Setup(x => x.AddAsync(It.IsAny<InstitutionEntity>(), It.IsAny<CancellationToken>())).Throws<Exception>();
            var handler = new CreateInstitutionRequestHandler(mockLogger.Object, mockRepo.Object);
            var result = await handler.Handle(new CreateInstitutionRequest { Institution = new Institution { InstitutionId = 123456, InstitutionName = "test" } }, CancellationToken.None);
            Assert.False(result.isSuccess);
        }

    }
}
