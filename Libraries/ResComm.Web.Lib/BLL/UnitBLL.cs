using ResComm.Web.Lib.DB.DAL;
using ResComm.Web.Lib.Interface.Common.Enums;
using ResComm.Web.Lib.Interface.Models.Unit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResComm.Web.Lib.BLL
{
    public class UnitBLL
    {
        private IUnitDAL UnitDALImpl = new UnitDALImpl();
        private IUnitTypeDAL UnitTypeDALImpl = new UnitTypeDALImpl();

        public UnitVO Get(long UnitId, long? PropertyId = null)
        {
            var UnitVO = UnitDALImpl.GetUnit(UnitId, PropertyId);
            if (UnitVO != null && UnitVO.ROW_ID != 0)
            {
                UnitVO.UnitType = UnitTypeDALImpl.GetUnitType(UnitVO.UNIT_TYPE_ID.Value);
                return UnitVO;
            }
            else
            {
                return null;
            }
        }

        public List<long> GetUnitIdsByUserId(long UserId)
        {
            var UnitIds = UnitDALImpl.GetUnitIdsByUserId(UserId);

            return UnitIds;
        }

        public List<UnitVO> GetUnitsByProperty(long PropertyId, int startIdx, int length, ref int TotalCount, string orderBy = "", string orderDirection = "", UnitVO qFilter = null)
        {
            int Total_Count = 0;

            qFilter.PROPERTY_ID = PropertyId;
            var UnitVOList = UnitDALImpl.GetUnits(startIdx, length, ref Total_Count, orderBy, orderDirection, qFilter);

            foreach (var v in UnitVOList)
            {
                v.UnitType = UnitTypeDALImpl.GetUnitType(v.UNIT_TYPE_ID.Value);
            }

            TotalCount = Total_Count;

            return UnitVOList;
        }

        public long CreateUnit(UnitVO VO)
        {
            VO.UNIT_NO = VO.UNIT_NO.Trim();

            int temp = 0;
            if (UnitDALImpl.GetUnits(0, int.MaxValue, ref temp, "", "", new UnitVO() { UNIT_TYPE_ID = VO.UNIT_TYPE_ID }).Where(m => m.UNIT_NO.ToLower() == VO.UNIT_NO.ToLower()).Count() > 0)
            {
                throw new Exception("This Unit No. already exists.");
            }
            else
            {
                return UnitDALImpl.CreateUnit(VO);
            }
        }

        public void UpdateUnit(UnitVO VO)
        {
            VO.UNIT_NO = VO.UNIT_NO.Trim();

            int temp = 0;
            if (UnitDALImpl.GetUnits(0, int.MaxValue, ref temp, "", "", new UnitVO() { UNIT_TYPE_ID = VO.UNIT_TYPE_ID }).Where(m => m.ROW_ID != VO.ROW_ID && m.UNIT_NO.ToLower() == VO.UNIT_NO.ToLower()).Count() > 0)
            {
                throw new Exception("This Unit No. already exists.");
            }
            else
            {
                UnitDALImpl.UpdateUnit(VO);
            }
        }

        public void ClearTenantFromUnit(long UserId)
        {
            UnitDALImpl.ClearTenantFromUnit(UserId);
        }
        public void ClearOwnerFromUnit(long UserId)
        {
            UnitDALImpl.ClearOwnerFromUnit(UserId);
        }

        public void Delete(long UnitId)
        {
            var Unit = Get(UnitId);
            if (Unit.OWNER_ID == null && Unit.TENANT_ID == null)
            {
                UnitDALImpl.DeleteUnit(UnitId);
            }
            else
            {
                throw new Exception("This Unit still has Tenant or Owner assigned to it. Please clear them from Unit first.");
            }
        }

    }
}
