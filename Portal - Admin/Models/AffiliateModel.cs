using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ResComm.Web.Models
{
    public class Affiliate
    {
        public long UserId { get; set; }
        public string Email { get; set; }
        [Display(Name = "Name")]
        public string ContactName { get; set; }
        [Display(Name = "Phone No.")]
        public string ContactNo { get; set; }

        public string Status { get; set; }
    }
}