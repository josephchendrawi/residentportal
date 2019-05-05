namespace ResComm.Web.Lib.DB.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class P_SRV_KEYVAL
    {
        [Key]
        public long ROW_ID { get; set; }

        public long? CREATED_BY { get; set; }

        public DateTime? CREATED { get; set; }

        public long? LAST_UPD_BY { get; set; }

        public DateTime? LAST_UPD { get; set; }

        public long? SRV_ID { get; set; }

        [StringLength(80)]
        public string KV_TYPE { get; set; }

        [StringLength(100)]
        public string KV_KEY { get; set; }

        public string VALUE { get; set; }
    }
}
