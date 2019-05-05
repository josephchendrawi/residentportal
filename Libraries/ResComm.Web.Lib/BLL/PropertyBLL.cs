using ResComm.Web.Lib.DB.DAL;
using ResComm.Web.Lib.Interface.Common.Enums;
using ResComm.Web.Lib.Interface.Models.Property;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResComm.Web.Lib.BLL
{
    public class PropertyBLL
    {
        private IPropertyDAL PropertyDALImpl = new PropertyDALImpl();

        public PropertyVO Get(long PropertyId)
        {
            var PropertyVO = PropertyDALImpl.GetProperty(PropertyId);
            if (PropertyVO != null && PropertyVO.ROW_ID != 0)
            {
                return PropertyVO;
            }
            else
            {
                return null;
            }
        }

        public PropertyVO Get(string PropertyName)
        {
            var PropertyVO = PropertyDALImpl.GetProperty(PropertyName);
            if (PropertyVO != null && PropertyVO.ROW_ID != 0)
            {
                return PropertyVO;
            }
            else
            {
                return null;
            }
        }

        public PropertyVO GetByAccountId(long AccountId)
        {
            var PropertyVO = PropertyDALImpl.GetPropertyByAccountId(AccountId);
            if (PropertyVO != null && PropertyVO.ROW_ID != 0)
            {
                return PropertyVO;
            }
            else
            {
                return null;
            }
        }
        
        public List<PropertyVO> GetAll(int startIdx, int length, ref int TotalCount, string orderBy = "", string orderDirection = "", PropertyVO qFilter = null, long? AffiliateId = null)
        {
            int Total_Count = 0;

            var PropertyVOList = PropertyDALImpl.GetProperties(startIdx, length, ref Total_Count, orderBy, orderDirection, qFilter, AffiliateId);

            TotalCount = Total_Count;

            return PropertyVOList;
        }

        public long CreateProperty(PropertyVO PropertyVO)
        {
            return PropertyDALImpl.CreateProperty(PropertyVO);
        }

        public void UpdateProperty(PropertyVO PropertyVO)
        {
            PropertyDALImpl.UpdateProperty(PropertyVO);
        }

    }
}
