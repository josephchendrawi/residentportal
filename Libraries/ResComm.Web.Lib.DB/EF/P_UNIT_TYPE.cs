namespace ResComm.Web.Lib.DB.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class P_UNIT_TYPE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public P_UNIT_TYPE()
        {
            P_UNIT = new HashSet<P_UNIT>();
        }

        [Key]
        public long ROW_ID { get; set; }

        public DateTime? CREATED { get; set; }

        public long? CREATED_BY { get; set; }

        public DateTime? LAST_UPDATED { get; set; }

        public long? LAST_UPDATED_BY { get; set; }

        public long? PROPERTY_ID { get; set; }

        [StringLength(255)]
        public string CATEGORY_NAME { get; set; }

        [StringLength(255)]
        public string CATEGORY_VALUE { get; set; }

        [StringLength(255)]
        public string FLOOR { get; set; }

        [StringLength(5)]
        public string ACTIVE_FLG { get; set; }

        public virtual P_PROPERTY P_PROPERTY { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<P_UNIT> P_UNIT { get; set; }
    }
}
