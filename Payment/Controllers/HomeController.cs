using ResComm.Web.Lib.BLL;
using ResComm.Web.Payment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ResComm.Web.Payment.Controllers
{
    public class HomeController : Controller
    {
        BillingBLL BillingBLL = new BillingBLL();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Payment(string id)
        {
            string returntobill = System.Configuration.ConfigurationManager.AppSettings["MEMBERURL"];
            if (id == null)
            {
                return Redirect(returntobill);
            }
            Billing VM = new Billing();
            string fullname = "";
            string email = "";
            var v = BillingBLL.GetByToken(id, ref fullname, ref email);
            ViewBag.username = fullname;
            ViewBag.useremail = email;
            VM.BillingId = v.ROW_ID;
            VM.Amount = v.AMOUNT ?? 0;
            VM.CreatedDate = v.CREATED;
            VM.CreatedDateText = v.CREATED == null ? "" : v.CREATED.Value.ToMalaysiaTime().ToString("dd MMM yyyy");
            VM.DueDate = v.DUE_DATE;
            VM.DueDateText = v.DUE_DATE == null ? "" : v.DUE_DATE.Value.ToMalaysiaTime().ToString("dd MMM yyyy");
            VM.InvoiceNo = v.INVOICE_NO;
            VM.Reference = v.REFERENCE;
            VM.Status = v.STATUS;
            VM.UnitName = v.UnitName;

            decimal total = VM.Amount;
            ViewBag.total = total;
            ViewBag.refno = VM.BillingId;
            ViewBag.code = System.Configuration.ConfigurationManager.AppSettings["IPAYCODE"];
            string key = System.Configuration.ConfigurationManager.AppSettings["IPAYKEY"];
            string signature = key + ViewBag.code + ViewBag.refno + total.ToString("N2").Replace(",", "").Replace(".", "") + "MYR";

            ViewBag.paymenturl = System.Configuration.ConfigurationManager.AppSettings["IPAYURL"];
            ViewBag.signature = Helper.SignatureEncrypt(signature); ;
            ViewBag.ResponseURL = System.Configuration.ConfigurationManager.AppSettings["ResponseURL"];
            ViewBag.BackendURL = System.Configuration.ConfigurationManager.AppSettings["BackendURL"];
            ViewBag.id = VM.BillingId;
            ViewBag.returntobill = returntobill + "/" + VM.BillingId;

            return View(VM);
        }

        public ActionResult TestUpload()
        {
            return View();
        }
        [HttpPost]
        public ActionResult TestUpload(HttpPostedFileBase FileImage)
        {
            ResComm.Web.Lib.Util.Upload upload = new Lib.Util.Upload();
            string result = upload.uploadFileToBlob(FileImage);

            return View();
        }
    }
}