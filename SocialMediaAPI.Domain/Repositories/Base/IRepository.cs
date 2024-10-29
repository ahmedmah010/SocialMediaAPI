using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaAPI.Domain.Repositories.Base
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAll();
        Task<bool> RemoveByIdAsync(int id);
        void Remove(T entity);
        Task<int> ExecuteSqlRawAsync(string query, params object[] objects);
        Task UpdateAsync(T entity);
        Task AddAsync(T entity);
        Task SaveChangesAsync();
        IRepository<T> Where(Expression<Func<T, bool>> predicate);
        IRepository<T> Include(Expression<Func<T, object>> navigationProperty);
        Task<T> FindWithIncludesAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
        Task<T?> NestedInclude(Func<IQueryable<T>, IIncludableQueryable<T, object>> includes);
        Task<T> FirstOrDefaultAsync();
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
        Task<bool> AnyAsync();
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
        Task<bool> AllAsync(Expression<Func<T, bool>> predicate);
        IRepository<T> Skip(int count);

        IRepository<T> Take(int count);
        Task<List<T>> ToListAsync();
        IEnumerable<NewEntity> Select<NewEntity>(Func<T, NewEntity> expression);
        IRepository<T> OrderBy(Expression<Func<T, object>> expression);
        IRepository<T> OrderByDesc(Expression<Func<T, object>> expression);
        Task<int> CountAsync();
        Task<int> CountAsync(Expression<Func<T, bool>> predicate);
        Task<T> SingleOrDefaultAsync();
        Task<IDbContextTransaction> BeginTransactionAsync();
        //IQueryBuilder<T> QueryBuilder();
    }
}
