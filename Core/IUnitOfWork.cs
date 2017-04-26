using asp.net_core_trip_manager.Core.Repositories;
using System.Threading.Tasks;

namespace asp.net_core_trip_manager.Persistence
{
    public interface IUnitOfWork
    {
        ITripRepository Trips { get; }

        IStopRepository Stops { get; }

        Task<bool> CompleteAsync();
    }
}