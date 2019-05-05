using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResComm.Web.Lib.Interface.Models.AccountNote
{
    public class AccountNoteVO : BaseVO
    {
        public long ROW_ID { get; set; }

        public long? CREATED_BY { get; set; }

        public DateTime? CREATED { get; set; }

        public long? LAST_UPD_BY { get; set; }

        public DateTime? LAST_UPD { get; set; }

        public long? ACCNT_ID { get; set; }

        public string NOTE { get; set; }

        public string ITEM_TYPE { get; set; }

        public string NAME { get; set; }


        public string CreatedBy { get; set; }
        public string LastUpdatedBy { get; set; }
    }
}
