using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HotelApp.Models;

namespace HotelApp.Controllers
{
    public class RoomAmenitiesController : Controller
    {
        private readonly MvcHotelContext _context;

        public RoomAmenitiesController(MvcHotelContext context)
        {
            _context = context;
        }

        // GET: RoomAmenities
        public async Task<IActionResult> Index()
        {
            return View(await _context.RoomAmenities.ToListAsync());
        }

        // GET: RoomAmenities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roomAmenities = await _context.RoomAmenities
                .FirstOrDefaultAsync(m => m.RoomAmenitiesId == id);
            if (roomAmenities == null)
            {
                return NotFound();
            }

            return View(roomAmenities);
        }

        // GET: RoomAmenities/Create
        public IActionResult Create()
        {
            ViewBag.RoomList = _context.Room.ToList();
            ViewBag.AmenitiesList = _context.Amenities.ToList();
            return View();
        }

        // POST: RoomAmenities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RoomAmenitiesId,RoomId,AmenitiesId")] RoomAmenities roomAmenities)
        {
            if (ModelState.IsValid)
            {
                _context.Add(roomAmenities);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(roomAmenities);
        }

        // GET: RoomAmenities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roomAmenities = await _context.RoomAmenities.FindAsync(id);
            if (roomAmenities == null)
            {
                return NotFound();
            }
            return View(roomAmenities);
        }

        // POST: RoomAmenities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RoomAmenitiesId,RoomId,AmenitiesId")] RoomAmenities roomAmenities)
        {
            if (id != roomAmenities.RoomAmenitiesId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(roomAmenities);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomAmenitiesExists(roomAmenities.RoomAmenitiesId))
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
            return View(roomAmenities);
        }

        // GET: RoomAmenities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roomAmenities = await _context.RoomAmenities
                .FirstOrDefaultAsync(m => m.RoomAmenitiesId == id);
            if (roomAmenities == null)
            {
                return NotFound();
            }

            return View(roomAmenities);
        }

        // POST: RoomAmenities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var roomAmenities = await _context.RoomAmenities.FindAsync(id);
            _context.RoomAmenities.Remove(roomAmenities);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RoomAmenitiesExists(int id)
        {
            return _context.RoomAmenities.Any(e => e.RoomAmenitiesId == id);
        }
    }
}
