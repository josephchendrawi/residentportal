using ResComm.Web.Lib.BLL;
using ResComm.Web.Lib.DB.DAL;
using ResComm.Web.Lib.Enums;
using ResComm.Web.Lib.Interface.Common.Enums;
using ResComm.Web.Lib.Interface.Models.Account;
using ResComm.Web.Lib.Interface.Models.Address;
using ResComm.Web.Lib.Interface.Models.Property;
using ResComm.Web.Lib.Interface.Models.PropertyInvoice;
using ResComm.Web.Lib.Interface.Models.Unit;
using ResComm.Web.Lib.Interface.Models.User;
using ResComm.Web.Models;
using ResComm.Web.Models.DataTable;
using Rotativa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ResComm.Web.Controllers
{
    public class SuperAdminController : SuperAdminBaseController
    {
        private UserBLL UserBLL = new UserBLL();
        private PropertyInvoiceDALImpl PropertyInvoiceBLL = new PropertyInvoiceDALImpl();
        private PropertyBLL PropertyBLL = new PropertyBLL();
        private AccountBLL AccountBLL = new AccountBLL();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Affiliate()
        {
            return View();
        }

        public ActionResult ListAffiliate(DTParameterModel param)
        {
            try
            {
                int TotalCount = 0;

                //sorting
                string orderingFunction = param.Columns.ElementAt(param.Order.FirstOrDefault().Column).Name == "" ? param.Columns.ElementAt(param.Order.FirstOrDefault().Column).Data : param.Columns.ElementAt(param.Order.FirstOrDefault().Column).Name;
                var sortDirection = param.Order.FirstOrDefault().Dir; // asc or desc

                //filtering
                UserVO qFilter = new UserVO();
                var List = UserBLL.GetAffiliateUsers(param.Start, param.Length, ref TotalCount, orderingFunction, sortDirection, qFilter);

                List<Affiliate> UserList = new List<Affiliate>();

                foreach (var v in List)
                {
                    Affiliate VM = new Affiliate();
                    VM.UserId = v.ROW_ID;
                    VM.Email = v.USERNAME;
                    VM.ContactName = v.FIRST_NAME ?? "-";
                    VM.ContactNo = v.MOBILE_NO ?? "-";
                    VM.Status = ((USER_ACCNT_STATUS)int.Parse(v.ACCNT_STATUS)).ToString();

                    UserList.Add(VM);
                }

                DTResult<Affiliate> model = new DTResult<Affiliate>
                {
                    draw = param.Draw,
                    data = UserList,
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
        
        public ActionResult ApproveAffiliate(long Id)
        {
            try
            {
                var User = UserBLL.Get(Id);

                if (User.USER_TYPE == ((int)USER_TYPE.Affiliate).ToString())
                {
                    UserBLL.UpdateStatus(Id, USER_ACCNT_STATUS.Active);

                    TempData["Message"] = "Successfully done.";
                }
                else
                {
                    throw new Exception("Invalid Affiliate.");
                }
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
            }

            return RedirectToAction("AffiliateList");
        }

        public ActionResult RejectAffiliate(long Id)
        {
            try
            {
                var User = UserBLL.Get(Id);

                if (User.USER_TYPE == ((int)USER_TYPE.Affiliate).ToString())
                {
                    UserBLL.UpdateStatus(Id, USER_ACCNT_STATUS.Inactive);

                    TempData["Message"] = "Successfully done.";
                }
                else
                {
                    throw new Exception("Invalid Affiliate.");
                }
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
            }

            return RedirectToAction("AffiliateList");
        }

        public ActionResult PropertyInvoice()
        {
            return View();
        }

        public ActionResult ListPropertyInvoice(DTParameterModel param, string InvoiceNo, string PropertyName, string InvoiceDateStart, string InvoiceDateEnd, int Status = 0, int PaymentStatus = 0)
        {
            try
            {
                var UserId = long.Parse((string)Session["UserId"]);

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
                    PAYMENT_STATUS = PaymentStatus
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

        public ActionResult PropertyInvoiceDetails(int Id)
        {
            var v = PropertyInvoiceBLL.GetPropertyInvoice(Id);

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

        private List<PropertyInfo> GetPropertyList()
        {
            List<PropertyInfo> PropertyList = new List<PropertyInfo>();
            int TotalCount = 0;
            foreach (var v in PropertyBLL.GetAll(0, int.MaxValue, ref TotalCount, "", "", new PropertyVO()))
            {
                PropertyList.Add(new PropertyInfo()
                {
                    PropertyId = v.ROW_ID,
                    PropertyName = v.NAME
                });
            }

            return PropertyList;
        }

        public ActionResult PropertyInvoiceAdd()
        {
            PropertyInvoice model = new PropertyInvoice();
            model.InvoiceDate = DateTime.Now;

            ViewBag.PropertyList = GetPropertyList();

            return View(model);
        }

        [HttpPost]
        public ActionResult PropertyInvoiceAdd(PropertyInvoice Model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var UserId = long.Parse((string)Session["UserId"]);

                    var PropertyInvoiceVO = new PropertyInvoiceVO();
                    //PropertyInvoiceVO.INVOICE_NUM = Model.InvoiceNo; //auto generated
                    PropertyInvoiceVO.INVOICE_DATE = Model.InvoiceDate;
                    PropertyInvoiceVO.PROPERTY_ID = Model.PropertyId;
                    PropertyInvoiceVO.DESCRIPTION = Model.Description;
                    PropertyInvoiceVO.AMOUNT = Model.Amount;
                    PropertyInvoiceVO.STATUS = Model.Status;

                    PropertyInvoiceVO.PAYMENT_STATUS = Model.PaymentStatus;
                    PropertyInvoiceVO.PAYMENT_DATE = Model.PaymentDate;
                    PropertyInvoiceVO.PAYMENT_METHOD = Model.PaymentMethod;
                    PropertyInvoiceVO.CHEQUE_NO = Model.ChequeNo;
                    PropertyInvoiceVO.BANK = Model.Bank;
                    PropertyInvoiceVO.TRANSACTION_NO = Model.TransactionNo;
                    PropertyInvoiceVO.APPROVAL_CODE = Model.ApprovalCode;

                    PropertyInvoiceVO.CREATED_BY = UserId;

                    var result = PropertyInvoiceBLL.CreatePropertyInvoice(PropertyInvoiceVO, UserId);

                    if (result != 0)
                    {
                        TempData["Message"] = "Successfully done.";
                        return RedirectToAction("PropertyInvoice");
                    }
                }
                catch (Exception ex)
                {
                    TempData["Message"] = ex.Message;
                }
            }
            else
            {
                TempData["Message"] = string.Join(" ", ModelState.Values
                                        .SelectMany(v => v.Errors)
                                        .Select(e => e.ErrorMessage));
            }

            ViewBag.PropertyList = GetPropertyList();

            return View(Model);
        }

        public ActionResult PropertyInvoiceEdit(int Id)
        {
            var v = PropertyInvoiceBLL.GetPropertyInvoice(Id);

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

        [HttpPost]
        public ActionResult PropertyInvoiceEdit(PropertyInvoice Model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var UserId = long.Parse((string)Session["UserId"]);

                    var PropertyInvoiceVO = PropertyInvoiceBLL.GetPropertyInvoice(Model.PropertyInvoiceId);

                    //PropertyInvoiceVO.INVOICE_DATE = Model.InvoiceDate;
                    //PropertyInvoiceVO.PROPERTY_ID = Model.PropertyId;
                    //PropertyInvoiceVO.DESCRIPTION = Model.Description;
                    //PropertyInvoiceVO.AMOUNT = Model.Amount;
                    PropertyInvoiceVO.STATUS = Model.Status;

                    PropertyInvoiceVO.PAYMENT_STATUS = Model.PaymentStatus;
                    PropertyInvoiceVO.PAYMENT_DATE = Model.PaymentDate;
                    PropertyInvoiceVO.PAYMENT_METHOD = Model.PaymentMethod;
                    PropertyInvoiceVO.CHEQUE_NO = Model.ChequeNo;
                    PropertyInvoiceVO.BANK = Model.Bank;
                    PropertyInvoiceVO.TRANSACTION_NO = Model.TransactionNo;
                    PropertyInvoiceVO.APPROVAL_CODE = Model.ApprovalCode;

                    PropertyInvoiceVO.CREATED_BY = UserId;

                    PropertyInvoiceBLL.UpdatePropertyInvoice(PropertyInvoiceVO, UserId);

                    TempData["Message"] = "Successfully done.";
                    return RedirectToAction("PropertyInvoice");
                }
                catch (Exception ex)
                {
                    TempData["Message"] = ex.Message;
                }
            }
            else
            {
                TempData["Message"] = string.Join(" ", ModelState.Values
                                        .SelectMany(v => v.Errors)
                                        .Select(e => e.ErrorMessage));
            }
            
            return View(Model);
        }

        public ActionResult PropertyInvoiceDownloadPDF(int Id)
        {
            var Invoice = PropertyInvoiceBLL.GetPropertyInvoice(Id);

            var Result = new Rotativa.UrlAsPdf(Url.Action("PropertyInvoicePDF", "Helper", new { Id = Id })) { FileName = "Invoice-"+Invoice.INVOICE_NUM+".pdf" };
            
            return Result;
        }
        public ActionResult Property()
        {
            return View();
        }

        public ActionResult ListProperty(DTParameterModel param, string Name)
        {
            try
            {
                int TotalCount = 0;

                //sorting
                string orderingFunction = param.Columns.ElementAt(param.Order.FirstOrDefault().Column).Name == "" ? param.Columns.ElementAt(param.Order.FirstOrDefault().Column).Data : param.Columns.ElementAt(param.Order.FirstOrDefault().Column).Name;
                var sortDirection = param.Order.FirstOrDefault().Dir; // asc or desc

                //filtering
                PropertyVO qFilter = new PropertyVO()
                {
                    NAME = Name,
                };
                var List = PropertyBLL.GetAll(param.Start, param.Length, ref TotalCount, orderingFunction, sortDirection, qFilter);

                List<PropertyInfo_SuperAdmin> PropertyList = new List<PropertyInfo_SuperAdmin>();

                foreach (var v in List)
                {
                    PropertyInfo_SuperAdmin VM = new PropertyInfo_SuperAdmin();

                    VM.PropertyId = v.ROW_ID;
                    VM.PropertyName = v.NAME;
                    VM.Address1 = v.ADDR_1;
                    VM.Address2 = v.ADDR_2;
                    VM.City = v.CITY;
                    VM.State = v.STATE;
                    VM.ZIP = v.POSTAL_CD;
                    VM.Country = v.COUNTRY;
                    VM.LogoURL = v.LOGO_URL;

                    var User = UserBLL.Get(AccountBLL.Get(v.ACCNT_ID.Value).CUST_USER_ID.Value);

                    VM.Status = ((USER_ACCNT_STATUS)int.Parse(User.ACCNT_STATUS)).ToString();

                    PropertyList.Add(VM);
                }

                DTResult<PropertyInfo_SuperAdmin> model = new DTResult<PropertyInfo_SuperAdmin>
                {
                    draw = param.Draw,
                    data = PropertyList,
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

        public ActionResult ApproveProperty(long Id)
        {
            var Property = PropertyBLL.Get(Id);
            var User = UserBLL.Get(AccountBLL.Get(Property.ACCNT_ID.Value).CUST_USER_ID.Value);
            UserBLL.UpdateStatus(User.ROW_ID, USER_ACCNT_STATUS.Active);

            return RedirectToAction("Property");
        }


    }
}