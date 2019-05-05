namespace ResComm.Web.Lib.DB.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class P_CLIENT_MAP
    {
        [Key]
        public long ROW_ID { get; set; }

        public long? CREATED_BY { get; set; }

        public DateTime? CREATED { get; set; }

        public long? LAST_UPD_BY { get; set; }

        public DateTime? LAST_UPD { get; set; }

        [StringLength(30)]
        public string CLIENT_TYPE { get; set; }

        [StringLength(30)]
        public string CLIENT_NAME { get; set; }

        [StringLength(100)]
        public string CLIENT_DESC { get; set; }

        [StringLength(30)]
        public string PAIR_DB_TABLE { get; set; }

        public long? PAIR_DB_ROWID { get; set; }
    }
}
