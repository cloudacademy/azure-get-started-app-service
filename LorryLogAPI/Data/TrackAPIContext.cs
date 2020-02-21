using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LorryModels;

namespace LorryLogAPI.Data
{
    public class LorryLogAPIContext : DbContext
    {
        public LorryLogAPIContext (DbContextOptions<LorryLogAPIContext> options)
            : base(options)
        {
        }

        public DbSet<LorryModels.Vehicle> Vehicle { get; set; }
    }
}
