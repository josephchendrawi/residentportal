using ResComm.Web.Lib.Interface.Common.Enums;
using ResComm.Web.Lib.Interface.Models.Facility;
using ResComm.Web.Lib.Interface.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResComm.Web.Lib.DB.DAL
{
    public interface IFacilityDAL
    {
        FacilityVO GetFacility(long FacilityId);
        List<FacilityVO> GetFacilities(int startIdx, int length, ref int TotalCount, string orderBy = "", string orderDirection = "", FacilityVO qFilter = null);
        long CreateFacility(FacilityVO _newFacility);
        void UpdateFacility(FacilityVO _updatedFacility);
    }
}
