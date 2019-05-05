using ResComm.Web.Lib.Interface.Models.UnitType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResComm.Web.Lib.Interface.Models.Unit
{
    public class UnitVO : BaseVO
    {
        public long ROW_ID { get; set; }

        public DateTime? CREATED { get; set; }

        public long? CREATED_BY { get; set; }

        public DateTime? LAST_UPDATED { get; set; }

        public long? LAST_UPDATED_BY { get; set; }

        public long? UNIT_TYPE_ID { get; set; }

        public string UNIT_NO { get; set; }

        public long? TENANT_ID { get; set; }

        public long? OWNER_ID { get; set; }

        public string ACTIVE_FLG { get; set; }
        

        public long? PROPERTY_ID { get; set; }
        public UnitTypeVO UnitType { get; set; }

        public string OWNER_NAME { get; set; }
        public string TENANT_NAME { get; set; }
    }
}
