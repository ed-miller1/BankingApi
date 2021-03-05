using System.ComponentModel.DataAnnotations;

namespace BankingApi.Application.DTOs
{
    public class Account
    {
        public int AccountId { get; set; }
        [Required]
        public double Balance { get; set; }
    }
}
