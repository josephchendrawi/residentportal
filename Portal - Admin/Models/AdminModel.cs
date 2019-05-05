using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ResComm.Web.Models
{
    public class AdminSetup
    {
        public AdminSetup()
        {
            UserInfo = new UserInfo();
            AccountInfo = new AccountInfo();
            PropertyInfo = new PropertyInfo();
        }
        public UserInfo UserInfo { get; set; }
        public AccountInfo AccountInfo { get; set; }
        public PropertyInfo PropertyInfo { get; set; }
    }
    public class AdminUpdate
    {
        public AdminUpdate()
        {
            UserInfo = new UserInfo();
            AccountInfo = new AccountInfo();
        }
        public UserInfo UserInfo { get; set; }
        public AccountInfo AccountInfo { get; set; }
    }

    public class UserInfo
    {
        [Required]
        [Display(Name = "Contact Name")]
        public string ContactName { get; set; }
        [Required]
        [Display(Name = "Phone No.")]
        public string ContactNo { get; set; }

        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Email Address not in the correct format")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [StringLength(int.MaxValue, MinimumLength = 6, ErrorMessage = "Password length is incorrect (at least 6 characters)")]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [CompareAttribute("Password", ErrorMessage = "Confirm Password does not match with Password")]
        public string ConfirmPassword { get; set; }
    }

    public class AccountInfo
    {
        public long? CustomerId { get; set; }

        public AccountInfo()
        {
            AddressInfo = new AddressInfo();
        }
        public AddressInfo AddressInfo { get; set; }

        [Required]
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }
        [Required]
        [Display(Name = "Company Email")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Email Address not in the correct format")]
        public string CompanyEmail { get; set; }
        [Required]
        [Display(Name = "Office No.")]
        public string OfficeNo { get; set; }
        [Display(Name = "Fax No.")]
        public string FaxNo { get; set; }
    }

    public class AddressInfo
    {
        public long? AddressId { get; set; }

        [Required]
        public string Address { get; set; }
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
    }

    public class PropertyInfo
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
    }

    public class PropertyAdminSignup
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Email Address not in the correct format")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [StringLength(int.MaxValue, MinimumLength = 6, ErrorMessage = "Password length is incorrect (at least 6 characters)")]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [CompareAttribute("Password", ErrorMessage = "Confirm Password does not match with Password")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Affiliate Referral Code")]
        public string AffiliateReferral { get; set; }
    }

    public class PropertyInfo_SuperAdmin : PropertyInfo
    {
        public string Status { get; set; }
    }

}