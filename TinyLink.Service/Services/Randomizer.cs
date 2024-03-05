using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyLink.Domain.Interfaces;

namespace TinyLink.Service.Services
{
    public class Randomizer : IRandomizer
    {
        private readonly Random _random;

        public Randomizer() => 
            _random = new Random();

        public int Next(int length)
            => _random.Next(length);
    }
}
