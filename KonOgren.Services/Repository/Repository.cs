
using KonOgren.DataAccess;
using KonOgren.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace KonOgren.Services
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly KonOgrenDBContext _context;
        public Repository(KonOgrenDBContext context)
        {
            _context = context;
        }
        public IQueryable<T> Table => _context.Set<T>();

        public T Add(T entity)
        {
            var result = _context.Set<T>().Add(entity);
            _context.SaveChanges();
            return result.Entity;
        }

        public T Update(T entity)
        {
            var result = _context.Set<T>().Update(entity);
            _context.SaveChanges();
            return result.Entity;
        }

        public void UpdateRange(IEnumerable<T> entities)
        {
            _context.Set<T>().UpdateRange(entities);
            _context.SaveChanges();
        }

        public T GetById(object id)
        {
            try
            {
                return _context.Set<T>().Find(id);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void Delete(object id)
        {
            var entity = _context.Set<T>().Find(id);
            if (null != entity)
            {
                _context.Set<T>().Remove(entity);
                _context.SaveChanges();
            }
        }
        public IQueryable<T> Get(Expression<Func<T, bool>> predicate = null,
                                 Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                 IList<Expression<Func<T, object>>> includes = null,
                                 Func<IQueryable<T>, IIncludableQueryable<T, object>> thenIncludes = null,
                                 bool disableTracking = true)
        {
            IQueryable<T> query = _context.Set<T>();

            if (disableTracking)
                query = query.AsNoTracking();

            if (includes != null)
                query = includes.Aggregate(query, (current, include) => current.Include(include));

            if (null != thenIncludes)
                query = thenIncludes(query);

            if (null != predicate)
            {
                query = query.Where(predicate);
            }

            if (null != orderBy)
            {
                return orderBy(query);
            }
            else
                return query;
        }

        public void Dispose()
        {
            if (null != _context)
            {
                _context.Dispose();
            }
        }

        public async Task DisposeAsync()
        {
            if (null != _context)
            {
                await _context.DisposeAsync();
            }
        }

        public async Task<T> GetByIdAsync(object id)
        {
            try
            {
                return await _context.Set<T>().FindAsync(id);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(object id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (null != entity)
            {
                _context.Set<T>().Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
