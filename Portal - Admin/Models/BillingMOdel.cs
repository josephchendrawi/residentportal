using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ResComm.Web.Models
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
        [Display(Name = "Billing Date")]
        public DateTime? BillingDate { get; set; }
        public string BillingDateText { get; set; }
        public string Reference { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public string FileName { get; set; }

        [Display(Name = "Unit Name")]
        public string UnitName { get; set; }
    }

    public class AddEditBillingVM : Billing
    {
        [Display(Name = "Unit No.")]
        public long UnitId { get; set; }
    }

}