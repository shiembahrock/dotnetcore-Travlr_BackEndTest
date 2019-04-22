using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HotelApp.Models;

namespace HotelApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly MvcHotelContext db;

        public HomeController(MvcHotelContext context)
        {
            db = context;
        }

        public IActionResult Index()
        {
            List<Hotel> oHotelList = db.Hotel.ToList();

            foreach (Hotel i_oHotel in oHotelList)
            {
                i_oHotel.Rooms = db.Room.Where(w => w.HotelId == i_oHotel.HotelId).ToList();

                foreach (Room i_oRoom in i_oHotel.Rooms)
                {
                    i_oRoom.RoomAmenitieses = db.RoomAmenities.Where(w => w.RoomId == i_oRoom.RoomId).ToList();

                    foreach (RoomAmenities i_oRoomAmenities in i_oRoom.RoomAmenitieses)
                    {
                        i_oRoomAmenities.Amenities = db.Amenities
                            .Where(w => w.AmenitiesId == i_oRoomAmenities.AmenitiesId).FirstOrDefault();
                    }
                } 
            }

            return View(oHotelList);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
