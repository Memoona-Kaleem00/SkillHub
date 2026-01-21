using System;
using System.ComponentModel.DataAnnotations;

namespace SkillHub.Models
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }
        public int ServiceId { get; set; }
        public string CustomerId { get; set; }
        public string ProviderId { get; set; }
        public DateTime BookingDate { get; set; }
        public string Status { get; set; } // Pending, Accepted, Completed, Cancelled
    }
}
