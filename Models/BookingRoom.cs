using System;
using System.ComponentModel.DataAnnotations;

namespace HotelApp.Models
{
    public class BookingRoom
    {
        public int BookingRoomId { get; set; }    
        [MaxLength(50)]    
        public string BookingCode { get; set; }
        public int ClientId { get; set; }
        [Required]
        public int RoomId { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime CheckInDate {get; set; }
        [DataType(DataType.DateTime)]
        public DateTime CheckOutDate { get; set; }
        [Required]
        [MaxLength(7)]
        public string UniqueCode { get; set; }

        public Room Room { get; set; }
    }
}