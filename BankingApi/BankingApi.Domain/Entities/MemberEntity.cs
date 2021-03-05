namespace BankingApi.Domain.Entities
{
    public class MemberEntity
    {
        public int MemberId { get; set; }
        public string GivenName { get; set; }
        public string Surname { get; set; }
        public int InstitutionId { get; set; }
        public InstitutionEntity Institution { get; set; }

    }
}
