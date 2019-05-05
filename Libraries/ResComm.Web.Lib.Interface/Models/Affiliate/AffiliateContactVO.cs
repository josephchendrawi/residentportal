using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResComm.Web.Lib.Interface.Models.Affiliate
{
    public class AffiliateContactVO : BaseVO
    {
        public long ROW_ID { get; set; }
        public Nullable<System.DateTime> CREATED { get; set; }
        public Nullable<long> CREATED_BY { get; set; }
        public Nullable<System.DateTime> LAST_UPDATED { get; set; }
        public Nullable<long> LAST_UPDATED_BY { get; set; }
        public Nullable<long> AFFILIATE_ID { get; set; }
        public string NAME { get; set; }
        public string PHONE_NO { get; set; }
        public string MOBILE_NO { get; set; }
        public string EMAIL { get; set; }
        public string REMARKS { get; set; }
        public Nullable<int> RATING { get; set; }
        public Nullable<int> STATUS { get; set; }
    }
}
