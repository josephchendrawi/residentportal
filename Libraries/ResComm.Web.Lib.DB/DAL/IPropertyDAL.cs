using ResComm.Web.Lib.Interface.Common.Enums;
using ResComm.Web.Lib.Interface.Models.Property;
using ResComm.Web.Lib.Interface.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResComm.Web.Lib.DB.DAL
{
    public interface IPropertyDAL
    {
        PropertyVO GetProperty(long propertyId);
        PropertyVO GetProperty(string propertyName);
        PropertyVO GetPropertyByAccountId(long AccountId);
        List<PropertyVO> GetProperties(int startIdx, int length, ref int TotalCount, string orderBy = "", string orderDirection = "", PropertyVO qFilter = null, long? AffiliateId = null);
        long CreateProperty(PropertyVO _newProperty);
        void UpdateProperty(PropertyVO _updatedProperty);
    }
}
