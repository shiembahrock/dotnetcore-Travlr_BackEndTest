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
    public class HotelApiController : ControllerBase
    {
        private readonly MvcHotelContext db;

        public HotelApiController(MvcHotelContext context)
        {
            db = context;
        }

        // GET api/values
        [HttpGet]        
        public ActionResult<List<Hotel>> Get()              
        {                        
            List<Hotel> oHotel = db.Hotel.ToList();
                                                
            return oHotel;
        }
        
        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<Hotel> Get(int id)
        {
            Hotel oHotel = db.Hotel.Where(w => w.HotelId == id).FirstOrDefault();
                        
            return oHotel;            
        }

        // POST api/values
        [HttpPost]        
        public void Post([FromBody] Hotel p_oHotel)        
        {
            bool bIsSuccess = false;

            using(var oTrans = db.Database.BeginTransaction())
            {
                try
                {                    
                    Hotel oHotelAdd = new Hotel();

                    oHotelAdd.HotelName = p_oHotel.HotelName;
                    oHotelAdd.Address = p_oHotel.Address;
                    oHotelAdd.Latitude = p_oHotel.Latitude;
                    oHotelAdd.Longitude = p_oHotel.Longitude;
                    oHotelAdd.CommissionRate = p_oHotel.CommissionRate;
                    
                    db.Hotel.Add(oHotelAdd);
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

        // // POST api/values
        // [HttpPost]        
        // public void Post([FromBody] Hotel[] oHotel)        
        // {
        //     bool bIsSuccess = false;

        //     using(var oTrans = db.Database.BeginTransaction())
        //     {
        //         try
        //         {
        //             foreach (Hotel i_oHotel in oHotel)
        //             {
        //                 Hotel oHotelAdd = new Hotel();

        //                 oHotelAdd.HotelName = i_oHotel.HotelName;
        //                 oHotelAdd.Address = i_oHotel.Address;
        //                 oHotelAdd.Latitude = i_oHotel.Latitude;
        //                 oHotelAdd.Longitude = i_oHotel.Longitude;
        //                 oHotelAdd.CommissionRate = i_oHotel.CommissionRate;
                        
        //                 db.Hotel.Add(oHotelAdd);
        //                 db.SaveChanges();    
        //             }                    

        //             bIsSuccess = true;
        //         }
        //         catch(Exception Ex)
        //         {
        //             oTrans.Rollback();
        //         }
        //         finally
        //         {
        //             if(bIsSuccess)
        //             {
        //                 oTrans.Commit();
        //             }
        //         }
        //     }            
        // }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Hotel p_oHotel)
        {
            bool bIsSuccess = false;

            using(var oTrans = db.Database.BeginTransaction())
            {
                try
                {
                    Hotel oHotelUpdate = db.Hotel.Find(id);

                    oHotelUpdate.HotelName = p_oHotel.HotelName;
                    oHotelUpdate.Address = p_oHotel.Address;
                    oHotelUpdate.Latitude = p_oHotel.Latitude;
                    oHotelUpdate.Longitude = p_oHotel.Longitude;
                    oHotelUpdate.CommissionRate = p_oHotel.CommissionRate;

                    db.Update(oHotelUpdate);
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
                    Hotel oHotelDeleted = db.Hotel.Find(id);

                    #region Deleting Child [Room]
                    List<Room> oRoomList = db.Room.Where(w => w.HotelId == oHotelDeleted.HotelId).ToList();

                    #region Deleting Child [RoomAmenities]
                    foreach(Room i_oRoom in oRoomList)
                    {
                        List<RoomAmenities> oRoomAmenitiesList =
                            db.RoomAmenities.Where(w => w.RoomId == i_oRoom.RoomId).ToList();
                        
                        db.RoomAmenities.RemoveRange(oRoomAmenitiesList);
                        db.SaveChanges();
                    }
                    #endregion

                    db.Room.RemoveRange(oRoomList);
                    db.SaveChanges();
                    #endregion

                    db.Hotel.Remove(oHotelDeleted);
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
