namespace ResComm.Web.Lib.DB.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class P_BILLING
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public P_BILLING()
        {
            P_BILLING_PAYMENT = new HashSet<P_BILLING_PAYMENT>();
        }

        [Key]
        public long ROW_ID { get; set; }

        public DateTime? CREATED { get; set; }

        public long? CREATED_BY { get; set; }

        public DateTime? LAST_UPDATED { get; set; }

        public long? LAST_UPDATED_BY { get; set; }

        [StringLength(100)]
        public string INVOICE_NO { get; set; }

        public DateTime? DUE_DATE { get; set; }

        [StringLength(255)]
        public string REFERENCE { get; set; }

        public decimal? AMOUNT { get; set; }

        [StringLength(50)]
        public string STATUS { get; set; }

        public long? UNIT_ID { get; set; }

        public DateTime? BILLING_DATE { get; set; }

        [StringLength(500)]
        public string FILE_NAME { get; set; }

        [StringLength(255)]
        public string TOKEN { get; set; }

        public virtual P_UNIT P_UNIT { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<P_BILLING_PAYMENT> P_BILLING_PAYMENT { get; set; }
    }
}
