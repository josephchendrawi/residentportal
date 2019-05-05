using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResComm.Web.Lib.Interface.Models.Ticket
{
    public class TicketVO : BaseVO
    {
        public long ROW_ID { get; set; }

        public long? CREATED_BY { get; set; }

        public DateTime? CREATED { get; set; }

        public long? LAST_UPD_BY { get; set; }

        public DateTime? LAST_UPD { get; set; }

        public long SR_NUM { get; set; }

        public long? CUST_ID { get; set; }

        public long? ASSET_ID { get; set; }

        public long? CATEGORY_ID { get; set; }

        public string SUB_CATEGORY { get; set; }

        public string PRIORITY { get; set; }

        public string STATUS_CD { get; set; }

        public long? CASE_OWNER_ID { get; set; }

        public DateTime? CASE_DUE_DT { get; set; }

        public string TEAM { get; set; }

        public string ASSIGNEE { get; set; }

        public DateTime? CASE_RESL_DT { get; set; }

        public string RESOLUTION { get; set; }

        public string RES_TYPE { get; set; }

        public string CASE_DESC { get; set; }

        public string BILL_ACCNT_NUM { get; set; }

        public string INV_NUM { get; set; }

        public decimal? DISPUTE_AMT { get; set; }

        public string CHQ_NUM { get; set; }

        public string CUST_COMP_FLG { get; set; }

        public string SVC_TYPE { get; set; }

        public string SVC_NUM { get; set; }

        public string NEW_ADR_UNIT { get; set; }

        public string NEW_ADR_FLR { get; set; }

        public string NEW_ADR_BLDG { get; set; }

        public string NEW_ADR_STREET { get; set; }

        public string NEW_ADR_SCT { get; set; }

        public string NEW_ADR_POST { get; set; }

        public string NEW_ADR_CITY { get; set; }

        public string NEW_ADR_STATE { get; set; }

        public string NEW_ADR_CTY { get; set; }

        public string NEW_CON_NUM { get; set; }

        public string NEW_CON_NAME { get; set; }

        public string NEW_CON_EMAIL { get; set; }

        public string NEW_PYMT_MET { get; set; }

        public string NEW_BILL_TYPE { get; set; }

        public string BILL_AJST_RSN { get; set; }

        public string NAME { get; set; }


        public string REQUESTER_NAME { get; set; }
        public string CATEGORY_NAME { get; set; }
    }
}
