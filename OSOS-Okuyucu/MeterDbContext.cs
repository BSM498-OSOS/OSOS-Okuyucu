using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework
{
    public class MeterDbContext: DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql("Host=localhost;Database=Bitirme;UserId=postgres;Password=Mert.123");

        public DbSet<Meter> Meters { get; set; }
        public DbSet<ReadingTime> ReadingTimes { get; set; }
        
    }
}
