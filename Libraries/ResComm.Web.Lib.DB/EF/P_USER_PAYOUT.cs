namespace ResComm.Web.Lib.DB.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class P_USER_PAYOUT
    {
        [Key]
        public long ROW_ID { get; set; }

        public DateTime? CREATED { get; set; }

        public long? CREATED_BY { get; set; }

        public DateTime? LAST_UPDATED { get; set; }

        public long? LAST_UPDATED_BY { get; set; }

        public long? USER_ID { get; set; }

        [StringLength(500)]
        public string DESCRIPTION { get; set; }

        public decimal? PAYOUT_AMOUNT { get; set; }

        [StringLength(50)]
        public string STATUS { get; set; }
    }
}
