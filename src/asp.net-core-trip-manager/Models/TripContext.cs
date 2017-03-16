using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace asp.net_core_trip_manager.Models
{
    public class TripContext : DbContext
    {
        private IConfigurationRoot _config;

        public TripContext(IConfigurationRoot config, DbContextOptions options) : base(options)
        {
            _config = config;
        }

        public DbSet<Trip> Trips { get; set; }
        public DbSet<Stop> Stops { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(_config["ConnectionStrings:TripContextConnection"]);
        }
    }
}
