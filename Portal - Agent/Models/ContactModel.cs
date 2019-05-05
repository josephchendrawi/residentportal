using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ResComm.Web.Models
{
    public class Contact
    {
        public long ContactId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Phone No.")]
        public string PhoneNo { get; set; }
        public string Email { get; set; }
        public string Remarks { get; set; }
        public int Rating { get; set; }
    }
}