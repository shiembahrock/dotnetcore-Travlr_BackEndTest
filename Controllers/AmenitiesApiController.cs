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
    public class AmenitiesApiController : ControllerBase
    {
        private readonly MvcHotelContext db;

        public AmenitiesApiController(MvcHotelContext context)
        {
            db = context;
        }

        // GET api/values
        [HttpGet]        
        public ActionResult<List<Amenities>> Get()        
        {
            List<Amenities> oAmenities = db.Amenities.ToList();
            
            return oAmenities;
        }
        
        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<Amenities> Get(int id)
        {
            Amenities oAmenities = db.Amenities.Where(w => w.AmenitiesId == id).FirstOrDefault();
            
            return oAmenities;
        }

        // POST api/values
        [HttpPost]
        // public void Post([FromBody] string value)
        public void Post([FromBody] Amenities p_oAmenities)
        {
            bool bIsSuccess = false;

            using(var oTrans = db.Database.BeginTransaction())
            {
                try
                {                
                    Amenities oAmenitiesAdd = new Amenities();

                    oAmenitiesAdd.AmenitiesName = p_oAmenities.AmenitiesName;                    
                    oAmenitiesAdd.IsActive = p_oAmenities.IsActive;
                    
                    db.Amenities.Add(oAmenitiesAdd);
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
        public void Put(int id, [FromBody] Amenities p_oAmenities)
        {
            bool bIsSuccess = false;

            using(var oTrans = db.Database.BeginTransaction())
            {
                try
                {
                    Amenities oAmenitiesUpdate = db.Amenities.Find(id);
                    
                    oAmenitiesUpdate.AmenitiesName = p_oAmenities.AmenitiesName;
                    oAmenitiesUpdate.IsActive = p_oAmenities.IsActive;

                    db.Update(oAmenitiesUpdate);
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
        /*
        This Delete Method only set [bool : IsActive] to 'false'
         */
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            bool bIsSuccess = false;

            using(var oTrans = db.Database.BeginTransaction())
            {
                try
                {   
                    #region Deleting Amenities
                    Amenities oAmenitiesUpdateDeleted = db.Amenities.Find(id);

                    oAmenitiesUpdateDeleted.IsActive = false;

                    db.Amenities.Update(oAmenitiesUpdateDeleted);
                    bIsSuccess = db.SaveChanges() > 0 ? true : false;
                    #endregion                    
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
