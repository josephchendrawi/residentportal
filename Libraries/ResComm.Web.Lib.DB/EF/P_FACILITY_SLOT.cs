namespace ResComm.Web.Lib.DB.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class P_FACILITY_SLOT
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public P_FACILITY_SLOT()
        {
            P_FACILITY_SLOT_BOOKING = new HashSet<P_FACILITY_SLOT_BOOKING>();
        }

        [Key]
        public long ROW_ID { get; set; }

        public DateTime? CREATED { get; set; }

        public long? CREATED_BY { get; set; }

        public DateTime? LAST_UPDATED { get; set; }

        public long? LAST_UPDATED_BY { get; set; }

        public long? FACILITY_ID { get; set; }

        public DateTime? START_TIME { get; set; }

        public DateTime? END_TIME { get; set; }

        public int? SLOT { get; set; }

        public int? STATUS { get; set; }

        public virtual P_FACILITY P_FACILITY { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<P_FACILITY_SLOT_BOOKING> P_FACILITY_SLOT_BOOKING { get; set; }
    }
}
