using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asp.net_core_trip_manager.Core.Models
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime FirstTrip { get; set; }
    }
}
