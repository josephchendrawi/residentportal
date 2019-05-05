using ResComm.Web.Lib.Interface.Common.Enums;
using ResComm.Web.Lib.Interface.Models.Billing;
using ResComm.Web.Lib.Interface.Models.Ticket;
using ResComm.Web.Lib.Interface.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResComm.Web.Lib.DB.DAL
{
    public interface IBillingCompileDAL
    {
        BillingCompileVO GetBillingCompile(long BillingCompileId);
        List<BillingCompileVO> GetBillingCompiles(long PropertyId = 0);
        long CreateBillingCompile(BillingCompileVO _newBilling);
        void UpdateBillingCompile(BillingCompileVO _updatedBilling);
    }
}
