using ResComm.Web.Lib.BLL;
using ResComm.Web.Lib.Interface.Models.Billing;
using ResComm.Web.Models;
using ResComm.Web.Models.DataTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ResComm.Web.Controllers
{
    public class BillingController : MemberBaseController
    {
        BillingBLL BillingBLL = new BillingBLL();
        UnitBLL UnitBLL = new UnitBLL();

        public ActionResult List()
        {
            ViewBag.payurl = System.Configuration.ConfigurationManager.AppSettings["PAYURL"];
            return View();
        }

        public ActionResult ListBilling(DTParameterModel param, string Reference = "", string InvoiceNo = "", string UnitName = "", string Status = "", string CreatedDate = "")
        {
            try
            {
                var UserId = long.Parse((string)Session["UserId"]);

                int TotalCount = 0;

                //sorting
                string orderingFunction = param.Columns.ElementAt(param.Order.FirstOrDefault().Column).Name == "" ? param.Columns.ElementAt(param.Order.FirstOrDefault().Column).Data : param.Columns.ElementAt(param.Order.FirstOrDefault().Column).Name;
                var sortDirection = param.Order.FirstOrDefault().Dir; // asc or desc

                //filtering
                BillingVO qFilter = new BillingVO()
                {
                    REFERENCE = Reference,
                    STATUS = Status,
                    INVOICE_NO = InvoiceNo,
                    UnitName = UnitName
                };
                try
                {
                    qFilter.CREATED = DateTime.ParseExact(CreatedDate, "MM/dd/yyyy", null);
                }
                catch { }
                var List = BillingBLL.GetBillingsByMemberId(UserId, param.Start, param.Length, ref TotalCount, orderingFunction, sortDirection, qFilter);

                List<Billing> BillingList = new List<Billing>();

                foreach (var v in List)
                {
                    Billing VM = new Billing();
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
                    VM.Token = v.TOKEN;
                    BillingList.Add(VM);
                }

                DTResult<Billing> model = new DTResult<Billing>
                {
                    draw = param.Draw,
                    data = BillingList,
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

        public ActionResult View(int Id)
        {
            Billing VM = new Billing();

            var v = BillingBLL.Get(Id);

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

            return View(VM);
        }

    }
}