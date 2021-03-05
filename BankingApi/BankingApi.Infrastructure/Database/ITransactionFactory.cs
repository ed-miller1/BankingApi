using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApi.Infrastructure.Database
{
    public interface ITransactionFactory
    {
        ITransaction BeginTransaction();
    }
}
