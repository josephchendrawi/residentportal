using ResComm.Web.Lib.BLL;
using ResComm.Web.Lib.DB.DAL;
using ResComm.Web.Lib.Interface.Models.AccountNote;
using ResComm.Web.Models;
using ResComm.Web.Models.DataTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace ResComm.Web.Controllers
{
    public class AnnouncementController : AdminBaseController
    {
        AnnouncementBLL AnnouncementBLL = new AnnouncementBLL();
        AccountBLL AccountBLL = new AccountBLL();

        public ActionResult List()
        {
            return View();
        }

        public ActionResult ListAnnouncement(DTParameterModel param)
        {
            try
            {
                
                var UserId = long.Parse((string)Session["UserId"]);
                var CustomerId = AccountBLL.GetRelation(UserId).CustomerId;

                int TotalCount = 0;

                //sorting
                string orderingFunction = param.Columns.ElementAt(param.Order.FirstOrDefault().Column).Name == "" ? param.Columns.ElementAt(param.Order.FirstOrDefault().Column).Data : param.Columns.ElementAt(param.Order.FirstOrDefault().Column).Name;
                var sortDirection = param.Order.FirstOrDefault().Dir; // asc or desc

                //filtering
                AccountNoteVO qFilter = new AccountNoteVO()
                {
                    ///
                };
                var List = AnnouncementBLL.GetAnnouncementByCustomerId(CustomerId, param.Start, param.Length, ref TotalCount, orderingFunction, sortDirection, qFilter);

                List<Announcement> AnnouncementList = new List<Announcement>();

                foreach (var v in List)
                {
                    Announcement VM = new Announcement();
                    VM.AccountNoteId = v.ROW_ID;
                    VM.Name = v.NAME;
                    VM.Content = Regex.Replace(v.NOTE.Substring(0, v.NOTE.IndexOf("\r\n")), "<.*?>", string.Empty);
                    VM.LastUpdatedText = (v.LAST_UPD ?? v.CREATED.Value).ToMalaysiaTime().ToString("dd MMM yyyy");
                    VM.CreatedBy = v.CreatedBy;
                    VM.LastUpdatedBy = v.LastUpdatedBy;

                    AnnouncementList.Add(VM);
                }

                DTResult<Announcement> model = new DTResult<Announcement>
                {
                    draw = param.Draw,
                    data = AnnouncementList,
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
            Announcement model = new Announcement();

            return View(model);
        }

        [HttpPost]
        public ActionResult Add(Announcement Model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    
                    var UserId = long.Parse((string)Session["UserId"]);
                    var CustomerId = AccountBLL.GetRelation(UserId).CustomerId;

                    var AccountNoteVO = new AccountNoteVO();

                    AccountNoteVO.NOTE = Model.Content;
                    AccountNoteVO.ACCNT_ID = CustomerId;
                    AccountNoteVO.NAME = Model.Name;
                    AccountNoteVO.CREATED_BY = UserId;

                    var result = AnnouncementBLL.CreateAnnouncement(AccountNoteVO);

                    if (result != 0)
                    {
                        TempData["Message"] = "Successfully done.";
                        return RedirectToAction("List", "Announcement");
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
            Announcement model = new Announcement();

            var v = AnnouncementBLL.Get(Id);

            model.AccountNoteId = v.ROW_ID;
            model.Name = v.NAME;
            model.Content = v.NOTE;

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Announcement Model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    
                    var UserId = long.Parse((string)Session["UserId"]);

                    var AccountNoteVO = AnnouncementBLL.Get(Model.AccountNoteId);
                    AccountNoteVO.NOTE = Model.Content;
                    AccountNoteVO.NAME = Model.Name;
                    AccountNoteVO.LAST_UPD_BY = UserId;

                    AnnouncementBLL.UpdateAnnouncement(AccountNoteVO);

                    TempData["Message"] = "Successfully done.";
                    return RedirectToAction("View", "Announcement", new { Id = Model.AccountNoteId });
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
            Announcement model = new Announcement();

            var v = AnnouncementBLL.Get(Id);

            model.AccountNoteId = v.ROW_ID;
            model.Name = v.NAME;
            model.Content = v.NOTE;
            model.LastUpdatedText = (v.LAST_UPD ?? v.CREATED.Value).ToMalaysiaTime().ToString("dd MMM yyyy HH:mm:ss AA");

            return View(model);
        }


    }
}