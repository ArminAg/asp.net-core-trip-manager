using asp.net_core_trip_manager.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace asp.net_core_trip_manager.Persistence.Services
{
    public class GMapsGeoCoordsService : IGeoCoordsService
    {
        private IConfigurationRoot _config;
        private ILogger<GMapsGeoCoordsService> _logger;

        public GMapsGeoCoordsService(ILogger<GMapsGeoCoordsService> logger, IConfigurationRoot config)
        {
            _logger = logger;
            _config = config;
        }

        public async Task<GeoCoordsResult> GetCoordsAsync(string name)
        {
            var result = new GeoCoordsResult
            {
                Success = false,
                Message = "Failed to get coordinates"
            };

            var apiKey = _config["Keys:GMapsKey"];
            var encodedName = WebUtility.UrlEncode(name);
            var url = $"https://maps.googleapis.com/maps/api/geocode/json?address={encodedName}&key={apiKey}";

            var client = new HttpClient();

            var json = await client.GetStringAsync(url);

            var results = JObject.Parse(json);
            var resources = results["results"][0];
            if (!results["results"][0].HasValues)
            {
                result.Message = $"Could not find '{name}' as a location";
            }
            else
            {
                var coords = resources["geometry"]["location"];
                result.Latitude = (double)coords["lat"];
                result.Longitude = (double)coords["lng"];
                result.Success = true;
                result.Message = "Success";
            }

            return result;
        }
    }
}
