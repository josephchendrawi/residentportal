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
using ResComm.Web.Lib.Interface.Models.Property;

namespace ResComm.Web.Lib.DB.DAL
{
    public class PropertyDALImpl : IPropertyDAL
    {
        IMapper Mapper;
        public PropertyDALImpl()
        {
            var AutoMapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<P_PROPERTY, PropertyVO>();
                cfg.CreateMap<PropertyVO, P_PROPERTY>();
            });

            Mapper = AutoMapperConfig.CreateMapper();
        }

        #region MapReturnList
        List<PropertyVO> MapReturnList(List<P_PROPERTY> aList)
        {
            // map
            var returnedResult = new List<PropertyVO>();

            foreach (var s in aList)
            {
                var a = Mapper.Map<P_PROPERTY, PropertyVO>(s);

                returnedResult.Add(a);
            }

            return returnedResult;
        }

        #endregion

        #region Get Property
        public PropertyVO GetProperty(long propertyId)
        {
            using (var DBContext = new PropComDbContext())
            {
                var result = from a in DBContext.P_PROPERTY
                             where a.ROW_ID == propertyId
                             select a;

                if (result.Count() > 0)
                {
                    var P_PROPERTY = result.First();
                    var PropertyVO = Mapper.Map<P_PROPERTY, PropertyVO>(P_PROPERTY);
                    
                    return PropertyVO;
                }
                else
                {
                    return null;
                }
            }
        }
        public PropertyVO GetProperty(string propertyName)
        {
            using (var DBContext = new PropComDbContext())
            {
                var result = from a in DBContext.P_PROPERTY
                             where a.NAME.ToLower() == propertyName.ToLower()
                             select a;

                if (result.Count() > 0)
                {
                    var P_PROPERTY = result.First();
                    var PropertyVO = Mapper.Map<P_PROPERTY, PropertyVO>(P_PROPERTY);

                    return PropertyVO;
                }
                else
                {
                    return null;
                }
            }
        }
        public PropertyVO GetPropertyByAccountId(long AccountId)
        {
            using (var DBContext = new PropComDbContext())
            {
                var result = from a in DBContext.P_PROPERTY
                             where a.ACCNT_ID == AccountId
                             select a;

                if (result.Count() > 0)
                {
                    var P_PROPERTY = result.First();
                    var PropertyVO = Mapper.Map<P_PROPERTY, PropertyVO>(P_PROPERTY);

                    return PropertyVO;
                }
                else
                {
                    return null;
                }
            }
        }

        public List<PropertyVO> GetProperties(int startIdx, int length, ref int TotalCount, string orderBy = "", string orderDirection = "", PropertyVO qFilter = null, long? AffiliateId = null)
        {
            List<PropertyVO> result = new List<PropertyVO>();
            using (var DBContext = new PropComDbContext())
            {
                var ett = from d in DBContext.P_PROPERTY
                          join e in DBContext.P_ACCNT on d.ACCNT_ID equals e.ROW_ID
                          join f in DBContext.P_USER on e.CUST_USER_ID equals f.ROW_ID
                          where f.ACTIVE_FLG == "1"
                          select d;

                //filtering
                if (qFilter.NAME != null && qFilter.NAME != "")
                    ett = ett.Where(m => m.NAME.ToLower().Contains(qFilter.NAME.ToLower()));
                if (qFilter.CREATED != null)
                {
                    DateTime date = qFilter.CREATED.Value;
                    ett = ett.Where(m => m.CREATED != null);
                    ett = ett.Where(m => m.CREATED.Value.Year == date.Year && m.CREATED.Value.Month == date.Month && m.CREATED.Value.Day == date.Day);
                }
                if (AffiliateId != null)
                {
                    ett = ett.Where(m => m.P_ACCNT.REFERRED_BY == AffiliateId);
                }

                TotalCount = ett.Count();

                //ordering && paging
                if (orderDirection == "asc")
                {
                    if (orderBy == "Name")
                        ett = ett.OrderBy(m => m.NAME);
                    else if (orderBy == "CreatedDate")
                        ett = ett.OrderBy(m => m.CREATED);
                    //
                    else
                        ett = ett.OrderBy(m => m.ROW_ID);
                }
                else
                {
                    if (orderBy == "Name")
                        ett = ett.OrderByDescending(m => m.NAME);
                    else if (orderBy == "CreatedDate")
                        ett = ett.OrderByDescending(m => m.CREATED);
                    //
                    else
                        ett = ett.OrderByDescending(m => m.ROW_ID);
                }

                ett = ett.Skip(startIdx).Take(length);

                //mapping
                foreach (var v in ett)
                {
                    result.Add(Mapper.Map<P_PROPERTY, PropertyVO>(v));
                }
            }

            return result;
        }
        #endregion

        #region Create new Property
        public long CreateProperty(PropertyVO _newProperty)
        {
            using (var db = new PropComDbContext())
            {
                P_PROPERTY P_Property = Mapper.Map<PropertyVO, P_PROPERTY>(_newProperty);
                P_Property.CREATED = DateTime.UtcNow;
                db.P_PROPERTY.Add(P_Property);
                db.SaveChanges();

                return P_Property.ROW_ID;
            }
        }
        #endregion

        #region Update Property
        public void UpdateProperty(PropertyVO _updatedProperty)
        {
            using (var DBContext = new PropComDbContext())
            {
                var P_Property = Mapper.Map<PropertyVO, P_PROPERTY>(_updatedProperty);
                P_Property.LAST_UPD = DateTime.UtcNow;

                DBContext.P_PROPERTY.Attach(P_Property);
                DBContext.Entry(P_Property).State = EntityState.Modified;
                DBContext.SaveChanges();
            }
        }
        #endregion



    }
}
