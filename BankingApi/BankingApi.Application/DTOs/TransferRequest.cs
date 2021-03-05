using System.ComponentModel.DataAnnotations;

namespace BankingApi.Application.DTOs
{
    public class TransferRequest
    {
        [Required]
        public int FromAccountId { get; set; }
        [Required]
        public int ToAccountId { get; set; }
        [Required]
        public double Amount { get; set; }
    }
}
