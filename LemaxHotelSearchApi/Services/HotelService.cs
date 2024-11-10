using LemaxHotelSearchApi.Models;
using System.Text.Json;

namespace LemaxHotelSearchApi.Services
{
    public class HotelService : IHotelService
    {
        private readonly List<Hotel> _hotels = new();
        private readonly string _filePath;

        public HotelService()
        {
            _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Mock", "mockHotels.json");
            if (!File.Exists(_filePath))
            {
                throw new FileNotFoundException("The mockHotels.json file could not be found.");
            }

            _hotels = GetHotelsFromMock();
        }

        public List<Hotel> GetAllHotels()
        {
            return _hotels;
        }

        public Hotel GetHotelById(int id)
        {
              return _hotels.FirstOrDefault(h => h.Id == id) ?? throw new KeyNotFoundException($"Hotel with ID {id} does not exist.");
        }

        public void AddHotel(Hotel hotel)
        {
            if (hotel == null)
            {
                throw new ArgumentNullException(nameof(hotel), "Hotel data is empty.");
            }

            int nextId = _hotels.Any() ? _hotels.Max(x => x.Id) + 1 : 0;
            hotel.Id = nextId;
            _hotels.Add(hotel);
            SaveHotelsToFile();
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
            SaveHotelsToFile();
        }

        public void DeleteHotel(int id)
        {
            var hotel = GetHotelById(id);
            if (hotel == null)
            {
                throw new KeyNotFoundException("Hotel with the given ID does not exist.");
            }

            _hotels.RemoveAll(h => h.Id == id);
            SaveHotelsToFile();
        }

        public List<Hotel> GetHotelsFromMock()
        {
            string json = File.ReadAllText(_filePath);
            List<Hotel> hotels = JsonSerializer.Deserialize<List<Hotel>>(json);

            if (hotels != null)
            {
                hotels.ForEach(x => _hotels.Add(x));
            }
            else
            {
                throw new ArgumentNullException(nameof(hotels), "Hotel data is empty.");
            }

            return hotels;
        }

        private void SaveHotelsToFile()
        {
            try
            {
                string json = JsonSerializer.Serialize(_hotels, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_filePath, json);
            }
            catch (Exception ex)
            {
                throw new IOException($"Failed to save hotels to file: {ex.Message}", ex);
            }
        }
    }
}
