namespace ResComm.Web.Lib.DB.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class P_BILLING_PAYMENT
    {
        [Key]
        public long ROW_ID { get; set; }

        public DateTime? CREATED { get; set; }

        public long? CREATED_BY { get; set; }

        public DateTime? LAST_UPDATED { get; set; }

        public long? LAST_UPDATED_BY { get; set; }

        [StringLength(100)]
        public string PAYMENT_METHOD { get; set; }

        public DateTime? PAYMENT_DATE { get; set; }

        [StringLength(255)]
        public string REFERENCE { get; set; }

        public decimal? AMOUNT { get; set; }

        [StringLength(50)]
        public string STATUS { get; set; }

        public long? BILLING_ID { get; set; }

        public long? PAID_BY { get; set; }

        public virtual P_BILLING P_BILLING { get; set; }

        public virtual P_USER P_USER { get; set; }
    }
}
