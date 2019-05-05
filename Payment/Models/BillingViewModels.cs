using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ResComm.Web.Payment.Models
{

    public class Billing
    {
        public long BillingId { get; set; }
        [Display(Name = "Created Date")]
        public DateTime? CreatedDate { get; set; }
        public string CreatedDateText { get; set; }
        [Display(Name = "Invoice No.")]
        public string InvoiceNo { get; set; }
        [Display(Name = "Due Date")]
        public DateTime? DueDate { get; set; }
        public string DueDateText { get; set; }
        [Display(Name = "Transaction Reference")]
        public string Reference { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }

        [Display(Name = "Unit Name")]
        public string UnitName { get; set; }
        public string Token { get; set; }
        public string UserFullName { get; set; }
        public string UserEmail { get; set; }
    }
}