using asp.net_core_trip_manager.Dtos;
using asp.net_core_trip_manager.Models;
using asp.net_core_trip_manager.ViewModels;
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
        public IActionResult Post([FromBody]TripDto trip)
        {
            if (ModelState.IsValid)
            {
                return Created($"api/trips/{trip.Name}", trip);
            }
            return BadRequest(ModelState);
        }
    }
}
