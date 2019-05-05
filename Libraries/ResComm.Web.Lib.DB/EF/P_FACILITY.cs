namespace ResComm.Web.Lib.DB.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class P_FACILITY
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public P_FACILITY()
        {
            P_FACILITY_SLOT = new HashSet<P_FACILITY_SLOT>();
        }

        [Key]
        public long ROW_ID { get; set; }

        public DateTime? CREATED { get; set; }

        public long? CREATED_BY { get; set; }

        public DateTime? LAST_UPDATED { get; set; }

        public long? LAST_UPDATED_BY { get; set; }

        public long? ACCNT_ID { get; set; }

        [StringLength(255)]
        public string NAME { get; set; }

        public string DESCRIPTION { get; set; }

        public int? STATUS { get; set; }

        public virtual P_ACCNT P_ACCNT { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<P_FACILITY_SLOT> P_FACILITY_SLOT { get; set; }
    }
}
