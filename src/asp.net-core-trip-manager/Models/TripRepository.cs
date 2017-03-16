using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asp.net_core_trip_manager.Models
{
    public class TripRepository : ITripRepository
    {
        private TripContext _context;

        public TripRepository(TripContext context)
        {
            _context = context;
        }

        public IEnumerable<Trip> GetAllTrips()
        {
            return _context.Trips.ToList();
        }
    }
}
