using LemaxHotelSearchApi.Models;

namespace LemaxHotelSearchApi.Services
{
    public class HotelService
    {
        private readonly List<Hotel> _hotels = new();
        private int _nextId = 1;

        public List<Hotel> GetAllHotels()
        {
            return _hotels;
        }

        public Hotel GetHotelById(int id)
        {
             return _hotels.FirstOrDefault(h => h.Id == id);
        }

        public void AddHotel(Hotel hotel)
        {
            if (hotel == null)
            {
                throw new ArgumentNullException(nameof(hotel), "Hotel data is empty.");
            }

            hotel.Id = _nextId++;
             _hotels.Add(hotel);
        }

        public void UpdateHotel(int id, Hotel updatedHotel)
        {
            var hotel = GetHotelById(id);
            if (hotel == null)
            {
                throw new KeyNotFoundException("Hotel with the given ID does not exist.");
            }

            hotel.Name = updatedHotel.Name;
            hotel.Price = updatedHotel.Price;
            hotel.Location = updatedHotel.Location;
        }

        public void DeleteHotel(int id)
        {
            var hotel = GetHotelById(id);
            if (hotel == null)
            {
                throw new KeyNotFoundException("Hotel with the given ID does not exist.");
            }

            _hotels.RemoveAll(h => h.Id == id);
        }
    }
}
