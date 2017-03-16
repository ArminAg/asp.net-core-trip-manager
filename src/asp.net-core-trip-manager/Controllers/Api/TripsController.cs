using asp.net_core_trip_manager.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asp.net_core_trip_manager.Controllers.Api
{
    [Route("api/trips")]
    public class TripsController : Controller
    {
        private ITripRepository _repository;

        public TripsController(ITripRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("")]
        public IActionResult Get()
        {
            return Ok(_repository.GetAllTrips());
        }

        [HttpPost("")]
        public IActionResult Post([FromBody]Trip trip)
        {
            return Ok(true);
        }
    }
}
