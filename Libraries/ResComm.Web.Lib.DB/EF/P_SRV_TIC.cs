namespace ResComm.Web.Lib.DB.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class P_SRV_TIC
    {
        [Key]
        public long ROW_ID { get; set; }

        public long? CREATED_BY { get; set; }

        public DateTime? CREATED { get; set; }

        public long? LAST_UPD_BY { get; set; }

        public DateTime? LAST_UPD { get; set; }

        public long SR_NUM { get; set; }

        public long? CUST_ID { get; set; }

        public long? ASSET_ID { get; set; }

        [StringLength(100)]
        public string SUB_CATEGORY { get; set; }

        [StringLength(30)]
        public string PRIORITY { get; set; }

        [StringLength(30)]
        public string STATUS_CD { get; set; }

        public long? CASE_OWNER_ID { get; set; }

        public DateTime? CASE_DUE_DT { get; set; }

        [StringLength(50)]
        public string TEAM { get; set; }

        [StringLength(100)]
        public string ASSIGNEE { get; set; }

        public DateTime? CASE_RESL_DT { get; set; }

        [StringLength(30)]
        public string RESOLUTION { get; set; }

        [StringLength(50)]
        public string RES_TYPE { get; set; }

        public string CASE_DESC { get; set; }

        [StringLength(100)]
        public string BILL_ACCNT_NUM { get; set; }

        [StringLength(50)]
        public string INV_NUM { get; set; }

        [Column(TypeName = "money")]
        public decimal? DISPUTE_AMT { get; set; }

        [StringLength(50)]
        public string CHQ_NUM { get; set; }

        [StringLength(1)]
        public string CUST_COMP_FLG { get; set; }

        [StringLength(30)]
        public string SVC_TYPE { get; set; }

        [StringLength(50)]
        public string SVC_NUM { get; set; }

        [StringLength(50)]
        public string NEW_ADR_UNIT { get; set; }

        [StringLength(50)]
        public string NEW_ADR_FLR { get; set; }

        [StringLength(50)]
        public string NEW_ADR_BLDG { get; set; }

        [StringLength(50)]
        public string NEW_ADR_STREET { get; set; }

        [StringLength(50)]
        public string NEW_ADR_SCT { get; set; }

        [StringLength(10)]
        public string NEW_ADR_POST { get; set; }

        [StringLength(100)]
        public string NEW_ADR_CITY { get; set; }

        [StringLength(30)]
        public string NEW_ADR_STATE { get; set; }

        [StringLength(100)]
        public string NEW_ADR_CTY { get; set; }

        [StringLength(30)]
        public string NEW_CON_NUM { get; set; }

        [StringLength(255)]
        public string NEW_CON_NAME { get; set; }

        [StringLength(50)]
        public string NEW_CON_EMAIL { get; set; }

        [StringLength(30)]
        public string NEW_PYMT_MET { get; set; }

        [StringLength(30)]
        public string NEW_BILL_TYPE { get; set; }

        [StringLength(30)]
        public string BILL_AJST_RSN { get; set; }

        [StringLength(200)]
        public string NAME { get; set; }

        public long? CATEGORY_ID { get; set; }

        public virtual P_SRV_CATEGORY P_SRV_CATEGORY { get; set; }

        public virtual P_USER P_USER { get; set; }
    }
}
