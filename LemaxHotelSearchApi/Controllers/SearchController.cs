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
        private readonly HotelService _hotelService;

        public SearchController(HotelService hotelService)
        {
            _hotelService = hotelService;
        }

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
