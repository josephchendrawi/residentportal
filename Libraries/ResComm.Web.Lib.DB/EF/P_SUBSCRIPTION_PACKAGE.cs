namespace ResComm.Web.Lib.DB.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class P_SUBSCRIPTION_PACKAGE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public P_SUBSCRIPTION_PACKAGE()
        {
            P_PROPERTY_SUBSCRIPTION = new HashSet<P_PROPERTY_SUBSCRIPTION>();
            P_PROPERTY_SUBSCRIPTION_ORDER = new HashSet<P_PROPERTY_SUBSCRIPTION_ORDER>();
        }

        [Key]
        public long ROW_ID { get; set; }

        public DateTime? CREATED { get; set; }

        public long? CREATED_BY { get; set; }

        public DateTime? LAST_UPDATED { get; set; }

        public long? LAST_UPDATED_BY { get; set; }

        [StringLength(255)]
        public string NAME { get; set; }

        public decimal? PRICE { get; set; }

        public int? DURATION { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<P_PROPERTY_SUBSCRIPTION> P_PROPERTY_SUBSCRIPTION { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<P_PROPERTY_SUBSCRIPTION_ORDER> P_PROPERTY_SUBSCRIPTION_ORDER { get; set; }
    }
}
