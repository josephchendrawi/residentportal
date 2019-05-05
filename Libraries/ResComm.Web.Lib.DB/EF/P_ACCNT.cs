namespace ResComm.Web.Lib.DB.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class P_ACCNT
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public P_ACCNT()
        {
            P_FACILITY = new HashSet<P_FACILITY>();
            P_PROPERTY = new HashSet<P_PROPERTY>();
            P_SRV_CATEGORY = new HashSet<P_SRV_CATEGORY>();
            P_USER = new HashSet<P_USER>();
        }

        [Key]
        public long ROW_ID { get; set; }

        public long? CREATED_BY { get; set; }

        public DateTime? CREATED { get; set; }

        public long? LAST_UPD_BY { get; set; }

        public DateTime? LAST_UPD { get; set; }

        [StringLength(255)]
        public string NAME { get; set; }

        [StringLength(30)]
        public string LOC { get; set; }

        public DateTime? BIRTH_DT { get; set; }

        [StringLength(30)]
        public string SALUTATION { get; set; }

        [StringLength(30)]
        public string NATIONALITY { get; set; }

        [StringLength(30)]
        public string ID_TYPE { get; set; }

        [StringLength(50)]
        public string ID_NUM { get; set; }

        [StringLength(30)]
        public string CUST_TYPE_CD { get; set; }

        [Required]
        [StringLength(30)]
        public string ACCNT_TYPE_CD { get; set; }

        [StringLength(30)]
        public string INTEGRATION_ID { get; set; }

        [StringLength(30)]
        public string CUSTOMER_NUM { get; set; }

        [StringLength(30)]
        public string ACCNT_STATUS { get; set; }

        public long? ADDR_ID { get; set; }

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

        [StringLength(50)]
        public string BANK_NAME { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CUST_SINCE { get; set; }

        public long? PAR_ACCNT_ID { get; set; }

        public long? MASTER_ACCNT_ID { get; set; }

        [StringLength(250)]
        public string CONTACT_NAME { get; set; }

        [StringLength(30)]
        public string BUSINESS_TYPE { get; set; }

        [StringLength(255)]
        public string WEBSITE_URL { get; set; }

        public int? EMP_NO { get; set; }

        [StringLength(30)]
        public string VIP_FLG { get; set; }

        [StringLength(30)]
        public string BILL_ACCNT_NUM { get; set; }

        [StringLength(100)]
        public string BILL_ACCNT_NAME { get; set; }

        [StringLength(100)]
        public string BILL_COMP_NAME { get; set; }

        [StringLength(10)]
        public string BILL_CYCLE { get; set; }

        [StringLength(10)]
        public string BILL_CURR { get; set; }

        [StringLength(30)]
        public string PAYMENT_METHOD { get; set; }

        [StringLength(30)]
        public string SALES_CD { get; set; }

        [StringLength(30)]
        public string INDUSTRIAL_CD { get; set; }

        public long? CUST_USER_ID { get; set; }

        [StringLength(30)]
        public string ACCNT_SUB_STATUS { get; set; }

        public long? REFERRED_BY { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<P_FACILITY> P_FACILITY { get; set; }

        public virtual P_ADDR P_ADDR { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<P_PROPERTY> P_PROPERTY { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<P_SRV_CATEGORY> P_SRV_CATEGORY { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<P_USER> P_USER { get; set; }
    }
}
