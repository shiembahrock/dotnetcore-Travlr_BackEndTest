using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace HotelApp.Models
{
    public class Currency
    {
        public int CurrencyId { get; set; }   
        [Required]
        [MaxLength(5)]     
        public string CurrencyCode { get; set; }
        [Required]
        [MaxLength(50)]
        public string CurrencyName { get; set; }
        [Required]
        public bool IsActive { get; set; }

        public List<Room> Rooms { get; set; }        
    }
}