using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResComm.Web.Lib.Interface.Models.Property
{
    public class PropertyVO : BaseVO
    {
        public long ROW_ID { get; set; }

        public long? CREATED_BY { get; set; }

        public DateTime? CREATED { get; set; }

        public long? LAST_UPD_BY { get; set; }

        public DateTime? LAST_UPD { get; set; }

        public long? ACCNT_ID { get; set; }

        public string SITE_CODE { get; set; }

        public string NAME { get; set; }

        public string ADDR_1 { get; set; }

        public string ADDR_2 { get; set; }

        public string POSTAL_CD { get; set; }

        public string CITY { get; set; }

        public string STATE { get; set; }

        public string COUNTRY { get; set; }

        public string LOGO_URL { get; set; }
    }
}
