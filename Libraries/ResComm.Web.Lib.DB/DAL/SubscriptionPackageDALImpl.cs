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
using ResComm.Web.Lib.Interface.Models.SubscriptionPackage;

namespace ResComm.Web.Lib.DB.DAL
{
    public class SubscriptionPackageDALImpl
    {
        IMapper Mapper;
        public SubscriptionPackageDALImpl()
        {
            var AutoMapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<P_SUBSCRIPTION_PACKAGE, SubscriptionPackageVO>();
                cfg.CreateMap<SubscriptionPackageVO, P_SUBSCRIPTION_PACKAGE>();
            });

            Mapper = AutoMapperConfig.CreateMapper();
        }

        #region MapReturnList
        List<SubscriptionPackageVO> MapReturnList(List<P_SUBSCRIPTION_PACKAGE> aList)
        {
            // map
            var returnedResult = new List<SubscriptionPackageVO>();

            foreach (var s in aList)
            {
                var a = Mapper.Map<P_SUBSCRIPTION_PACKAGE, SubscriptionPackageVO>(s);

                returnedResult.Add(a);
            }

            return returnedResult;
        }

        #endregion

        #region Get SubscriptionPackage
        public SubscriptionPackageVO GetSubscriptionPackage(long SubscriptionPackageId)
        {
            using (var DBContext = new PropComDbContext())
            {
                var result = from d in DBContext.P_SUBSCRIPTION_PACKAGE
                             where d.ROW_ID == SubscriptionPackageId
                             select d;

                if (result.Count() > 0)
                {
                    var P_SUBSCRIPTION_PACKAGE = result.First();
                    var SubscriptionPackageVO = Mapper.Map<P_SUBSCRIPTION_PACKAGE, SubscriptionPackageVO>(P_SUBSCRIPTION_PACKAGE);

                    return SubscriptionPackageVO;
                }
                else
                {
                    return null;
                }
            }
        }

        public List<SubscriptionPackageVO> GetFacilities(int startIdx, int length, ref int TotalCount, string orderBy = "", string orderDirection = "", SubscriptionPackageVO qFilter = null)
        {
            List<SubscriptionPackageVO> result = new List<SubscriptionPackageVO>();
            using (var DBContext = new PropComDbContext())
            {
                var ett = from d in DBContext.P_SUBSCRIPTION_PACKAGE
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
                    result.Add(Mapper.Map<P_SUBSCRIPTION_PACKAGE, SubscriptionPackageVO>(v));
                }
            }

            return result;
        }

        #endregion

        #region Create New SubscriptionPackage
        public long CreateSubscriptionPackage(SubscriptionPackageVO _newSubscriptionPackage)
        {
            using (var db = new PropComDbContext())
            {
                P_SUBSCRIPTION_PACKAGE P_SUBSCRIPTION_PACKAGE = Mapper.Map<SubscriptionPackageVO, P_SUBSCRIPTION_PACKAGE>(_newSubscriptionPackage);
                P_SUBSCRIPTION_PACKAGE.CREATED = DateTime.UtcNow;
                db.P_SUBSCRIPTION_PACKAGE.Add(P_SUBSCRIPTION_PACKAGE);
                db.SaveChanges();

                return P_SUBSCRIPTION_PACKAGE.ROW_ID;
            }
        }
        #endregion

        #region Update SubscriptionPackage
        public void UpdateSubscriptionPackage(SubscriptionPackageVO _updatedSubscriptionPackage)
        {
            using (var DBContext = new PropComDbContext())
            {
                var P_SUBSCRIPTION_PACKAGE = Mapper.Map<SubscriptionPackageVO, P_SUBSCRIPTION_PACKAGE>(_updatedSubscriptionPackage);
                P_SUBSCRIPTION_PACKAGE.LAST_UPDATED = DateTime.UtcNow;

                DBContext.P_SUBSCRIPTION_PACKAGE.Attach(P_SUBSCRIPTION_PACKAGE);
                DBContext.Entry(P_SUBSCRIPTION_PACKAGE).State = EntityState.Modified;
                DBContext.SaveChanges();
            }
        }
        #endregion



    }
}
