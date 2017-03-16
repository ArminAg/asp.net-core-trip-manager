using System.Collections.Generic;
using System.Threading.Tasks;

namespace asp.net_core_trip_manager.Models
{
    public interface ITripRepository
    {
        IEnumerable<Trip> GetAllTrips();

        void Add(Trip trip);
        Task<bool> SaveChangesAsync();
    }
}