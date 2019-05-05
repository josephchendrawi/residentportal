using ResComm.Web.Lib.DB.DAL;
using ResComm.Web.Lib.Interface.Common.Enums;
using ResComm.Web.Lib.Interface.Models.Facility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResComm.Web.Lib.BLL
{
    public class FacilityBLL
    {
        private IFacilityDAL FacilityDALImpl = new FacilityDALImpl();
        private IFacilitySlotDAL FacilitySlotDALImpl = new FacilitySlotDALImpl();

        public FacilityVO GetFacility(long FacilityId)
        {
            var FacilityVO = FacilityDALImpl.GetFacility(FacilityId);
            if (FacilityVO != null && FacilityVO.ROW_ID != 0)
            {
                return FacilityVO;
            }
            else
            {
                return null;
            }
        }

        public List<FacilityVO> GetFacilitiesByProperty(long PropertyId, int startIdx, int length, ref int TotalCount, string orderBy = "", string orderDirection = "", FacilityVO qFilter = null)
        {
            qFilter.ACCNT_ID = PropertyId;

            int Total_Count = 0;

            var FacilityVOList = FacilityDALImpl.GetFacilities(startIdx, length, ref Total_Count, orderBy, orderDirection, qFilter);

            TotalCount = Total_Count;

            return FacilityVOList;
        }

        public long CreateFacility(FacilityVO FacilityVO)
        {
            return FacilityDALImpl.CreateFacility(FacilityVO);
        }

        public void UpdateFacility(FacilityVO FacilityVO)
        {
            FacilityDALImpl.UpdateFacility(FacilityVO);
        }

        public FacilitySlotVO GetFacilitySlot(long FacilitySlotId)
        {
            var FacilitySlotVO = FacilitySlotDALImpl.GetFacilitySlot(FacilitySlotId);
            if (FacilitySlotVO != null && FacilitySlotVO.ROW_ID != 0)
            {
                return FacilitySlotVO;
            }
            else
            {
                return null;
            }
        }

        public List<FacilitySlotVO> GetFacilitySlotsByFacilityId(long FacilityId)
        {
            return FacilitySlotDALImpl.GetFacilitySlotsByFacilityId(FacilityId);
        }

        public List<FacilitySlotVO> GetFacilitySlots(int startIdx, int length, ref int TotalCount, string orderBy = "", string orderDirection = "", FacilitySlotVO qFilter = null)
        {
            int Total_Count = 0;

            var FacilitySlotVOList = FacilitySlotDALImpl.GetFacilitySlots(startIdx, length, ref Total_Count, orderBy, orderDirection, qFilter);

            TotalCount = Total_Count;

            return FacilitySlotVOList;
        }

        public long CreateFacilitySlot(FacilitySlotVO FacilitySlotVO)
        {
            if (GetFacilitySlotsByFacilityId(FacilitySlotVO.FACILITY_ID ?? 0).Where(m => m.START_TIME == FacilitySlotVO.START_TIME && m.END_TIME == FacilitySlotVO.END_TIME && m.STATUS == 1).Count() > 0)
            {
                throw new Exception("This Time Slot already exists.");
            }
            else
            {
                return FacilitySlotDALImpl.CreateFacilitySlot(FacilitySlotVO);
            }
        }

        public void UpdateFacilitySlot(FacilitySlotVO FacilitySlotVO)
        {
            if (GetFacilitySlotsByFacilityId(FacilitySlotVO.FACILITY_ID ?? 0).Where(m => m.ROW_ID != FacilitySlotVO.ROW_ID && m.START_TIME == FacilitySlotVO.START_TIME && m.END_TIME == FacilitySlotVO.END_TIME && m.STATUS == 1).Count() > 0)
            {
                throw new Exception("This Time Slot already exists.");
            }
            else
            {
                FacilitySlotDALImpl.UpdateFacilitySlot(FacilitySlotVO);
            }
        }

        public void DeleteFacilitySlot(long FacilitySlotId, long ByUserId)
        {
            var FacilitySlotVO = GetFacilitySlot(FacilitySlotId);

            FacilitySlotVO.STATUS = 2;
            FacilitySlotVO.LAST_UPDATED_BY = ByUserId;

            FacilitySlotDALImpl.UpdateFacilitySlot(FacilitySlotVO);
        }

    }
}
