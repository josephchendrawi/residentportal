using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResComm.Web.Lib.Interface.Models.Billing
{
    public class BillingVO : BaseVO
    {
        public long ROW_ID { get; set; }

        public DateTime? CREATED { get; set; }

        public long? CREATED_BY { get; set; }

        public DateTime? LAST_UPDATED { get; set; }

        public long? LAST_UPDATED_BY { get; set; }

        public string INVOICE_NO { get; set; }

        public DateTime? DUE_DATE { get; set; }

        public string REFERENCE { get; set; }

        public decimal? AMOUNT { get; set; }

        public string STATUS { get; set; }

        public long? UNIT_ID { get; set; }

        public DateTime? BILLING_DATE { get; set; }

        public string FILE_NAME { get; set; }
        public string TOKEN { get; set; }

        public string UnitName { get; set; }
    }
}
