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
using ResComm.Web.Lib.Interface.Models.Unit;

namespace ResComm.Web.Lib.DB.DAL
{
    public class UnitDALImpl : IUnitDAL
    {
        IMapper Mapper;
        public UnitDALImpl()
        {
            var AutoMapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<P_UNIT, UnitVO>();
                cfg.CreateMap<UnitVO, P_UNIT>();
            });

            Mapper = AutoMapperConfig.CreateMapper();
        }

        #region MapReturnList
        List<UnitVO> MapReturnList(List<P_UNIT> aList)
        {
            // map
            var returnedResult = new List<UnitVO>();

            foreach (var s in aList)
            {
                var a = Mapper.Map<P_UNIT, UnitVO>(s);

                if (s.P_USER != null)
                    a.OWNER_NAME = s.P_USER.FIRST_NAME ?? string.Empty;
                if (s.P_USER1 != null)
                    a.TENANT_NAME = s.P_USER1.FIRST_NAME ?? string.Empty;

                returnedResult.Add(a);
            }

            return returnedResult;
        }

        #endregion

        #region Get Unit
        public UnitVO GetUnit(long UnitId, long? PropertyId = null)
        {
            using (var DBContext = new PropComDbContext())
            {
                var result = from a in DBContext.P_UNIT
                             where a.ROW_ID == UnitId
                             select a;

                if (PropertyId != null)
                {
                    result = result.Where(m => m.P_UNIT_TYPE.PROPERTY_ID == PropertyId);
                }

                if (result.Count() > 0)
                {
                    var P_Unit = result.First();
                    var UnitVO = Mapper.Map<P_UNIT, UnitVO>(P_Unit);

                    if (P_Unit.P_USER != null)
                        UnitVO.OWNER_NAME = P_Unit.P_USER.FIRST_NAME ?? string.Empty;
                    if (P_Unit.P_USER1 != null)
                        UnitVO.TENANT_NAME = P_Unit.P_USER1.FIRST_NAME ?? string.Empty;

                    return UnitVO;
                }
                else
                {
                    return null;
                }
            }
        }

        public List<UnitVO> GetUnits(int startIdx, int length, ref int TotalCount, string orderBy = "", string orderDirection = "", UnitVO qFilter = null)
        {
            List<UnitVO> result = new List<UnitVO>();
            using (var DBContext = new PropComDbContext())
            {
                var ett = from d in DBContext.P_UNIT
                          where d.ACTIVE_FLG == "1"
                          select d;

                //filtering
                if (qFilter.PROPERTY_ID != null && qFilter.PROPERTY_ID != 0)
                {
                    ett = from d in DBContext.P_UNIT
                          join e in DBContext.P_UNIT_TYPE on d.UNIT_TYPE_ID equals e.ROW_ID
                          where e.PROPERTY_ID == qFilter.PROPERTY_ID
                          && d.ACTIVE_FLG == "1"
                          && e.ACTIVE_FLG == "1"
                          select d;
                }

                if (qFilter.UNIT_TYPE_ID != null && qFilter.UNIT_TYPE_ID != 0)
                    ett = ett.Where(m => m.UNIT_TYPE_ID == qFilter.UNIT_TYPE_ID);
                if (qFilter.UNIT_NO != null && qFilter.UNIT_NO != "")
                    ett = ett.Where(m => m.UNIT_NO.ToLower().Contains(qFilter.UNIT_NO.ToLower()));

                TotalCount = ett.Count();

                //ordering && paging
                if (orderDirection == "asc")
                {
                    if (orderBy == "UnitType")
                        ett = ett.OrderBy(m => m.UNIT_TYPE_ID);
                    else if (orderBy == "UnitNo")
                        ett = ett.OrderBy(m => m.UNIT_NO);
                    else if (orderBy == "Owner")
                        ett = ett.OrderBy(m => m.P_USER.FIRST_NAME);
                    else if (orderBy == "Tenant")
                        ett = ett.OrderBy(m => m.P_USER1.FIRST_NAME);
                    else
                        ett = ett.OrderBy(m => m.ROW_ID);
                }
                else
                {
                    if (orderBy == "UnitType")
                        ett = ett.OrderBy(m => m.UNIT_TYPE_ID);
                    else if (orderBy == "UnitNo")
                        ett = ett.OrderByDescending(m => m.UNIT_NO);
                    else if (orderBy == "Owner")
                        ett = ett.OrderByDescending(m => m.P_USER.FIRST_NAME);
                    else if (orderBy == "Tenant")
                        ett = ett.OrderByDescending(m => m.P_USER1.FIRST_NAME);
                    else
                        ett = ett.OrderByDescending(m => m.ROW_ID);
                }

                ett = ett.Skip(startIdx).Take(length);

                //mapping
                result = MapReturnList(ett.ToList());
            }

            return result;
        }

        public List<long> GetUnitIdsByUserId(long UserId)
        {
            using (var DBContext = new PropComDbContext())
            {
                List<long> list = new List<long>();
                var result = from a in DBContext.P_UNIT
                             where a.TENANT_ID == UserId || a.OWNER_ID == UserId
                             select a;

                foreach(var v in result.Distinct())
                {
                    list.Add(v.ROW_ID);
                }

                return list;
            }
        }

        #endregion

        #region Create New Unit
        public long CreateUnit(UnitVO _newUnit)
        {
            using (var db = new PropComDbContext())
            {
                P_UNIT P_Unit = Mapper.Map<UnitVO, P_UNIT>(_newUnit);
                P_Unit.CREATED = DateTime.UtcNow;
                db.P_UNIT.Add(P_Unit);
                db.SaveChanges();

                return P_Unit.ROW_ID;
            }
        }
        #endregion

        #region Update Unit
        public void UpdateUnit(UnitVO _updatedUnit)
        {
            using (var DBContext = new PropComDbContext())
            {
                var P_Unit = Mapper.Map<UnitVO, P_UNIT>(_updatedUnit);
                P_Unit.LAST_UPDATED = DateTime.UtcNow;

                DBContext.P_UNIT.Attach(P_Unit);
                DBContext.Entry(P_Unit).State = EntityState.Modified;
                DBContext.SaveChanges();
            }
        }

        public void ClearTenantFromUnit(long UserId)
        {
            using (var DBContext = new PropComDbContext())
            {
                var P_UNIT = from d in DBContext.P_UNIT
                             where d.TENANT_ID == UserId
                             select d;

                foreach (var v in P_UNIT)
                {
                    v.TENANT_ID = null;
                }
                DBContext.SaveChanges();
            }
        }

        public void ClearOwnerFromUnit(long UserId)
        {
            using (var DBContext = new PropComDbContext())
            {
                var P_UNIT = from d in DBContext.P_UNIT
                             where d.OWNER_ID == UserId
                             select d;

                foreach(var v in P_UNIT)
                {
                    v.OWNER_ID = null;
                }
                DBContext.SaveChanges();
            }
        }

        public void UpdateUnitActiveFlag(long UnitId, UNIT_ACTIVE_FLAG ActiveFlag)
        {
            using (var db = new PropComDbContext())
            {
                var P_Units = from d in db.P_UNIT
                              where d.ROW_ID == UnitId
                              select d;

                if (P_Units.Count() > 0)
                {
                    var P_Unit = P_Units.First();
                    P_Unit.ACTIVE_FLG = ((int)ActiveFlag).ToString();

                    db.SaveChanges();
                }
            }
        }

        public void DeleteUnit(long UnitId)
        {
            using (var db = new PropComDbContext())
            {
                var P_Units = from d in db.P_UNIT.Include(m => m.P_BILLING)
                              where d.ROW_ID == UnitId
                              select d;

                if (P_Units.Count() > 0)
                {
                    db.P_UNIT.Remove(P_Units.First());
                    db.SaveChanges();
                }
            }
        }

        #endregion



    }
}
