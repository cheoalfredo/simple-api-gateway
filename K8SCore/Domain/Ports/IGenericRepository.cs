using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace K8SCore.Domain.Ports
{
    public interface IGenericRepository<E> : IDisposable where E : Domain.DomainEntity

    {
        Task<IEnumerable<E>> GetAsync(Expression<Func<E, bool>> filter = null,
            Func<IQueryable<E>, IOrderedQueryable<E>> orderBy = null, string includeStringProperties = "",
            bool isTracking = false);

        Task<IEnumerable<E>> GetAsync(Expression<Func<E, bool>> filter = null,
            Func<IQueryable<E>, IOrderedQueryable<E>> orderBy = null,
             bool isTracking = false, params Expression<Func<E, object>>[] includeObjectProperties);

        Task<E> GetByIdAsync(object id);
        Task<E> AddAsync(E entity);
        Task UpdateAsync(E entity);
        Task DeleteAsync(E entity);
    }
}
