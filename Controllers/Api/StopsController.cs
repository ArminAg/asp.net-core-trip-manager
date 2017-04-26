using asp.net_core_trip_manager.Core.Dtos;
using asp.net_core_trip_manager.Core.Models;
using asp.net_core_trip_manager.Core.Repositories;
using asp.net_core_trip_manager.Core.Services;
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
    [Route("/api/trips/{tripName}/stops")]
    public class StopsController : Controller
    {
        private IGeoCoordsService _coordsService;
        private ILogger<StopsController> _logger;
        private IUnitOfWork _unitOfWork;

        public StopsController(IUnitOfWork unitOfWork, ILogger<StopsController> logger, IGeoCoordsService coordsService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _coordsService = coordsService;

        }

        [HttpGet("")]
        public IActionResult Get(string tripName)
        {
            try
            {
                var stops = _unitOfWork.Stops.GetAllStopsForUsersTrip(tripName, User.Identity.Name);
                return Ok(Mapper.Map<IEnumerable<StopDto>>(stops));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get stops: {ex}");
            }

            return BadRequest("Failed to get stops");
        }

        [HttpPost("")]
        public async Task<IActionResult> Post(string tripName, [FromBody]StopDto dto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newStop = Mapper.Map<Stop>(dto);

                    // Lookup the Geocodes
                    var result = await _coordsService.GetCoordsAsync(newStop.Name);
                    if (!result.Success)
                    {
                        _logger.LogError(result.Message);
                    }
                    else
                    {
                        newStop.Latitude = result.Latitude;
                        newStop.Longitude = result.Longitude;

                        _unitOfWork.Stops.AddStop(tripName, newStop, User.Identity.Name);

                        if (await _unitOfWork.CompleteAsync())
                        {
                            return Created($"api/trips/{tripName}/stops/{newStop.Name}",
                            Mapper.Map<StopDto>(newStop));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to save new Stop: {ex}");
            }
            return BadRequest("Failed to save the stop");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var stop = _unitOfWork.Stops.GetStop(Convert.ToInt32(id));

                if (stop == null)
                    return NotFound();

                _unitOfWork.Stops.RemoveStop(stop);

                if (await _unitOfWork.CompleteAsync())
                    return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to delete the Stop: {ex}");
            }
            return BadRequest("Failed to delete the Stop");
        }
    }
}
