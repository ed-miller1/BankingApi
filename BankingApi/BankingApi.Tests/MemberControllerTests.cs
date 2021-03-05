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
using BankingApi.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace BankingApi
{
    public class MemberControllerTests
    {
        [Fact]
        public async Task Test_GetAllMembers_Success()
        {
            Mock<IMediator> mockMediator = new Mock<IMediator>();
            Mock<ILogger<MemberController>> mockLogger = new Mock<ILogger<MemberController>>();
            mockMediator.Setup(x => x.Send(It.IsAny<GetAllMembersRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(
                new CommandResponse<IEnumerable<Member>>
                {
                    isSuccess = true,
                    Value = new List<Member>()
                        {new Member {GivenName = "test", Surname = "test", InstitutionId = 12456, MemberId = 123456}}
                });
            var controller = new MemberController(mockLogger.Object, mockMediator.Object);
            var result = await controller.GetAllMembers(CancellationToken.None);
            Assert.IsType<ActionResult<IEnumerable<Member>>>(result);
        }

        [Fact]
        public async Task Test_GetAllMember_Fail()
        {
            Mock<IMediator> mockMediator = new Mock<IMediator>();
            Mock<ILogger<MemberController>> mockLogger = new Mock<ILogger<MemberController>>();
            mockMediator.Setup(x => x.Send(It.IsAny<GetAllMembersRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(
                new CommandResponse<IEnumerable<Member>>
                {
                    isSuccess = true,
                    Value = new List<Member>()
                        {new Member {GivenName = "test", Surname = "test", InstitutionId = 12456, MemberId = 123456}}
                });
            var controller = new MemberController(mockLogger.Object, mockMediator.Object);
            var result = await controller.GetAllMembers(CancellationToken.None);
            Assert.IsType<ActionResult<IEnumerable<Member>>>(result);
        }

        [Fact]
        public async Task Test_GetMember_Success()
        {
            Mock<IMediator> mockMediator = new Mock<IMediator>();
            Mock<ILogger<MemberController>> mockLogger = new Mock<ILogger<MemberController>>();
            mockMediator.Setup(x => x.Send(It.IsAny<GetMemberRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(
                new CommandResponse<Member>
                {
                    isSuccess = true,
                    Value = new Member {GivenName = "test", Surname = "test", InstitutionId = 12456, MemberId = 123456}
                });
            var controller = new MemberController(mockLogger.Object, mockMediator.Object);
            var result = await controller.GetMember(123456, CancellationToken.None);
            Assert.IsType<ActionResult<Member>>(result);
        }

        [Fact]
        public async Task Test_AddMember_Success()
        {
            Mock<IMediator> mockMediator = new Mock<IMediator>();
            Mock<ILogger<MemberController>> mockLogger = new Mock<ILogger<MemberController>>();
            mockMediator.Setup(x => x.Send(It.IsAny<AddMemberRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(
                new CommandResponse<Member>
                {
                    isSuccess = true,
                    Value = new Member {GivenName = "test", Surname = "test", InstitutionId = 12456, MemberId = 123456}
                });
            var controller = new MemberController(mockLogger.Object, mockMediator.Object);
            var result = await controller.AddMember(new Member { GivenName = "test", Surname = "test", InstitutionId = 12456, MemberId = 123456 }, CancellationToken.None);
            Assert.IsType<ActionResult<Member>>(result);
        }

        [Fact]
        public async Task Test_UpdateMember_Success()
        {
            Mock<IMediator> mockMediator = new Mock<IMediator>();
            Mock<ILogger<MemberController>> mockLogger = new Mock<ILogger<MemberController>>();
            mockMediator.Setup(x => x.Send(It.IsAny<UpdateMemberRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(
                new CommandResponse<bool>
                {
                    isSuccess = true,
                    Value = true
                });
            var controller = new MemberController(mockLogger.Object, mockMediator.Object);
            var result = await controller.UpdateMember(123456, new Member { GivenName = "test", Surname = "test", InstitutionId = 12456, MemberId = 123456 }, CancellationToken.None);
            Assert.IsType<AcceptedResult>(result);
        }

        [Fact]
        public async Task Test_UpdateMember_Fail()
        {
            Mock<IMediator> mockMediator = new Mock<IMediator>();
            Mock<ILogger<MemberController>> mockLogger = new Mock<ILogger<MemberController>>();
            mockMediator.Setup(x => x.Send(It.IsAny<UpdateMemberRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(
                new CommandResponse<bool>
                {
                    isSuccess = false,
                    Value = false
                });
            var controller = new MemberController(mockLogger.Object, mockMediator.Object);
            var result = await controller.UpdateMember(123456, new Member { GivenName = "test", Surname = "test", InstitutionId = 12456, MemberId = 123456 }, CancellationToken.None);
            Assert.IsType<StatusCodeResult>(result);
        }

        [Fact]
        public async Task Test_DeleteMember_Success()
        {
            Mock<IMediator> mockMediator = new Mock<IMediator>();
            Mock<ILogger<MemberController>> mockLogger = new Mock<ILogger<MemberController>>();
            mockMediator.Setup(x => x.Send(It.IsAny<DeleteMemberRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(
                new CommandResponse<bool>
                {
                    isSuccess = true,
                    Value = true
                });
            var controller = new MemberController(mockLogger.Object, mockMediator.Object);
            var result = await controller.DeleteMember(123456, CancellationToken.None);
            Assert.IsType<AcceptedResult>(result);
        }

        [Fact]
        public async Task Test_DeleteMember_Fail()
        {
            Mock<IMediator> mockMediator = new Mock<IMediator>();
            Mock<ILogger<MemberController>> mockLogger = new Mock<ILogger<MemberController>>();
            mockMediator.Setup(x => x.Send(It.IsAny<DeleteMemberRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(
                new CommandResponse<bool>
                {
                    isSuccess = false,
                    Value = false
                });
            var controller = new MemberController(mockLogger.Object, mockMediator.Object);
            var result = await controller.DeleteMember(123456, CancellationToken.None);
            Assert.IsType<StatusCodeResult>(result);
        }

        [Fact]
        public async Task Handler_GetAllMembers_Success()
        {
            var mockLogger = new Mock<ILogger<GetAllMembersRequestHandler>>();
            var mockRepository = new Mock<IMemberRepository>();
            mockRepository.Setup(x => x.ListAsync(CancellationToken.None)).ReturnsAsync(new List<MemberEntity>()
                {new MemberEntity {GivenName = "test", Surname = "test", InstitutionId = 12456, MemberId = 123456}});
            var handler = new GetAllMembersRequestHandler(mockRepository.Object, mockLogger.Object);
            var result = await handler.Handle(new GetAllMembersRequest(), CancellationToken.None);
            Assert.True(result.isSuccess);
        }

        [Fact]
        public async Task Handler_GetAllMember_Fail()
        {
            var mockLogger = new Mock<ILogger<GetAllMembersRequestHandler>>();
            var mockRepository = new Mock<IMemberRepository>();
            mockRepository.Setup(x => x.ListAsync(CancellationToken.None)).Throws<Exception>();
            var handler = new GetAllMembersRequestHandler(mockRepository.Object, mockLogger.Object);
            var result = await handler.Handle(new GetAllMembersRequest(), CancellationToken.None);
            Assert.False(result.isSuccess);
        }

        [Fact]
        public async Task Handler_GetMember_Success()
        {
            var mockLogger = new Mock<ILogger<GetMemberRequestHandler>>();
            var mockRepository = new Mock<IMemberRepository>();
            mockRepository.Setup(x => x.FindByIdAsync(123456, CancellationToken.None)).ReturnsAsync(new MemberEntity
                { GivenName = "test", Surname = "test", InstitutionId = 12456, MemberId = 123456 });
            var handler = new GetMemberRequestHandler(mockLogger.Object, mockRepository.Object);
            var result = await handler.Handle(new GetMemberRequest { MemberId = 123456 }, CancellationToken.None);
            Assert.True(result.isSuccess);

            
        }

        [Fact]
        public async Task Handler_GetMember_Fail()
        {
            var mockLogger = new Mock<ILogger<GetAllMembersRequestHandler>>();
            var mockRepository = new Mock<IMemberRepository>();
            mockRepository.Setup(x => x.ListAsync(CancellationToken.None)).Throws<Exception>();
            var handler = new GetAllMembersRequestHandler(mockRepository.Object, mockLogger.Object);
            var result = await handler.Handle(new GetAllMembersRequest(), CancellationToken.None);
            Assert.False(result.isSuccess);
        }

        [Fact]
        public async Task Handler_AddMember_Success()
        {
            var mockLogger = new Mock<ILogger<AddMemberRequestHandler>>();
            var mockRepository = new Mock<IMemberRepository>();
            Mock<ITransactionFactory> mockTransactionFactory = new Mock<ITransactionFactory>();
            Mock<IAccountRepository> mockAccountRepository = new Mock<IAccountRepository>();
            mockRepository.Setup(x => x.AddAsync(new MemberEntity{ GivenName = "test", Surname = "test", InstitutionId = 12456, MemberId = 123456 }, CancellationToken.None)).ReturnsAsync(new MemberEntity{ GivenName = "test", Surname = "test", InstitutionId = 12456, MemberId = 123456 });
            var handler = new AddMemberRequestHandler(mockLogger.Object, mockRepository.Object, mockTransactionFactory.Object, mockAccountRepository.Object);
            var result = await handler.Handle(new AddMemberRequest { Member = new Member { GivenName = "test", Surname = "test", InstitutionId = 12456, MemberId = 123456 } }, CancellationToken.None);
            Assert.True(result.isSuccess);
        }

        [Fact]
        public async Task Handler_AddMember_Fail()
        {
            var mockLogger = new Mock<ILogger<AddMemberRequestHandler>>();
            var mockRepository = new Mock<IMemberRepository>();
            Mock<ITransactionFactory> mockTransactionFactory = new Mock<ITransactionFactory>();
            Mock<IAccountRepository> mockAccountRepository = new Mock<IAccountRepository>();
            mockRepository.Setup(x => x.AddAsync(It.IsAny<MemberEntity>(), It.IsAny<CancellationToken>())).Throws<Exception>();
            var handler = new AddMemberRequestHandler(mockLogger.Object, mockRepository.Object, mockTransactionFactory.Object, mockAccountRepository.Object);
            var result = await handler.Handle(new AddMemberRequest { Member = new Member { GivenName = "test", Surname = "test", InstitutionId = 12456, MemberId = 123456 } }, CancellationToken.None);
            Assert.False(result.isSuccess);
        }

        [Fact]
        public async Task Handler_UpdateMember_Success()
        {
            var mockLogger = new Mock<ILogger<UpdateMemberRequestHandler>>();
            var mockRepository = new Mock<IMemberRepository>();
            mockRepository.Setup(x => x.Update(new MemberEntity
                {GivenName = "test", Surname = "test", InstitutionId = 12456, MemberId = 123456}));
            var handler = new UpdateMemberRequestHandler(mockLogger.Object,mockRepository.Object);
            var result = await handler.Handle(new UpdateMemberRequest { Member = new Member { GivenName = "test", Surname = "test", InstitutionId = 12456, MemberId = 123456 } }, CancellationToken.None);
            Assert.True(result.isSuccess);
        }

        [Fact]
        public async Task Handler_UpdateMember_Fail()
        {
            var mockLogger = new Mock<ILogger<UpdateMemberRequestHandler>>();
            var mockRepository = new Mock<IMemberRepository>();
            mockRepository.Setup(x => x.Update(It.IsAny<MemberEntity>())).Throws<Exception>();
            var handler = new UpdateMemberRequestHandler(mockLogger.Object, mockRepository.Object);
            var result = await handler.Handle(new UpdateMemberRequest { Member = new Member { GivenName = "test", Surname = "test", InstitutionId = 12456, MemberId = 123456 } }, CancellationToken.None);
            Assert.False(result.isSuccess);
        }

        [Fact]
        public async Task Handler_DeleteMember_Success()
        {
            var mockLogger = new Mock<ILogger<DeleteMemberRequestHandler>>();
            var mockRepository = new Mock<IMemberRepository>();
            mockRepository.Setup(x => x.Delete(It.IsAny<MemberEntity>()));
            var handler = new DeleteMemberRequestHandler(mockLogger.Object, mockRepository.Object);
            var result = await handler.Handle(new DeleteMemberRequest { MemberId = 123456 } , CancellationToken.None);
            Assert.True(result.isSuccess);
        }

        [Fact]
        public async Task Handler_DeleteMember_Fail()
        {
            var mockLogger = new Mock<ILogger<DeleteMemberRequestHandler>>();
            var mockRepository = new Mock<IMemberRepository>();
            mockRepository.Setup(x => x.Delete(It.IsAny<MemberEntity>())).Throws<Exception>();
            var handler = new DeleteMemberRequestHandler(mockLogger.Object, mockRepository.Object);
            var result = await handler.Handle(new DeleteMemberRequest { MemberId = 123456 }, CancellationToken.None);
            Assert.False(result.isSuccess);
        }
    }
}
