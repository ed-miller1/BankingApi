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
    public class InstitutionEntityConfiguration : IEntityTypeConfiguration<InstitutionEntity>
    {
        public void Configure(EntityTypeBuilder<InstitutionEntity> builder)
        {
            builder
                .HasKey(k => new {k.InstitutionId})
                .HasName("PK_InsitutionId");
            builder.ToTable("tbl_Institution");
        }
    }
}
