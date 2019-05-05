namespace ResComm.Web.Lib.DB.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class P_ADDR
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public P_ADDR()
        {
            P_ACCNT = new HashSet<P_ACCNT>();
        }

        [Key]
        public long ROW_ID { get; set; }

        public long? CREATED_BY { get; set; }

        public DateTime? CREATED { get; set; }

        public long? LAST_UPD_BY { get; set; }

        public DateTime? LAST_UPD { get; set; }

        [Required]
        [StringLength(100)]
        public string ADDR_NAME { get; set; }

        [StringLength(30)]
        public string ADDR_TYPE { get; set; }

        [StringLength(255)]
        public string ADDR { get; set; }

        [StringLength(50)]
        public string ADDR_1 { get; set; }

        [StringLength(50)]
        public string ADDR_2 { get; set; }

        [StringLength(100)]
        public string BUILDING_NAME { get; set; }

        [StringLength(30)]
        public string STREET_TYPE { get; set; }

        [StringLength(50)]
        public string STREET_NAME { get; set; }

        [StringLength(100)]
        public string SECTION { get; set; }

        [StringLength(10)]
        public string POSTAL_CD { get; set; }

        [StringLength(100)]
        public string CITY { get; set; }

        [StringLength(30)]
        public string STATE { get; set; }

        [StringLength(30)]
        public string COUNTRY { get; set; }

        [StringLength(15)]
        public string INTEGRATION_ID { get; set; }

        [StringLength(50)]
        public string SITE_NAME { get; set; }

        public DateTime? APPT_DT { get; set; }

        [StringLength(100)]
        public string STATUS_CD { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<P_ACCNT> P_ACCNT { get; set; }
    }
}
