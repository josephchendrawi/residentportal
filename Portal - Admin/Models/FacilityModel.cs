using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ResComm.Web.Models
{
    public class Facility
    {
        public long FacilityId { get; set; }
        public string CreatedDate { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        [Display(Name = "No. of Time Slot")]
        public int TimeSlotCount { get; set; }
    }
    public class FacilitySlot
    {
        public long FacilitySlotId { get; set; }
        [Required]
        [Display(Name = "Start Time")]
        public string StartTime { get; set; }
        [Required]
        [Display(Name = "End Time")]
        public string EndTime { get; set; }
        [Display(Name = "No. of Slot Available")]
        public int Slot { get; set; }

        public long FacilityId { get; set; }
    }

    public class FacilityBooking
    {
        public long FacilityId { get; set; }
        public long? UserId { get; set; }

        public string FacilityName { get; set; }
        public string UserName { get; set; }
        public string BookingTime { get; set; }
        public string BookingDate { get; set; }
    }
}