using ResComm.Web.Lib.Interface.Models.Unit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResComm.Web.Lib.Interface.Models.User
{
    public class UserVO : BaseVO
    {
        public long ROW_ID { get; set; }

        public long? CREATED_BY { get; set; }

        public DateTime? CREATED { get; set; }

        public long? LAST_UPD_BY { get; set; }

        public DateTime? LAST_UPD { get; set; }

        public string FIRST_NAME { get; set; }

        public string LAST_NAME { get; set; }

        public string USERNAME { get; set; }

        public string PASSWORD { get; set; }

        public DateTime? BIRTH_DT { get; set; }

        public string SALUTATION { get; set; }

        public string NATIONALITY { get; set; }

        public string ID_TYPE { get; set; }

        public string ID_NUM { get; set; }

        public string EMAIL_ADDR { get; set; }

        public string MOBILE_NO { get; set; }

        public string HOUSE_NO { get; set; }

        public string OFFICE_NO { get; set; }

        public string FAX_NO { get; set; }

        public string USER_TYPE { get; set; }

        public string ACCNT_STATUS { get; set; }

        public string ACTIVE_FLG { get; set; }

        public long? ADDR_ID { get; set; }

        public long? ACCNT_ID { get; set; }

        public string TOKEN { get; set; }

        public DateTime? TOKEN_EXPIRY { get; set; }

        public string REFERRAL_CODE { get; set; }

        public decimal? BALANCE { get; set; }

        public int? PAYMENT_STATUS { get; set; }


        public UnitVO Unit { get; set; }
    }

}
