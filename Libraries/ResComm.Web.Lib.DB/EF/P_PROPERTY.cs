namespace ResComm.Web.Lib.DB.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class P_PROPERTY
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public P_PROPERTY()
        {
            P_BILLING_COMPILE = new HashSet<P_BILLING_COMPILE>();
            P_PROPERTY_INVOICE = new HashSet<P_PROPERTY_INVOICE>();
            P_PROPERTY_SUBSCRIPTION = new HashSet<P_PROPERTY_SUBSCRIPTION>();
            P_PROPERTY_SUBSCRIPTION_ORDER = new HashSet<P_PROPERTY_SUBSCRIPTION_ORDER>();
            P_UNIT_TYPE = new HashSet<P_UNIT_TYPE>();
        }

        [Key]
        public long ROW_ID { get; set; }

        public long? CREATED_BY { get; set; }

        public DateTime? CREATED { get; set; }

        public long? LAST_UPD_BY { get; set; }

        public DateTime? LAST_UPD { get; set; }

        public long? ACCNT_ID { get; set; }

        [StringLength(100)]
        public string SITE_CODE { get; set; }

        [StringLength(255)]
        public string NAME { get; set; }

        [StringLength(255)]
        public string ADDR_1 { get; set; }

        [StringLength(255)]
        public string ADDR_2 { get; set; }

        [StringLength(50)]
        public string POSTAL_CD { get; set; }

        [StringLength(255)]
        public string CITY { get; set; }

        [StringLength(50)]
        public string STATE { get; set; }

        [StringLength(50)]
        public string COUNTRY { get; set; }

        [StringLength(500)]
        public string LOGO_URL { get; set; }

        public virtual P_ACCNT P_ACCNT { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<P_BILLING_COMPILE> P_BILLING_COMPILE { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<P_PROPERTY_INVOICE> P_PROPERTY_INVOICE { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<P_PROPERTY_SUBSCRIPTION> P_PROPERTY_SUBSCRIPTION { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<P_PROPERTY_SUBSCRIPTION_ORDER> P_PROPERTY_SUBSCRIPTION_ORDER { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<P_UNIT_TYPE> P_UNIT_TYPE { get; set; }
    }
}
