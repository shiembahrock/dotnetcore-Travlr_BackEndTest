using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HotelApp.Models;

namespace HotelApp.Models
{
    public class MvcHotelContext : DbContext
    {
        public MvcHotelContext (DbContextOptions<MvcHotelContext> options)
            : base(options)
        {
        }

        public DbSet<HotelApp.Models.Hotel> Hotel { get; set; }
        public DbSet<HotelApp.Models.Room> Room { get; set; }
        public DbSet<HotelApp.Models.Amenities> Amenities { get; set; }
        public DbSet<HotelApp.Models.RoomAmenities> RoomAmenities { get; set; }
        public DbSet<HotelApp.Models.Currency> Currency { get; set; }
        public DbSet<HotelApp.Models.BookingRoom> BookingRoom { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {            
            modelBuilder.Entity<Room>()
                .HasOne(r => r.Hotel)                
                .WithMany(h => h.Rooms)
                .HasForeignKey(r => r.HotelId)
                .HasConstraintName("ForeignKey_Room_Hotel");
            modelBuilder.Entity<Room>()    
                .HasOne(r => r.Currency)
                .WithMany(c => c.Rooms)
                .HasForeignKey(r => r.CurrencyId)
                .HasConstraintName("ForeignKey_Room_Currency");
            modelBuilder.Entity<RoomAmenities>()    
                .HasOne(ra => ra.Room)
                .WithMany(r => r.RoomAmenitieses)
                .HasForeignKey(ra => ra.RoomId)
                .HasConstraintName("ForeignKey_RoomAmenities_Room");
            modelBuilder.Entity<RoomAmenities>()    
                .HasOne(ra => ra.Amenities)
                .WithMany(a => a.RoomAmenitieses)
                .HasForeignKey(ra => ra.AmenitiesId)
                .HasConstraintName("ForeignKey_RoomAmenities_Amenities");
            modelBuilder.Entity<BookingRoom>()    
                .HasOne(br => br.Room)
                .WithMany(r => r.BookingRooms)
                .HasForeignKey(br => br.RoomId)
                .HasConstraintName("ForeignKey_BookingRoom_Room");
        }
    }
}