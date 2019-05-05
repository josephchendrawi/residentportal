using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ResComm.Web.Models
{
    public class PropertyInfo
    {
        public long? PropertyId { get; set; }
        [Display(Name = "Date Registered")]
        public string CreatedAt { get; set; }
        public int Status { get; set; }
        public string StatusText { get; set; }

        public string PackageName { get; set; }
        public string SubscriptionPeriod { get; set; }
        public decimal Price { get; set; }
        public int Commission { get; set; }
        public bool isCommissionPaid { get; set; }

        [Required]
        [Display(Name = "Property Name")]
        public string PropertyName { get; set; }
        [Required]
        [Display(Name = "Address 1")]
        public string Address1 { get; set; }
        [Display(Name = "Address 2")]
        public string Address2 { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        [Display(Name = "State/Province")]
        public string State { get; set; }
        [Required]
        [StringLength(5, MinimumLength = 5, ErrorMessage = "ZIP/Postal Code can only be 5 digits")]
        [Display(Name = "ZIP/Postal Code")]
        public string ZIP { get; set; }
        [Required]
        public string Country { get; set; }
        public string LogoURL { get; set; }
    }

    public class ViewPropertyVM
    {
        public long? PropertyId { get; set; }
        [Required]
        [Display(Name = "Property Name")]
        public string PropertyName { get; set; }
        [Required]
        [Display(Name = "Address 1")]
        public string Address1 { get; set; }
        [Display(Name = "Address 2")]
        public string Address2 { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        [Display(Name = "State/Province")]
        public string State { get; set; }
        [Required]
        [StringLength(5, MinimumLength = 5, ErrorMessage = "ZIP/Postal Code can only be 5 digits")]
        [Display(Name = "ZIP/Postal Code")]
        public string ZIP { get; set; }
        [Required]
        public string Country { get; set; }
        public string LogoURL { get; set; }

        [Display(Name = "Name")]
        public string ContactName { get; set; }
        [Display(Name = "Phone")]
        public string ContactNo { get; set; }
        [Display(Name = "Email")]
        public string ContactEmail { get; set; }
    }
}