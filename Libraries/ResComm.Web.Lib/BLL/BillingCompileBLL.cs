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
    public class BillingCompileBLL
    {
        BillingCompileDALImpl BillingCompileDALImpl = new BillingCompileDALImpl();

        public BillingCompileVO Get(long BillingCompileId)
        {
            var BillingCompileVO = BillingCompileDALImpl.GetBillingCompile(BillingCompileId);

            if (BillingCompileVO != null && BillingCompileVO.ROW_ID != 0)
            {
                return BillingCompileVO;
            }
            else
            {
                return null;
            }
        }

        public List<BillingCompileVO> GetAll(long PropertyId)
        {
            var BillingCompileVOs = BillingCompileDALImpl.GetBillingCompiles(PropertyId);

            return BillingCompileVOs;
        }

        public long CreateBillingCompile(BillingCompileVO BillingCompileVO)
        {
            return BillingCompileDALImpl.CreateBillingCompile(BillingCompileVO);
        }

        public void UpdateBillingCompile(BillingCompileVO BillingCompileVO)
        {
            BillingCompileDALImpl.UpdateBillingCompile(BillingCompileVO);
        }

    }
}
