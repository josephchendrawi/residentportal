using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ResComm.Web.Models
{
    public class UnitType
    {
        public long UnitTypeId { get; set; }
        [Required]
        [Display(Name = "Category")]
        public string CategoryName { get; set; }
        [Required]
        [Display(Name = "Name")]
        public string CategoryValue { get; set; }
    }
}