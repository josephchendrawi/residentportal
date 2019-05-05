using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ResComm.Web.Models
{
    public class Commission
    {
        [Display(Name = "Created Date")]
        public string CreatedDate { get; set; }
        public string Property { get; set; }
        [Display(Name = "Subscription Package")]
        public string SubscriptionPackage { get; set; }
        [Display(Name = "Package Price")]
        public decimal? PackagePrice { get; set; }
        [Display(Name = "Commission %")]
        public int? CommissionPercentage { get; set; }
        [Display(Name = "Commission")]
        public decimal CommissionAmount { get; set; }
    }
}