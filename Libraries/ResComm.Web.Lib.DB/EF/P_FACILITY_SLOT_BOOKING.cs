namespace ResComm.Web.Lib.DB.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class P_FACILITY_SLOT_BOOKING
    {
        [Key]
        public long ROW_ID { get; set; }

        public DateTime? CREATED { get; set; }

        public long? CREATED_BY { get; set; }

        public DateTime? LAST_UPDATED { get; set; }

        public long? LAST_UPDATED_BY { get; set; }

        public long? BOOKING_ID { get; set; }

        public long? USER_ID { get; set; }

        public long? FACILITY_SLOT_ID { get; set; }

        public DateTime? BOOK_DATE { get; set; }

        public int? STATUS { get; set; }

        public DateTime? CHECK_IN { get; set; }

        public DateTime? CHECK_OUT { get; set; }

        [StringLength(300)]
        public string REMARKS { get; set; }

        public virtual P_FACILITY_SLOT P_FACILITY_SLOT { get; set; }

        public virtual P_USER P_USER { get; set; }
    }
}
