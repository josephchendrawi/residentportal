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
    public interface IFacilitySlotBookingDAL
    {
        FacilitySlotBookingVO GetFacilitySlotBooking(long FacilitySlotBookingId);
        List<FacilitySlotBookingVO> GetFacilitySlotBookings(int startIdx, int length, ref int TotalCount, string orderBy = "", string orderDirection = "", FacilitySlotBookingVO qFilter = null, long PropertyId = 0);
        long CreateFacilitySlotBooking(FacilitySlotBookingVO _newFacilitySlotBooking);
        void UpdateFacilitySlotBooking(FacilitySlotBookingVO _updatedFacilitySlotBooking);
    }
}
