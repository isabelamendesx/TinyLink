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
    public class UrlRepository : Repository<Url>, IUrlRepository
    {
        public UrlRepository(TinyUrlContext appDbContext, ILogger logger) : base(appDbContext, logger) { }

        public override async Task<IEnumerable<Url>> GetAllAsync(Expression<Func<Url, bool>> filter = null!)
        {
            try
            {
                var query = _dbSet.AsQueryable();

                if (filter != null)
                {
                    query = query
                         .Where(filter)
                         .AsNoTracking();
                }

                return await query.ToListAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} All function error", typeof(UrlRepository));
                throw;
            }
        }

        public override async Task<bool> Delete(Url url)
        {
            try
            {
                _dbSet.Remove(url);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} Delete function error", typeof(UrlRepository));
                throw;
            }
        }

        public override async Task<bool> UpdateAsync(Url url)
        {
            try
            {
                _dbSet.Update(url);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} Update function error", typeof(UrlRepository));
                throw;
            }
        }

        public Task<bool> Exists(Expression<Func<Url, bool>> filter)
        {
            try
            {
                return Task.FromResult(_dbSet.Any(filter));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} Update function error", typeof(UrlRepository));
                throw;
            }
        }

        public Task<Url?> GetByShortUrl(string shortUrl)
        {
            try
            {
                return Task.FromResult(_dbSet.FirstOrDefault(url => url.ShortUrl == shortUrl));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} Update function error", typeof(UrlRepository));
                throw;
            }
        }
    }
}
