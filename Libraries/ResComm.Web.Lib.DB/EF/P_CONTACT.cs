namespace ResComm.Web.Lib.DB.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class P_CONTACT
    {
        [Key]
        public long ROW_ID { get; set; }

        public long? CREATED_BY { get; set; }

        public DateTime? CREATED { get; set; }

        public long? LAST_UPD_BY { get; set; }

        public DateTime? LAST_UPD { get; set; }

        [Required]
        [StringLength(100)]
        public string PERSON_UID { get; set; }

        [StringLength(1)]
        public string DEFAULT_FLG { get; set; }

        [StringLength(1)]
        public string CONTACT_FLG { get; set; }

        [StringLength(30)]
        public string SALUTATION { get; set; }

        [StringLength(100)]
        public string FST_NAME { get; set; }

        [StringLength(50)]
        public string MIDDLE_NAME { get; set; }

        [StringLength(100)]
        public string LAST_NAME { get; set; }

        [StringLength(250)]
        public string FULL_NAME { get; set; }

        [StringLength(30)]
        public string ID_TYPE { get; set; }

        [StringLength(50)]
        public string ID_NUM { get; set; }

        [StringLength(30)]
        public string GENDER { get; set; }

        public DateTime? BIRTH_DT { get; set; }

        [StringLength(30)]
        public string NATIONALITY { get; set; }

        [StringLength(50)]
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
        public string POSTN { get; set; }

        [StringLength(30)]
        public string INTEGRATION_ID { get; set; }

        [StringLength(30)]
        public string PREF_LANG { get; set; }
    }
}
