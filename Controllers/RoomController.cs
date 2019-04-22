using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HotelApp.Models;

namespace HotelApp.Controllers
{
    public class RoomController : Controller
    {
        private readonly MvcHotelContext _context;

        public RoomController(MvcHotelContext context)
        {
            _context = context;
        }

        // GET: Room
        public IActionResult Index()
        {   
            List<Room> oRoomList = _context.Room.Include(h => h.Hotel).Include(c => c.Currency).ToList();
            
            return View(oRoomList);
        }

        // GET: Room/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Room oRoom = await _context.Room
                .FirstOrDefaultAsync(m => m.RoomId == id);
            oRoom.Hotel = await _context.Hotel.FirstOrDefaultAsync(w => w.HotelId == oRoom.HotelId);
            oRoom.Currency = await _context.Currency.FirstOrDefaultAsync(w => w.CurrencyId == oRoom.CurrencyId);

            if (oRoom == null)
            {
                return NotFound();
            }

            return View(oRoom);
        }

        // GET: Room/Create
        public IActionResult Create()
        {            
            ViewBag.HotelList = _context.Hotel.ToList();
            ViewBag.CurrencyList = _context.Currency.ToList();            
            vmRoom ovmRoom = new vmRoom();
            ovmRoom.oRoom = new Room();
            ovmRoom.oAmenitiesList = _context.Amenities.ToList();
            
            return View(ovmRoom);
        }
        
        // POST: Room/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        // public async Task<IActionResult> Create([Bind("RoomId,HotelId,RoomName,MaxOccupancy,NetRate,SellRate,CurrencyId")] Room room)
        public IActionResult Create(vmRoom ovmRoom,            
            string[] amenitiesChecked
        )
        {            
            bool bIsSuccess = false;

            Hotel oHotel = _context.Hotel
                .Include(r => r.Rooms).Where(w => w.HotelId == ovmRoom.oRoom.HotelId).FirstOrDefault();
            
            if (oHotel.Rooms.Count() >= 2)
            {
                ModelState.AddModelError("oRoom.HotelId", "Maximum limit 5 rooms for 1 hotel.");
            }

            if (ModelState.IsValid)
            {            
                using(var oTrans = _context.Database.BeginTransaction())
                {
                    try
                    {                    
                        _context.Add(ovmRoom.oRoom);
                        _context.SaveChanges();

                        foreach(string Amenities in amenitiesChecked)
                        {
                            RoomAmenities oRoomAmenities = new RoomAmenities();

                            oRoomAmenities.RoomId = ovmRoom.oRoom.RoomId;
                            oRoomAmenities.AmenitiesId = Convert.ToInt32(Amenities);

                            _context.Add(oRoomAmenities);
                            _context.SaveChanges();
                        }

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
                        
            return RedirectToAction(nameof(Index));
        }

        // GET: Room/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            vmRoom ovmRoom = new vmRoom();
            ovmRoom.oRoom = await _context.Room.Where(w => w.RoomId == id).FirstOrDefaultAsync();
            ovmRoom.oRoom.RoomAmenitieses =
                await _context.RoomAmenities.Where(w => w.RoomId == id).ToListAsync();
            ovmRoom.oAmenitiesList = await _context.Amenities.ToListAsync();
            
            if (ovmRoom.oRoom == null)
            {
                return NotFound();
            }
            ViewBag.HotelList = await _context.Hotel
                .Select(s => new { s.HotelId, s.HotelName, s.CommissionRate}).ToListAsync();//_context.Hotel.ToList();
            ViewBag.CurrencyList = await _context.Currency.ToListAsync();
                                    
            return View(ovmRoom);
        }
        
        // POST: Room/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, 
            vmRoom ovmRoom,
            string[] amenitiesChecked
            )
        {
            bool bIsSuccess = false;
            int[] iArrAmenitiesIdChecked = new int[amenitiesChecked.Length];

            if (id != ovmRoom.oRoom.RoomId)
            {
                return NotFound();
            }

            if(ModelState.IsValid)
            {
                using(var oTrans = _context.Database.BeginTransaction())
                {
                    try
                    {
                        _context.Update(ovmRoom.oRoom);
                        _context.SaveChanges();

                        for(int i = 0; i < amenitiesChecked.Length; i++)
                        {                            
                            iArrAmenitiesIdChecked[i] = Convert.ToInt32(amenitiesChecked[i]);
                        }

                        List<RoomAmenities> oRoomAmenitiesListExisting =
                            _context.RoomAmenities.Where(w => w.RoomId == id).AsNoTracking().ToList();
                        
                        #region Existing Check
                        foreach(RoomAmenities i_oRoomAmenities in oRoomAmenitiesListExisting)
                        {
                            if(!iArrAmenitiesIdChecked.Contains(i_oRoomAmenities.AmenitiesId))
                            {
                                //delete existing
                                int iRoomAmenitiesId = _context.RoomAmenities
                                    .Where(w => w.RoomId == id && w.AmenitiesId == i_oRoomAmenities.AmenitiesId)
                                    .Select(s => s.RoomAmenitiesId).FirstOrDefault();

                                RoomAmenities oRoomAmenitiesDeleted = _context.RoomAmenities.Find(iRoomAmenitiesId);
                                _context.RoomAmenities.Remove(oRoomAmenitiesDeleted);
                                _context.SaveChanges();
                            }                            
                        }
                        #endregion

                        #region Insert New
                        foreach(int i_iAmenitiesId in iArrAmenitiesIdChecked)
                        {
                            RoomAmenities oRoomAmenitiesCheck = _context.RoomAmenities
                                .Where(w => w.RoomId == id && w.AmenitiesId == i_iAmenitiesId).FirstOrDefault();
                            if(oRoomAmenitiesCheck == null)
                            {
                                //Add
                                RoomAmenities oRoomAmenitiesAdd = new RoomAmenities();

                                oRoomAmenitiesAdd.RoomId = id;
                                oRoomAmenitiesAdd.AmenitiesId = i_iAmenitiesId;

                                _context.Add(oRoomAmenitiesAdd);
                                _context.SaveChanges();
                            }
                        }
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
            
            return RedirectToAction(nameof(Index));
        }

        // GET: Room/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Room
                .FirstOrDefaultAsync(m => m.RoomId == id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // POST: Room/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            bool bIsSuccess = false;

            using(var oTrans = _context.Database.BeginTransaction())
            {
                try
                {
                    List<RoomAmenities> oRoomAmenitiesDeletedList =
                        await _context.RoomAmenities.Where(w => w.RoomId == id).ToListAsync();
                    
                    _context.RoomAmenities.RemoveRange(oRoomAmenitiesDeletedList);
                    await _context.SaveChangesAsync();

                    Room oRoomDeleted = await _context.Room.FindAsync(id);
                    _context.Room.Remove(oRoomDeleted);
                    _context.SaveChangesAsync();

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

            return RedirectToAction(nameof(Index));
        }

        private bool RoomExists(int id)
        {
            return _context.Room.Any(e => e.RoomId == id);
        }        
    }
}
