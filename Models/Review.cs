using System;
using System.ComponentModel.DataAnnotations;

namespace SkillHub.Models
{
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }
        public int BookingId { get; set; }
        public string CustomerId { get; set; }
        [Range(1, 5)]
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime ReviewDate { get; set; } = DateTime.Now;
    }
}
