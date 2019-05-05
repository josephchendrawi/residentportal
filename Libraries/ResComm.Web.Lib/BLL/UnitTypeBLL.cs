using ResComm.Web.Lib.DB.DAL;
using ResComm.Web.Lib.Interface.Common.Enums;
using ResComm.Web.Lib.Interface.Models.UnitType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResComm.Web.Lib.BLL
{
    public class UnitTypeBLL
    {
        private IUnitTypeDAL UnitTypeDALImpl = new UnitTypeDALImpl();

        public UnitTypeVO Get(long UnitTypeId)
        {
            var UnitTypeVO = UnitTypeDALImpl.GetUnitType(UnitTypeId);
            if (UnitTypeVO != null && UnitTypeVO.ROW_ID != 0)
            {
                return UnitTypeVO;
            }
            else
            {
                return null;
            }
        }

        public List<UnitTypeVO> GetUnitTypesByProperty(long PropertyId, int startIdx, int length, ref int TotalCount, string orderBy = "", string orderDirection = "", UnitTypeVO qFilter = null)
        {
            int Total_Count = 0;

            qFilter.PROPERTY_ID = PropertyId;
            var UnitTypeVOList = UnitTypeDALImpl.GetUnitTypes(startIdx, length, ref Total_Count, orderBy, orderDirection, qFilter);
            
            TotalCount = Total_Count;

            return UnitTypeVOList;
        }

        public long CreateUnitType(UnitTypeVO UnitTypeVO)
        {
            UnitTypeVO.CATEGORY_VALUE = UnitTypeVO.CATEGORY_VALUE.Trim();

            int temp = 0;
            if (GetUnitTypesByProperty(UnitTypeVO.PROPERTY_ID ?? 0, 0, int.MaxValue, ref temp, "", "", new UnitTypeVO()).Where(m => m.CATEGORY_NAME == UnitTypeVO.CATEGORY_NAME && m.CATEGORY_VALUE.ToLower() == UnitTypeVO.CATEGORY_VALUE.ToLower()).Count() > 0)
            {
                throw new Exception("This Street / Block already exists.");
            }
            else
            {
                return UnitTypeDALImpl.CreateUnitType(UnitTypeVO);
            }
        }

        public void UpdateUnitType(UnitTypeVO UnitTypeVO)
        {
            UnitTypeVO.CATEGORY_VALUE = UnitTypeVO.CATEGORY_VALUE.Trim();

            int temp = 0;
            if (GetUnitTypesByProperty(UnitTypeVO.PROPERTY_ID ?? 0, 0, int.MaxValue, ref temp, "", "", new UnitTypeVO()).Where(m => m.ROW_ID != UnitTypeVO.ROW_ID && m.CATEGORY_NAME == UnitTypeVO.CATEGORY_NAME && m.CATEGORY_VALUE.ToLower() == UnitTypeVO.CATEGORY_VALUE.ToLower()).Count() > 0)
            {
                throw new Exception("This Street / Block already exists.");
            }
            else
            {
                UnitTypeDALImpl.UpdateUnitType(UnitTypeVO);
            }
        }

        public void Delete(long UnitTypeId)
        {
            var TotalCount = 0;
            var UnitType = new UnitDALImpl().GetUnits(0, int.MaxValue, ref TotalCount, "", "", new Interface.Models.Unit.UnitVO(){ UNIT_TYPE_ID = UnitTypeId});
            if (TotalCount <= 0)
            {
                UnitTypeDALImpl.DeleteUnitType(UnitTypeId);
            }
            else
            {
                throw new Exception("This Street/Block still has Unit(s) assigned to it. Please delete the Unit(s) first.");
            }
        }

    }
}
