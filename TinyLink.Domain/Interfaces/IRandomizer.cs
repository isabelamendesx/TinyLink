using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyLink.Domain.Interfaces
{
    public interface IRandomizer
    {
       int Next(int length);
    }
}
