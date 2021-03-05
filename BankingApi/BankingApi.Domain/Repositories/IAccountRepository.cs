using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankingApi.Domain.Entities;

namespace BankingApi.Domain.Repositories
{
    public interface IAccountRepository : IDbRepository<AccountEntity>
    {
    }
}
