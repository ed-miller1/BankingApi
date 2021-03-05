namespace BankingApi.Domain.Entities
{
    public class AccountEntity
    {
        public int AccountId { get; set; }
        public double Balance { get; set; }
        public int MemberId { get; set; }
        public MemberEntity Member { get; set; }
    }
}
