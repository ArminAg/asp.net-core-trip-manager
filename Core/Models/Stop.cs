using System;

namespace asp.net_core_trip_manager.Core.Models
{
    public class Stop
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int Order { get; set; }
        public DateTime Arrival { get; set; }

        // Add Foreign Key Association
        public int TripId { get; set; }
        public Trip Trip { get; set; }
    }
}