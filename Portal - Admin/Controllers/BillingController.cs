using Excel;
using ResComm.Web.Lib.BLL;
using ResComm.Web.Lib.DB.DAL;
using ResComm.Web.Lib.Interface.Common.Enums;
using ResComm.Web.Lib.Interface.Models.Billing;
using ResComm.Web.Lib.Interface.Models.Unit;
using ResComm.Web.Lib.Interface.Models.User;
using ResComm.Web.Models;
using ResComm.Web.Models.DataTable;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ResComm.Web.Controllers
{
    public class BillingController : AdminBaseController
    {
        BillingBLL BillingBLL = new BillingBLL();
        AccountBLL AccountBLL = new AccountBLL();
        UnitBLL UnitBLL = new UnitBLL();

        public ActionResult List()
        {
            return View();
        }

        public ActionResult ListBilling(DTParameterModel param, string Reference = "", string InvoiceNo = "", string UnitName = "", string Status = "", string BillingDate = "")
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
                    UnitName = UnitName,
                };
                try
                {
                    qFilter.BILLING_DATE = DateTime.ParseExact(BillingDate, "MM/dd/yyyy", null);
                }
                catch { }
                var List = BillingBLL.GetBillings(param.Start, param.Length, ref TotalCount, orderingFunction, sortDirection, qFilter);

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
                    VM.BillingDate = v.BILLING_DATE;
                    VM.BillingDateText = v.BILLING_DATE == null ? "" : v.BILLING_DATE.Value.ToMalaysiaTime().ToString("dd MMM yyyy");
                    VM.InvoiceNo = v.INVOICE_NO;
                    VM.Reference = v.REFERENCE;
                    VM.Status = v.STATUS;
                    VM.UnitName = v.UnitName;

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
        
        public ActionResult Add()
        {
            AddEditBillingVM model = new AddEditBillingVM();
            model.BillingDate = DateTime.Now;

            return View(model);
        }

        [HttpPost]
        public ActionResult Add(AddEditBillingVM Model, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var UserId = long.Parse((string)Session["UserId"]);

                    var BillingVO = new BillingVO();
                    BillingVO.AMOUNT = Model.Amount;
                    BillingVO.DUE_DATE = Model.DueDate;
                    BillingVO.BILLING_DATE = Model.BillingDate;
                    BillingVO.REFERENCE = Model.Reference;
                    BillingVO.UNIT_ID = Model.UnitId;
                    BillingVO.STATUS = BILLING_STATUS.Unpaid.ToString();
                    BillingVO.CREATED_BY = UserId;

                    if (file != null)
                    {
                        var FilePath = FileHelper.SaveFile(file, DateTime.Now.ToString("yyyyMMddHHmmss"));

                        BillingVO.FILE_NAME = FilePath;
                    }

                    var result = BillingBLL.CreateBilling(BillingVO);

                    if (result != 0)
                    {
                        TempData["Message"] = "Successfully done.";
                        return RedirectToAction("List", "Billing");
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

            return View(Model);
        }

        public ActionResult Edit(int Id)
        {
            AddEditBillingVM VM = new AddEditBillingVM();

            var v = BillingBLL.Get(Id);

            VM.BillingId = v.ROW_ID;
            VM.Amount = v.AMOUNT ?? 0;
            VM.CreatedDate = v.CREATED;
            VM.CreatedDateText = v.CREATED == null ? "" : v.CREATED.Value.ToMalaysiaTime().ToString("dd MMM yyyy");
            VM.DueDate = v.DUE_DATE;
            VM.DueDateText = v.DUE_DATE == null ? "" : v.DUE_DATE.Value.ToMalaysiaTime().ToString("dd MMM yyyy");
            VM.BillingDate = v.BILLING_DATE;
            VM.BillingDateText = v.BILLING_DATE == null ? "" : v.BILLING_DATE.Value.ToMalaysiaTime().ToString("dd MMM yyyy");
            VM.InvoiceNo = v.INVOICE_NO;
            VM.Reference = v.REFERENCE;
            VM.UnitId = v.UNIT_ID ?? 0;
            VM.Status = v.STATUS;
            VM.FileName = v.FILE_NAME;
            VM.UnitName = v.UnitName;

            return View(VM);
        }

        [HttpPost]
        public ActionResult Edit(AddEditBillingVM Model, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var UserId = long.Parse((string)Session["UserId"]);

                    var BillingVO = BillingBLL.Get(Model.BillingId);
                    BillingVO.AMOUNT = Model.Amount;
                    BillingVO.DUE_DATE = Model.DueDate;
                    BillingVO.BILLING_DATE = Model.BillingDate;
                    BillingVO.REFERENCE = Model.Reference;
                    BillingVO.STATUS = Model.Status;
                    BillingVO.LAST_UPDATED_BY = UserId;
                    if (file != null)
                    {
                        var FilePath = FileHelper.SaveFile(file, DateTime.Now.ToString("yyyyMMddHHmmss"));

                        BillingVO.FILE_NAME = FilePath;
                    }

                    BillingBLL.UpdateBilling(BillingVO);

                    TempData["Message"] = "Successfully done.";
                    return RedirectToAction("List", "Billing", new { Id = Model.BillingId });
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
            VM.BillingDate = v.BILLING_DATE;
            VM.BillingDateText = v.BILLING_DATE == null ? "" : v.BILLING_DATE.Value.ToMalaysiaTime().ToString("dd MMM yyyy");
            VM.InvoiceNo = v.INVOICE_NO;
            VM.Reference = v.REFERENCE;
            VM.Status = v.STATUS;
            VM.FileName = v.FILE_NAME;
            VM.UnitName = v.UnitName;

            return View(VM);
        }

        public string Delete(long BillingId)
        {
            var UserId = long.Parse((string)Session["UserId"]);

            var Billing = BillingBLL.Get(BillingId);

            if (Billing != null)
            {
                BillingBLL.DeleteBilling(BillingId, UserId);
                return "success";
            }
            else
            {
                return "Billing not found.";
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ImportExcel(HttpPostedFileBase upload)
        {
            var UserId = long.Parse((string)Session["UserId"]);
            var PropertyId = AccountBLL.GetRelation(UserId).PropertyId;

            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    // ExcelDataReader works with the binary Excel file, so it needs a FileStream
                    // to get started. This is how we avoid dependencies on ACE or Interop:
                    Stream stream = upload.InputStream;

                    // We return the interface, so that
                    IExcelDataReader reader = null;


                    if (upload.FileName.EndsWith(".xls"))
                    {
                        reader = ExcelReaderFactory.CreateBinaryReader(stream);
                    }
                    else if (upload.FileName.EndsWith(".xlsx"))
                    {
                        reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    }
                    else
                    {
                        ModelState.AddModelError("File", "This file format is not supported");
                        return View();
                    }

                    reader.IsFirstRowAsColumnNames = true;

                    DataSet result = reader.AsDataSet();
                    reader.Close();

                    foreach (DataRow rw in result.Tables[0].Rows)
                    {
                        try
                        {
                            var BillingDate = new DateTime();
                            try
                            {
                                BillingDate = (DateTime)rw.ItemArray[1];
                            }
                            catch
                            {
                                BillingDate = DateTime.ParseExact(rw.ItemArray[1].ToString(), "yyyy-MM-dd", null);
                            }

                            var UnitId = int.Parse(rw.ItemArray[2].ToString());
                            if (UnitBLL.Get(UnitId, PropertyId) == null)
                            {
                                throw new Exception("Unit not found.");
                            }

                            var InvoiceNo = rw.ItemArray[3].ToString();

                            var Amount = decimal.Parse(rw.ItemArray[4].ToString());

                            var DueDate = new DateTime();
                            try
                            {
                                DueDate = (DateTime)rw.ItemArray[5];
                            }
                            catch
                            {
                                DueDate = DateTime.ParseExact(rw.ItemArray[5].ToString(), "yyyy-MM-dd", null);
                            }

                            var FileName = rw.ItemArray[6].ToString();
                            
                            var BillingVO = new BillingVO();
                            BillingVO.AMOUNT = Amount;
                            BillingVO.DUE_DATE = DueDate;
                            BillingVO.BILLING_DATE = BillingDate;
                            BillingVO.UNIT_ID = UnitId;
                            BillingVO.INVOICE_NO = InvoiceNo;
                            BillingVO.FILE_NAME = FileName;

                            BillingVO.STATUS = BILLING_STATUS.Unpaid.ToString();
                            BillingVO.CREATED_BY = UserId;

                            BillingBLL.CreateBilling(BillingVO);
                        }
                        catch { }
                    }
                }
                else
                {
                    ModelState.AddModelError("File", "Please Upload Your file");
                }
            }

            return RedirectToAction("List");
        }


    }
}