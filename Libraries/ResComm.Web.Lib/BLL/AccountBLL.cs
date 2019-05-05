using ResComm.Web.Lib.DB.DAL;
using ResComm.Web.Lib.Interface.Common.Enums;
using ResComm.Web.Lib.Interface.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResComm.Web.Lib.BLL
{
    public class AccountBLL
    {
        private IAccountDAL AccountDALImpl = new AccountDALImpl();
        private IUserDAL UserDALImpl = new UserDALImpl();

        public CustomerVO Get(long customerId)
        {
            var CustomerVO = AccountDALImpl.GetCustomer(customerId);
            if (CustomerVO != null && CustomerVO.ROW_ID != 0)
            {
                return CustomerVO;
            }
            else
            {
                return null;
            }
        }

        public long CreateAccount(CustomerVO CustomerVO, string REFERRED_BY_AFFILIATE_CODE = "")
        {
            //referred by
            if (!string.IsNullOrWhiteSpace(REFERRED_BY_AFFILIATE_CODE))
            {
                var ReferredBy = UserDALImpl.GetUserByReferral(REFERRED_BY_AFFILIATE_CODE);
                if (ReferredBy != null && ReferredBy.ROW_ID != 0)
                {
                    CustomerVO.REFERRED_BY = ReferredBy.ROW_ID;
                }
                else
                {
                    //throw new Exception("Affiliate Referral is not correct.");
                }
            }

            return long.Parse(AccountDALImpl.CreateAccount(CustomerVO));
        }

        public void UpdateCustomer(CustomerVO CustomerVO)
        {
            AccountDALImpl.UpdateCustomer(CustomerVO);
        }

        public AccountR GetRelation(long UserId)
        {
            return AccountDALImpl.GetAccountRelatedIds(UserId);
        }

    }
}
