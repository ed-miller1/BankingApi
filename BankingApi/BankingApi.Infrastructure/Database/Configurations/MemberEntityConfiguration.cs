using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankingApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankingApi.Infrastructure.Database.Configurations
{
    public class MemberEntityConfiguration : IEntityTypeConfiguration<MemberEntity>
    {
        public void Configure(EntityTypeBuilder<MemberEntity> builder)
        {
            builder
                .HasKey(k => new { k.MemberId })
                .HasName("PK_MemberId");
            builder.ToTable("tbl_Member");
        }
    }
}
