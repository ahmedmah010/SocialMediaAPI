using Microsoft.EntityFrameworkCore;
using SocialMediaAPI.Domain.Repositories.Base;
using SocialMediaAPI.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace SocialMediaAPI.Infrastructure.Repositories.Base
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        public Repository(AppDbContext context)
        {
            _context = context;
        }
        public IQueryBuilder<T> QueryBuilder()
        {
            return new QueryBuilder<T>(_context.Set<T>());
        }
        public async Task<T> FindWithIncludesAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>().Where(predicate);
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return await query.FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<T>> WhereAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).ToListAsync();
        }
        public async Task<IEnumerable<T>> IncludeAsync(Expression<Func<T, object>> navigationProperty)
        {
            return await _context.Set<T>().Include(navigationProperty).ToListAsync();
        }
        public async Task<IEnumerable<T>> PaginateAsync(int size, int pageNo)
        {
            return await _context.Set<T>().Take(size).Skip((pageNo-1)*size).ToListAsync();
        }
        public async Task<IEnumerable<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }
        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }
        public async Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
        }
        public async Task<int> CountAsync()
        {
            return await _context.Set<T>().CountAsync();
        }
        public async Task<bool> DeleteByIdAsync(int id)
        {
            T entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.Set<T>().Remove(entity);
                return true; // Indication of success
            }
            return false; // Entity not found
        }
        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync();
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        public async Task<T> FirstOrDefaultAsync()
        {
            return await _context.Set<T>().FirstOrDefaultAsync();
        }
        public async Task<List<T>> ToListAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }
        public async Task<bool> AnyAsync()
        {
            return await _context.Set<T>().AnyAsync();
        }
        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().AnyAsync(predicate);
        }
        public async Task<bool> AllAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().AllAsync(predicate);
        }
        public async Task<T> SingleOrDefaultAsync()
        {
            return await _context.Set<T>().SingleOrDefaultAsync();
        }
    }
}
