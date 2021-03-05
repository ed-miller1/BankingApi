using System.ComponentModel.DataAnnotations;

namespace BankingApi.Application.DTOs
{
    public class Institution
    {
        
        public int InstitutionId { get; set; }
        [Required]
        public string InstitutionName { get; set; }
    }
}
