using OfficeOpenXml;
using ResComm.Web.Lib.BLL;
using ResComm.Web.Lib.DB.DAL;
using ResComm.Web.Lib.Interface.Common.Enums;
using ResComm.Web.Lib.Interface.Models.Unit;
using ResComm.Web.Models;
using ResComm.Web.Models.DataTable;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ResComm.Web.Controllers
{
    public class UnitController : AdminBaseController
    {
        UnitBLL UnitBLL = new UnitBLL();
        UnitTypeBLL UnitTypeBLL = new UnitTypeBLL();
        AccountBLL AccountBLL = new AccountBLL();

        public ActionResult List()
        {
            return View();
        }

        public ActionResult ListUnit(DTParameterModel param)
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
                UnitVO qFilter = new UnitVO()
                {
                    ///
                };
                var List = UnitBLL.GetUnitsByProperty(PropertyId, param.Start, param.Length, ref TotalCount, orderingFunction, sortDirection, qFilter);

                List<Unit> UnitList = new List<Unit>();

                foreach (var v in List)
                {
                    Unit VM = new Unit();
                    VM.UnitId = v.ROW_ID;
                    VM.UnitNo = v.UNIT_NO;
                    VM.PropertyId = v.PROPERTY_ID;
                    VM.UnitType.UnitTypeId = v.UnitType.ROW_ID;
                    VM.UnitType.CategoryName = v.UnitType.CATEGORY_NAME;
                    VM.UnitType.CategoryValue = v.UnitType.CATEGORY_VALUE;
                    //VM.UnitType.Floor = v.UnitType.CATEGORY_NAME == UNIT_TYPE_CATEGORY.Block.ToString() ? v.UnitType.FLOOR : "-";

                    VM.TenantName = v.TENANT_NAME;
                    VM.TenantContactNo = "";//v.TENANT_CONTACTNO;
                    VM.OwnerName = v.OWNER_NAME;
                    VM.OwnerContactNo = "";//v.OWNER_CONTACTNO;

                    UnitList.Add(VM);
                }

                DTResult<Unit> model = new DTResult<Unit>
                {
                    draw = param.Draw,
                    data = UnitList,
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

        public int GetUnitExcel()
        {
            var UserId = long.Parse((string)Session["UserId"]);
            var PropertyId = AccountBLL.GetRelation(UserId).PropertyId;

            //get data list
            int TotalCount = 0;
            var List = UnitBLL.GetUnitsByProperty(PropertyId, 0, int.MaxValue, ref TotalCount, "", "", new UnitVO());


            //create excel file
            using (ExcelPackage pck = new ExcelPackage())
            {
                //Create the worksheet
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Result");

                //Populate Heading
                ws.Cells[1, 1].Value = "System Id";
                ws.Cells[1, 2].Value = "Unit";

                //Populate Data
                int row = 2;
                foreach (var v in List)
                {
                    ws.Cells[row, 1].Value = v.ROW_ID;
                    ws.Cells[row, 2].Value = v.UnitType.CATEGORY_VALUE + "-" + v.UNIT_NO;

                    row ++;
                }

                //Format the header
                using (ExcelRange rng = ws.Cells["A1:BZ1"])
                {
                    rng.Style.Font.Bold = true;
                    rng.Style.Font.Color.SetColor(Color.Black);
                }

                //Write it back to the client
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=UnitList-" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx");
                Response.BinaryWrite(pck.GetAsByteArray());
                Response.End();
            }

            return 0;
        }

        public JsonResult GetUnits(long UnitTypeId)
        {
            
            var UserId = long.Parse((string)Session["UserId"]);
            var PropertyId = AccountBLL.GetRelation(UserId).PropertyId;

            int TotalCount = 0;

            //filtering
            UnitVO qFilter = new UnitVO()
            {
                UNIT_TYPE_ID = UnitTypeId
            };
            var List = UnitBLL.GetUnitsByProperty(PropertyId, 0, int.MaxValue, ref TotalCount, "", "asc", qFilter);

            List<SelectListItem> Result = new List<SelectListItem>();
            foreach (var v in List)
            {
                Result.Add(new SelectListItem()
                {
                    Value = v.ROW_ID.ToString(),
                    Text = v.UNIT_NO
                });
            }

            return Json(Result, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult Add(long UnitTypeId = 0)
        {
            Unit model = new Unit();

            if (UnitTypeId != 0)
            {
                var UnitType = UnitTypeBLL.Get(UnitTypeId);

                model.UnitType.UnitTypeId = UnitType.ROW_ID;
                model.UnitType.CategoryName = UnitType.CATEGORY_NAME;
                model.UnitType.CategoryValue = UnitType.CATEGORY_VALUE;// +(UnitType.CATEGORY_NAME == UNIT_TYPE_CATEGORY.Block.ToString() ? " - " + UnitType.FLOOR + "F" : "");
                //model.UnitType.Floor = UnitType.FLOOR;
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Add(Unit Model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    
                    var UserId = long.Parse((string)Session["UserId"]);
                    var PropertyId = AccountBLL.GetRelation(UserId).PropertyId;

                    var UnitVO = new UnitVO();
                    UnitVO.UNIT_NO = Model.UnitNo;
                    UnitVO.UNIT_TYPE_ID = Model.UnitType.UnitTypeId;
                    UnitVO.CREATED_BY = UserId;
                    UnitVO.ACTIVE_FLG = "1";

                    var result = UnitBLL.CreateUnit(UnitVO);

                    if (result != 0)
                    {
                        TempData["Message"] = "Successfully done.";
                        return RedirectToAction("List", "Unit");
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
            Unit model = new Unit();

            var v = UnitBLL.Get(Id);

            model.UnitId = v.ROW_ID;
            model.UnitNo = v.UNIT_NO;
            model.UnitType.UnitTypeId = v.UnitType.ROW_ID;
            model.UnitType.CategoryName = v.UnitType.CATEGORY_NAME;
            model.UnitType.CategoryValue = v.UnitType.CATEGORY_VALUE;// +(v.UnitType.CATEGORY_NAME == UNIT_TYPE_CATEGORY.Block.ToString() ? " - " + v.UnitType.FLOOR + "F" : "");
            //model.UnitType.Floor = v.UnitType.FLOOR;

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Unit Model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    
                    var UserId = long.Parse((string)Session["UserId"]);

                    var UnitVO = UnitBLL.Get(Model.UnitId);
                    UnitVO.UNIT_NO = Model.UnitNo;
                    UnitVO.LAST_UPDATED_BY = UserId;

                    UnitBLL.UpdateUnit(UnitVO);

                    TempData["Message"] = "Successfully done.";
                    return RedirectToAction("View", "Unit", new { Id = Model.UnitId });
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
            Unit model = new Unit();

            var v = UnitBLL.Get(Id);

            model.UnitId = v.ROW_ID;
            model.UnitNo = v.UNIT_NO;
            model.UnitType.UnitTypeId = v.UnitType.ROW_ID;
            model.UnitType.CategoryName = v.UnitType.CATEGORY_NAME;
            model.UnitType.CategoryValue = v.UnitType.CATEGORY_VALUE;
            //model.UnitType.Floor = v.UnitType.FLOOR;

            return View(model);
        }

        public string Delete(long Id)
        {
            try
            {
                UnitBLL.Delete(Id);

                return true.ToString();
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public string Clear(long Id)
        {
            try
            {
                var Unit = UnitBLL.Get(Id);

                if (Unit != null)
                {
                    if (Unit.OWNER_ID != null)
                        UnitBLL.ClearOwnerFromUnit(Unit.OWNER_ID.Value);
                    if (Unit.TENANT_ID != null)
                        UnitBLL.ClearTenantFromUnit(Unit.TENANT_ID.Value);
                }

                return true.ToString();
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }


    }
}