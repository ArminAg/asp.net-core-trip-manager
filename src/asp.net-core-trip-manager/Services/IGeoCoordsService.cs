using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asp.net_core_trip_manager.Services
{
    public interface IGeoCoordsService
    {
        Task<GeoCoordsResult> GetCoordsAsync(string name);
    }
}
