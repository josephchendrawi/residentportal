using ResComm.Web.Lib.Interface.Common.Enums;
using ResComm.Web.Lib.Interface.Models.Unit;
using ResComm.Web.Lib.Interface.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResComm.Web.Lib.DB.DAL
{
    public interface IUnitDAL
    {
        UnitVO GetUnit(long UnitId, long? PropertyId = null);
        List<long> GetUnitIdsByUserId(long UserId);
        List<UnitVO> GetUnits(int startIdx, int length, ref int TotalCount, string orderBy = "", string orderDirection = "", UnitVO qFilter = null);
        long CreateUnit(UnitVO _newUnit);
        void UpdateUnit(UnitVO _updatedUnit);
        void ClearTenantFromUnit(long UserId);
        void ClearOwnerFromUnit(long UserId);
        void UpdateUnitActiveFlag(long UnitId, UNIT_ACTIVE_FLAG ActiveFlag);
        void DeleteUnit(long UnitId);
    }
}
