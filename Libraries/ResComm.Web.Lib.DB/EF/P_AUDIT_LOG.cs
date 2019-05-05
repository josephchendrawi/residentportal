namespace ResComm.Web.Lib.DB.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class P_AUDIT_LOG
    {
        [Key]
        public long ROW_ID { get; set; }

        public long? CREATED_BY { get; set; }

        public DateTime? CREATED { get; set; }

        public long? LAST_UPD_BY { get; set; }

        public DateTime? LAST_UPD { get; set; }

        [StringLength(30)]
        public string ENTITY { get; set; }

        [StringLength(30)]
        public string EVENT_TYPE { get; set; }

        public long? COMMIT_BY { get; set; }

        public long? RECORD_KEY { get; set; }

        public string LOG_DETAILS { get; set; }
    }
}
