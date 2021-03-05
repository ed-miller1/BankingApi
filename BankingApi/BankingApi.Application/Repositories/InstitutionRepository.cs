using BankingApi.Domain.Entities;
using BankingApi.Domain.Repositories;
using BankingApi.Infrastructure.Database;

namespace BankingApi.Application.Repositories
{
    public class InstitutionRepository : DbRepository<InstitutionEntity>, IInstitutionRepository
    {
        public InstitutionRepository(BankApiDbContext context) : base(context)
        {
        }
    }
}
