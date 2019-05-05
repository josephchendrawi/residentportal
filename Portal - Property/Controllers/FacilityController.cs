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
    public class FacilityController : MemberBaseController
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


        public ActionResult FC_FacilityBooking(long FacilityId = 0)
        {
            try
            {
                var UserId = long.Parse((string)Session["UserId"]);

                int temp = 0;
                var List = FacilityBookingBLL.GetFacilitySlotBookings(0, int.MaxValue, ref temp, "", "",
                    new FacilitySlotBookingVO() { VO_FACILITIY_ID = FacilityId, USER_ID = UserId });

                List<FullCalendarModel> FacilityBookingList = new List<FullCalendarModel>();

                foreach (var v in List)
                {
                    FullCalendarModel VM = new FullCalendarModel();
                    VM.id = v.ROW_ID;
                    VM.title = v.VO_FACILITY_START_TIME == null ? "" : v.VO_FACILITY_START_TIME.Value.ToString("HH:mm");
                    VM.title += " - ";
                    VM.title += v.VO_FACILITY_END_TIME == null ? "" : v.VO_FACILITY_END_TIME.Value.ToString("HH:mm");
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

        public ActionResult Book(int FacilityId, string Date)
        {
            var BookDate = DateTime.ParseExact(Date, "yyyy-MM-dd", null);

            var Facility = FacilityBLL.GetFacility(FacilityId);
            FacilityBook VM = new FacilityBook();
            VM.FacilityId = Facility.ROW_ID;
            VM.FacilityName = Facility.NAME;
            VM.BookDate = BookDate;

            int temp = 0;
            List<FacilitySlot> FacilitySlotList = new List<FacilitySlot>();
            foreach (var v in FacilityBLL.GetFacilitySlotsByFacilityId(FacilityId))
            {
                FacilitySlot FS = new FacilitySlot();
                FS.FacilitySlotId = v.ROW_ID;
                FS.StartTime = v.START_TIME == null ? "" : v.START_TIME.Value.ToString("HH:mm");
                FS.EndTime = v.END_TIME == null ? "" : v.END_TIME.Value.ToString("HH:mm");
                FS.Slot = v.SLOT ?? 0;

                var SlotBooked = FacilityBookingBLL.GetFacilitySlotBookings(0, int.MaxValue, ref temp, "", "",
                    new FacilitySlotBookingVO() { FACILITY_SLOT_ID = v.ROW_ID, BOOK_DATE = BookDate });
                FS.Slot -= SlotBooked.Count();

                FacilitySlotList.Add(FS);
            }
            ViewBag.FacilitySlotList = FacilitySlotList;

            return View(VM);
        }

        [HttpPost]
        public ActionResult Book(FacilityBook Model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var UserId = long.Parse((string)Session["UserId"]);

                    //check if slot is empty
                    //check if user already booked
                    //

                    var FacilitySlotBookingVO = new FacilitySlotBookingVO();
                    FacilitySlotBookingVO.BOOK_DATE = Model.BookDate;
                    FacilitySlotBookingVO.FACILITY_SLOT_ID = Model.FacilitySlotId;
                    FacilitySlotBookingVO.USER_ID = UserId;
                    FacilitySlotBookingVO.STATUS = 1;
                    FacilitySlotBookingVO.CREATED_BY = UserId;

                    var result = FacilityBookingBLL.CreateFacilitySlotBooking(FacilitySlotBookingVO);

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

            return RedirectToAction("Book", "Facility", new { FacilityId = Model.FacilityId, Date = Model.BookDate.Value.ToString("yyyy-MM-dd") });
        }
        public ActionResult DeleteBooking(long FacilityId, long FacilityBookingId)
        {
            try
            {
                var UserId = long.Parse((string)Session["UserId"]);

                var FacilityBooking = FacilityBookingBLL.GetFacilitySlotBooking(FacilityBookingId);

                if (FacilityBooking != null)
                {
                    FacilityBooking.STATUS = 2;
                    FacilityBookingBLL.UpdateFacilitySlotBooking(FacilityBooking);

                    TempData["Message"] = "Successfully done.";
                }
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
            }

            return RedirectToAction("View", "Facility", new { Id = FacilityId });
        }


    }
}