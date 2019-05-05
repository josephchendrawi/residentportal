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
    public class FacilityDALImpl : IFacilityDAL
    {
        IMapper Mapper;
        public FacilityDALImpl()
        {
            var AutoMapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<P_FACILITY, FacilityVO>();
                cfg.CreateMap<FacilityVO, P_FACILITY>();

                cfg.CreateMap<P_FACILITY_SLOT, FacilitySlotVO>();
                cfg.CreateMap<FacilitySlotVO, P_FACILITY_SLOT>();
            });

            Mapper = AutoMapperConfig.CreateMapper();
        }

        #region MapReturnList
        List<FacilityVO> MapReturnList(List<P_FACILITY> aList)
        {
            // map
            var returnedResult = new List<FacilityVO>();

            foreach (var s in aList)
            {
                var a = Mapper.Map<P_FACILITY, FacilityVO>(s);

                returnedResult.Add(a);
            }

            return returnedResult;
        }

        #endregion

        #region Get Facility
        public FacilityVO GetFacility(long FacilityId)
        {
            using (var DBContext = new PropComDbContext())
            {
                var result = from d in DBContext.P_FACILITY
                             where d.ROW_ID == FacilityId
                             select d;

                if (result.Count() > 0)
                {
                    var P_FACILITY = result.First();
                    var FacilityVO = Mapper.Map<P_FACILITY, FacilityVO>(P_FACILITY);

                    return FacilityVO;
                }
                else
                {
                    return null;
                }
            }
        }

        public List<FacilityVO> GetFacilities(int startIdx, int length, ref int TotalCount, string orderBy = "", string orderDirection = "", FacilityVO qFilter = null)
        {
            List<FacilityVO> result = new List<FacilityVO>();
            using (var DBContext = new PropComDbContext())
            {
                var ett = from d in DBContext.P_FACILITY
                          where d.STATUS == 1
                          select d;

                //filtering
                if (qFilter.ACCNT_ID != null && qFilter.ACCNT_ID != 0)
                    ett = ett.Where(m => m.ACCNT_ID == qFilter.ACCNT_ID);
                if (qFilter.NAME != null && qFilter.NAME != "")
                    ett = ett.Where(m => m.NAME.ToLower().Contains(qFilter.NAME.ToLower()));
                if (qFilter.CREATED != null)
                {
                    DateTime date = qFilter.CREATED.Value;
                    ett = ett.Where(m => m.CREATED != null);
                    ett = ett.Where(m => m.CREATED.Value.Year == date.Year && m.CREATED.Value.Month == date.Month && m.CREATED.Value.Day == date.Day);
                }
                if (qFilter.DESCRIPTION != null && qFilter.DESCRIPTION != "")
                    ett = ett.Where(m => m.DESCRIPTION.ToLower().Contains(qFilter.DESCRIPTION.ToLower()));

                TotalCount = ett.Count();

                //ordering && paging
                if (orderDirection == "asc")
                {
                    if (orderBy == "Name")
                        ett = ett.OrderBy(m => m.NAME);
                    else if (orderBy == "CreatedDate")
                        ett = ett.OrderBy(m => m.CREATED);
                    else if (orderBy == "Description")
                        ett = ett.OrderBy(m => m.DESCRIPTION);
                    else
                        ett = ett.OrderBy(m => m.ROW_ID);
                }
                else
                {
                    if (orderBy == "Name")
                        ett = ett.OrderByDescending(m => m.NAME);
                    else if (orderBy == "CreatedDate")
                        ett = ett.OrderByDescending(m => m.CREATED);
                    else if (orderBy == "Description")
                        ett = ett.OrderByDescending(m => m.DESCRIPTION);
                    else
                        ett = ett.OrderByDescending(m => m.ROW_ID);
                }

                ett = ett.Skip(startIdx).Take(length);

                //mapping
                foreach (var v in ett)
                {
                    result.Add(Mapper.Map<P_FACILITY, FacilityVO>(v));
                }
            }

            return result;
        }

        #endregion

        #region Create New Facility
        public long CreateFacility(FacilityVO _newFacility)
        {
            using (var db = new PropComDbContext())
            {
                P_FACILITY P_FACILITY = Mapper.Map<FacilityVO, P_FACILITY>(_newFacility);
                P_FACILITY.CREATED = DateTime.UtcNow;
                db.P_FACILITY.Add(P_FACILITY);
                db.SaveChanges();

                return P_FACILITY.ROW_ID;
            }
        }
        #endregion

        #region Update Facility
        public void UpdateFacility(FacilityVO _updatedFacility)
        {
            using (var DBContext = new PropComDbContext())
            {
                var P_FACILITY = Mapper.Map<FacilityVO, P_FACILITY>(_updatedFacility);
                P_FACILITY.LAST_UPDATED = DateTime.UtcNow;

                DBContext.P_FACILITY.Attach(P_FACILITY);
                DBContext.Entry(P_FACILITY).State = EntityState.Modified;
                DBContext.SaveChanges();
            }
        }
        #endregion



    }
}
