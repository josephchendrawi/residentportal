using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ResComm.Web.Models
{
    public class EditMemberModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Email Address not in the correct format")]
        public string Email { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        
        [DataType(DataType.Password)]
        [StringLength(int.MaxValue, MinimumLength = 6, ErrorMessage = "Password length is incorrect (at least 6 characters)")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [CompareAttribute("Password", ErrorMessage = "Confirm Password does not match with Password")]
        public string ConfirmPassword { get; set; }
    }

    public class Member
    {
        public long UserId { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Email Address not in the correct format")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Type")]
        public string UserType { get; set; }
        [Required]
        [Display(Name = "Name")]
        public string ContactName { get; set; }
        [Required]
        [Display(Name = "Phone No.")]
        public string ContactNo { get; set; }

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
}