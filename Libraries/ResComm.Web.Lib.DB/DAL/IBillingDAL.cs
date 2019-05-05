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
    public interface IBillingDAL
    {
        BillingVO GetBilling(long BillingId);
        List<BillingVO> GetBillings(int startIdx, int length, ref int TotalCount, string orderBy = "", string orderDirection = "", BillingVO qFilter = null, List<long> UnitIds = null);
        long CreateBilling(BillingVO _newBilling);
        void UpdateBilling(BillingVO _updatedBilling);
    }
}
