using BankingApi.Domain.Entities;
using BankingApi.Domain.Repositories;
using BankingApi.Infrastructure.Database;

namespace BankingApi.Application.Repositories
{
    public class AccountRepository : DbRepository<AccountEntity>, IAccountRepository
    {
        public AccountRepository(BankApiDbContext context) : base(context)
        {
        }
    }
}
