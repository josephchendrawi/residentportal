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
using ResComm.Web.Lib.Interface.Models.PropertySubscription;
using ResComm.Web.Lib.Interface.Models.SubscriptionPackage;

namespace ResComm.Web.Lib.DB.DAL
{
    public class PropertySubscriptionDALImpl
    {
        IMapper Mapper;
        public PropertySubscriptionDALImpl()
        {
            var AutoMapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<P_PROPERTY_SUBSCRIPTION, PropertySubscriptionVO>();
                cfg.CreateMap<PropertySubscriptionVO, P_PROPERTY_SUBSCRIPTION>();

                cfg.CreateMap<P_SUBSCRIPTION_PACKAGE, SubscriptionPackageVO>();
                cfg.CreateMap<SubscriptionPackageVO, P_SUBSCRIPTION_PACKAGE>();
            });

            Mapper = AutoMapperConfig.CreateMapper();
        }

        #region MapReturnList
        List<PropertySubscriptionVO> MapReturnList(List<P_PROPERTY_SUBSCRIPTION> aList)
        {
            // map
            var returnedResult = new List<PropertySubscriptionVO>();

            foreach (var s in aList)
            {
                var a = Mapper.Map<P_PROPERTY_SUBSCRIPTION, PropertySubscriptionVO>(s);

                returnedResult.Add(a);
            }

            return returnedResult;
        }

        #endregion

        #region Get PropertySubscription
        public PropertySubscriptionVO GetPropertySubscription(long PropertySubscriptionId)
        {
            using (var DBContext = new PropComDbContext())
            {
                var result = from d in DBContext.P_PROPERTY_SUBSCRIPTION
                             where d.ROW_ID == PropertySubscriptionId
                             select d;

                if (result.Count() > 0)
                {
                    var P_PROPERTY_SUBSCRIPTION = result.First();
                    var PropertySubscriptionVO = Mapper.Map<P_PROPERTY_SUBSCRIPTION, PropertySubscriptionVO>(P_PROPERTY_SUBSCRIPTION);

                    PropertySubscriptionVO.P_SUBSCRIPTION_PACKAGE = Mapper.Map<P_SUBSCRIPTION_PACKAGE, SubscriptionPackageVO>(P_PROPERTY_SUBSCRIPTION.P_SUBSCRIPTION_PACKAGE);
                    
                    return PropertySubscriptionVO;
                }
                else
                {
                    return null;
                }
            }
        }

        public PropertySubscriptionVO GetPropertySubscriptionByPropertyId(long PropertyId)
        {
            using (var DBContext = new PropComDbContext())
            {
                var result = from d in DBContext.P_PROPERTY_SUBSCRIPTION
                             where d.PROPERTY_ID == PropertyId
                             select d;

                if (result.Count() > 0)
                {
                    var P_PROPERTY_SUBSCRIPTION = result.First();
                    var PropertySubscriptionVO = Mapper.Map<P_PROPERTY_SUBSCRIPTION, PropertySubscriptionVO>(P_PROPERTY_SUBSCRIPTION);

                    PropertySubscriptionVO.P_SUBSCRIPTION_PACKAGE = Mapper.Map<P_SUBSCRIPTION_PACKAGE, SubscriptionPackageVO>(P_PROPERTY_SUBSCRIPTION.P_SUBSCRIPTION_PACKAGE);
                    
                    return PropertySubscriptionVO;
                }
                else
                {
                    return null;
                }
            }
        }

        #endregion

        #region Create New PropertySubscription
        public long CreatePropertySubscription(PropertySubscriptionVO _newPropertySubscription)
        {
            using (var db = new PropComDbContext())
            {
                P_PROPERTY_SUBSCRIPTION P_PROPERTY_SUBSCRIPTION = Mapper.Map<PropertySubscriptionVO, P_PROPERTY_SUBSCRIPTION>(_newPropertySubscription);
                P_PROPERTY_SUBSCRIPTION.CREATED = DateTime.UtcNow;
                db.P_PROPERTY_SUBSCRIPTION.Add(P_PROPERTY_SUBSCRIPTION);
                db.SaveChanges();

                return P_PROPERTY_SUBSCRIPTION.ROW_ID;
            }
        }
        #endregion

        #region Update PropertySubscription
        public void UpdatePropertySubscription(PropertySubscriptionVO _updatedPropertySubscription)
        {
            using (var DBContext = new PropComDbContext())
            {
                var P_PROPERTY_SUBSCRIPTION = Mapper.Map<PropertySubscriptionVO, P_PROPERTY_SUBSCRIPTION>(_updatedPropertySubscription);
                P_PROPERTY_SUBSCRIPTION.LAST_UPDATED = DateTime.UtcNow;

                DBContext.P_PROPERTY_SUBSCRIPTION.Attach(P_PROPERTY_SUBSCRIPTION);
                DBContext.Entry(P_PROPERTY_SUBSCRIPTION).State = EntityState.Modified;
                DBContext.SaveChanges();
            }
        }
        #endregion



    }
}
