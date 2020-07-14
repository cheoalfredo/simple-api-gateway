using K8SCore.Domain.Ports;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace K8SCore.Infrastructure
{
    public class GenericRepository<E> : IGenericRepository<E> where E : Domain.DomainEntity
    {
        private readonly PersistenceContext _Context;

        public GenericRepository(PersistenceContext context)
        {
            _Context = context ?? throw new ArgumentNullException("context", "No hay dbcontext disponible");
        }

        public async Task<IEnumerable<E>> GetAsync(Expression<Func<E, bool>> filter = null,
            Func<IQueryable<E>, IOrderedQueryable<E>> orderBy = null,
            string includeStringProperties = "", bool isTracking = false)
        {
            IQueryable<E> query = _Context.Set<E>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (!string.IsNullOrEmpty(includeStringProperties))
            {
                foreach (var includeProperty in includeStringProperties.Split
                    (new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync().ConfigureAwait(false);
            }

            return (!isTracking) ? await query.AsNoTracking().ToListAsync().ConfigureAwait(false)
                : await query.ToListAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<E>> GetAsync(Expression<Func<E, bool>> filter = null,
            Func<IQueryable<E>, IOrderedQueryable<E>> orderBy = null,
             bool isTracking = false, params Expression<Func<E, object>>[] includeObjectProperties)
        {
            IQueryable<E> query = _Context.Set<E>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeObjectProperties != null)
            {
                foreach (Expression<Func<E, object>> include in includeObjectProperties)
                {
                    query = query.Include(include);
                }
            }

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync().ConfigureAwait(false);
            }

            return (!isTracking) ? await query.AsNoTracking().ToListAsync().ConfigureAwait(false)
                : await query.ToListAsync().ConfigureAwait(false);
        }


        public async Task<E> GetByIdAsync(object id)
        {
            return await _Context.Set<E>().FindAsync(id).ConfigureAwait(false);
        }

        public async Task<E> AddAsync(E entity)
        {
            if (entity != null)
            {
                var item = _Context.Set<E>();
                item.Add(entity);
                await this.CommitAsync().ConfigureAwait(false);
            }
            return entity;
        }

        public async Task UpdateAsync(E entity)
        {
            if (entity != null)
            {
                var item = _Context.Set<E>();
                item.Update(entity);
                await this.CommitAsync().ConfigureAwait(false);
            }
        }

        public async Task DeleteAsync(E entity)
        {
            if (entity != null)
            {
                _Context.Set<E>().Remove(entity);
                await this.CommitAsync().ConfigureAwait(false);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        protected virtual void Dispose(bool disposing)
        {
            this._Context.Dispose();
        }

        public async Task CommitAsync()
        {
            _Context.ChangeTracker.DetectChanges();

            foreach (var entry in _Context.ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Property("CreatedOn").CurrentValue = DateTime.UtcNow;
                        break;
                    case EntityState.Modified:
                        entry.Property("LastModifiedOn").CurrentValue = DateTime.UtcNow;
                        break;
                    default:
                        throw new InvalidOperationException("Unexpected value state = " + entry.State);
                }
            }

            await _Context.CommitAsync().ConfigureAwait(false);
        }

    }
}
