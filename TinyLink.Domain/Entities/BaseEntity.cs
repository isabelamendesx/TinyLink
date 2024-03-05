using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyLink.Domain.Entities
{
    public abstract class BaseEntity
    {
        public uint Id { get; set; }
    }
}
