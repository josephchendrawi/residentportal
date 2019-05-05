using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ResComm.Web.Models
{
    public class Unit
    {
        public Unit()
        {
            UnitType = new UnitType();
        }

        public long UnitId { get; set; }
        [Required]
        [Display(Name = "Unit No.")]
        public string UnitNo { get; set; }
        [Required]
        public UnitType UnitType { get; set; }
        public long? PropertyId { get; set; }


        [Display(Name = "Name")]
        public string TenantName { get; set; }
        [Display(Name = "Contact No.")]
        public string TenantContactNo { get; set; }
        [Display(Name = "Name")]
        public string OwnerName { get; set; }
        [Display(Name = "Contact No.")]
        public string OwnerContactNo { get; set; }
    }
}