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
    public class StopRepository : IStopRepository
    {
        private TripContext _context;
        private ILogger<StopRepository> _logger;

        public StopRepository(TripContext context, ILogger<StopRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IEnumerable<Stop> GetAllStopsForUsersTrip(string tripName, string username)
        {
            var trip = GetUserTripByName(tripName, username);
            return trip.Stops.OrderBy(s => s.Order);
        }

        public Stop GetStop(int id)
        {
            return _context.Stops.SingleOrDefault(s => s.Id == id);
        }

        public void AddStop(string tripName, Stop newStop, string username)
        {
            var trip = GetUserTripByName(tripName, username);

            if (trip != null)
            {
                var currentStops = _context.Stops
                    .Where(s => s.TripId == trip.Id)
                    .Count();

                // Automatically increment order of Stops
                newStop.Order = (currentStops == 0) ? 1 : currentStops + 1;
                // Foreign Key being set
                trip.Stops.Add(newStop);
                // Added as new object
                _context.Stops.Add(newStop);
            }
        }

        private Trip GetUserTripByName(string tripName, string username)
        {
            return _context.Trips
                .Include(t => t.Stops)
                .Where(t => t.Name == tripName && t.UserName == username)
                .FirstOrDefault();
        }

        public void RemoveStop(Stop stop)
        {
            _context.Stops.Remove(stop);
        }
    }
}
