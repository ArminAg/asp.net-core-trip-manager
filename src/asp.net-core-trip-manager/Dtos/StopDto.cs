using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace asp.net_core_trip_manager.Dtos
{
    public class StopDto
    {
        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        [Required]
        public int Order { get; set; }
        [Required]
        public DateTime Arrival { get; set; }
    }
}
