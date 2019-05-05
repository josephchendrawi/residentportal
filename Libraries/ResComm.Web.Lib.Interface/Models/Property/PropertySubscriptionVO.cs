using ResComm.Web.Lib.Interface.Models.SubscriptionPackage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResComm.Web.Lib.Interface.Models.PropertySubscription
{
    public class PropertySubscriptionVO : BaseVO
    {
        public long ROW_ID { get; set; }

        public DateTime? CREATED { get; set; }

        public long? CREATED_BY { get; set; }

        public DateTime? LAST_UPDATED { get; set; }

        public long? LAST_UPDATED_BY { get; set; }

        public long? PROPERTY_ID { get; set; }

        public long? SUBSCRIPTION_PACKAGE_ID { get; set; }

        public int? STATUS { get; set; }

        public DateTime? START_DATE { get; set; }

        public DateTime? END_DATE { get; set; }

        public long? SUBSCRIPTION_ORDER_ID { get; set; }

        public SubscriptionPackageVO P_SUBSCRIPTION_PACKAGE { get; set; }
    }
}
