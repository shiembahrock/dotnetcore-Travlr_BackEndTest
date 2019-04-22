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
    public class AmenitiesController : Controller
    {
        private readonly MvcHotelContext _context;

        public AmenitiesController(MvcHotelContext context)
        {
            _context = context;
        }

        // GET: Amenities
        public async Task<IActionResult> Index()
        {
            return View(await _context.Amenities.ToListAsync());
        }

        // GET: Amenities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var amenities = await _context.Amenities
                .FirstOrDefaultAsync(m => m.AmenitiesId == id);
            if (amenities == null)
            {
                return NotFound();
            }

            return View(amenities);
        }

        // GET: Amenities/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Amenities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AmenitiesId,AmenitiesName,IsActive")] Amenities amenities)
        {
            if (ModelState.IsValid)
            {
                _context.Add(amenities);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(amenities);
        }

        // GET: Amenities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var amenities = await _context.Amenities.FindAsync(id);
            if (amenities == null)
            {
                return NotFound();
            }
            return View(amenities);
        }

        // POST: Amenities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AmenitiesId,AmenitiesName,IsActive")] Amenities amenities)
        {
            if (id != amenities.AmenitiesId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(amenities);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AmenitiesExists(amenities.AmenitiesId))
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
            return View(amenities);
        }

        // GET: Amenities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var amenities = await _context.Amenities
                .FirstOrDefaultAsync(m => m.AmenitiesId == id);
            if (amenities == null)
            {
                return NotFound();
            }

            return View(amenities);
        }

        // POST: Amenities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var amenities = await _context.Amenities.FindAsync(id);
            _context.Amenities.Remove(amenities);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AmenitiesExists(int id)
        {
            return _context.Amenities.Any(e => e.AmenitiesId == id);
        }
    }
}
