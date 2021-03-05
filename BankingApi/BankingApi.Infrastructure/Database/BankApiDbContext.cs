using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BankingApi.Domain.Entities;
using BankingApi.Domain.Settings;
using BankingApi.Utilities.AutoMapping;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BankingApi.Infrastructure.Database
{
    public class BankApiDbContext : DbContext, ITransactionFactory
    {
        private static readonly IReadOnlyDictionary<Type, MethodInfo> __applyConfigurationConcrete;
        private readonly BankApiSettings _settings;

        public BankApiDbContext(DbContextOptions<BankApiDbContext> options, BankApiSettings settings) : base(options)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            this.ApplyAllConfigurations(modelBuilder);
            this.SetGlobalUtcDateHandling(modelBuilder);

            if (!string.IsNullOrWhiteSpace(_settings.DbSeedPath))
            {
                var seed = SeedUtilities.LoadSeedDatabase(_settings.DbSeedPath);
                seed.Institutions.ToList().ForEach(x =>
                {
                    modelBuilder.Entity<InstitutionEntity>()
                        .HasData(AutoMapper.MapTo<SeedInstitution, InstitutionEntity>(x));
                });
                seed.Members.ToList().ForEach(x =>
                {
                    modelBuilder.Entity<MemberEntity>()
                        .HasData(AutoMapper.MapTo<SeedMember, MemberEntity>(x));
                    if (x?.Accounts != null && x.Accounts.Any())
                    {
                        x.Accounts.ToList().ForEach(y =>
                        {
                            var accountEntity = AutoMapper.MapTo<SeedAccount, AccountEntity>(y);
                            accountEntity.MemberId = x.MemberId;
                            modelBuilder.Entity<AccountEntity>().HasData(accountEntity);
                        });
                    }
                });
            }
        }

        private void ApplyAllConfigurations(ModelBuilder modelBuilder)
        {
            foreach (var applyConfigurationConcrete in __applyConfigurationConcrete)
            {
                applyConfigurationConcrete.Value.Invoke(modelBuilder, new object[] { Activator.CreateInstance(applyConfigurationConcrete.Key) });
            }
        }

        private void SetGlobalUtcDateHandling(ModelBuilder modelBuilder)
        {
            ValueConverter<DateTime, DateTime> dateTimeConverter = new ValueConverter<DateTime, DateTime>(
                (DateTime dateTime) => dateTime,
                (DateTime dateTime) => DateTime.SpecifyKind(dateTime, DateTimeKind.Utc));

            foreach (IMutableEntityType entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (IMutableProperty property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(DateTime) || property.ClrType == typeof(DateTime?))
                    {
                        property.SetValueConverter(dateTimeConverter);
                    }
                }
            }
        }

        static BankApiDbContext()
        {
            MethodInfo applyConfigurationGeneric = typeof(ModelBuilder)
                .GetMethods(BindingFlags.Instance | BindingFlags.Public)
                .Where((MethodInfo methodInfo) => methodInfo.Name == nameof(ModelBuilder.ApplyConfiguration))
                .Where((MethodInfo methodInfo) =>
                {
                    ParameterInfo[] parameters = methodInfo.GetParameters();

                    if (parameters.Length != 1) return false;
                    if (!parameters[0].ParameterType.IsGenericType) return false;

                    return parameters[0].ParameterType.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>);
                })
                .Single();

            __applyConfigurationConcrete = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where((Type type) => type.IsClass && !type.IsAbstract && !type.ContainsGenericParameters)
                .Select((Type type) => new
                {
                    Type = type,
                    Interface = type.GetInterfaces()
                        .Where((Type @interface) => @interface.IsConstructedGenericType &&
                                                    @interface.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>))
                        .FirstOrDefault(),
                })
                .Where(_ => _.Interface != null)
                .Select(_ => new
                {
                    Type = _.Type,
                    MethodInfo = applyConfigurationGeneric.MakeGenericMethod(_.Interface.GenericTypeArguments[0]),
                })
                .ToDictionary(_ => _.Type, _ => _.MethodInfo);
        }

        public ITransaction BeginTransaction()
        {
            return new BankApiTransaction(this.Database.BeginTransaction());
        }

        public DbSet<MemberEntity> Members { get; set; }
        public DbSet<InstitutionEntity> Institutions { get; set; }
        public DbSet<AccountEntity> Accounts { get; set; }
        
    }
}
