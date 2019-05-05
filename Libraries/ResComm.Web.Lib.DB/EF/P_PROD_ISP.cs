namespace ResComm.Web.Lib.DB.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class P_PROD_ISP
    {
        [Key]
        public long ROW_ID { get; set; }

        public long? CREATED_BY { get; set; }

        public DateTime? CREATED { get; set; }

        public long? LAST_UPD_BY { get; set; }

        public DateTime? LAST_UPD { get; set; }

        [Required]
        [StringLength(30)]
        public string PROVIDER_NAME { get; set; }

        [StringLength(255)]
        public string IMAGE_FILE_PATH { get; set; }

        [StringLength(255)]
        public string PROVIDER_DESC { get; set; }

        public string POLICY { get; set; }
    }
}
