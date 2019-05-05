namespace ResComm.Web.Lib.DB.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class P_AUDIT_TRAIL
    {
        [Key]
        public long ROW_ID { get; set; }

        public long? CREATED_BY { get; set; }

        public DateTime? CREATED { get; set; }

        public long? LAST_UPD_BY { get; set; }

        public DateTime? LAST_UPD { get; set; }

        public long? AUDIT_ROW_ID { get; set; }

        [StringLength(30)]
        public string ACTION_CD { get; set; }

        public string OLD_VAL { get; set; }

        public string NEW_VAL { get; set; }

        [StringLength(50)]
        public string MODULE_NAME { get; set; }

        [StringLength(50)]
        public string SCREEN_NAME { get; set; }

        [StringLength(50)]
        public string FIELD_NAME { get; set; }
    }
}
