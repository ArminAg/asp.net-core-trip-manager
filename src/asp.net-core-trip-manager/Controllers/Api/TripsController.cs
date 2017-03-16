using asp.net_core_trip_manager.Dtos;
using asp.net_core_trip_manager.Models;
using asp.net_core_trip_manager.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asp.net_core_trip_manager.Controllers.Api
{
    [Route("api/trips")]
    public class TripsController : Controller
    {
        private ILogger<TripsController> _logger;
        private ITripRepository _repository;

        public TripsController(ITripRepository repository, ILogger<TripsController> logger)
        {
            _repository = repository;
            _logger = logger;
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
                _logger.LogError($"Failed to get all Trips: {ex}");
                return BadRequest("Error occured");
            }
        }

        [HttpPost("")]
        public async Task<IActionResult> Post([FromBody]TripDto trip)
        {
            if (ModelState.IsValid)
            {
                var newTrip = Mapper.Map<Trip>(trip);
                _repository.Add(newTrip);

                if (await _repository.SaveChangesAsync())
                    return Created($"api/trips/{trip.Name}", Mapper.Map<TripDto>(newTrip));
                
            }
            return BadRequest("Failed to save the trip");
        }
    }
}
