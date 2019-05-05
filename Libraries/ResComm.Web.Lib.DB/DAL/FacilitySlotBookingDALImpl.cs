using AutoMapper;
using ResComm.Web.Lib.Interface.Common.Constants;
using ResComm.Web.Lib.DB.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ResComm.Web.Lib.Interface.Common.Enums;
using System.Data.Entity;
using ResComm.Web.Lib.Interface.Models.Facility;

namespace ResComm.Web.Lib.DB.DAL
{
    public class FacilitySlotBookingDALImpl : IFacilitySlotBookingDAL
    {
        IMapper Mapper;
        public FacilitySlotBookingDALImpl()
        {
            var AutoMapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<P_FACILITY_SLOT_BOOKING, FacilitySlotBookingVO>();
                cfg.CreateMap<FacilitySlotBookingVO, P_FACILITY_SLOT_BOOKING>();
            });

            Mapper = AutoMapperConfig.CreateMapper();
        }

        #region MapReturnList
        List<FacilitySlotBookingVO> MapReturnList(List<P_FACILITY_SLOT_BOOKING> aList)
        {
            // map
            var returnedResult = new List<FacilitySlotBookingVO>();

            foreach (var s in aList)
            {
                var a = Mapper.Map<P_FACILITY_SLOT_BOOKING, FacilitySlotBookingVO>(s);

                returnedResult.Add(a);
            }

            return returnedResult;
        }

        #endregion

        #region Get FacilitySlotBooking
        public FacilitySlotBookingVO GetFacilitySlotBooking(long FacilitySlotBookingId)
        {
            using (var DBContext = new PropComDbContext())
            {
                var result = from d in DBContext.P_FACILITY_SLOT_BOOKING
                             where d.ROW_ID == FacilitySlotBookingId
                             select d;

                if (result.Count() > 0)
                {
                    var P_FACILITY_SLOT_BOOKING = result.First();
                    var FacilitySlotBookingVO = Mapper.Map<P_FACILITY_SLOT_BOOKING, FacilitySlotBookingVO>(P_FACILITY_SLOT_BOOKING);

                    FacilitySlotBookingVO.VO_FACILITY_START_TIME = P_FACILITY_SLOT_BOOKING.P_FACILITY_SLOT.START_TIME;
                    FacilitySlotBookingVO.VO_FACILITY_END_TIME = P_FACILITY_SLOT_BOOKING.P_FACILITY_SLOT.END_TIME;

                    return FacilitySlotBookingVO;
                }
                else
                {
                    return null;
                }
            }
        }

        public List<FacilitySlotBookingVO> GetFacilitySlotBookings(int startIdx, int length, ref int TotalCount, string orderBy = "", string orderDirection = "", FacilitySlotBookingVO qFilter = null, long PropertyId = 0)
        {
            List<FacilitySlotBookingVO> result = new List<FacilitySlotBookingVO>();
            using (var DBContext = new PropComDbContext())
            {
                var ett = from d in DBContext.P_FACILITY_SLOT_BOOKING
                          where d.STATUS == 1 ///
                          select d;

                //filtering
                if (qFilter.USER_ID != null && qFilter.USER_ID != 0)
                {
                    ett = ett.Where(m => m.USER_ID == qFilter.USER_ID);
                }
                if (qFilter.VO_FACILITIY_ID != null && qFilter.VO_FACILITIY_ID != 0)
                {
                    ett = ett.Where(m => m.P_FACILITY_SLOT.FACILITY_ID == qFilter.VO_FACILITIY_ID);
                }
                if (qFilter.FACILITY_SLOT_ID != null && qFilter.FACILITY_SLOT_ID != 0)
                {
                    ett = ett.Where(m => m.FACILITY_SLOT_ID == qFilter.FACILITY_SLOT_ID);
                }
                if (qFilter.BOOK_DATE != null)
                {
                    DateTime date = qFilter.BOOK_DATE.Value;
                    ett = ett.Where(m => m.BOOK_DATE != null);
                    ett = ett.Where(m => m.BOOK_DATE.Value.Year == date.Year && m.BOOK_DATE.Value.Month == date.Month && m.BOOK_DATE.Value.Day == date.Day);
                }
                if (qFilter.CREATED != null)
                {
                    DateTime date = qFilter.CREATED.Value;
                    ett = ett.Where(m => m.CREATED != null);
                    ett = ett.Where(m => m.CREATED.Value.Year == date.Year && m.CREATED.Value.Month == date.Month && m.CREATED.Value.Day == date.Day);
                }
                if (PropertyId != 0)
                {
                    ett = ett.Where(m => m.P_FACILITY_SLOT.P_FACILITY.P_ACCNT.ROW_ID == PropertyId);
                }
                /*if (qFilter.FACILITY_START_TIME != null)
                {
                    DateTime date = qFilter.FACILITY_START_TIME.Value;
                    ett = ett.Where(m => m.P_FACILITY_SLOT.START_TIME != null);
                    ett = ett.Where(m => m.P_FACILITY_SLOT.START_TIME.Value.Hour == date.Hour && m.P_FACILITY_SLOT.START_TIME.Value.Minute == date.Minute);
                }
                if (qFilter.FACILITY_END_TIME != null)
                {
                    DateTime date = qFilter.FACILITY_END_TIME.Value;
                    ett = ett.Where(m => m.P_FACILITY_SLOT.END_TIME != null);
                    ett = ett.Where(m => m.P_FACILITY_SLOT.END_TIME.Value.Hour == date.Hour && m.P_FACILITY_SLOT.END_TIME.Value.Minute == date.Minute);
                }*/

                TotalCount = ett.Count();

                //ordering && paging
                if (orderDirection == "asc")
                {
                    if (orderBy == "StartTime")
                        ett = ett.OrderBy(m => m.P_FACILITY_SLOT.START_TIME);
                    else if (orderBy == "EndTime")
                        ett = ett.OrderBy(m => m.P_FACILITY_SLOT.END_TIME);
                    else if (orderBy == "BookDate")
                        ett = ett.OrderBy(m => m.BOOK_DATE);
                    else if (orderBy == "CreatedDate")
                        ett = ett.OrderBy(m => m.CREATED);
                    else
                        ett = ett.OrderBy(m => m.ROW_ID);
                }
                else
                {
                    if (orderBy == "StartTime")
                        ett = ett.OrderByDescending(m => m.P_FACILITY_SLOT.START_TIME);
                    else if (orderBy == "EndTime")
                        ett = ett.OrderByDescending(m => m.P_FACILITY_SLOT.END_TIME);
                    else if (orderBy == "BookDate")
                        ett = ett.OrderByDescending(m => m.BOOK_DATE);
                    else if (orderBy == "CreatedDate")
                        ett = ett.OrderByDescending(m => m.CREATED);
                    else
                        ett = ett.OrderByDescending(m => m.ROW_ID);
                }

                ett = ett.Skip(startIdx).Take(length);

                //mapping
                foreach (var v in ett)
                {
                    var FacilitySlotBookingVO = Mapper.Map<P_FACILITY_SLOT_BOOKING, FacilitySlotBookingVO>(v);

                    FacilitySlotBookingVO.VO_FACILITY_START_TIME = v.P_FACILITY_SLOT.START_TIME;
                    FacilitySlotBookingVO.VO_FACILITY_END_TIME = v.P_FACILITY_SLOT.END_TIME;
                    FacilitySlotBookingVO.VO_USER_FIRST_NAME = v.P_USER.FIRST_NAME;

                    result.Add(FacilitySlotBookingVO);
                }
            }

            return result;
        }

        #endregion

        #region Create New FacilitySlotBooking
        public long CreateFacilitySlotBooking(FacilitySlotBookingVO _newFacilitySlotBooking)
        {
            using (var db = new PropComDbContext())
            {
                P_FACILITY_SLOT_BOOKING P_FACILITY_SLOT_BOOKING = Mapper.Map<FacilitySlotBookingVO, P_FACILITY_SLOT_BOOKING>(_newFacilitySlotBooking);
                P_FACILITY_SLOT_BOOKING.CREATED = DateTime.UtcNow;
                db.P_FACILITY_SLOT_BOOKING.Add(P_FACILITY_SLOT_BOOKING);
                db.SaveChanges();

                return P_FACILITY_SLOT_BOOKING.ROW_ID;
            }
        }
        #endregion

        #region Update FacilitySlotBooking
        public void UpdateFacilitySlotBooking(FacilitySlotBookingVO _updatedFacilitySlotBooking)
        {
            using (var DBContext = new PropComDbContext())
            {
                var P_FACILITY_SLOT_BOOKING = Mapper.Map<FacilitySlotBookingVO, P_FACILITY_SLOT_BOOKING>(_updatedFacilitySlotBooking);
                P_FACILITY_SLOT_BOOKING.LAST_UPDATED = DateTime.UtcNow;

                DBContext.P_FACILITY_SLOT_BOOKING.Attach(P_FACILITY_SLOT_BOOKING);
                DBContext.Entry(P_FACILITY_SLOT_BOOKING).State = EntityState.Modified;
                DBContext.SaveChanges();
            }
        }
        #endregion



    }
}
