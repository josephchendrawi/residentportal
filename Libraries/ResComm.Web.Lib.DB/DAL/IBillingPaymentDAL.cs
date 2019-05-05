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
    public interface IBillingPaymentDAL
    {
        BillingPaymentVO GetBillingPayment(long BillingPaymentId);
        BillingPaymentVO GetBillingPaymentByBillingId(long BillingId);
        List<BillingPaymentVO> GetBillingPayments(long PropertyId, DateTime? FromDate = null, DateTime? ToDate = null);
        long CreateBillingPayment(BillingPaymentVO _newBilling);
        void UpdateBillingPayment(BillingPaymentVO _updatedBilling);
    }
}
