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
    public class FacilitySlotDALImpl : IFacilitySlotDAL
    {
        IMapper Mapper;
        public FacilitySlotDALImpl()
        {
            var AutoMapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<P_FACILITY_SLOT, FacilitySlotVO>();
                cfg.CreateMap<FacilitySlotVO, P_FACILITY_SLOT>();
            });

            Mapper = AutoMapperConfig.CreateMapper();
        }

        #region MapReturnList
        List<FacilitySlotVO> MapReturnList(List<P_FACILITY_SLOT> aList)
        {
            // map
            var returnedResult = new List<FacilitySlotVO>();

            foreach (var s in aList)
            {
                var a = Mapper.Map<P_FACILITY_SLOT, FacilitySlotVO>(s);

                returnedResult.Add(a);
            }

            return returnedResult;
        }

        #endregion

        #region Get FacilitySlot
        public FacilitySlotVO GetFacilitySlot(long FacilitySlotId)
        {
            using (var DBContext = new PropComDbContext())
            {
                var result = from d in DBContext.P_FACILITY_SLOT
                             where d.ROW_ID == FacilitySlotId
                             select d;

                if (result.Count() > 0)
                {
                    var P_FACILITY_SLOT = result.First();
                    var FacilitySlotVO = Mapper.Map<P_FACILITY_SLOT, FacilitySlotVO>(P_FACILITY_SLOT);

                    return FacilitySlotVO;
                }
                else
                {
                    return null;
                }
            }
        }
        public List<FacilitySlotVO> GetFacilitySlotsByFacilityId(long FacilityId)
        {
            List<FacilitySlotVO> result = new List<FacilitySlotVO>();
            using (var DBContext = new PropComDbContext())
            {
                var ett = from d in DBContext.P_FACILITY_SLOT
                          where d.FACILITY_ID == FacilityId
                          && d.STATUS == 1
                          orderby d.START_TIME ascending
                          select d;

                foreach (var v in ett)
                {
                    result.Add(Mapper.Map<P_FACILITY_SLOT, FacilitySlotVO>(v));
                }
            }
            return result;
        }

        public List<FacilitySlotVO> GetFacilitySlots(int startIdx, int length, ref int TotalCount, string orderBy = "", string orderDirection = "", FacilitySlotVO qFilter = null)
        {
            List<FacilitySlotVO> result = new List<FacilitySlotVO>();
            using (var DBContext = new PropComDbContext())
            {
                var ett = from d in DBContext.P_FACILITY_SLOT
                          where d.STATUS == 1
                          select d;

                //filtering
                if (qFilter.START_TIME != null)
                {
                    DateTime date = qFilter.START_TIME.Value;
                    ett = ett.Where(m => m.START_TIME != null);
                    ett = ett.Where(m => m.START_TIME.Value.Hour == date.Hour && m.START_TIME.Value.Minute == date.Minute);
                }
                if (qFilter.END_TIME != null)
                {
                    DateTime date = qFilter.END_TIME.Value;
                    ett = ett.Where(m => m.END_TIME != null);
                    ett = ett.Where(m => m.END_TIME.Value.Hour == date.Hour && m.END_TIME.Value.Minute == date.Minute);
                }
                if (qFilter.CREATED != null)
                {
                    DateTime date = qFilter.CREATED.Value;
                    ett = ett.Where(m => m.CREATED != null);
                    ett = ett.Where(m => m.CREATED.Value.Year == date.Year && m.CREATED.Value.Month == date.Month && m.CREATED.Value.Day == date.Day);
                }

                TotalCount = ett.Count();

                //ordering && paging
                if (orderDirection == "asc")
                {
                    if (orderBy == "StartTime")
                        ett = ett.OrderBy(m => m.START_TIME);
                    else if (orderBy == "EndTime")
                        ett = ett.OrderBy(m => m.END_TIME);
                    else if (orderBy == "CreatedDate")
                        ett = ett.OrderBy(m => m.CREATED);
                    else if (orderBy == "Slot")
                        ett = ett.OrderBy(m => m.SLOT);
                    else
                        ett = ett.OrderBy(m => m.ROW_ID);
                }
                else
                {
                    if (orderBy == "StartTime")
                        ett = ett.OrderByDescending(m => m.START_TIME);
                    else if (orderBy == "EndTime")
                        ett = ett.OrderByDescending(m => m.END_TIME);
                    else if (orderBy == "CreatedDate")
                        ett = ett.OrderByDescending(m => m.CREATED);
                    else if (orderBy == "Slot")
                        ett = ett.OrderByDescending(m => m.SLOT);
                    else
                        ett = ett.OrderByDescending(m => m.ROW_ID);
                }

                ett = ett.Skip(startIdx).Take(length);

                //mapping
                foreach (var v in ett)
                {
                    result.Add(Mapper.Map<P_FACILITY_SLOT, FacilitySlotVO>(v));
                }
            }

            return result;
        }

        #endregion

        #region Create New FacilitySlot
        public long CreateFacilitySlot(FacilitySlotVO _newFacilitySlot)
        {
            using (var db = new PropComDbContext())
            {
                P_FACILITY_SLOT P_FACILITY_SLOT = Mapper.Map<FacilitySlotVO, P_FACILITY_SLOT>(_newFacilitySlot);
                P_FACILITY_SLOT.CREATED = DateTime.UtcNow;
                db.P_FACILITY_SLOT.Add(P_FACILITY_SLOT);
                db.SaveChanges();

                return P_FACILITY_SLOT.ROW_ID;
            }
        }
        #endregion

        #region Update FacilitySlot
        public void UpdateFacilitySlot(FacilitySlotVO _updatedFacilitySlot)
        {
            using (var DBContext = new PropComDbContext())
            {
                var P_FACILITY_SLOT = Mapper.Map<FacilitySlotVO, P_FACILITY_SLOT>(_updatedFacilitySlot);
                P_FACILITY_SLOT.LAST_UPDATED = DateTime.UtcNow;

                DBContext.P_FACILITY_SLOT.Attach(P_FACILITY_SLOT);
                DBContext.Entry(P_FACILITY_SLOT).State = EntityState.Modified;
                DBContext.SaveChanges();
            }
        }
        #endregion



    }
}
