using LemaxHotelSearchApi.Models;
using LemaxHotelSearchApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using LemaxHotelSearchApi.Security;

namespace LemaxHotelSearchApi.Controllers
{
    /// <summary>
    /// Controller responsible for handling hotel-related requests.
    /// Provides CRUD operations on hotel data.
    /// </summary>
    [Route("api/hotels")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private readonly JwtAuthenticationManager _jwtAuthenticationManager;
        private readonly IHotelService _hotelService;

        public HotelController(JwtAuthenticationManager jwtAuthenticationManager, IHotelService hotelService)
        {
            _jwtAuthenticationManager = jwtAuthenticationManager;
            _hotelService = hotelService;
        }

        /// <summary>
        /// Retrieves a list of all hotels.
        /// </summary>
        [HttpGet]
        public ActionResult<List<Hotel>> Get()
        {
            var hotels = _hotelService.GetAllHotels();

            return hotels;
        }

        /// <summary>
        /// Retrieves the details of a specific hotel by its ID.
        /// This endpoint requires authorization.
        /// </summary>
        /// <param name="id">The ID of the hotel to retrieve.</param>
        [Authorize]
        [HttpGet("{id:int}")]
        public ActionResult<Hotel> Get(int id)
        {
            var hotel = _hotelService.GetHotelById(id);
            if (hotel == null)
            {
                return NotFound("Hotel not found.");
            }

            return hotel;
        }

        /// <summary>
        /// Adds a new hotel to the system.
        /// This endpoint requires authorization.
        /// </summary>
        /// <param name="hotel">The hotel to be added.</param>
        [Authorize]
        [HttpPost]
        public ActionResult Add(Hotel hotel)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            _hotelService.AddHotel(hotel);
            return CreatedAtAction(nameof(Get), new { id = hotel.Id }, hotel);
        }

        /// <summary>
        /// Updates the details of an existing hotel.
        /// This endpoint requires authorization.
        /// </summary>
        /// <param name="id">The ID of the hotel to update.</param>
        /// <param name="updatedHotel">The updated hotel information.</param>
        [Authorize]
        [HttpPut("{id:int}")]
        public IActionResult Update(int id, Hotel updatedHotel)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _hotelService.UpdateHotel(id, updatedHotel);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Hotel not found");
            }
        }

        /// <summary>
        /// Deletes a hotel by its ID.
        /// This endpoint requires authorization.
        /// </summary>
        /// <param name="id">The ID of the hotel to delete.</param>
        [Authorize]
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _hotelService.DeleteHotel(id);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Authenticates a user and returns a JWT token.
        /// This endpoint does not require authorization (allow anonymous).
        /// </summary>
        /// <param name="user">The user credentials for authentication.</param>
        [AllowAnonymous]
        [HttpPost("Authorize")]
        public IActionResult AuthUser([FromBody] User user)
        {
            var token = _jwtAuthenticationManager.Authenticate(user.Username, user.Password);

            if (token == null)
            {
                return Unauthorized();
            }

            return Ok(token);
        }
    }
}
