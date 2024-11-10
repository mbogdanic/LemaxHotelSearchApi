using System.ComponentModel.DataAnnotations;

namespace LemaxHotelSearchApi.Models
{
    /// <summary>
    /// Represents a hotel entity
    /// </summary>
    public class Hotel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Hotel name is mandatory.")]
        [StringLength(100, ErrorMessage = "Hotel name cannot be longer than 100 characters.")]
        public string Name { get; set; }

        [Range(0, 10000, ErrorMessage = "Price must be between 0 and 10,000.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Location is mandatory.")]
        public GeoLocation Location { get; set; }
    }

    /// <summary>
    /// Data Transfer Object for Hotel entity
    /// </summary>
    public class HotelDTO
    {
        [Required(ErrorMessage = "Hotel name is mandatory.")]
        [StringLength(100, ErrorMessage = "Hotel name cannot be longer than 100 characters.")]
        public string Name { get; set; }

        [Range(0, 10000, ErrorMessage = "Price must be between 0 and 10,000.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Location is mandatory.")]
        public GeoLocation Location { get; set; }
    }
}
