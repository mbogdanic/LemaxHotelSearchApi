using LemaxHotelSearchApi.Models;
using LemaxHotelSearchApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LemaxHotelSearchApi.Controllers
{
    [Route("api/hotels")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private readonly HotelService _hotelService;

        public HotelController(HotelService hotelService)
        {
            _hotelService = hotelService;
        }

        [HttpGet]
        public ActionResult<List<Hotel>> Get()
        {
            return _hotelService.GetAllHotels();
        }

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
    }
}
