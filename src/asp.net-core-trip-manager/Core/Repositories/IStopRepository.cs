using asp.net_core_trip_manager.Core.Models;

namespace asp.net_core_trip_manager.Core.Repositories
{
    public interface IStopRepository
    {
        Stop GetStop(int id);
        Trip GetUserTripByName(string tripName, string username);
        void AddStop(string tripName, Stop newStop, string username);
        void RemoveStop(Stop stop);
    }
}