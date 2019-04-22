using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HotelApp.Models;
using Newtonsoft.Json;

namespace HotelApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingRoomApiController : ControllerBase
    {
        private readonly MvcHotelContext db;

        public BookingRoomApiController(MvcHotelContext context)
        {
            db = context;
        }

        // GET api/values
        [HttpGet]        
        public ActionResult<List<BookingRoom>> Get()              
        {                        
            List<BookingRoom> oBookingRoom = db.BookingRoom.ToList();
                                                
            return oBookingRoom;
        }
        
        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<BookingRoom> Get(int id)
        {
            BookingRoom oBookingRoom = db.BookingRoom.Where(w => w.BookingRoomId == id).FirstOrDefault();
                        
            return oBookingRoom;            
        }

        // POST api/values
        [HttpPost]        
        public void Post([FromBody] BookingRoom p_oBookingRoom)        
        {
            bool bIsSuccess = false;

            using(var oTrans = db.Database.BeginTransaction())
            {
                try
                {                    
                    BookingRoom oBookingRoomAdd = new BookingRoom();

                    oBookingRoomAdd.BookingCode = p_oBookingRoom.BookingCode;
                    oBookingRoomAdd.ClientId = p_oBookingRoom.ClientId;
                    oBookingRoomAdd.RoomId = p_oBookingRoom.RoomId;
                    oBookingRoomAdd.CheckInDate = p_oBookingRoom.CheckInDate;
                    oBookingRoomAdd.CheckOutDate = p_oBookingRoom.CheckOutDate;
                    oBookingRoomAdd.UniqueCode = p_oBookingRoom.UniqueCode;
                    
                    db.BookingRoom.Add(oBookingRoomAdd);
                    db.SaveChanges();                                         

                    bIsSuccess = true;
                }
                catch(Exception Ex)
                {
                    oTrans.Rollback();
                }
                finally
                {
                    if(bIsSuccess)
                    {
                        oTrans.Commit();
                    }
                }
            }            
        }
        
        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] BookingRoom p_oBookingRoom)
        {
            bool bIsSuccess = false;

            using(var oTrans = db.Database.BeginTransaction())
            {
                try
                {
                    BookingRoom oBookingRoomUpdate = db.BookingRoom.Find(id);

                    oBookingRoomUpdate.BookingCode = p_oBookingRoom.BookingCode;
                    oBookingRoomUpdate.ClientId = p_oBookingRoom.ClientId;
                    oBookingRoomUpdate.RoomId = p_oBookingRoom.RoomId;
                    oBookingRoomUpdate.CheckInDate = p_oBookingRoom.CheckInDate;
                    oBookingRoomUpdate.CheckOutDate = p_oBookingRoom.CheckOutDate;
                    oBookingRoomUpdate.UniqueCode = p_oBookingRoom.UniqueCode;

                    db.Update(oBookingRoomUpdate);
                    bIsSuccess = db.SaveChanges() > 0 ? true : false;                    
                }
                catch(Exception Ex)
                {
                    oTrans.Rollback();
                }
                finally
                {
                    if(bIsSuccess)
                    {
                        oTrans.Commit();
                    }
                }
            }
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            bool bIsSuccess = false;

            using(var oTrans = db.Database.BeginTransaction())
            {
                try
                {
                    BookingRoom oBookingRoomDeleted = db.BookingRoom.Find(id);
                    
                    db.BookingRoom.Remove(oBookingRoomDeleted);
                    db.SaveChanges();

                    bIsSuccess = true;
                }
                catch(Exception Ex)
                {
                    oTrans.Rollback();
                }
                finally
                {
                    if(bIsSuccess)
                    {
                        oTrans.Commit();
                    }
                }
            }
        }
    }
}
