using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApi.Application.DTOs
{
    public class UpdateAccountBalanceRequest
    {
        [Required]
        public double NewBalance { get; set; }
    }
}
