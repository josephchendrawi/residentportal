using ResComm.Web.Lib.BLL;
using ResComm.Web.Lib.DB.DAL;
using ResComm.Web.Lib.Enums;
using ResComm.Web.Lib.Interface.Models.PropertyInvoice;
using ResComm.Web.Models;
using ResComm.Web.Models.DataTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ResComm.Web.Controllers
{
    public class MyAccountController : AdminBaseController
    {
        private AccountBLL AccountBLL = new AccountBLL();
        private PropertySubscriptionDALImpl PropertySubscriptionBLL = new PropertySubscriptionDALImpl();
        private PropertyInvoiceDALImpl PropertyInvoiceBLL = new PropertyInvoiceDALImpl();
        
        public ActionResult MySubscription()
        {
            var model = new MySubscriptionVM();

            var UserId = long.Parse((string)Session["UserId"]);
            var PropertyId = AccountBLL.GetRelation(UserId).PropertyId;

            var PropertySubscription = PropertySubscriptionBLL.GetPropertySubscriptionByPropertyId(PropertyId);

            if (PropertySubscription != null)
            {
                model.PackageName = PropertySubscription.P_SUBSCRIPTION_PACKAGE == null ? "-" : PropertySubscription.P_SUBSCRIPTION_PACKAGE.NAME;
                model.StartDate = PropertySubscription.START_DATE;
                model.StartDateText = PropertySubscription.START_DATE == null ? "-" : PropertySubscription.START_DATE.Value.ToString("dd MMM yyyy");
                model.EndDate = PropertySubscription.END_DATE;
                model.EndDateText = PropertySubscription.END_DATE == null ? "-" : PropertySubscription.END_DATE.Value.ToString("dd MMM yyyy");
            }
            else
            {
                model.PackageName = "-";
                model.StartDateText = "-";
                model.EndDateText = "-";
            }

            return View(model);
        }

        public ActionResult MyInvoices()
        {
            return View();
        }

        public ActionResult ListPropertyInvoice(DTParameterModel param, string InvoiceNo, string PropertyName, string InvoiceDateStart, string InvoiceDateEnd, int Status = 0, int PaymentStatus = 0)
        {
            try
            {
                var UserId = long.Parse((string)Session["UserId"]);
                var PropertyId = AccountBLL.GetRelation(UserId).PropertyId;

                int TotalCount = 0;

                //sorting
                string orderingFunction = param.Columns.ElementAt(param.Order.FirstOrDefault().Column).Name == "" ? param.Columns.ElementAt(param.Order.FirstOrDefault().Column).Data : param.Columns.ElementAt(param.Order.FirstOrDefault().Column).Name;
                var sortDirection = param.Order.FirstOrDefault().Dir; // asc or desc

                //filtering
                PropertyInvoiceVO qFilter = new PropertyInvoiceVO()
                {
                    INVOICE_NUM = InvoiceNo,
                    PropertyName = PropertyName,
                    STATUS = Status,
                    PAYMENT_STATUS = PaymentStatus,
                    PROPERTY_ID = PropertyId,
                };
                DateTime? InvoiceDateStart_DateTime = null;
                DateTime? InvoiceDateEnd_DateTime = null;
                try
                {
                    InvoiceDateStart_DateTime = DateTime.ParseExact(InvoiceDateStart, "MM/dd/yyyy", null);
                    InvoiceDateEnd_DateTime = DateTime.ParseExact(InvoiceDateEnd, "MM/dd/yyyy", null);
                }
                catch { }
                var List = PropertyInvoiceBLL.GetPropertyInvoices(param.Start, param.Length, ref TotalCount, orderingFunction, sortDirection, qFilter, InvoiceDateStart_DateTime, InvoiceDateEnd_DateTime);

                List<PropertyInvoice> PropertyInvoiceList = new List<PropertyInvoice>();

                foreach (var v in List)
                {
                    PropertyInvoice VM = new PropertyInvoice();

                    VM.PropertyInvoiceId = v.ROW_ID;
                    VM.PropertyName = v.PropertyName;
                    VM.InvoiceDate = v.INVOICE_DATE;
                    VM.InvoiceDateText = v.INVOICE_DATE == null ? "" : v.INVOICE_DATE.Value.ToString("dd MMM yyyy HH:mm");
                    VM.InvoiceNo = v.INVOICE_NUM;
                    VM.Amount = v.AMOUNT;
                    VM.Status = v.STATUS;
                    VM.StatusName = ((PROPERTY_INVOICE_STATUS)v.STATUS).ToString();
                    VM.PaymentMethod = v.PAYMENT_METHOD;
                    VM.PaymentMethodName = ((PROPERTY_INVOICE_PAYMENT_METHOD)v.PAYMENT_METHOD).ToString();
                    VM.PaymentStatus = v.PAYMENT_STATUS;
                    VM.PaymentStatusName = ((PROPERTY_INVOICE_PAYMENT_STATUS)v.PAYMENT_STATUS).ToString();
                    VM.Description = v.DESCRIPTION;

                    PropertyInvoiceList.Add(VM);
                }

                DTResult<PropertyInvoice> model = new DTResult<PropertyInvoice>
                {
                    draw = param.Draw,
                    data = PropertyInvoiceList,
                    recordsFiltered = TotalCount,
                    recordsTotal = TotalCount
                };

                return Json(model, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public ActionResult InvoiceDetails(int Id)
        {
            var UserId = long.Parse((string)Session["UserId"]);
            var PropertyId = AccountBLL.GetRelation(UserId).PropertyId;

            var v = PropertyInvoiceBLL.GetPropertyInvoice(Id);

            if (v.PROPERTY_ID != PropertyId)
            {
                return RedirectToAction("MyInvoices");
            }

            PropertyInvoice VM = new PropertyInvoice();

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

            return View(VM);
        }

        public ActionResult InvoiceDownloadPDF(int Id)
        {
            var UserId = long.Parse((string)Session["UserId"]);
            var PropertyId = AccountBLL.GetRelation(UserId).PropertyId;

            var Invoice = PropertyInvoiceBLL.GetPropertyInvoice(Id);

            if (Invoice.PROPERTY_ID != PropertyId)
            {
                return RedirectToAction("MyInvoices");
            }

            var Result = new Rotativa.UrlAsPdf(Url.Action("PropertyInvoicePDF", "Helper", new { Id = Id })) { FileName = "Invoice-" + Invoice.INVOICE_NUM + ".pdf" };

            return Result;
        }

    }
}