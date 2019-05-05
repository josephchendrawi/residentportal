using ResComm.Web.Lib.BLL;
using ResComm.Web.Lib.DB.DAL;
using ResComm.Web.Lib.Interface.Models.Ticket;
using ResComm.Web.Models;
using ResComm.Web.Models.DataTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ResComm.Web.Controllers
{
    public class TicketCategoryController : AdminBaseController
    {
        TicketBLL TicketBLL = new TicketBLL();
        AccountBLL AccountBLL = new AccountBLL();

        public ActionResult List()
        {
            return View();
        }

        public ActionResult ListTicketCategory(DTParameterModel param, string Name = "")
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
                TicketCategoryVO qFilter = new TicketCategoryVO()
                {
                    NAME = Name,
                    ACCNT_ID = PropertyId
                };
                var List = TicketBLL.GetTicketCategories(param.Start, param.Length, ref TotalCount, orderingFunction, sortDirection, qFilter);

                List<TicketCategory> TicketCategoryList = new List<TicketCategory>();

                foreach (var v in List)
                {
                    TicketCategory VM = new TicketCategory();
                    VM.TicketCategoryId = v.ROW_ID;
                    VM.Description = v.DESCRIPTION;
                    VM.Name = v.NAME;

                    TicketCategoryList.Add(VM);
                }

                DTResult<TicketCategory> model = new DTResult<TicketCategory>
                {
                    draw = param.Draw,
                    data = TicketCategoryList,
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
            TicketCategory model = new TicketCategory();

            return View(model);
        }

        [HttpPost]
        public ActionResult Add(TicketCategory Model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var UserId = long.Parse((string)Session["UserId"]);
                    var PropertyId = AccountBLL.GetRelation(UserId).PropertyId;

                    var TicketCategoryVO = new TicketCategoryVO();
                    TicketCategoryVO.ACCNT_ID = PropertyId;
                    TicketCategoryVO.DESCRIPTION = Model.Description;
                    TicketCategoryVO.NAME = Model.Name;
                    TicketCategoryVO.CREATED_BY = UserId;

                    var result = TicketBLL.CreateTicketCategory(TicketCategoryVO);

                    if (result != 0)
                    {
                        TempData["Message"] = "Successfully done.";
                        return RedirectToAction("List", "TicketCategory");
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
            var UserId = long.Parse((string)Session["UserId"]);
            var PropertyId = AccountBLL.GetRelation(UserId).PropertyId;

            TicketCategory model = new TicketCategory();

            var v = TicketBLL.GetTicketCategory(Id);
            if (v.ACCNT_ID == PropertyId)
            {
                model.TicketCategoryId = v.ROW_ID;
                model.Description = v.DESCRIPTION;
                model.Name = v.NAME;

                return View(model);
            }
            else
            {
                throw new Exception("Not Found");
            }
        }

        [HttpPost]
        public ActionResult Edit(TicketCategory Model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var UserId = long.Parse((string)Session["UserId"]);
                    var PropertyId = AccountBLL.GetRelation(UserId).PropertyId;

                    var TicketCategoryVO = TicketBLL.GetTicketCategory(Model.TicketCategoryId);
                    if (TicketCategoryVO.ACCNT_ID != PropertyId)
                    {
                        TempData["Message"] = "Not Found";
                    }
                    else
                    {
                        TicketCategoryVO.DESCRIPTION = Model.Description;
                        TicketCategoryVO.NAME = Model.Name;
                        TicketCategoryVO.LAST_UPDATED_BY = UserId;

                        TicketBLL.UpdateTicketCategory(TicketCategoryVO);

                        TempData["Message"] = "Successfully done.";
                        return RedirectToAction("View", "TicketCategory", new { Id = Model.TicketCategoryId });
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

        public ActionResult View(int Id)
        {
            var UserId = long.Parse((string)Session["UserId"]);
            var PropertyId = AccountBLL.GetRelation(UserId).PropertyId;

            TicketCategory model = new TicketCategory();

            var v = TicketBLL.GetTicketCategory(Id);
            if (v.ACCNT_ID == PropertyId)
            {
                model.TicketCategoryId = v.ROW_ID;
                model.Description = v.DESCRIPTION;
                model.Name = v.NAME;

                return View(model);
            }
            else
            {
                throw new Exception("Not Found");
            }
        }

    }
}