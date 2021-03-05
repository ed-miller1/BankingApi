using MediatR;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using BankingApi.Application.Commands;
using BankingApi.Application.DTOs;
using BankingApi.Controllers;
using BankingApi.Domain.Entities;
using BankingApi.Domain.Models;
using BankingApi.Domain.Repositories;
using BankingApi.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Xunit;

namespace BankingApi
{
    public class AccountTests
    {
        //private Mock<IMediator> _mockMediator;

        public AccountTests()
        {
        }

        [Fact]
        public async Task Controller_UpdateAccountBalance_Success()
        {
            Mock<IMediator> mockMediator = new Mock<IMediator>();
            Mock<ILogger<AccountController>> mockLogger = new Mock<ILogger<AccountController>>();
            mockMediator.Setup(m => m.Send(It.IsAny<Application.DTOs.UpdateAccountBalanceRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CommandResponse<bool> {isSuccess = true});
            var controller = new AccountController(mockLogger.Object, mockMediator.Object);

            var controllerResponse = await controller.UpdateAccountBalance(new Application.DTOs.UpdateAccountBalanceRequest
            {
                NewBalance = 25.00
            },23456, CancellationToken.None);

            Assert.IsType<AcceptedResult>(controllerResponse);
        }

        [Fact]
        public async Task Controller_TransferAmountToAccount_Success()
        {
            Mock<IMediator> mockMediator = new Mock<IMediator>();
            Mock<ILogger<AccountController>> mockLogger = new Mock<ILogger<AccountController>>();
            mockMediator.Setup(m => m.Send(It.IsAny<TransferAmountToAccountRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CommandResponse<bool> { isSuccess = true });
            var controller = new AccountController(mockLogger.Object, mockMediator.Object);

            var controllerResponse = await controller.TransferAmountToAccount(new TransferRequest
            {
                FromAccountId = 23456,
                ToAccountId = 36547,
                Amount = 20.00
            }, CancellationToken.None);

            Assert.IsType<AcceptedResult>(controllerResponse);
        }

        [Fact]
        public async Task Handler_UpdateAccountBalanceRequestHandler_Success()
        {
            var account = new AccountEntity
            {
                AccountId = 123456,
                Balance = 100.25,
                MemberId = 123798
            };
            Mock<ILogger<UpdateAccountBalanceRequestHandler>> mockLogger = new Mock<ILogger<UpdateAccountBalanceRequestHandler>>();
            Mock<IAccountRepository> mockRepository = new Mock<IAccountRepository>();
            mockRepository.Setup(x => x.FindByIdAsync(account.AccountId, CancellationToken.None)).ReturnsAsync(account);
            mockRepository.Setup(x => x.Update(account));
            mockRepository.Setup(x => x.SaveChangesAsync(CancellationToken.None));

            var handler = new UpdateAccountBalanceRequestHandler(mockLogger.Object, mockRepository.Object);
            var result = await handler.Handle(new Application.Commands.UpdateAccountBalanceRequest
            {
                AccountId = account.AccountId,
                newBalance = 150.25
                
            }, CancellationToken.None);
            Assert.True(result.isSuccess);
        }

        [Fact]
        public async Task Handler_TransferAmountToAccountRequestHandler_Success()
        {
            var fromAccount = new AccountEntity
            {
                AccountId = 123456,
                Balance = 100.25,
                MemberId = 123798
            };

            var toAccount = new AccountEntity
            {
                AccountId = 456789,
                Balance = 75.25,
                MemberId = 123798
            };
            Mock<ILogger<TransferAmountToAccountRequestHandler>> mockLogger = new Mock<ILogger<TransferAmountToAccountRequestHandler>>();
            Mock<IAccountRepository> mockRepository = new Mock<IAccountRepository>();
            mockRepository.Setup(x => x.FindByIdAsync(It.IsIn(fromAccount.AccountId), It.IsAny<CancellationToken>())).ReturnsAsync(fromAccount);
            mockRepository.Setup(x => x.FindByIdAsync(It.IsIn(toAccount.AccountId), It.IsAny<CancellationToken>())).ReturnsAsync(toAccount);
            mockRepository.Setup(x => x.Update(It.IsAny<AccountEntity>()));
            mockRepository.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()));
            Mock<ITransactionFactory> mockTransactionFactory = new Mock<ITransactionFactory>();
            mockTransactionFactory.Setup(x => x.BeginTransaction()).Returns(new Mock<ITransaction>().Object);

            var handler = new TransferAmountToAccountRequestHandler(mockLogger.Object, mockRepository.Object, mockTransactionFactory.Object);
            var result = await handler.Handle(new TransferAmountToAccountRequest
            {
                FromAccountId = fromAccount.AccountId,
                ToAccountId = toAccount.AccountId,
                Amount = 25.00
            }, CancellationToken.None);
            Assert.True(result.isSuccess);
        }

        [Fact]
        public async Task Handler_TransferAmountToAccountRequestHandler_AmountMoreThanBalance()
        {
            var fromAccount = new AccountEntity
            {
                AccountId = 123456,
                Balance = 100.25,
                MemberId = 123798
            };

            var toAccount = new AccountEntity
            {
                AccountId = 456789,
                Balance = 75.25,
                MemberId = 123798
            };
            Mock<ILogger<TransferAmountToAccountRequestHandler>> mockLogger = new Mock<ILogger<TransferAmountToAccountRequestHandler>>();
            Mock<IAccountRepository> mockRepository = new Mock<IAccountRepository>();
            mockRepository.Setup(x => x.FindByIdAsync(fromAccount.AccountId, CancellationToken.None)).ReturnsAsync(fromAccount);
            mockRepository.Setup(x => x.FindByIdAsync(toAccount.AccountId, CancellationToken.None)).ReturnsAsync(toAccount);
            mockRepository.Setup(x => x.Update(fromAccount));
            mockRepository.Setup(x => x.Update(toAccount));
            Mock<ITransactionFactory> mockTransactionFactory = new Mock<ITransactionFactory>();
            mockTransactionFactory.Setup(x => x.BeginTransaction()).Returns(new Mock<ITransaction>().Object);

            var handler = new TransferAmountToAccountRequestHandler(mockLogger.Object, mockRepository.Object, mockTransactionFactory.Object);
            var result = await handler.Handle(new TransferAmountToAccountRequest
            {
                FromAccountId = fromAccount.AccountId,
                ToAccountId = toAccount.AccountId,
                Amount = 500.00
            }, CancellationToken.None);
            Assert.False(result.isSuccess);
        }
    }
}
