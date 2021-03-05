using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApi.Domain.Models
{
    public class CommandResponse<T>
    {
        public bool isSuccess { get; set; }
        public string Message { get; set; }
        public T Value { get; set; }
    }
}
