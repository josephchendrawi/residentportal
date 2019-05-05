namespace ResComm.Web.Lib.DB.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class P_SRV_ACT
    {
        [Key]
        public long ROW_ID { get; set; }

        public long? CREATED_BY { get; set; }

        public DateTime? CREATED { get; set; }

        public long? LAST_UPD_BY { get; set; }

        public DateTime? LAST_UPD { get; set; }

        public long? SRV_ID { get; set; }

        public DateTime? VISIT_DT { get; set; }

        [StringLength(100)]
        public string FIELD_STAFF { get; set; }

        [StringLength(30)]
        public string STATUS_CD { get; set; }

        public string NOTES { get; set; }
    }
}
