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
    public class TicketController : AdminBaseController
    {
        TicketBLL TicketBLL = new TicketBLL();
        AccountBLL AccountBLL = new AccountBLL();

        public ActionResult List()
        {
            return View();
        }

        public ActionResult ListTicket(DTParameterModel param, string Name = "", string Priority = "", string Status = "", string Requester = "", string CreatedDate = "")
        {
            try
            {
                
                var UserId = long.Parse((string)Session["UserId"]);

                int TotalCount = 0;

                //sorting
                string orderingFunction = param.Columns.ElementAt(param.Order.FirstOrDefault().Column).Name == "" ? param.Columns.ElementAt(param.Order.FirstOrDefault().Column).Data : param.Columns.ElementAt(param.Order.FirstOrDefault().Column).Name;
                var sortDirection = param.Order.FirstOrDefault().Dir; // asc or desc

                //filtering
                TicketVO qFilter = new TicketVO()
                {
                    NAME = Name,
                    PRIORITY = Priority,
                    STATUS_CD = Status,
                    REQUESTER_NAME = Requester,
                };
                try
                {
                    qFilter.CREATED = DateTime.ParseExact(CreatedDate, "MM/dd/yyyy", null);
                }
                catch { }
                var List = TicketBLL.GetTickets(param.Start, param.Length, ref TotalCount, orderingFunction, sortDirection, qFilter);

                List<Ticket> TicketList = new List<Ticket>();

                foreach (var v in List)
                {
                    Ticket VM = new Ticket();
                    VM.TicketId = v.ROW_ID;
                    VM.RequesterAccountId = v.CASE_OWNER_ID;
                    VM.Requester = v.REQUESTER_NAME;
                    VM.CreatedDate = v.CREATED == null ? "" : v.CREATED.Value.ToMalaysiaTime().ToString("dd MMM yyyy");
                    VM.CategoryName = v.CATEGORY_NAME;
                    VM.Priority = v.PRIORITY;
                    VM.Status = v.STATUS_CD;
                    VM.Description = v.CASE_DESC;
                    VM.Name = v.NAME;
                    VM.EstStartDate = v.CASE_RESL_DT;
                    VM.EstEndDate = v.CASE_DUE_DT;
                    VM.Type = v.SVC_TYPE;

                    TicketList.Add(VM);
                }

                DTResult<Ticket> model = new DTResult<Ticket>
                {
                    draw = param.Draw,
                    data = TicketList,
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
            Ticket model = new Ticket();
            model.EstStartDate = DateTime.Now;
            model.EstEndDate = DateTime.Now;

            return View(model);
        }

        [HttpPost]
        public ActionResult Add(Ticket Model)
        {
            if (ModelState.IsValid)
            {
                if (Model.EstStartDate > Model.EstEndDate)
                {
                    TempData["Message"] = "Estimated Start Date cannot be later than Estimated End Date";
                }
                else
                {
                    try
                    {
                        
                        var UserId = long.Parse((string)Session["UserId"]);

                        var TicketVO = new TicketVO();
                        TicketVO.CASE_OWNER_ID = UserId;
                        TicketVO.CATEGORY_ID = Model.CategoryId;
                        TicketVO.PRIORITY = Model.Priority;
                        TicketVO.STATUS_CD = Model.Status;
                        TicketVO.CASE_DESC = Model.Description;
                        TicketVO.NAME = Model.Name;
                        TicketVO.CASE_RESL_DT = Model.EstStartDate;
                        TicketVO.CASE_DUE_DT = Model.EstEndDate;
                        TicketVO.SVC_TYPE = Model.Type;
                        TicketVO.CREATED_BY = UserId;

                        var result = TicketBLL.CreateTicket(TicketVO);

                        if (result != 0)
                        {
                            TempData["Message"] = "Successfully done.";
                            return RedirectToAction("List", "Ticket");
                        }
                    }
                    catch (Exception ex)
                    {
                        TempData["Message"] = ex.Message;
                    }
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
            Ticket model = new Ticket();

            var v = TicketBLL.Get(Id);

            model.TicketId = v.ROW_ID;
            model.RequesterAccountId = v.CASE_OWNER_ID;
            model.Requester = v.REQUESTER_NAME;
            model.Created = v.CREATED;
            model.CategoryId = v.CATEGORY_ID ?? 0;
            model.Priority = v.PRIORITY;
            model.Status = v.STATUS_CD;
            model.Description = v.CASE_DESC;
            model.Name = v.NAME;
            model.EstStartDate = v.CASE_RESL_DT;
            model.EstEndDate = v.CASE_DUE_DT;
            model.Type = v.SVC_TYPE;

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Ticket Model)
        {
            if (ModelState.IsValid)
            {
                if (Model.EstStartDate > Model.EstEndDate)
                {
                    TempData["Message"] = "Estimated Start Date cannot be later than Estimated End Date";
                }
                else
                {
                    try
                    {
                        
                        var UserId = long.Parse((string)Session["UserId"]);

                        var TicketVO = TicketBLL.Get(Model.TicketId);
                        TicketVO.CATEGORY_ID = Model.CategoryId;
                        TicketVO.PRIORITY = Model.Priority;
                        TicketVO.STATUS_CD = Model.Status;
                        TicketVO.CASE_DESC = Model.Description;
                        TicketVO.NAME = Model.Name;
                        TicketVO.CASE_RESL_DT = Model.EstStartDate;
                        TicketVO.CASE_DUE_DT = Model.EstEndDate;
                        TicketVO.SVC_TYPE = Model.Type;
                        TicketVO.LAST_UPD_BY = UserId;

                        TicketBLL.UpdateTicket(TicketVO);

                        TempData["Message"] = "Successfully done.";
                        return RedirectToAction("View", "Ticket", new { Id = Model.TicketId });
                    }
                    catch (Exception ex)
                    {
                        TempData["Message"] = ex.Message;
                    }
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
            Ticket model = new Ticket();

            var v = TicketBLL.Get(Id);

            model.TicketId = v.ROW_ID;
            model.RequesterAccountId = v.CASE_OWNER_ID;
            model.Requester = v.REQUESTER_NAME;
            model.Created = v.CREATED;
            model.CategoryName = v.CATEGORY_NAME;
            model.Priority = v.PRIORITY;
            model.Status = v.STATUS_CD;
            model.Description = v.CASE_DESC;
            model.Name = v.NAME;
            model.EstStartDate = v.CASE_RESL_DT;
            model.EstEndDate = v.CASE_DUE_DT;
            model.EstStartDateText = v.CASE_RESL_DT == null ? "" : v.CASE_RESL_DT.Value.ToString("dd MMM yyyy");
            model.EstEndDateText = v.CASE_DUE_DT == null ? "" : v.CASE_DUE_DT.Value.ToString("dd MMM yyyy");
            model.Type = v.SVC_TYPE;

            return View(model);
        }


    }
}