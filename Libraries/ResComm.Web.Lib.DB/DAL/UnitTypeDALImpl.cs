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
using ResComm.Web.Lib.Interface.Models.UnitType;
using System.Data.Entity.SqlServer;

namespace ResComm.Web.Lib.DB.DAL
{
    public class UnitTypeDALImpl : IUnitTypeDAL
    {
        IMapper Mapper;
        public UnitTypeDALImpl()
        {
            var AutoMapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<P_UNIT_TYPE, UnitTypeVO>();
                cfg.CreateMap<UnitTypeVO, P_UNIT_TYPE>();
            });

            Mapper = AutoMapperConfig.CreateMapper();
        }

        #region MapReturnList
        List<UnitTypeVO> MapReturnList(List<P_UNIT_TYPE> aList)
        {
            // map
            var returnedResult = new List<UnitTypeVO>();

            foreach (var s in aList)
            {
                var a = Mapper.Map<P_UNIT_TYPE, UnitTypeVO>(s);

                returnedResult.Add(a);
            }

            return returnedResult;
        }

        #endregion

        #region Get UnitType
        public UnitTypeVO GetUnitType(long UnitTypeId)
        {
            using (var DBContext = new PropComDbContext())
            {
                var result = from a in DBContext.P_UNIT_TYPE
                             where a.ROW_ID == UnitTypeId
                             select a;

                if (result.Count() > 0)
                {
                    var P_UnitType = result.First();
                    var UnitTypeVO = Mapper.Map<P_UNIT_TYPE, UnitTypeVO>(P_UnitType);

                    return UnitTypeVO;
                }
                else
                {
                    return null;
                }
            }
        }

        public List<UnitTypeVO> GetUnitTypes(int startIdx, int length, ref int TotalCount, string orderBy = "", string orderDirection = "", UnitTypeVO qFilter = null)
        {
            List<UnitTypeVO> result = new List<UnitTypeVO>();
            using (var DBContext = new PropComDbContext())
            {
                var ett = from d in DBContext.P_UNIT_TYPE
                          where d.ACTIVE_FLG == "1"
                          select d;

                //filtering
                if (qFilter.PROPERTY_ID != null && qFilter.PROPERTY_ID != 0)
                    ett = ett.Where(m => m.PROPERTY_ID == qFilter.PROPERTY_ID);
                if (qFilter.CATEGORY_NAME != null && qFilter.CATEGORY_NAME != "")
                    ett = ett.Where(m => m.CATEGORY_NAME.ToLower().Contains(qFilter.CATEGORY_NAME.ToLower()));
                if (qFilter.CATEGORY_VALUE != null && qFilter.CATEGORY_VALUE != "")
                    ett = ett.Where(m => m.CATEGORY_VALUE.ToLower().Contains(qFilter.CATEGORY_VALUE.ToLower()));
                //if (qFilter.FLOOR != null && qFilter.FLOOR != "")
                //    ett = ett.Where(m => m.FLOOR.ToLower().Contains(qFilter.FLOOR.ToLower()));

                TotalCount = ett.Count();

                //ordering && paging
                if (orderDirection == "asc")
                {
                    if (orderBy == "CategoryName")
                        ett = ett.OrderBy(m => m.CATEGORY_NAME);
                    else if (orderBy == "CategoryValue")
                        ett = ett.OrderBy(m => m.CATEGORY_VALUE);
                    else if (orderBy == "Floor")
                        ett = ett.OrderBy(m => m.FLOOR);
                    else
                        ett = ett.OrderBy(m => m.ROW_ID);
                }
                else
                {
                    if (orderBy == "CategoryName")
                        ett = ett.OrderByDescending(m => m.CATEGORY_NAME);
                    else if (orderBy == "CategoryValue")
                        ett = ett.OrderByDescending(m => m.CATEGORY_VALUE);
                    else if (orderBy == "Floor")
                        ett = ett.OrderByDescending(m => m.FLOOR);
                    else
                        ett = ett.OrderByDescending(m => m.ROW_ID);
                }

                ett = ett.Skip(startIdx).Take(length);

                //mapping
                result = MapReturnList(ett.ToList());
            }

            return result;
        }

        #endregion

        #region Create New UnitType
        public long CreateUnitType(UnitTypeVO _newUnitType)
        {
            using (var db = new PropComDbContext())
            {
                P_UNIT_TYPE P_UnitType = Mapper.Map<UnitTypeVO, P_UNIT_TYPE>(_newUnitType);
                P_UnitType.CREATED = DateTime.UtcNow;
                db.P_UNIT_TYPE.Add(P_UnitType);
                db.SaveChanges();

                return P_UnitType.ROW_ID;
            }
        }
        #endregion

        #region Update UnitType
        public void UpdateUnitType(UnitTypeVO _updatedUnitType)
        {
            using (var DBContext = new PropComDbContext())
            {
                var P_UnitType = Mapper.Map<UnitTypeVO, P_UNIT_TYPE>(_updatedUnitType);
                P_UnitType.LAST_UPDATED = DateTime.UtcNow;

                DBContext.P_UNIT_TYPE.Attach(P_UnitType);
                DBContext.Entry(P_UnitType).State = EntityState.Modified;
                DBContext.SaveChanges();
            }
        }

        public void UpdateUnitTypeActiveFlag(long UnitTypeId, UNIT_TYPE_ACTIVE_FLAG ActiveFlag)
        {
            using (var db = new PropComDbContext())
            {
                var P_UnitTypes = from d in db.P_UNIT_TYPE
                              where d.ROW_ID == UnitTypeId
                              select d;

                if (P_UnitTypes.Count() > 0)
                {
                    var P_UnitType = P_UnitTypes.First();
                    P_UnitType.ACTIVE_FLG = ((int)ActiveFlag).ToString();

                    db.SaveChanges();
                }
            }
        }

        public void DeleteUnitType(long UnitTypeId)
        {
            using (var db = new PropComDbContext())
            {
                var P_UnitTypes = from d in db.P_UNIT_TYPE
                                  where d.ROW_ID == UnitTypeId
                                  select d;

                if (P_UnitTypes.Count() > 0)
                {

                    var P_UNITs = from d in db.P_UNIT
                                  where d.UNIT_TYPE_ID == UnitTypeId
                                  select d;                    
                    db.P_UNIT.RemoveRange(P_UNITs);
                    db.SaveChanges();

                    db.P_UNIT_TYPE.Remove(P_UnitTypes.First());
                    db.SaveChanges();
                }
            }
        }

        #endregion



    }
}
