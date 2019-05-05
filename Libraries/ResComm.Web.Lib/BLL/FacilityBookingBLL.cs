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
    public class FacilityBookingBLL
    {
        private IFacilitySlotBookingDAL FacilitySlotBookingDALImpl = new FacilitySlotBookingDALImpl();

        public FacilitySlotBookingVO GetFacilitySlotBooking(long FacilitySlotBookingId)
        {
            var FacilitySlotBookingVO = FacilitySlotBookingDALImpl.GetFacilitySlotBooking(FacilitySlotBookingId);
            if (FacilitySlotBookingVO != null && FacilitySlotBookingVO.ROW_ID != 0)
            {
                return FacilitySlotBookingVO;
            }
            else
            {
                return null;
            }
        }

        public List<FacilitySlotBookingVO> GetFacilitySlotBookings(int startIdx, int length, ref int TotalCount, string orderBy = "", string orderDirection = "", FacilitySlotBookingVO qFilter = null, long PropertyId = 0)
        {
            int Total_Count = 0;

            var FacilitySlotBookingVOList = FacilitySlotBookingDALImpl.GetFacilitySlotBookings(startIdx, length, ref Total_Count, orderBy, orderDirection, qFilter, PropertyId);

            TotalCount = Total_Count;

            return FacilitySlotBookingVOList;
        }

        public long CreateFacilitySlotBooking(FacilitySlotBookingVO FacilitySlotBookingVO)
        {
            return FacilitySlotBookingDALImpl.CreateFacilitySlotBooking(FacilitySlotBookingVO);
        }

        public void UpdateFacilitySlotBooking(FacilitySlotBookingVO FacilitySlotBookingVO)
        {
            FacilitySlotBookingDALImpl.UpdateFacilitySlotBooking(FacilitySlotBookingVO);
        }
        
    }
}
