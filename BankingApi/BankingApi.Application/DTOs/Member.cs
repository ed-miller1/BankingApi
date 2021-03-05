using System.ComponentModel.DataAnnotations;

namespace BankingApi.Application.DTOs
{
    public class Member
    {
        
        public int MemberId { get; set; }
        [Required]
        public string GivenName { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public int InstitutionId { get; set; }
    }
}
