using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BankingApi.Infrastructure.Database
{
    public static class SeedUtilities
    {
        public static SeedDatabase LoadSeedDatabase(string seedPath)
        {
            var json = File.ReadAllText(seedPath);
            return JsonConvert.DeserializeObject<SeedDatabase>(json);
        }
    }

    public class SeedDatabase
    {
        public IEnumerable<SeedInstitution> Institutions { get; set; }
        public IEnumerable<SeedMember> Members { get; set; }
    }

    public class SeedAccount
    {
        public int AccountId { get; set; }
        public double Balance { get; set; }
    }

    public class SeedMember
    {
        public int MemberId { get; set; }
        public string GivenName { get; set; }
        public string Surname { get; set; }
        public int InstitutionId { get; set; }
        public IEnumerable<SeedAccount> Accounts { get; set; }
    }

    public class SeedInstitution
    {
        public int InstitutionId { get; set; }
        public string InstitutionName { get; set; }
    }
}
