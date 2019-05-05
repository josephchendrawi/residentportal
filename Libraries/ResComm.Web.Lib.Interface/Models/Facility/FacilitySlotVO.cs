using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResComm.Web.Lib.Interface.Models.Facility
{
    public class FacilitySlotVO : BaseVO
    {
        public long ROW_ID { get; set; }

        public DateTime? CREATED { get; set; }

        public long? CREATED_BY { get; set; }

        public DateTime? LAST_UPDATED { get; set; }

        public long? LAST_UPDATED_BY { get; set; }

        public long? FACILITY_ID { get; set; }

        public DateTime? START_TIME { get; set; }

        public DateTime? END_TIME { get; set; }

        public int? SLOT { get; set; }

        public int? STATUS { get; set; }
    }
}
