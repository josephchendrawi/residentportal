namespace ResComm.Web.Lib.DB.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class P_UNIT
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public P_UNIT()
        {
            P_BILLING = new HashSet<P_BILLING>();
        }

        [Key]
        public long ROW_ID { get; set; }

        public DateTime? CREATED { get; set; }

        public long? CREATED_BY { get; set; }

        public DateTime? LAST_UPDATED { get; set; }

        public long? LAST_UPDATED_BY { get; set; }

        public long? UNIT_TYPE_ID { get; set; }

        [StringLength(255)]
        public string UNIT_NO { get; set; }

        public long? TENANT_ID { get; set; }

        public long? OWNER_ID { get; set; }

        [StringLength(5)]
        public string ACTIVE_FLG { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<P_BILLING> P_BILLING { get; set; }

        public virtual P_UNIT_TYPE P_UNIT_TYPE { get; set; }

        public virtual P_USER P_USER { get; set; }

        public virtual P_USER P_USER1 { get; set; }
    }
}
