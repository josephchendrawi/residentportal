namespace ResComm.Web.Lib.DB.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class P_PROPERTY_INVOICE
    {
        [Key]
        public long ROW_ID { get; set; }

        public DateTime? CREATED { get; set; }

        public long? CREATED_BY { get; set; }

        public DateTime? LAST_UPDATED { get; set; }

        public long? LAST_UPDATED_BY { get; set; }

        [StringLength(20)]
        public string INVOICE_NUM { get; set; }

        public DateTime? INVOICE_DATE { get; set; }

        public long? PROPERTY_ID { get; set; }

        public string DESCRIPTION { get; set; }

        public decimal? AMOUNT { get; set; }

        public int? STATUS { get; set; }

        public int? PAYMENT_STATUS { get; set; }

        public DateTime? PAYMENT_DATE { get; set; }

        public int? PAYMENT_METHOD { get; set; }

        [StringLength(50)]
        public string CHEQUE_NO { get; set; }

        [StringLength(50)]
        public string TRANSACTION_NO { get; set; }

        [StringLength(50)]
        public string APPROVAL_CODE { get; set; }

        [StringLength(50)]
        public string BANK { get; set; }

        public virtual P_PROPERTY P_PROPERTY { get; set; }
    }
}
