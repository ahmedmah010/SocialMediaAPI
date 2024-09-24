using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace SocialMediaAPI.Domain.Repositories.Base
{
    public interface IQueryBuilder<T> where T : class
    {
        IQueryBuilder<T> Where(Expression<Func<T, bool>> predicate);

        IQueryBuilder<T> Include(Expression<Func<T, object>> navigationProperty);

        IQueryBuilder<T> Skip(int count);

        IQueryBuilder<T> Take(int count);

        // In some cases, it may be useful to allow access to the IQueryable<T> directly, especially if you need further customization or execution of queries outside of the QueryBuilder
        IQueryable<T> Build();
        // Functions that terminate the chaining (returns the final results of chaining)
        Task<T> FirstOrDefaultAsync();
        Task<List<T>> ToListAsync();
        Task<bool> AnyAsync();
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
        Task<bool> AllAsync(Expression<Func<T, bool>> predicate);
        Task<int> CountAsync();
        Task<T> SingleOrDefaultAsync();
    }
}
