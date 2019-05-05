using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ResComm.Web.Models
{
    public class Ticket
    {
        public long TicketId { get; set; }
        public long? RequesterAccountId { get; set; }
        public string Requester { get; set; }
        public DateTime? Created { get; set; }
        [Display(Name = "Requested On")]
        public string CreatedDate { get; set; }
        [Required]
        [Display(Name = "Category")]
        public long CategoryId { get; set; }
        [Required]
        public string Priority { get; set; }
        [Required]
        public string Status { get; set; }
        public string Description { get; set; }
        [Required]
        [Display(Name = "Subject")]
        public string Name { get; set; }
        [Display(Name = "Estimated Start At")]
        public DateTime? EstStartDate { get; set; }
        [Display(Name = "Estimated End At")]
        public DateTime? EstEndDate { get; set; }
        public string EstStartDateText { get; set; }
        public string EstEndDateText { get; set; }
        [Required]
        public string Type { get; set; }

        public string CategoryName { get; set; }
    }

    public class TicketCategory
    {
        public long TicketCategoryId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
    }

}