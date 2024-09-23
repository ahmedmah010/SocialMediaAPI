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
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetAsync(Expression<Func<T,bool>> predicate);
        Task<IEnumerable<T>> WhereAsync(Expression<Func<T,bool>> predicate);
        Task<bool> DeleteByIdAsync(int id);
        Task UpdateAsync(T entity);
        Task AddAsync(T entity);
        Task SaveChangesAsync();
        Task<int> CountAsync();

    }
}
