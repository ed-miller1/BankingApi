using BankingApi.Domain.Entities;
using BankingApi.Domain.Repositories;
using BankingApi.Infrastructure.Database;

namespace BankingApi.Application.Repositories
{
    public class MemberRepository : DbRepository<MemberEntity>, IMemberRepository
    {
        public MemberRepository(BankApiDbContext dbContext) : base(dbContext) { }
    }
}
