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
        Task<bool> DeleteByIdAsync(int id);
        Task UpdateAsync(T entity);
        Task AddAsync(T entity);
        Task SaveChangesAsync();
        Task<IEnumerable<T>> WhereAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> IncludeAsync(Expression<Func<T, object>> navigationProperty);
        Task<IEnumerable<T>> PaginateAsync(int size, int pageNo);
        Task<T> FindWithIncludesAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
        Task<T> FirstOrDefaultAsync();
        Task<List<T>> ToListAsync();
        Task<bool> AnyAsync();
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
        Task<bool> AllAsync(Expression<Func<T, bool>> predicate);
        Task<int> CountAsync();
        Task<T> SingleOrDefaultAsync();
        IQueryBuilder<T> QueryBuilder();
    }
}
