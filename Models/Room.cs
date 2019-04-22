using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace HotelApp.Models
{
    public class Room
    {
        public int RoomId { get; set; }
        public int HotelId { get; set; }
        [Required]
        [MaxLength(50)]
        public string RoomName { get; set; }
        [Required]
        public int MaxOccupancy { get; set; }
        [Required]
        public double NetRate { get; set; }
        public double SellRate { get; set; }
        [Required]
        public int CurrencyId { get; set; }

        public List<RoomAmenities> RoomAmenitieses { get; set; }
        public List<BookingRoom> BookingRooms { get; set; }

        public Hotel Hotel { get; set; }
        public Currency Currency { get; set; }
    }
}