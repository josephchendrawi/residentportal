using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResComm.Web.Lib.Interface.Models.PropertyInvoice
{
    public class PropertyInvoiceVO : BaseVO
    {
        public long ROW_ID { get; set; }

        public DateTime? CREATED { get; set; }

        public long? CREATED_BY { get; set; }

        public DateTime? LAST_UPDATED { get; set; }

        public long? LAST_UPDATED_BY { get; set; }

        public string INVOICE_NUM { get; set; }

        public DateTime? INVOICE_DATE { get; set; }

        public long? PROPERTY_ID { get; set; }

        public string DESCRIPTION { get; set; }

        public decimal? AMOUNT { get; set; }

        public int? STATUS { get; set; }

        public int? PAYMENT_STATUS { get; set; }

        public DateTime? PAYMENT_DATE { get; set; }

        public int? PAYMENT_METHOD { get; set; }

        public string CHEQUE_NO { get; set; }

        public string TRANSACTION_NO { get; set; }

        public string APPROVAL_CODE { get; set; }

        public string BANK { get; set; }

        public string PropertyName { get; set; }
    }
}
