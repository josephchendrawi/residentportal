namespace ResComm.Web.Lib.DB.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class P_AFFLIATE_LEADS
    {
        [Key]
        public long ROW_ID { get; set; }

        public DateTime? CREATED { get; set; }

        public long? CREATED_BY { get; set; }

        public DateTime? LAST_UPDATED { get; set; }

        public long? LAST_UPDATED_BY { get; set; }

        public long? AFFILIATE_ID { get; set; }

        [StringLength(100)]
        public string NAME { get; set; }

        [StringLength(20)]
        public string PHONE_NO { get; set; }

        [StringLength(20)]
        public string MOBILE_NO { get; set; }

        [StringLength(150)]
        public string EMAIL { get; set; }

        [StringLength(500)]
        public string REMARKS { get; set; }

        public int? RATING { get; set; }

        public int? STATUS { get; set; }

        public virtual P_USER P_USER { get; set; }
    }
}
