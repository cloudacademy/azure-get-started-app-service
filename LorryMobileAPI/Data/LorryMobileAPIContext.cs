using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LorryModels;

namespace LorryMobileAPI.Data
{
    public class LorryMobileAPIContext : DbContext
    {
        public LorryMobileAPIContext (DbContextOptions<LorryMobileAPIContext> options)
            : base(options)
        {
        }

        public DbSet<LorryModels.Pickup> Pickup { get; set; }
    }
}
