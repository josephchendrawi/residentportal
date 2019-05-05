using ResComm.Web.Lib.DB.DAL;
using ResComm.Web.Lib.Interface.Common.Enums;
using ResComm.Web.Lib.Interface.Models.Billing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResComm.Web.Lib.BLL
{
    public class BillingPaymentBLL
    {
        BillingPaymentDALImpl BillingPaymentDALImpl = new BillingPaymentDALImpl();

        public BillingPaymentVO Get(long BillingPaymentId)
        {
            var BillingPaymentVO = BillingPaymentDALImpl.GetBillingPayment(BillingPaymentId);

            if (BillingPaymentVO != null && BillingPaymentVO.ROW_ID != 0)
            {
                return BillingPaymentVO;
            }
            else
            {
                return null;
            }
        }
        public BillingPaymentVO GetByBillingId(long BillingId)
        {
            var BillingPaymentVO = BillingPaymentDALImpl.GetBillingPaymentByBillingId(BillingId);

            if (BillingPaymentVO != null && BillingPaymentVO.ROW_ID != 0)
            {
                return BillingPaymentVO;
            }
            else
            {
                return null;
            }
        }

        public List<BillingPaymentVO> GetAll(long PropertyId, DateTime? FromDate = null, DateTime? ToDate = null)
        {
            var BillingPaymentVOs = BillingPaymentDALImpl.GetBillingPayments(PropertyId, FromDate, ToDate);

            return BillingPaymentVOs;
        }

        public long CreateBillingPayment(BillingPaymentVO BillingPaymentVO)
        {
            return BillingPaymentDALImpl.CreateBillingPayment(BillingPaymentVO);
        }

        public void UpdateBillingPayment(BillingPaymentVO BillingPaymentVO)
        {
            BillingPaymentDALImpl.UpdateBillingPayment(BillingPaymentVO);
        }

        public void DeleteBillingPayment(long BillingId, long ByUserId)
        {
            var BillingPaymentVO = Get(BillingId);

            BillingPaymentVO.STATUS = BILLING_STATUS.Deleted.ToString();
            BillingPaymentVO.LAST_UPDATED_BY = ByUserId;

            BillingPaymentDALImpl.UpdateBillingPayment(BillingPaymentVO);
        }

    }
}
