using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResComm.Web.Lib.Interface.Models.Facility
{
    public class FacilityVO : BaseVO
    {
        public long ROW_ID { get; set; }

        public DateTime? CREATED { get; set; }

        public long? CREATED_BY { get; set; }

        public DateTime? LAST_UPDATED { get; set; }

        public long? LAST_UPDATED_BY { get; set; }

        public long? ACCNT_ID { get; set; }

        public string NAME { get; set; }

        public string DESCRIPTION { get; set; }

        public int? STATUS { get; set; }
    }
}
