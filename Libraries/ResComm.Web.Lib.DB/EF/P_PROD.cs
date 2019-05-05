namespace ResComm.Web.Lib.DB.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class P_PROD
    {
        [Key]
        public long ROW_ID { get; set; }

        public long? CREATED_BY { get; set; }

        public DateTime? CREATED { get; set; }

        public long? LAST_UPD_BY { get; set; }

        public DateTime? LAST_UPD { get; set; }

        [Required]
        [StringLength(100)]
        public string PRD_NAME { get; set; }

        [StringLength(255)]
        public string PRD_DESC { get; set; }

        [StringLength(50)]
        public string PRD_CATEGORY { get; set; }

        [StringLength(30)]
        public string PRD_TYPE { get; set; }

        [StringLength(10)]
        public string PRD_LVL { get; set; }

        [StringLength(10)]
        public string PRD_CURRENCY { get; set; }

        [Column(TypeName = "money")]
        public decimal? PRD_PRICE { get; set; }

        [StringLength(30)]
        public string PRD_PRICE_TYPE { get; set; }

        public long? PROVIDER_ID { get; set; }

        [StringLength(30)]
        public string QUOTA { get; set; }

        [StringLength(30)]
        public string DL_SPD { get; set; }

        [StringLength(30)]
        public string UL_SPD { get; set; }

        public int? CONT_TERM { get; set; }

        [StringLength(10)]
        public string CON_PERIOD { get; set; }

        public string POLICY { get; set; }

        [StringLength(255)]
        public string IMAGE_FILE_PATH { get; set; }

        [StringLength(1)]
        public string ACTIVE_FLG { get; set; }

        [StringLength(30)]
        public string SEGMENT_TYPE { get; set; }

        [StringLength(50)]
        public string PART_NUM { get; set; }

        [StringLength(30)]
        public string INTEGRATION_ID { get; set; }

        [StringLength(100)]
        public string EXT_PROD_NAME { get; set; }

        public int? MAX_QTY { get; set; }

        [StringLength(1)]
        public string VIS_ASSET { get; set; }

        [StringLength(1)]
        public string VIS_INVOICE { get; set; }

        [StringLength(1)]
        public string VAS_FLG { get; set; }

        [StringLength(1)]
        public string SVC_ID_REQ { get; set; }

        [StringLength(1)]
        public string EQ_ID_REQ { get; set; }

        [StringLength(1)]
        public string PWD_REQ { get; set; }

        [StringLength(1)]
        public string PROVI_REQ { get; set; }

        [StringLength(10)]
        public string GST_CD { get; set; }

        public int? GST_PT { get; set; }

        [StringLength(30)]
        public string RADIUS_GROUP_CD { get; set; }

        [StringLength(50)]
        public string INV_GROUP { get; set; }

        [StringLength(1)]
        public string OTC_FLG { get; set; }
    }
}
