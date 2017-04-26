using asp.net_core_trip_manager.Core.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asp.net_core_trip_manager.Persistence
{
    public class TripContextSeedData
    {
        private TripContext _context;
        private UserManager<ApplicationUser> _userManager;

        public TripContextSeedData(TripContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task EnsureSeedData()
        {
            if (await _userManager.FindByEmailAsync("user1@domain.com") == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "user1",
                    Email = "user1@domain.com"
                };

                await _userManager.CreateAsync(user, "P@ssw0rd!");
            }

            if (!_context.Trips.Any())
            {
                var londonTrip = new Trip
                {
                    DateCreated = DateTime.UtcNow,
                    Name = "London Trip",
                    UserName = "user1",
                    Stops = new List<Stop>
                    {
                        new Stop { Name="Big Ben", Arrival = new DateTime(2017, 3, 4), Latitude = 51.500766, Longitude= -0.124264, Order = 1 },
                        new Stop { Name="London Eye", Arrival = new DateTime(2017, 3, 5), Latitude = 51.503372, Longitude= -0.122316, Order = 2 },
                        new Stop { Name="London Bridge", Arrival = new DateTime(2017, 3, 6), Latitude = 51.507954, Longitude= -0.090992, Order = 3 },
                        new Stop { Name="BOXPARK Shoreditch", Arrival = new DateTime(2017, 3, 7), Latitude = 51.523629, Longitude= -0.080454, Order = 4 }
                    }
                };

                _context.Trips.Add(londonTrip);

                _context.Stops.AddRange(londonTrip.Stops);

                var baliTrip = new Trip
                {
                    DateCreated = DateTime.UtcNow,
                    Name = "Bali Trip",
                    UserName = "user1",
                    Stops = new List<Stop>
                    {
                        new Stop { Name="Nusa Dua", Arrival = new DateTime(2016, 6, 4), Latitude = -8.817335, Longitude= 115.218294, Order = 1 },
                        new Stop { Name="Kuta", Arrival = new DateTime(2016, 6, 5), Latitude = -8.723937, Longitude= 115.175628, Order = 2 },
                        new Stop { Name="Denpasar", Arrival = new DateTime(2016, 6, 6), Latitude = -8.670301, Longitude= 115.211901, Order = 3 },
                        new Stop { Name="Ubud", Arrival = new DateTime(2016, 6, 7), Latitude = -8.506675, Longitude= 115.262369, Order = 4 }
                    }
                };

                _context.Trips.Add(baliTrip);

                _context.Stops.AddRange(baliTrip.Stops);

                await _context.SaveChangesAsync();
            }
        }
    }
}
