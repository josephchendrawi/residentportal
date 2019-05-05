namespace ResComm.Web.Lib.DB.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class P_GROUP
    {
        [Key]
        public long ROW_ID { get; set; }

        public long? CREATED_BY { get; set; }

        public DateTime? CREATED { get; set; }

        public long? LAST_UPD_BY { get; set; }

        public DateTime? LAST_UPD { get; set; }

        [StringLength(50)]
        public string GROUP_CD { get; set; }

        [StringLength(50)]
        public string GROUP_DESC { get; set; }

        [StringLength(50)]
        public string ACCESS_LVL { get; set; }

        [StringLength(1)]
        public string ACTIVE_FLG { get; set; }
    }
}
