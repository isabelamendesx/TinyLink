using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyLink.Domain.Entities;

namespace TinyLink.Infra.Data.Context
{
    public class TinyUrlContext : DbContext
    {
        public TinyUrlContext(DbContextOptions<TinyUrlContext> options) : base(options) { }

        public virtual DbSet<Url> Urls { get; set; }
    }
}
