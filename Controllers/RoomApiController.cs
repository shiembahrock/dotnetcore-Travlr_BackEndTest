using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HotelApp.Models;

namespace HotelApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomApiController : ControllerBase
    {
        private readonly MvcHotelContext db;

        public RoomApiController(MvcHotelContext context)
        {
            db = context;
        }

        // GET api/values
        [HttpGet]        
        public ActionResult<List<Room>> Get()        
        {
            List<Room> oRoom = db.Room.ToList();
            
            return oRoom;
        }
        
        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<Room> Get(int id)
        {
            Room oRoom = db.Room.Where(w => w.RoomId == id).FirstOrDefault();
            
            return oRoom;
        }

        // POST api/values
        [HttpPost]
        // public void Post([FromBody] string value)
        public void Post([FromBody] Room p_oRoom)
        {
            bool bIsSuccess = false;

            using(var oTrans = db.Database.BeginTransaction())
            {
                try
                {                
                    Room oRoomAdd = new Room();

                    oRoomAdd.RoomName = p_oRoom.RoomName;
                    oRoomAdd.HotelId = p_oRoom.HotelId;
                    oRoomAdd.MaxOccupancy = p_oRoom.MaxOccupancy;                        
                    oRoomAdd.NetRate = p_oRoom.NetRate;
                    oRoomAdd.SellRate = p_oRoom.SellRate;
                    oRoomAdd.CurrencyId = p_oRoom.CurrencyId;
                    
                    db.Room.Add(oRoomAdd);
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

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Room p_oRoom)
        {
            bool bIsSuccess = false;

            using(var oTrans = db.Database.BeginTransaction())
            {
                try
                {
                    Room oRoomUpdate = db.Room.Find(id);

                    oRoomUpdate.RoomName = p_oRoom.RoomName;
                    oRoomUpdate.HotelId = p_oRoom.HotelId;
                    oRoomUpdate.MaxOccupancy = p_oRoom.MaxOccupancy;                        
                    oRoomUpdate.NetRate = p_oRoom.NetRate;
                    oRoomUpdate.SellRate = p_oRoom.SellRate;
                    oRoomUpdate.CurrencyId = p_oRoom.CurrencyId;

                    db.Update(oRoomUpdate);
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
                    #region Deleting Child [RoomAmenities]                    
                    List<RoomAmenities> oRoomAmenitiesList =
                        db.RoomAmenities.Where(w => w.RoomId == id).ToList();
                    
                    db.RoomAmenities.RemoveRange(oRoomAmenitiesList);
                    db.SaveChanges();                    
                    #endregion

                    #region Deleting Room
                    Room oRoomDeleted = db.Room.Find(id);
                    db.Room.Remove(oRoomDeleted);
                    db.SaveChanges();
                    #endregion
                    
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
