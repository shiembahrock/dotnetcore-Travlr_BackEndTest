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
    public class CurrencyApiController : ControllerBase
    {
        private readonly MvcHotelContext db;

        public CurrencyApiController(MvcHotelContext context)
        {
            db = context;
        }

        // GET api/values
        [HttpGet]        
        public ActionResult<List<Currency>> Get()        
        {
            List<Currency> oCurrency = db.Currency.ToList();
            
            return oCurrency;
        }
        
        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<Currency> Get(int id)
        {
            Currency oCurrency = db.Currency.Where(w => w.CurrencyId == id).FirstOrDefault();
            
            return oCurrency;
        }

        // POST api/values
        [HttpPost]
        // public void Post([FromBody] string value)
        public void Post([FromBody] Currency p_oCurrency)
        {
            bool bIsSuccess = false;

            using(var oTrans = db.Database.BeginTransaction())
            {
                try
                {                
                    Currency oCurrencyAdd = new Currency();

                    oCurrencyAdd.CurrencyCode = p_oCurrency.CurrencyCode;
                    oCurrencyAdd.CurrencyName = p_oCurrency.CurrencyName;
                    oCurrencyAdd.IsActive = p_oCurrency.IsActive;
                    
                    db.Currency.Add(oCurrencyAdd);
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
        public void Put(int id, [FromBody] Currency p_oCurrency)
        {
            bool bIsSuccess = false;

            using(var oTrans = db.Database.BeginTransaction())
            {
                try
                {
                    Currency oCurrencyUpdate = db.Currency.Find(id);

                    oCurrencyUpdate.CurrencyCode = p_oCurrency.CurrencyCode;
                    oCurrencyUpdate.CurrencyName = p_oCurrency.CurrencyName;
                    oCurrencyUpdate.IsActive = p_oCurrency.IsActive;

                    db.Update(oCurrencyUpdate);
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
                    #region Deleting Currency
                    Currency oCurrencyUpdateDeleted = db.Currency.Find(id);

                    oCurrencyUpdateDeleted.IsActive = false;

                    db.Currency.Update(oCurrencyUpdateDeleted);
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
