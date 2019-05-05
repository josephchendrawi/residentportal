using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ResComm.Web.Models
{
    public class PropertyInvoice
    {
        public long PropertyInvoiceId { get; set; }
        [Display(Name = "Invoice No.")]
        public string InvoiceNo { get; set; }
        [Display(Name = "Invoice Date")]
        [Required]
        public DateTime? InvoiceDate { get; set; }
        public string InvoiceDateText { get; set; }
        public string Description { get; set; }
        [Required]
        public decimal? Amount { get; set; }
        [Required]
        public int? Status { get; set; }
        [Required]
        [Display(Name = "Payment Status")]
        public int? PaymentStatus { get; set; }
        [Display(Name = "Payment Date")]
        public DateTime? PaymentDate { get; set; }
        public string PaymentDateText { get; set; }
        [Display(Name = "Payment Method")]
        public int? PaymentMethod { get; set; }
        [Display(Name = "Cheque No.")]
        public string ChequeNo { get; set; }
        [Display(Name = "Transaction No.")]
        public string TransactionNo { get; set; }
        [Display(Name = "Approval Code")]
        public string ApprovalCode { get; set; }
        public string Bank { get; set; }
        [Required]
        public long PropertyId { get; set; }
        [Display(Name = "Property")]
        public string PropertyName { get; set; }

        public string StatusName { get; set; }
        public string PaymentStatusName { get; set; }
        public string PaymentMethodName { get; set; }
    }

    public class PropertyInvoicePDF : PropertyInvoice
    {
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string Contact { get; set; }
        public string PhoneNo { get; set; }
    }

}