using ResComm.Web.Lib.BLL;
using ResComm.Web.Lib.DB.DAL;
using ResComm.Web.Lib.Interface.Common.Enums;
using ResComm.Web.Lib.Interface.Models.Unit;
using ResComm.Web.Lib.Interface.Models.UnitType;
using ResComm.Web.Models;
using ResComm.Web.Models.DataTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ResComm.Web.Controllers
{
    public class UnitTypeController : AdminBaseController
    {
        UnitBLL UnitBLL = new UnitBLL();
        UnitTypeBLL UnitTypeBLL = new UnitTypeBLL();
        AccountBLL AccountBLL = new AccountBLL();

        public ActionResult List()
        {
            return View();
        }

        public ActionResult ListUnitType(DTParameterModel param)
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
                UnitTypeVO qFilter = new UnitTypeVO()
                {
                    ///
                };
                var List = UnitTypeBLL.GetUnitTypesByProperty(PropertyId, param.Start, param.Length, ref TotalCount, orderingFunction, sortDirection, qFilter);

                List<UnitType> UnitTypeList = new List<UnitType>();

                foreach (var v in List)
                {
                    UnitType VM = new UnitType();
                    VM.UnitTypeId = v.ROW_ID;
                    VM.CategoryName = v.CATEGORY_NAME;
                    VM.CategoryValue = v.CATEGORY_VALUE;
                    //VM.Floor = v.CATEGORY_NAME == UNIT_TYPE_CATEGORY.Block.ToString() ? v.FLOOR : "-";

                    UnitTypeList.Add(VM);
                }

                DTResult<UnitType> model = new DTResult<UnitType>
                {
                    draw = param.Draw,
                    data = UnitTypeList,
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

        public JsonResult GetUnitTypes(string CategoryName)
        {
            
            var UserId = long.Parse((string)Session["UserId"]);
            var PropertyId = AccountBLL.GetRelation(UserId).PropertyId;

            int TotalCount = 0;
            
            //filtering
            UnitTypeVO qFilter = new UnitTypeVO()
            {
                CATEGORY_NAME = CategoryName
            };
            var List = UnitTypeBLL.GetUnitTypesByProperty(PropertyId, 0, int.MaxValue, ref TotalCount, "", "asc", qFilter);

            List<SelectListItem> Result = new List<SelectListItem>();
            foreach (var v in List)
            {
                Result.Add(new SelectListItem()
                {
                    Value = v.ROW_ID.ToString(),
                    Text = v.CATEGORY_VALUE// + (v.CATEGORY_NAME == UNIT_TYPE_CATEGORY.Block.ToString() ? " - " + v.FLOOR + "F" : "")
                });
            }

            return Json(Result, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult Add()
        {
            UnitType model = new UnitType();

            return View(model);
        }

        [HttpPost]
        public ActionResult Add(UnitType Model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    
                    var UserId = long.Parse((string)Session["UserId"]);
                    var PropertyId = AccountBLL.GetRelation(UserId).PropertyId;

                    var UnitTypeVO = new UnitTypeVO();
                    UnitTypeVO.CATEGORY_NAME = Model.CategoryName;
                    UnitTypeVO.CATEGORY_VALUE = Model.CategoryValue;
                    //UnitTypeVO.FLOOR = Model.Floor;
                    UnitTypeVO.PROPERTY_ID = PropertyId;
                    UnitTypeVO.CREATED_BY = UserId;
                    UnitTypeVO.ACTIVE_FLG = "1";

                    var result = UnitTypeBLL.CreateUnitType(UnitTypeVO);

                    if (result != 0)
                    {
                        TempData["Message"] = "Successfully done.";
                        return RedirectToAction("List", "UnitType");
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
            UnitType model = new UnitType();

            var v = UnitTypeBLL.Get(Id);

            model.UnitTypeId = v.ROW_ID;
            model.CategoryName = v.CATEGORY_NAME;
            model.CategoryValue = v.CATEGORY_VALUE;
            //model.Floor = v.FLOOR;

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(UnitType Model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    
                    var UserId = long.Parse((string)Session["UserId"]);

                    var UnitTypeVO = UnitTypeBLL.Get(Model.UnitTypeId);
                    UnitTypeVO.CATEGORY_NAME = Model.CategoryName;
                    UnitTypeVO.CATEGORY_VALUE = Model.CategoryValue;
                    //UnitTypeVO.FLOOR = Model.Floor;
                    UnitTypeVO.LAST_UPDATED_BY = UserId;

                    UnitTypeBLL.UpdateUnitType(UnitTypeVO);

                    TempData["Message"] = "Successfully done.";
                    return RedirectToAction("View", "UnitType", new { Id = Model.UnitTypeId });
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
            UnitType model = new UnitType();

            var v = UnitTypeBLL.Get(Id);

            model.UnitTypeId = v.ROW_ID;
            model.CategoryName = v.CATEGORY_NAME;
            model.CategoryValue = v.CATEGORY_VALUE;
            //model.Floor = v.FLOOR;

            return View(model);
        }

        public string Delete(long Id)
        {
            try
            {
                UnitTypeBLL.Delete(Id);

                return true.ToString();
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }


    }
}