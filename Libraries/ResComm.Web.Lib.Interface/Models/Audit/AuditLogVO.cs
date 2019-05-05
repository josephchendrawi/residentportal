using System;
namespace ResComm.Web.Lib.Interface.Models.Audit
{
    public partial class AuditLogVO
    {
        public long ROW_ID { get; set; }

        public long? CREATED_BY { get; set; }

        public DateTime? CREATED { get; set; }

        public long? LAST_UPD_BY { get; set; }

        public DateTime? LAST_UPD { get; set; }

        public string ENTITY { get; set; }

        public string EVENT_TYPE { get; set; }

        public long? COMMIT_BY { get; set; }

        public long? RECORD_KEY { get; set; }

        public string LOG_DETAILS { get; set; }

    }
}
