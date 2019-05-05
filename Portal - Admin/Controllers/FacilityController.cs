using ResComm.Web.Lib.BLL;
using ResComm.Web.Lib.DB.DAL;
using ResComm.Web.Lib.Interface.Models.Facility;
using ResComm.Web.Models;
using ResComm.Web.Models.DataTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ResComm.Web.Controllers
{
    public class FacilityController : AdminBaseController
    {
        FacilityBLL FacilityBLL = new FacilityBLL();
        FacilityBookingBLL FacilityBookingBLL = new FacilityBookingBLL();
        AccountBLL AccountBLL = new AccountBLL();

        public ActionResult List()
        {
            return View();
        }

        public ActionResult ListFacility(DTParameterModel param, string Name = "", string Description = "", string CreatedDate = "")
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
                FacilityVO qFilter = new FacilityVO()
                {
                    NAME = Name,
                    DESCRIPTION = Description,
                };
                try
                {
                    qFilter.CREATED = DateTime.ParseExact(CreatedDate, "MM/dd/yyyy", null);
                }
                catch { }
                var List = FacilityBLL.GetFacilitiesByProperty(PropertyId, param.Start, param.Length, ref TotalCount, orderingFunction, sortDirection, qFilter);

                List<Facility> FacilityList = new List<Facility>();

                foreach (var v in List)
                {
                    Facility VM = new Facility();
                    VM.FacilityId = v.ROW_ID;
                    VM.CreatedDate = v.CREATED == null ? "" : v.CREATED.Value.ToMalaysiaTime().ToString("dd MMM yyyy");
                    VM.Description = v.DESCRIPTION;
                    VM.Name = v.NAME;

                    VM.TimeSlotCount = FacilityBLL.GetFacilitySlotsByFacilityId(v.ROW_ID).Count();

                    FacilityList.Add(VM);
                }

                DTResult<Facility> model = new DTResult<Facility>
                {
                    draw = param.Draw,
                    data = FacilityList,
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
            Facility model = new Facility();

            return View(model);
        }

        [HttpPost]
        public ActionResult Add(Facility Model)
        {
            if (ModelState.IsValid)
            {
                    try
                    {

                        var UserId = long.Parse((string)Session["UserId"]);
                        var PropertyId = AccountBLL.GetRelation(UserId).PropertyId;

                        var FacilityVO = new FacilityVO();
                        FacilityVO.NAME = Model.Name;
                        FacilityVO.DESCRIPTION = Model.Description;
                        FacilityVO.ACCNT_ID = PropertyId;
                        FacilityVO.STATUS = 1;
                        FacilityVO.CREATED_BY = UserId;

                        var result = FacilityBLL.CreateFacility(FacilityVO);

                        if (result != 0)
                        {
                            TempData["Message"] = "Successfully done.";
                            return RedirectToAction("View", "Facility", new { Id = result });
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
            var v = FacilityBLL.GetFacility(Id);

            Facility VM = new Facility();
            VM.FacilityId = v.ROW_ID;
            VM.CreatedDate = v.CREATED == null ? "" : v.CREATED.Value.ToMalaysiaTime().ToString("dd MMM yyyy HH:mm:ss AA");
            VM.Description = v.DESCRIPTION;
            VM.Name = v.NAME;

            return View(VM);
        }

        [HttpPost]
        public ActionResult Edit(Facility Model)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    var UserId = long.Parse((string)Session["UserId"]);

                    var FacilityVO = FacilityBLL.GetFacility(Model.FacilityId);
                    FacilityVO.NAME = Model.Name;
                    FacilityVO.DESCRIPTION = Model.Description;
                    FacilityVO.LAST_UPDATED_BY = UserId;

                    FacilityBLL.UpdateFacility(FacilityVO);

                    TempData["Message"] = "Successfully done.";
                    return RedirectToAction("View", "Facility", new { Id = Model.FacilityId });
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
            var v = FacilityBLL.GetFacility(Id);

            Facility VM = new Facility();
            VM.FacilityId = v.ROW_ID;
            VM.CreatedDate = v.CREATED == null ? "" : v.CREATED.Value.ToMalaysiaTime().ToString("dd MMM yyyy HH:mm:ss AA");
            VM.Description = v.DESCRIPTION;
            VM.Name = v.NAME;

            return View(VM);
        }
        
        public string Delete(long FacilityId)
        {
            var UserId = long.Parse((string)Session["UserId"]);

            var Facility = FacilityBLL.GetFacility(FacilityId);

            if (Facility != null)
            {
                Facility.STATUS = 2;
                Facility.LAST_UPDATED_BY = UserId;
                FacilityBLL.UpdateFacility(Facility);
                return "success";
            }
            else
            {
                return "Facility not found.";
            }
        }

        public ActionResult ListFacilitySlot(DTParameterModel param, long FacilityId = 0)
        {
            try
            {
                var List = FacilityBLL.GetFacilitySlotsByFacilityId(FacilityId);

                List<FacilitySlot> FacilitySlotList = new List<FacilitySlot>();

                foreach (var v in List)
                {
                    FacilitySlot VM = new FacilitySlot();
                    VM.FacilitySlotId = v.ROW_ID;
                    VM.StartTime = v.START_TIME == null ? "" : v.START_TIME.Value.ToString("HH:mm");
                    VM.EndTime = v.END_TIME == null ? "" : v.END_TIME.Value.ToString("HH:mm");
                    VM.Slot = v.SLOT ?? 0;

                    FacilitySlotList.Add(VM);
                }

                DTResult<FacilitySlot> model = new DTResult<FacilitySlot>
                {
                    draw = param.Draw,
                    data = FacilitySlotList,
                    recordsFiltered = FacilitySlotList.Count(),
                    recordsTotal = FacilitySlotList.Count()
                };

                return Json(model, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public ActionResult AddSlot(long FacilityId)
        {
            FacilitySlot model = new FacilitySlot();
            model.FacilityId = FacilityId;

            return View(model);
        }

        [HttpPost]
        public ActionResult AddSlot(FacilitySlot Model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    DateTime StartTime;
                    try { StartTime = DateTime.ParseExact("1753/01/01 " + Model.StartTime, "yyyy/MM/dd HH:mm", null); }
                    catch { throw new Exception("Invalid 'Start Time' Format."); }

                    DateTime EndTime;
                    try { EndTime = DateTime.ParseExact("1753/01/01 " + Model.EndTime, "yyyy/MM/dd HH:mm", null); }
                    catch { throw new Exception("Invalid 'End Time' Format."); }

                    if (StartTime == EndTime)
                    {
                        throw new Exception("'Start Time' and 'End Time' can't be same.");
                    }

                    var UserId = long.Parse((string)Session["UserId"]);

                    var FacilitySlotVO = new FacilitySlotVO();
                    FacilitySlotVO.FACILITY_ID = Model.FacilityId;
                    FacilitySlotVO.START_TIME = StartTime;
                    FacilitySlotVO.END_TIME = EndTime;
                    FacilitySlotVO.SLOT = Model.Slot;
                    FacilitySlotVO.STATUS = 1;
                    FacilitySlotVO.CREATED_BY = UserId;

                    var result = FacilityBLL.CreateFacilitySlot(FacilitySlotVO);

                    if (result != 0)
                    {
                        TempData["Message"] = "Successfully done.";
                        return RedirectToAction("View", "Facility", new { Id = Model.FacilityId });
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

        public ActionResult EditSlot(long FacilitySlotId)
        {
            var FacilitySlot = FacilityBLL.GetFacilitySlot(FacilitySlotId);

            FacilitySlot model = new FacilitySlot();
            model.FacilitySlotId = FacilitySlotId;
            model.FacilityId = FacilitySlot.FACILITY_ID ?? 0;
            model.StartTime = (FacilitySlot.START_TIME ?? DateTime.Now).ToString("HH:mm");
            model.EndTime = (FacilitySlot.END_TIME ?? DateTime.Now).ToString("HH:mm");
            model.Slot = FacilitySlot.SLOT ?? 0;

            return View(model);
        }

        [HttpPost]
        public ActionResult EditSlot(FacilitySlot Model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    DateTime StartTime;
                    try { StartTime = DateTime.ParseExact("1753/01/01 " + Model.StartTime, "yyyy/MM/dd HH:mm", null); }
                    catch { throw new Exception("Invalid 'Start Time' Format."); }

                    DateTime EndTime;
                    try { EndTime = DateTime.ParseExact("1753/01/01 " + Model.EndTime, "yyyy/MM/dd HH:mm", null); }
                    catch { throw new Exception("Invalid 'End Time' Format."); }

                    if (StartTime == EndTime)
                    {
                        throw new Exception("'Start Time' and 'End Time' can't be same.");
                    }

                    var UserId = long.Parse((string)Session["UserId"]);

                    var FacilitySlot = FacilityBLL.GetFacilitySlot(Model.FacilitySlotId);

                    if (FacilitySlot != null)
                    {
                        FacilitySlot.START_TIME = StartTime;
                        FacilitySlot.END_TIME = EndTime;
                        FacilitySlot.SLOT = Model.Slot;
                        FacilitySlot.LAST_UPDATED_BY = UserId;

                        FacilityBLL.UpdateFacilitySlot(FacilitySlot);

                        TempData["Message"] = "Successfully done.";
                        return RedirectToAction("View", "Facility", new { Id = Model.FacilityId });
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

        public string DeleteSlot(long FacilitySlotId)
        {
            var UserId = long.Parse((string)Session["UserId"]);

            var FacilitySlot = FacilityBLL.GetFacilitySlot(FacilitySlotId);

            if (FacilitySlot != null)
            {
                FacilityBLL.DeleteFacilitySlot(FacilitySlotId, UserId);
                return "success";
            }
            else
            {
                return "Facility Slot not found.";
            }
        }
        
        public ActionResult Calendar(int Id)
        {
            var v = FacilityBLL.GetFacility(Id);

            Facility VM = new Facility();
            VM.FacilityId = v.ROW_ID;
            VM.CreatedDate = v.CREATED == null ? "" : v.CREATED.Value.ToMalaysiaTime().ToString("dd MMM yyyy HH:mm:ss AA");
            VM.Description = v.DESCRIPTION;
            VM.Name = v.NAME;

            return View(VM);
        }

        public ActionResult FC_FacilityBooking(long FacilityId = 0)
        {
            try
            {
                int temp = 0;
                var List = FacilityBookingBLL.GetFacilitySlotBookings(0, int.MaxValue, ref temp, "", "",
                    new FacilitySlotBookingVO() { VO_FACILITIY_ID = FacilityId });

                List<FullCalendarModel> FacilityBookingList = new List<FullCalendarModel>();

                foreach (var v in List)
                {
                    FullCalendarModel VM = new FullCalendarModel();
                    VM.id = v.ROW_ID;
                    VM.title = v.VO_FACILITY_START_TIME == null ? "" : v.VO_FACILITY_START_TIME.Value.ToString("HH:mm");
                    VM.title += " - ";
                    VM.title += v.VO_FACILITY_END_TIME == null ? "" : v.VO_FACILITY_END_TIME.Value.ToString("HH:mm");
                    VM.title += " : ";
                    VM.title += v.VO_USER_FIRST_NAME;
                    VM.start = (v.BOOK_DATE ?? DateTime.MinValue).ToString("yyyy-MM-dd");

                    FacilityBookingList.Add(VM);
                }

                return Json(FacilityBookingList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    }
}