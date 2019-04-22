using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace HotelApp.Models
{
    public class Amenities
    {
        public int AmenitiesId { get; set; }  
        [Required]  
        [MaxLength(50)]    
        public string AmenitiesName { get; set; }
        [Required]
        public bool IsActive { get; set; }

        public List<RoomAmenities> RoomAmenitieses { get; set; }
    }
}