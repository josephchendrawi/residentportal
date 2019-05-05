namespace ResComm.Web.Lib.DB.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class P_ACCNT_ACT
    {
        [Key]
        public long ROW_ID { get; set; }

        public long? CREATED_BY { get; set; }

        public DateTime? CREATED { get; set; }

        public long? LAST_UPD_BY { get; set; }

        public DateTime? LAST_UPD { get; set; }

        public long? ACCNT_ID { get; set; }

        [StringLength(100)]
        public string ACT_DESC { get; set; }

        public DateTime? INSTALL_DT { get; set; }

        [StringLength(100)]
        public string ASSIGNEE { get; set; }

        [StringLength(30)]
        public string STATUS { get; set; }

        [StringLength(100)]
        public string REASON { get; set; }
    }
}
