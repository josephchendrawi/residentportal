using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ResComm.Web.Models
{
    public class MySubscriptionVM
    {
        [Display(Name = "Package Name")]
        public string PackageName { get; set; }

        [Display(Name = "Start Date")]
        public DateTime? StartDate { get; set; }
        public string StartDateText { get; set; }

        [Display(Name = "End Date")]
        public DateTime? EndDate { get; set; }
        public string EndDateText { get; set; }
    }
}