﻿using Microsoft.EntityFrameworkCore.Query;
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
        public void Remove(T entity);
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

        Task<int> CountAsync();
        Task<T> SingleOrDefaultAsync();
        //IQueryBuilder<T> QueryBuilder();
    }
}
