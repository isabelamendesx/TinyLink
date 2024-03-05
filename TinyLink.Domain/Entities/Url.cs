using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyLink.Domain.Entities
{
    public class Url : BaseEntity
    {
        public string Destination { get; set; } = "";
        public string ShortUrl { get; set; } = "";
        public DateTime Expiration { get; set; }
    }
}
