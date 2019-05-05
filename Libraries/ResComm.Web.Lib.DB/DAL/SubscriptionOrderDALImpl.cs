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
using ResComm.Web.Lib.Interface.Models.SubscriptionOrder;

namespace ResComm.Web.Lib.DB.DAL
{
    public class SubscriptionOrderDALImpl
    {
        IMapper Mapper;
        public SubscriptionOrderDALImpl()
        {
            var AutoMapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<P_PROPERTY_SUBSCRIPTION_ORDER, SubscriptionOrderVO>();
                cfg.CreateMap<SubscriptionOrderVO, P_PROPERTY_SUBSCRIPTION_ORDER>();
            });

            Mapper = AutoMapperConfig.CreateMapper();
        }

        #region MapReturnList
        List<SubscriptionOrderVO> MapReturnList(List<P_PROPERTY_SUBSCRIPTION_ORDER> aList)
        {
            // map
            var returnedResult = new List<SubscriptionOrderVO>();

            foreach (var s in aList)
            {
                var a = Mapper.Map<P_PROPERTY_SUBSCRIPTION_ORDER, SubscriptionOrderVO>(s);

                returnedResult.Add(a);
            }

            return returnedResult;
        }

        #endregion

        #region Get SubscriptionOrder
        public SubscriptionOrderVO GetSubscriptionOrder(long SubscriptionOrderId)
        {
            using (var DBContext = new PropComDbContext())
            {
                var result = from d in DBContext.P_PROPERTY_SUBSCRIPTION_ORDER
                             where d.ROW_ID == SubscriptionOrderId
                             select d;

                if (result.Count() > 0)
                {
                    var P_PROPERTY_SUBSCRIPTION_ORDER = result.First();
                    var SubscriptionOrderVO = Mapper.Map<P_PROPERTY_SUBSCRIPTION_ORDER, SubscriptionOrderVO>(P_PROPERTY_SUBSCRIPTION_ORDER);
                    SubscriptionOrderVO.PropertyName = P_PROPERTY_SUBSCRIPTION_ORDER.P_PROPERTY.NAME;

                    return SubscriptionOrderVO;
                }
                else
                {
                    return null;
                }
            }
        }

        public List<SubscriptionOrderVO> GetSubscriptionOrders(int startIdx, int length, ref int TotalCount, string orderBy = "", string orderDirection = "", SubscriptionOrderVO qFilter = null)
        {
            List<SubscriptionOrderVO> result = new List<SubscriptionOrderVO>();
            using (var DBContext = new PropComDbContext())
            {
                var ett = from d in DBContext.P_PROPERTY_SUBSCRIPTION_ORDER
                          select d;

                //filtering
                if (qFilter.AFFILIATE_ID != null && qFilter.AFFILIATE_ID != 0)
                {
                    ett = ett.Where(m => m.AFFILIATE_ID == qFilter.AFFILIATE_ID);
                }
                if (qFilter.PROPERTY_ID != null && qFilter.PROPERTY_ID != 0)
                {
                    ett = ett.Where(m => m.PROPERTY_ID == qFilter.PROPERTY_ID);
                }
                if (qFilter.STATUS != null && qFilter.STATUS != 0)
                {
                    ett = ett.Where(m => m.STATUS == qFilter.STATUS);
                }
                if (qFilter.ORDER_NUM != null && qFilter.ORDER_NUM != "")
                    ett = ett.Where(m => m.ORDER_NUM.ToLower().Contains(qFilter.ORDER_NUM.ToLower()));
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
                    if (orderBy == "OrderNum")
                        ett = ett.OrderBy(m => m.ORDER_NUM);
                    else if (orderBy == "CreatedDate")
                        ett = ett.OrderBy(m => m.CREATED);
                    else if (orderBy == "Property")
                        ett = ett.OrderBy(m => m.P_PROPERTY.NAME);
                    else if (orderBy == "SubscriptionPackage")
                        ett = ett.OrderBy(m => m.P_SUBSCRIPTION_PACKAGE.NAME);
                    else if (orderBy == "PackagePrice")
                        ett = ett.OrderBy(m => m.PRICE);
                    else if (orderBy == "CommissionPercentage")
                        ett = ett.OrderBy(m => m.COMMISSION);
                    else if (orderBy == "CommissionAmount")
                        ett = ett.OrderBy(m => (m.PRICE * m.COMMISSION));
                    else
                        ett = ett.OrderBy(m => m.ROW_ID);
                }
                else
                {
                    if (orderBy == "OrderNum")
                        ett = ett.OrderByDescending(m => m.ORDER_NUM);
                    else if (orderBy == "CreatedDate")
                        ett = ett.OrderByDescending(m => m.CREATED);
                    else if (orderBy == "Property")
                        ett = ett.OrderByDescending(m => m.P_PROPERTY.NAME);
                    else if (orderBy == "SubscriptionPackage")
                        ett = ett.OrderByDescending(m => m.P_SUBSCRIPTION_PACKAGE.NAME);
                    else if (orderBy == "PackagePrice")
                        ett = ett.OrderByDescending(m => m.PRICE);
                    else if (orderBy == "CommissionPercentage")
                        ett = ett.OrderByDescending(m => m.COMMISSION);
                    else if (orderBy == "CommissionAmount")
                        ett = ett.OrderByDescending(m => (m.PRICE * m.COMMISSION));
                    else
                        ett = ett.OrderByDescending(m => m.ROW_ID);
                }

                ett = ett.Skip(startIdx).Take(length);

                //mapping
                foreach (var v in ett)
                {
                    var order = Mapper.Map<P_PROPERTY_SUBSCRIPTION_ORDER, SubscriptionOrderVO>(v);
                    order.PropertyName = v.P_PROPERTY.NAME;
                    order.SubscriptionPackageName = v.P_SUBSCRIPTION_PACKAGE.NAME;

                    result.Add(order);
                }
            }

            return result;
        }

        #endregion

        #region Create New SubscriptionOrder
        public long CreateSubscriptionOrder(SubscriptionOrderVO _newSubscriptionOrder)
        {
            using (var db = new PropComDbContext())
            {
                P_PROPERTY_SUBSCRIPTION_ORDER P_PROPERTY_SUBSCRIPTION_ORDER = Mapper.Map<SubscriptionOrderVO, P_PROPERTY_SUBSCRIPTION_ORDER>(_newSubscriptionOrder);
                P_PROPERTY_SUBSCRIPTION_ORDER.CREATED = DateTime.UtcNow;
                db.P_PROPERTY_SUBSCRIPTION_ORDER.Add(P_PROPERTY_SUBSCRIPTION_ORDER);
                db.SaveChanges();

                return P_PROPERTY_SUBSCRIPTION_ORDER.ROW_ID;
            }
        }
        #endregion

        #region Update SubscriptionOrder
        public void UpdateSubscriptionOrder(SubscriptionOrderVO _updatedSubscriptionOrder)
        {
            using (var DBContext = new PropComDbContext())
            {
                var P_PROPERTY_SUBSCRIPTION_ORDER = Mapper.Map<SubscriptionOrderVO, P_PROPERTY_SUBSCRIPTION_ORDER>(_updatedSubscriptionOrder);
                P_PROPERTY_SUBSCRIPTION_ORDER.LAST_UPDATED = DateTime.UtcNow;

                DBContext.P_PROPERTY_SUBSCRIPTION_ORDER.Attach(P_PROPERTY_SUBSCRIPTION_ORDER);
                DBContext.Entry(P_PROPERTY_SUBSCRIPTION_ORDER).State = EntityState.Modified;
                DBContext.SaveChanges();
            }
        }
        #endregion



    }
}
