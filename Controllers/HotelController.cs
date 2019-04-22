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
    public class HotelController : Controller
    {
        private readonly MvcHotelContext _context;

        public HotelController(MvcHotelContext context)
        {
            _context = context;
        }

        // GET: Hotel
        public async Task<IActionResult> Index()
        {
            List<Hotel> oHotelList = await _context.Hotel.Include(r => r.Rooms).ToListAsync();
            return View(await _context.Hotel.ToListAsync());
        }

        // GET: Hotel/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hotel = await _context.Hotel
                .FirstOrDefaultAsync(m => m.HotelId == id);
            if (hotel == null)
            {
                return NotFound();
            }

            return View(hotel);
        }

        // GET: Hotel/Create
        public IActionResult Create()
        {            
            return View();
        }

        // POST: Hotel/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HotelId,HotelName,Address,Latitude,Longitude,CommissionRate")] Hotel hotel,
            string[] RoomId
            )
        {
            if (ModelState.IsValid)
            {   
                _context.Add(hotel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(hotel);
        }

        // GET: Hotel/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hotel = await _context.Hotel.FindAsync(id);
            if (hotel == null)
            {
                return NotFound();
            }
            return View(hotel);
        }

        // POST: Hotel/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("HotelId,HotelName,Address,Latitude,Longitude,CommissionRate")] Hotel hotel)
        {
            if (id != hotel.HotelId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hotel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HotelExists(hotel.HotelId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(hotel);
        }

        // GET: Hotel/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hotel = await _context.Hotel
                .FirstOrDefaultAsync(m => m.HotelId == id);
            if (hotel == null)
            {
                return NotFound();
            }

            return View(hotel);
        }

        // POST: Hotel/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            bool bIsSuccess = false;

            using(var oTrans = _context.Database.BeginTransaction())
            {
                try
                {
                    Hotel oHotelDeleted = _context.Hotel.Find(id);

                    #region Deleting Child [Room]
                    List<Room> oRoomList = _context.Room.Where(w => w.HotelId == oHotelDeleted.HotelId).ToList();

                    #region Deleting Child [RoomAmenities]
                    foreach(Room i_oRoom in oRoomList)
                    {
                        List<RoomAmenities> oRoomAmenitiesList =
                            _context.RoomAmenities.Where(w => w.RoomId == i_oRoom.RoomId).ToList();
                        
                        _context.RoomAmenities.RemoveRange(oRoomAmenitiesList);
                        _context.SaveChanges();
                    }
                    #endregion

                    _context.Room.RemoveRange(oRoomList);
                    _context.SaveChanges();
                    #endregion

                    _context.Hotel.Remove(oHotelDeleted);
                    _context.SaveChanges();

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

        private bool HotelExists(int id)
        {
            return _context.Hotel.Any(e => e.HotelId == id);
        }
    }
}
