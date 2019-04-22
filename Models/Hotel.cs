using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace HotelApp.Models
{
    public class Hotel
    {
        public int HotelId { get; set; }
        [Required]
        [MaxLength(50)]
        public string HotelName { get; set; }
        [MaxLength(255)]
        public string Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        [Required]
        public double CommissionRate { get; set; }
        
        public List<Room> Rooms { get; set; }        
    }
}