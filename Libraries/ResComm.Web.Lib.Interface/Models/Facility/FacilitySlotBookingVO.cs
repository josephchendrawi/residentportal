using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResComm.Web.Lib.Interface.Models.Facility
{
    public class FacilitySlotBookingVO : BaseVO
    {
        public long ROW_ID { get; set; }

        public DateTime? CREATED { get; set; }

        public long? CREATED_BY { get; set; }

        public DateTime? LAST_UPDATED { get; set; }

        public long? LAST_UPDATED_BY { get; set; }

        public long? USER_ID { get; set; }

        public long? FACILITY_SLOT_ID { get; set; }

        public DateTime? BOOK_DATE { get; set; }

        public int? STATUS { get; set; }

        public DateTime? CHECK_IN { get; set; }

        public DateTime? CHECK_OUT { get; set; }

        public string REMARKS { get; set; }
        

        public DateTime? VO_FACILITY_START_TIME { get; set; }
        public DateTime? VO_FACILITY_END_TIME { get; set; }
        public string VO_USER_FIRST_NAME { get; set; }

        public long? VO_FACILITIY_ID { get; set; }
    }
}
