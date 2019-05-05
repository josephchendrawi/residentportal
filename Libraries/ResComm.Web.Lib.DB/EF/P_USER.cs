namespace ResComm.Web.Lib.DB.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class P_USER
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public P_USER()
        {
            P_AFFLIATE_LEADS = new HashSet<P_AFFLIATE_LEADS>();
            P_BILLING_PAYMENT = new HashSet<P_BILLING_PAYMENT>();
            P_FACILITY_SLOT_BOOKING = new HashSet<P_FACILITY_SLOT_BOOKING>();
            P_SRV_TIC = new HashSet<P_SRV_TIC>();
            P_UNIT = new HashSet<P_UNIT>();
            P_UNIT1 = new HashSet<P_UNIT>();
        }

        [Key]
        public long ROW_ID { get; set; }

        public long? CREATED_BY { get; set; }

        public DateTime? CREATED { get; set; }

        public long? LAST_UPD_BY { get; set; }

        public DateTime? LAST_UPD { get; set; }

        [StringLength(100)]
        public string FIRST_NAME { get; set; }

        [StringLength(100)]
        public string LAST_NAME { get; set; }

        [Required]
        [StringLength(100)]
        public string USERNAME { get; set; }

        [StringLength(30)]
        public string PASSWORD { get; set; }

        public DateTime? BIRTH_DT { get; set; }

        [StringLength(30)]
        public string SALUTATION { get; set; }

        [StringLength(30)]
        public string NATIONALITY { get; set; }

        [StringLength(30)]
        public string ID_TYPE { get; set; }

        [StringLength(50)]
        public string ID_NUM { get; set; }

        [StringLength(100)]
        public string EMAIL_ADDR { get; set; }

        [StringLength(30)]
        public string MOBILE_NO { get; set; }

        [StringLength(30)]
        public string HOUSE_NO { get; set; }

        [StringLength(30)]
        public string OFFICE_NO { get; set; }

        [StringLength(30)]
        public string FAX_NO { get; set; }

        [StringLength(30)]
        public string USER_TYPE { get; set; }

        [StringLength(30)]
        public string ACCNT_STATUS { get; set; }

        [StringLength(1)]
        public string ACTIVE_FLG { get; set; }

        public long? ADDR_ID { get; set; }

        public long? ACCNT_ID { get; set; }

        [StringLength(255)]
        public string TOKEN { get; set; }

        public DateTime? TOKEN_EXPIRY { get; set; }

        [StringLength(10)]
        public string REFERRAL_CODE { get; set; }

        public decimal? BALANCE { get; set; }

        public int? PAYMENT_STATUS { get; set; }

        public virtual P_ACCNT P_ACCNT { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<P_AFFLIATE_LEADS> P_AFFLIATE_LEADS { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<P_BILLING_PAYMENT> P_BILLING_PAYMENT { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<P_FACILITY_SLOT_BOOKING> P_FACILITY_SLOT_BOOKING { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<P_SRV_TIC> P_SRV_TIC { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<P_UNIT> P_UNIT { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<P_UNIT> P_UNIT1 { get; set; }
    }
}
