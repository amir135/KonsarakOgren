using KonOgren.Domain.Model;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace KonOgren.Services
{
    public interface IRepository<T> where T : BaseEntity
    {
        IQueryable<T> Get(Expression<Func<T, bool>> predicate = null,
                          Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                          IList<Expression<Func<T, object>>> includes = null,
                          Func<IQueryable<T>, IIncludableQueryable<T, object>> thenIncludes = null,
                          bool disableTracking = true);

        T GetById(object id);
        T Add(T entity);
        T Update(T entity);
        void UpdateRange(IEnumerable<T> entities);
        void Delete(object id);
        void Dispose();
        Task<T> GetByIdAsync(object id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(object id);
        Task DisposeAsync();
    }

}
