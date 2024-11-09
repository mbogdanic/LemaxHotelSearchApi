using LemaxHotelSearchApi.Models;
using LemaxHotelSearchApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LemaxHotelSearchApi.Controllers
{
    [Route("api/hotels/search")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly IHotelService _hotelService;

        public SearchController(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }

        /// <summary>
        /// Searches for hotels near a specified geographical location and supports pagination.
        /// </summary>
        /// <param name="latitude">The latitude of the user's location (should be between -90 and 90).</param>
        /// <param name="longitude">The longitude of the user's location (should be between -180 and 180).</param>
        /// <param name="page">The page number for pagination (defaults to 1).</param>
        /// <param name="pageSize">The number of hotels per page (defaults to 10).</param>
        [HttpGet]
        public ActionResult<List<Hotel>> Search([FromQuery] double latitude, [FromQuery] double longitude, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (latitude < 90 || latitude > 90 || longitude < -180 || longitude > 180)
            {
                return BadRequest("Invalid latitude or longitude values");
            }

            if (page <= 0 || pageSize <= 0)
            {
                return BadRequest("Page and page size values are invalid.");
            }

            var currentLocation = new GeoLocation
            {
                Latitude = latitude,
                Longitude = longitude
            };

            var hotels = _hotelService.GetAllHotels().
                Select(hotel => new
                {
                    Hotel = hotel,
                    Distance = hotel.Location.DistanceTo(currentLocation)
                })
                .OrderBy(h => h.Hotel.Price)
                .ThenBy(h => h.Distance)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(h => new Hotel
                {
                    Id = h.Hotel.Id,
                    Name = h.Hotel.Name,
                    Price = h.Hotel.Price,
                    Location = h.Hotel.Location
                })
                .ToList();

            return hotels;
        }
    }
}
