namespace ResComm.Web.Lib.DB.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class P_GROUP_ACCESS
    {
        [Key]
        public long ROW_ID { get; set; }

        public long? CREATED_BY { get; set; }

        public DateTime? CREATED { get; set; }

        public long? LAST_UPD_BY { get; set; }

        public DateTime? LAST_UPD { get; set; }

        public long? GROUP_ID { get; set; }

        public long? MODULE_ID { get; set; }

        public int? IS_VIEWABLE { get; set; }

        public int? IS_ADDABLE { get; set; }

        public int? IS_EDITABLE { get; set; }

        public int? IS_DELETABLE { get; set; }

        public int? IS_APPROVABLE { get; set; }

        public int? IS_REJECTABLE { get; set; }
    }
}
