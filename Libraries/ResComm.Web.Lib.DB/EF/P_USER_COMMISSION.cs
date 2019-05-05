namespace ResComm.Web.Lib.DB.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class P_USER_COMMISSION
    {
        [Key]
        public long ROW_ID { get; set; }

        public DateTime? CREATED { get; set; }

        public long? CREATED_BY { get; set; }

        public DateTime? LAST_UPDATED { get; set; }

        public long? LAST_UPDATED_BY { get; set; }

        public long? USER_ID { get; set; }

        public long? ACCNT_ID { get; set; }

        [StringLength(500)]
        public string DESCRIPTION { get; set; }

        public decimal? COMMISSION_AMOUNT { get; set; }

        public bool? PROCESSED_FLG { get; set; }
    }
}
