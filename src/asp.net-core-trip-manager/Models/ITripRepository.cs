using System.Collections.Generic;
using System.Threading.Tasks;

namespace asp.net_core_trip_manager.Models
{
    public interface ITripRepository
    {
        IEnumerable<Trip> GetAllTrips();
        Trip GetTripByName(string tripName);
        void Add(Trip trip);
        void AddStop(string tripName, Stop newStop);
        Task<bool> SaveChangesAsync();
    }
}