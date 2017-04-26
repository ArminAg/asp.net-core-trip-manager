using asp.net_core_trip_manager.Core.Models;
using System.Collections.Generic;

namespace asp.net_core_trip_manager.Core.Repositories
{
    public interface IStopRepository
    {
        IEnumerable<Stop> GetAllStopsForUsersTrip(string tripName, string username);
        Stop GetStop(int id);
        void AddStop(string tripName, Stop newStop, string username);
        void RemoveStop(Stop stop);
    }
}