using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyLink.Domain.Entities;

namespace TinyLink.Domain.Interfaces
{
    public interface IUrlShortenerService
    {
        Task<string> ManageUrl(string destinationUrl);
        Url? MatchShortUrl(string shorturl);
    }
}
