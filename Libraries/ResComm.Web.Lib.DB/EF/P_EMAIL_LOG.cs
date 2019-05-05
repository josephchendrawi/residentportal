namespace ResComm.Web.Lib.DB.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class P_EMAIL_LOG
    {
        [Key]
        public long ROW_ID { get; set; }

        public long? CREATED_BY { get; set; }

        public DateTime? CREATED { get; set; }

        public long? LAST_UPD_BY { get; set; }

        public DateTime? LAST_UPD { get; set; }

        [StringLength(250)]
        public string FROM_EMAIL { get; set; }

        [StringLength(250)]
        public string TP_EMAIL { get; set; }

        [StringLength(250)]
        public string SUBJECT { get; set; }

        public string BODY { get; set; }
    }
}
