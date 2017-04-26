using asp.net_core_trip_manager.Core.Dtos;
using asp.net_core_trip_manager.Core.Models;
using asp.net_core_trip_manager.Persistence;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asp.net_core_trip_manager.Controllers.Api
{
    [Authorize]
    [Route("api/trips")]
    public class TripsController : Controller
    {
        private ILogger<TripsController> _logger;
        private IUnitOfWork _unitOfWork;

        public TripsController(IUnitOfWork unitOfWork, ILogger<TripsController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        [HttpGet("")]
        public IActionResult Get()
        {
            try
            {
                var trips = _unitOfWork.Trips.GetTripsByUsername(User.Identity.Name);
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
                newTrip.UserName = User.Identity.Name;
                _unitOfWork.Trips.Add(newTrip);

                if (await _unitOfWork.CompleteAsync())
                    return Created($"api/trips/{trip.Name}", Mapper.Map<TripDto>(newTrip));
                
            }
            return BadRequest("Failed to save the trip");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var trip = _unitOfWork.Trips.GetTripById(id);

                if (trip == null)
                    return NotFound();

                _unitOfWork.Trips.RemoveTrip(trip);
                if (await _unitOfWork.CompleteAsync())
                    return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to delete Trip: {ex}");
            }
            return BadRequest("Failed to delete Trip");
        }
    }
}
