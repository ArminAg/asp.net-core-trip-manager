using System.Collections.Generic;

namespace asp.net_core_trip_manager.Models
{
    public interface ITripRepository
    {
        IEnumerable<Trip> GetAllTrips();
    }
}