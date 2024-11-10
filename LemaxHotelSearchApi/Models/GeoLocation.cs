using System.ComponentModel.DataAnnotations;

namespace LemaxHotelSearchApi.Models
{
    public class GeoLocation
    {
        [Range(-90, 90, ErrorMessage = "Latitude must be between -90 and 90.")]
        public double Latitude { get; set; }

        [Range(-180, 180, ErrorMessage = "Longitude must be between -180 and 180.")]
        public double Longitude { get; set; }

        /// <summary>
        /// Creates a new instance of GeoLocation
        /// </summary>
        /// <param name="latitude">Latitude in degrees (-90 to 90)</param>
        /// <param name="longitude">Longitude in degrees (-180 to 180)</param>
        public double DistanceTo(GeoLocation otherLocation)
        {
            if (otherLocation == null)
            {
                throw new ArgumentNullException(nameof(otherLocation));
            }

            //Haversine formula
            double R = 6371;
            var dLat = (otherLocation.Latitude - Latitude) * (Math.PI / 180);
            var dLon = (otherLocation.Longitude - Longitude) * (Math.PI / 180);
            var lat1 = Latitude * (Math.PI / 180);
            var lat2 = otherLocation.Latitude * (Math.PI / 180);

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2) * Math.Cos(lat1) * Math.Cos(lat2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c; // Distance in kilometers
        }
    }
}
