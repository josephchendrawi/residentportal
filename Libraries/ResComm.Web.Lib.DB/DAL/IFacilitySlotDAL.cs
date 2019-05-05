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
    public interface IFacilitySlotDAL
    {
        FacilitySlotVO GetFacilitySlot(long FacilitySlotId);
        List<FacilitySlotVO> GetFacilitySlotsByFacilityId(long FacilityId);
        List<FacilitySlotVO> GetFacilitySlots(int startIdx, int length, ref int TotalCount, string orderBy = "", string orderDirection = "", FacilitySlotVO qFilter = null);
        long CreateFacilitySlot(FacilitySlotVO _newFacilitySlot);
        void UpdateFacilitySlot(FacilitySlotVO _updatedFacilitySlot);
    }
}
