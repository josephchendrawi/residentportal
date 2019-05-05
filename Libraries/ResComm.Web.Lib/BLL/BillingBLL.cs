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
    public class BillingBLL
    {
        BillingDALImpl BillingDALImpl = new BillingDALImpl();
        UnitDALImpl UnitDALImpl = new UnitDALImpl();

        public BillingVO Get(long BillingId)
        {
            var BillingVO = BillingDALImpl.GetBilling(BillingId);

            if (BillingVO != null && BillingVO.ROW_ID != 0)
            {
                return BillingVO;
            }
            else
            {
                return null;
            }
        }
        public BillingVO GetByToken(string token,ref string fullname, ref string email)
        {
            var BillingVO = BillingDALImpl.GetBillingByToken(token);

            if (BillingVO != null && BillingVO.ROW_ID != 0)
            {
                string _fullname = "";
                string _email = "";
                BillingDALImpl.GetBillingNameEmail(BillingVO.ROW_ID, ref _fullname , ref _email);
                fullname = _fullname;
                email = _email;
                return BillingVO;
            }
            else
            {
                return null;
            }
        }

        public List<BillingVO> GetBillings(int startIdx, int length, ref int TotalCount, string orderBy = "", string orderDirection = "", BillingVO qFilter = null)
        {
            int Total_Count = 0;

            var BillingVOList = BillingDALImpl.GetBillings(startIdx, length, ref Total_Count, orderBy, orderDirection, qFilter);

            TotalCount = Total_Count;

            return BillingVOList;
        }

        public List<BillingVO> GetBillingsByMemberId(long MemberId, int startIdx, int length, ref int TotalCount, string orderBy = "", string orderDirection = "", BillingVO qFilter = null)
        {
            var UnitIds = UnitDALImpl.GetUnitIdsByUserId(MemberId);

            int Total_Count = 0;

            var BillingVOList = BillingDALImpl.GetBillings(startIdx, length, ref Total_Count, orderBy, orderDirection, qFilter, UnitIds);

            TotalCount = Total_Count;

            return BillingVOList;
        }

        public long CreateBilling(BillingVO BillingVO)
        {
            return BillingDALImpl.CreateBilling(BillingVO);
        }

        public void UpdateBilling(BillingVO BillingVO)
        {
            BillingDALImpl.UpdateBilling(BillingVO);
        }

        public void DeleteBilling(long BillingId, long ByUserId)
        {
            var BillingVO = Get(BillingId);

            BillingVO.STATUS = BILLING_STATUS.Deleted.ToString();
            BillingVO.LAST_UPDATED_BY = ByUserId;

            BillingDALImpl.UpdateBilling(BillingVO);
        }

    }
}
