namespace ResComm.Web.Lib.DB.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class P_ORDER
    {
        [Key]
        public long ROW_ID { get; set; }

        public long? CREATED_BY { get; set; }

        public DateTime? CREATED { get; set; }

        public long? LAST_UPD_BY { get; set; }

        public DateTime? LAST_UPD { get; set; }

        [Required]
        [StringLength(100)]
        public string ORDER_NUM { get; set; }

        [StringLength(30)]
        public string ORDER_TYPE { get; set; }

        [StringLength(30)]
        public string ORDER_STATUS { get; set; }

        public DateTime? ORDER_SUBMIT_DT { get; set; }

        public DateTime? PREF_INSTALL_DT { get; set; }

        [StringLength(100)]
        public string ASSIGNEE { get; set; }

        [StringLength(30)]
        public string ORDER_SOURCE { get; set; }

        [StringLength(30)]
        public string CAMPAIGN_CD { get; set; }

        [StringLength(30)]
        public string ORDER_SUBMITTED_BY { get; set; }

        [StringLength(100)]
        public string CUST_REP_ID { get; set; }

        public long? VERIFICATION_ID { get; set; }

        [StringLength(100)]
        public string TERM_STATUS_CD { get; set; }
    }
}
