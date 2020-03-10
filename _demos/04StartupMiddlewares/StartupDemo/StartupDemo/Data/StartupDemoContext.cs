using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace StartupDemo.Models
{
    public class StartupDemoContext : DbContext
    {
        public StartupDemoContext (DbContextOptions<StartupDemoContext> options)
            : base(options)
        {
        }

        public DbSet<StartupDemo.Models.Evento> Evento { get; set; }
    }
}
