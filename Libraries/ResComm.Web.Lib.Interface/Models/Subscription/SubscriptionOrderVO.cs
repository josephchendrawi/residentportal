using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResComm.Web.Lib.Interface.Models.SubscriptionOrder
{
    public class SubscriptionOrderVO : BaseVO
    {
        public long ROW_ID { get; set; }

        public DateTime? CREATED { get; set; }

        public long? CREATED_BY { get; set; }

        public DateTime? LAST_UPDATED { get; set; }

        public long? LAST_UPDATED_BY { get; set; }

        public string ORDER_NUM { get; set; }

        public long? ORDER_SUBMITTED_BY { get; set; }

        public long? PROPERTY_ID { get; set; }

        public long? SUBSCRIPTION_PACKAGE_ID { get; set; }

        public int? STATUS { get; set; }

        public decimal? PRICE { get; set; }

        public long? AFFILIATE_ID { get; set; }

        public int? COMMISSION { get; set; }

        public string PropertyName { get; set; }
        public string SubscriptionPackageName { get; set; }
    }
}
