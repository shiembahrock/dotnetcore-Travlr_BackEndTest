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
    public class RoomAmenitiesApiController : ControllerBase
    {
        private readonly MvcHotelContext db;

        public RoomAmenitiesApiController(MvcHotelContext context)
        {
            db = context;
        }

        // GET api/values
        [HttpGet]        
        public ActionResult<List<RoomAmenities>> Get()        
        {
            List<RoomAmenities> oRoomAmenities = db.RoomAmenities.ToList();
            
            return oRoomAmenities;
        }
        
        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<RoomAmenities> Get(int id)
        {
            RoomAmenities oRoomAmenities = db.RoomAmenities.Where(w => w.RoomAmenitiesId == id).FirstOrDefault();
            
            return oRoomAmenities;
        }

        // POST api/values
        [HttpPost]
        // public void Post([FromBody] string value)
        public void Post([FromBody] RoomAmenities p_oRoomAmenities)
        {
            bool bIsSuccess = false;

            using(var oTrans = db.Database.BeginTransaction())
            {
                try
                {                
                    RoomAmenities oRoomAmenitiesAdd = new RoomAmenities();

                    oRoomAmenitiesAdd.RoomId = p_oRoomAmenities.RoomId;
                    oRoomAmenitiesAdd.AmenitiesId = p_oRoomAmenities.AmenitiesId;
                                        
                    db.RoomAmenities.Add(oRoomAmenitiesAdd);
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
        public void Put(int id, [FromBody] RoomAmenities p_oRoomAmenities)
        {
            bool bIsSuccess = false;

            using(var oTrans = db.Database.BeginTransaction())
            {
                try
                {
                    RoomAmenities oRoomAmenitiesUpdate = db.RoomAmenities.Find(id);

                    oRoomAmenitiesUpdate.RoomId = p_oRoomAmenities.RoomId;
                    oRoomAmenitiesUpdate.AmenitiesId = p_oRoomAmenities.AmenitiesId;

                    db.Update(oRoomAmenitiesUpdate);
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
                    #region Deleting RoomAmenities
                    RoomAmenities oRoomAmenitiesDelete = db.RoomAmenities.Find(id);
                    
                    db.RoomAmenities.Remove(oRoomAmenitiesDelete);
                    bIsSuccess = db.SaveChanges() > 0 ? true : false;
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
