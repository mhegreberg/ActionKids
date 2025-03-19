using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ActionKids.Models;

namespace ActionKids
{
    public class Context : DbContext
    {
        public Context (DbContextOptions<Context> options)
            : base(options)
        {
        }

        public DbSet<Kid> Kids { get; set; } = default!;
        public DbSet<Service> Services { get; set; } = default!;
        public DbSet<KidServiceRecord> KidServiceRecords { get; set; } = default!;
    }
}
