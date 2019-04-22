using System;
using System.ComponentModel.DataAnnotations;

namespace HotelApp.Models
{
    public class RoomAmenities
    {
        public int RoomAmenitiesId { get; set; }
        [Required]
        public int RoomId { get; set; }
        [Required]
        public int AmenitiesId { get; set; }

        public Room Room { get; set; }
        public Amenities Amenities { get; set; }
    }
}