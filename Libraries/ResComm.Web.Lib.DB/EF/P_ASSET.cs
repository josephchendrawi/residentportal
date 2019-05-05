namespace ResComm.Web.Lib.DB.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class P_ASSET
    {
        [Key]
        public long ROW_ID { get; set; }

        public long? CREATED_BY { get; set; }

        public DateTime? CREATED { get; set; }

        public long? LAST_UPD_BY { get; set; }

        public DateTime? LAST_UPD { get; set; }

        [Required]
        [StringLength(100)]
        public string ASSET_NUM { get; set; }

        public long? PROD_ID { get; set; }

        [StringLength(30)]
        public string STATUS_CD { get; set; }

        public long? CUST_ID { get; set; }

        public long? BILL_ID { get; set; }

        public long? SVC_AC_ID { get; set; }

        public long? PAR_ASSET_ID { get; set; }

        public long? ROOT_ASSET_ID { get; set; }

        [StringLength(40)]
        public string SERVICE_NUM { get; set; }

        [StringLength(30)]
        public string INTEGRATION_ID { get; set; }

        public DateTime? INSTALL_DT { get; set; }

        public DateTime? END_DT { get; set; }

        public int? QTY { get; set; }

        [StringLength(30)]
        public string SUBNET_MASK { get; set; }

        [StringLength(50)]
        public string BTU_ID { get; set; }

        [StringLength(50)]
        public string WORK_ORDER { get; set; }
    }
}
