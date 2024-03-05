using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyLink.Domain.Entities;

namespace TinyLink.Domain.Interfaces
{
    public interface IUrlValidator
    {
        Task<bool> ValidateDestination(string destination);
        Task<bool> IsShortUrlUnique(string shorturl);
        Task<bool> IsDestinationExpiredUrl(string destination);
        bool IsExpired(Url url);
    }
}
