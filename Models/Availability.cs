using System;
using System.ComponentModel.DataAnnotations;

namespace SkillHub.Models
{
    public class Availability
    {
        [Key]
        public int AvailabilityId { get; set; }
        public string ProviderId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}
