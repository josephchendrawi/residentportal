namespace ResComm.Web.Lib.DB.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class P_PROD_ITEM
    {
        [Key]
        public long ROW_ID { get; set; }

        public long? CREATED_BY { get; set; }

        public DateTime? CREATED { get; set; }

        public long? LAST_UPD_BY { get; set; }

        public DateTime? LAST_UPD { get; set; }

        public long PROD_ITEM_ID { get; set; }

        public long PROD_ID { get; set; }

        public long? PAR_ITEM_ID { get; set; }

        public long? ROOT_ITEM_ID { get; set; }

        [StringLength(1)]
        public string ACTIVE_FLG { get; set; }

        [StringLength(1)]
        public string REQ_FLG { get; set; }

        [StringLength(50)]
        public string ITEM_ATR1 { get; set; }

        [StringLength(50)]
        public string ITEM_ATR2 { get; set; }
    }
}
