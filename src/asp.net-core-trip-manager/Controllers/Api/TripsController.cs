using asp.net_core_trip_manager.Dtos;
using asp.net_core_trip_manager.Models;
using asp.net_core_trip_manager.ViewModels;
using AutoMapper;
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
            try
            {
                var trips = _repository.GetAllTrips();
                return Ok(Mapper.Map<IEnumerable<TripDto>>(trips));
            }
            catch (Exception ex)
            {
                // TODO Logging
                return BadRequest("Error occured");
            }
        }

        [HttpPost("")]
        public IActionResult Post([FromBody]TripDto trip)
        {
            if (ModelState.IsValid)
            {
                var newTrip = Mapper.Map<Trip>(trip);
                return Created($"api/trips/{trip.Name}", Mapper.Map<TripDto>(newTrip));
            }
            return BadRequest(ModelState);
        }
    }
}
