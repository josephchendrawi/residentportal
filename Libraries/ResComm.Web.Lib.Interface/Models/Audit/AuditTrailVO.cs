using System;
namespace ResComm.Web.Lib.Interface.Models.Audit
{
    public partial class AuditTrailVO
    {
        public long ROW_ID { get; set; }

        public long? CREATED_BY { get; set; }

        public DateTime? CREATED { get; set; }

        public long? LAST_UPD_BY { get; set; }

        public DateTime? LAST_UPD { get; set; }

        public long? AUDIT_ROW_ID { get; set; }

        public string ACTION_CD { get; set; }

        public string OLD_VAL { get; set; }

        public string NEW_VAL { get; set; }

        public string MODULE_NAME { get; set; }

        public string SCREEN_NAME { get; set; }
        public string FIELD_NAME { get; set; }
        public string CreatedByUserName { get; set; }

    }
}
