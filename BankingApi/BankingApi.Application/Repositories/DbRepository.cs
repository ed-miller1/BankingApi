using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BankingApi.Domain.Repositories;
using BankingApi.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace BankingApi.Application.Repositories
{
    public class DbRepository<T> : IDbRepository<T> where T : class
    {
        protected readonly BankApiDbContext _context;

        public DbRepository(BankApiDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public virtual async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        public virtual void SaveChanges(CancellationToken cancellationToken = default)
        {
            _context.SaveChanges();
        }

        public virtual async Task<IEnumerable<T>> ListAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Set<T>()
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public virtual async Task<T> FindByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Set<T>().FindAsync(new object[] {id}, cancellationToken);
        }

        public virtual async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _context.Set<T>().AddAsync(entity, cancellationToken);
            return entity;
        }

        public virtual void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }

        public virtual void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }
    }
}
