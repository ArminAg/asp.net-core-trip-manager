using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace asp.net_core_trip_manager.Dtos
{
    public class TripDto
    {
        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string Name { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    }
}
