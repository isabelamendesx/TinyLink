using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TinyLink.Domain.Entities;

namespace TinyLink.Domain.Interfaces
{
    public interface IUrlRepository : IRepository<Url>
    {
        Task<bool> Exists(Expression<Func<Url, bool>> filter);
    }
}
