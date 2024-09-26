using Microsoft.EntityFrameworkCore;
using SocialMediaAPI.Domain.Repositories.Base;
using SocialMediaAPI.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaAPI.Infrastructure.Repositories.Base
{
    // NOT USED - IT WAS IMPLEMENTED FOR LEARNING PURPOSES
    public class QueryBuilder<T> : IQueryBuilder<T> where T : class
    {
        private IQueryable<T> _query;
        public QueryBuilder(IQueryable<T> query)
        {
            _query = query;
        }
        public IQueryBuilder<T> Where(Expression<Func<T, bool>> predicate)
        {
            _query = _query.Where(predicate);
            return this;
        }
        public IQueryBuilder<T> Include(Expression<Func<T, object>> navigationProperty)
        {
            _query = _query.Include(navigationProperty);
            return this;
        }
        public IQueryBuilder<T> Skip(int count)
        {
            _query = _query.Skip(count);
            return this;
        }

        public IQueryBuilder<T> Take(int count)
        {
            _query = _query.Take(count);
            return this;
        }

        // In some cases, it may be useful to allow access to the IQueryable<T> directly, especially if you need further customization or execution of queries outside of the QueryBuilder
        public IQueryable<T> Build()
        {
            return _query;
        }
        // Functions that terminate the chaining (returns the final results of chaining)
        public async Task<T> FirstOrDefaultAsync()
        {
            return await _query.FirstOrDefaultAsync();
        }
        public async Task<List<T>> ToListAsync()
        {
            return await _query.ToListAsync();
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
        public async Task<int> CountAsync()
        {
            return await _query.CountAsync();
        }
        public async Task<T> SingleOrDefaultAsync()
        {
            return await _query.SingleOrDefaultAsync();
        }

    }
}
