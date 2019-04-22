using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HotelApp.Models;

namespace HotelApp.Controllers
{
    public class BookingRoomController : Controller
    {
        private readonly MvcHotelContext _context;

        public BookingRoomController(MvcHotelContext context)
        {
            _context = context;
        }

        // GET: BookingRoom
        public async Task<IActionResult> Index()
        {
            var MvcHotelContext = _context.BookingRoom.Include(b => b.Room);
            return View(await MvcHotelContext.ToListAsync());
        }

        // GET: BookingRoom/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookingRoom = await _context.BookingRoom
                .Include(b => b.Room)
                .FirstOrDefaultAsync(m => m.BookingRoomId == id);
            if (bookingRoom == null)
            {
                return NotFound();
            }

            return View(bookingRoom);
        }

        // GET: BookingRoom/Create
        public IActionResult Create(int id)
        {
            // ViewData["RoomId"] = new SelectList(_context.Set<Room>(), "RoomId", "RoomName");

            
            ViewBag.oRoomId = id;                        
            return View();
        }

        public string RandomString(int size, bool lowerCase)  
        {  
            StringBuilder builder = new StringBuilder();  
            Random random = new Random();  
            char ch;  

            for (int i = 0; i < size; i++)  
            {  
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));  
                builder.Append(ch);  
            }  

            if (lowerCase)  
                return builder.ToString().ToLower();  
            
            string sRand = builder.ToString();
            string sRandFix = string.Empty;

            sRandFix = _context.BookingRoom.Where(w => w.UniqueCode == sRand).FirstOrDefault() == null ?
                sRand : RandomString(size, lowerCase);

            //return builder.ToString();  
            return sRandFix;
        }

        // POST: BookingRoom/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        // public async Task<IActionResult> Create([Bind("BookingRoomId,BookingCode,ClientId,RoomId,CheckInDate,CheckOutDate,UniqueCode")] BookingRoom bookingRoom)
        public async Task<IActionResult> Create(BookingRoom p_oBookingRoom)
        {
            bool bIsSuccess = false;

            #region Validation
            if(p_oBookingRoom.CheckOutDate <= p_oBookingRoom.CheckInDate)
            {
                ModelState.AddModelError("CheckOutDate", "Check out date must more bigger than check in date");
            }    
            #endregion            

            if (ModelState.IsValid)
            {
                // _context.Add(p_oBookingRoom);
                // await _context.SaveChangesAsync();
                // return RedirectToAction(nameof(Index));
                using(var oTrans = _context.Database.BeginTransaction())
                {
                    try
                    {
                        p_oBookingRoom.BookingCode = DateTime.Now.ToString("yyyyMMddhhmmssfff");                        
                        p_oBookingRoom.UniqueCode = RandomString(7, false);

                        _context.Add(p_oBookingRoom);
                        await _context.SaveChangesAsync();

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
            if(bIsSuccess)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(p_oBookingRoom);
        }

        // GET: BookingRoom/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookingRoom = await _context.BookingRoom.FindAsync(id);
            if (bookingRoom == null)
            {
                return NotFound();
            }
            ViewData["RoomId"] = new SelectList(_context.Set<Room>(), "RoomId", "RoomName", bookingRoom.RoomId);
            return View(bookingRoom);
        }

        // POST: BookingRoom/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookingRoomId,BookingCode,ClientId,RoomId,CheckInDate,CheckOutDate,UniqueCode")] BookingRoom bookingRoom)
        {
            if (id != bookingRoom.BookingRoomId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bookingRoom);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingRoomExists(bookingRoom.BookingRoomId))
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
            ViewData["RoomId"] = new SelectList(_context.Set<Room>(), "RoomId", "RoomName", bookingRoom.RoomId);
            return View(bookingRoom);
        }

        // GET: BookingRoom/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookingRoom = await _context.BookingRoom
                .Include(b => b.Room)
                .FirstOrDefaultAsync(m => m.BookingRoomId == id);
            if (bookingRoom == null)
            {
                return NotFound();
            }

            return View(bookingRoom);
        }

        // POST: BookingRoom/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bookingRoom = await _context.BookingRoom.FindAsync(id);
            _context.BookingRoom.Remove(bookingRoom);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookingRoomExists(int id)
        {
            return _context.BookingRoom.Any(e => e.BookingRoomId == id);
        }
    }
}
