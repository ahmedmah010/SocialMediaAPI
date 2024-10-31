using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;
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
        protected readonly AppDbContext _context;
        protected IQueryable<T> _query;
        public Repository(AppDbContext context)
        {
            _context = context;
            _query = _context.Set<T>();
        }
        // NOT USED
        //public IQueryBuilder<T> QueryBuilder()
        //{
        //    return new QueryBuilder<T>(_context.Set<T>());
        //}

        public IRepository<T> Where(Expression<Func<T, bool>> predicate)
        {
            _query = _query.Where(predicate);
            return this;
        }
        public IRepository<T> Include(Expression<Func<T, object>> navigationProperty)
        {
            _query = _query.Include(navigationProperty);
            return this;
        }

        public IRepository<T> Skip(int count)
        {
            _query = _query.Skip(count);
            return this;
        }

        public IRepository<T> Take(int count)
        {
            _query = _query.Take(count);
            return this;
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
        // I guess I have never used this method
        public async Task<T?> NestedInclude(Func<IQueryable<T>, IIncludableQueryable<T, object>> includes) // Include().ThenInclude()
        {
            _query = includes(_query); // includes is a delegate (a pointer to a function)
            return await _query.FirstOrDefaultAsync();
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
        public async Task<int> ExecuteSqlRawAsync(string query, params object[] objects) // returns the number of rows affected
        {
            return await _context.Database.ExecuteSqlRawAsync(query, objects);
        }
        public async Task<List<T>> FromSqlRawAsync(string query, params object[] objects)
        {
            return await _context.Set<T>().FromSqlRaw(query, objects).ToListAsync();
        }
        public async Task<int> CountAsync()
        {
            return await _context.Set<T>().CountAsync();
        }
        public async Task<int> CountAsync(Expression<Func<T,bool>> predicate)
        {
            return await _context.Set<T>().CountAsync(predicate);
        }
        public async Task<bool> RemoveByIdAsync(int id)
        {
            T entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.Set<T>().Remove(entity);
                return true; // Indication of success
            }
            return false; // Entity not found
        }
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }
        public void Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
        }
        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        public async Task<T> FirstOrDefaultAsync()
        {
            return await _query.FirstOrDefaultAsync();
        }
        public async Task<T> FirstOrDefaultAsync(Expression<Func<T,bool>> predicate)
        {
            return await _query.FirstOrDefaultAsync(predicate);
        }
        public async Task<bool> AnyAsync()
        {
            return await _query.AnyAsync();
        }
        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return await _query.AnyAsync(predicate);
        }
        public async Task<bool> AllAsync(Expression<Func<T, bool>> predicate)
        {
            return await _query.AllAsync(predicate);
        }
        public async Task<T> SingleOrDefaultAsync()
        {
            return await _query.SingleOrDefaultAsync();
        }
        public async Task<List<T>> ToListAsync()
        {
            return await _query.ToListAsync();
        }
        public IEnumerable<NewEntity> Select<NewEntity>(Func<T,NewEntity> expression)
        { 
            return _query.Select(expression);
        }
        public IRepository<T> OrderBy(Expression<Func<T,object>> expression)
        {
            _query = _query.OrderBy(expression);
            return this;
        }
        public IRepository<T> OrderByDesc(Expression<Func<T, object>> expression)
        {
            _query = _query.OrderByDescending(expression);
            return this;
        }
    }
}
