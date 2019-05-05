namespace ResComm.Web.Lib.DB.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class P_DEMAND_LST
    {
        [Key]
        public long ROW_ID { get; set; }

        public long? CREATED_BY { get; set; }

        public DateTime? CREATED { get; set; }

        public long? LAST_UPD_BY { get; set; }

        public DateTime? LAST_UPD { get; set; }

        [StringLength(30)]
        public string DL_NUM { get; set; }

        [StringLength(255)]
        public string DL_CUST_NAME { get; set; }

        [StringLength(30)]
        public string DL_ADDR_TYPE { get; set; }

        [StringLength(50)]
        public string DL_ADDR { get; set; }

        [StringLength(50)]
        public string DL_ADDR_1 { get; set; }

        [StringLength(50)]
        public string DL_ADDR_2 { get; set; }

        [StringLength(100)]
        public string DL_BUILDING_NAME { get; set; }

        [StringLength(30)]
        public string DL_STREET_TYPE { get; set; }

        [StringLength(50)]
        public string DL_STREET_NAME { get; set; }

        [StringLength(50)]
        public string DL_SECTION { get; set; }

        [StringLength(10)]
        public string DL_POSTAL_CD { get; set; }

        [StringLength(100)]
        public string DL_CITY { get; set; }

        [StringLength(100)]
        public string DL_STATE { get; set; }

        [StringLength(30)]
        public string DL_COUNTRY { get; set; }

        [StringLength(30)]
        public string DL_STATUS { get; set; }

        [StringLength(50)]
        public string DL_SOURCE { get; set; }

        [StringLength(100)]
        public string DL_REASON { get; set; }

        public string DL_COMMENT { get; set; }

        [StringLength(50)]
        public string DL_EMAIL { get; set; }

        [StringLength(30)]
        public string DL_CONTACT_NUM { get; set; }

        public long? DL_PROD_ID { get; set; }
    }
}
