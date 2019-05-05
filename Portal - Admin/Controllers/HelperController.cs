using ResComm.Web.Lib.BLL;
using ResComm.Web.Lib.DB.DAL;
using ResComm.Web.Lib.Enums;
using ResComm.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ResComm.Web.Controllers
{
    public class HelperController : Controller
    {
        public ActionResult PropertyInvoicePDF(int Id)
        {
            var v = new PropertyInvoiceDALImpl().GetPropertyInvoice(Id);

            PropertyInvoicePDF VM = new PropertyInvoicePDF();

            VM.PropertyInvoiceId = v.ROW_ID;
            VM.PropertyName = v.PropertyName;
            VM.InvoiceDate = v.INVOICE_DATE;
            VM.InvoiceDateText = v.INVOICE_DATE == null ? "" : v.INVOICE_DATE.Value.ToString("dd MMM yyyy");
            VM.InvoiceNo = v.INVOICE_NUM;
            VM.Amount = v.AMOUNT;
            VM.Status = v.STATUS;
            VM.StatusName = ((PROPERTY_INVOICE_STATUS)v.STATUS).ToString();
            VM.PaymentMethod = v.PAYMENT_METHOD;
            VM.PaymentMethodName = ((PROPERTY_INVOICE_PAYMENT_METHOD)v.PAYMENT_METHOD).ToString();
            VM.PaymentStatus = v.PAYMENT_STATUS;
            VM.PaymentStatusName = ((PROPERTY_INVOICE_PAYMENT_STATUS)v.PAYMENT_STATUS).ToString();

            VM.ApprovalCode = v.APPROVAL_CODE;
            VM.Bank = v.BANK;
            VM.ChequeNo = v.CHEQUE_NO;
            VM.Description = v.DESCRIPTION;
            VM.PaymentDate = v.PAYMENT_DATE;
            VM.PaymentDateText = v.PAYMENT_DATE == null ? "" : v.PAYMENT_DATE.Value.ToString("dd MMM yyyy");
            VM.TransactionNo = v.TRANSACTION_NO;

            var Property = new PropertyBLL().Get(v.PROPERTY_ID.Value);
            VM.AddressLine1 = Property.ADDR_1;
            VM.AddressLine2 = Property.ADDR_2;
            VM.AddressLine3 = Property.POSTAL_CD + " " + Property.CITY + ", " + Property.STATE + ", " + Property.COUNTRY;

            var Account = new AccountBLL().Get(Property.ACCNT_ID.Value);
            VM.Contact = Account.NAME;
            VM.PhoneNo = Account.OFFICE_NO;

            return View(VM);
        }
    }
}