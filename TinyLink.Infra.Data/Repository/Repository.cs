using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TinyLink.Domain.Entities;
using TinyLink.Domain.Interfaces;
using TinyLink.Infra.Data.Context;

namespace TinyLink.Infra.Data.Repository
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        internal readonly DbSet<T> _dbSet;
        protected readonly TinyUrlContext _context;
        protected readonly ILogger _logger;

        public Repository(TinyUrlContext context, ILogger logger)
        {
            _context = context;
            _dbSet = _context.Set<T>();
            _logger = logger;
        }

        public virtual async Task<bool> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public virtual async Task<bool> Delete(T entity) => throw new NotImplementedException();

        public virtual Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null!) => throw new NotImplementedException();

        public virtual Task<bool> UpdateAsync(T entity) => throw new NotImplementedException();

        public virtual Task<T?> GetByParameter(Expression<Func<T, bool>> filter) => _dbSet.FirstOrDefaultAsync(filter);
    }
}
