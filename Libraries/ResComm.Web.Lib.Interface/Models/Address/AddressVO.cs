using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResComm.Web.Lib.Interface.Models.Address
{
    public class AddressVO : BaseVO
    {
        public long ROW_ID { get; set; }

        public long? CREATED_BY { get; set; }

        public DateTime? CREATED { get; set; }

        public long? LAST_UPD_BY { get; set; }

        public DateTime? LAST_UPD { get; set; }

        public string ADDR_NAME { get; set; }

        public string ADDR_TYPE { get; set; }

        public string ADDR { get; set; }

        public string ADDR_1 { get; set; }

        public string ADDR_2 { get; set; }

        public string BUILDING_NAME { get; set; }

        public string STREET_TYPE { get; set; }

        public string STREET_NAME { get; set; }

        public string SECTION { get; set; }

        public string POSTAL_CD { get; set; }

        public string CITY { get; set; }

        public string STATE { get; set; }

        public string COUNTRY { get; set; }

        public string INTEGRATION_ID { get; set; }

        public string SITE_NAME { get; set; }

        public DateTime? APPT_DT { get; set; }

        public string STATUS_CD { get; set; }
    }
}
