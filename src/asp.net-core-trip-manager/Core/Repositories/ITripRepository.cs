using asp.net_core_trip_manager.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace asp.net_core_trip_manager.Core.Repositories
{
    public interface ITripRepository
    {
        IEnumerable<Trip> GetAllTrips();
        IEnumerable<Trip> GetTripsByUsername(string name);
        Trip GetTripById(int id);
        Trip GetTripByName(string tripName);
        void Add(Trip trip);
        void RemoveTrip(Trip trip);
    }
}