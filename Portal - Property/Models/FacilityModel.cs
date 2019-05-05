using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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

    public class FacilityBook
    {
        public long FacilityId { get; set; }
        [Display(Name = "Facility Name")]
        public string FacilityName { get; set; }
        [Required]
        [Display(Name = "Facility Slot")]
        public long? FacilitySlotId { get; set; }
        [Display(Name = "Date")]
        public DateTime? BookDate { get; set; }
    }

}