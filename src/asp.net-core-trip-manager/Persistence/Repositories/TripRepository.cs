using asp.net_core_trip_manager.Core.Models;
using asp.net_core_trip_manager.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asp.net_core_trip_manager.Persistence.Repositories
{
    public class TripRepository : ITripRepository
    {
        private TripContext _context;
        private ILogger<TripRepository> _logger;

        public TripRepository(TripContext context, ILogger<TripRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IEnumerable<Trip> GetAllTrips()
        {
            _logger.LogInformation("Getting All Trips from the Database");
            return _context.Trips.ToList();
        }

        public IEnumerable<Trip> GetTripsByUsername(string name)
        {
            return _context.Trips
                .Include(t => t.Stops)
                .Where(t => t.UserName == name)
                .ToList();
        }

        public Trip GetTripById(int id)
        {
            return _context.Trips
                .Include(t => t.Stops)
                .SingleOrDefault(t => t.Id == id);
        }

        public Trip GetTripByName(string tripName)
        {
            return _context.Trips
                .Include(t => t.Stops)
                .Where(t => t.Name == tripName)
                .FirstOrDefault();
        }

        public void Add(Trip trip)
        {
            _context.Add(trip);
        }
        
        public void RemoveTrip(Trip trip)
        {
            _context.Trips.Remove(trip);
        }
    }
}
