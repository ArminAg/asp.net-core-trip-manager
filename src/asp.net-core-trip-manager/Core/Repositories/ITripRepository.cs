using System.Collections.Generic;
using System.Threading.Tasks;

namespace asp.net_core_trip_manager.Core.Models
{
    public interface ITripRepository
    {
        IEnumerable<Trip> GetAllTrips();
        IEnumerable<Trip> GetTripsByUsername(string name);
        Trip GetTripByName(string tripName);
        Trip GetUserTripByName(string tripName, string username);
        void Add(Trip trip);
        void AddStop(string tripName, Stop newStop, string username);
        Task<bool> SaveChangesAsync();
    }
}