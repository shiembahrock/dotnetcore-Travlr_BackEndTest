using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HotelApp.Models
{
    public class vmHotel
    {
        public Hotel oHotel { get; set; }
        public List<Room> oRoomList { get; set; }
        public List<RoomAmenities> oRoomAmenitiesList { get; set; }
    }

    public class vmRoom
    {        
        public Room oRoom { get; set; }
        public List<Amenities> oAmenitiesList { get; set; }
        // public List<RoomAmenities> oRoomAmenitiesList { get; set; }        
    }        
}