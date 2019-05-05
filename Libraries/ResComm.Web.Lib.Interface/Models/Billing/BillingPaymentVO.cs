using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResComm.Web.Lib.Interface.Models.Billing
{
    public class BillingPaymentVO : BaseVO
    {
        public long ROW_ID { get; set; }

        public DateTime? CREATED { get; set; }

        public long? CREATED_BY { get; set; }

        public DateTime? LAST_UPDATED { get; set; }

        public long? LAST_UPDATED_BY { get; set; }

        public string PAYMENT_METHOD { get; set; }

        public DateTime? PAYMENT_DATE { get; set; }

        public string REFERENCE { get; set; }

        public decimal? AMOUNT { get; set; }

        public string STATUS { get; set; }

        public long? BILLING_ID { get; set; }

        public long? PAID_BY { get; set; }

        public string PAIDNAME { get; set; }
    }
}
