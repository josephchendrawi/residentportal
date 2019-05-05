using ResComm.Web.Lib.Interface.Common.Enums;
using ResComm.Web.Lib.Interface.Models.UnitType;
using ResComm.Web.Lib.Interface.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResComm.Web.Lib.DB.DAL
{
    public interface IUnitTypeDAL
    {
        UnitTypeVO GetUnitType(long UnitTypeId);
        List<UnitTypeVO> GetUnitTypes(int startIdx, int length, ref int TotalCount, string orderBy = "", string orderDirection = "", UnitTypeVO qFilter = null);
        long CreateUnitType(UnitTypeVO _newUnitType);
        void UpdateUnitType(UnitTypeVO _updatedUnitType);
        void UpdateUnitTypeActiveFlag(long UnitTypeId, UNIT_TYPE_ACTIVE_FLAG ActiveFlag);
        void DeleteUnitType(long UnitTypeId);
    }
}
