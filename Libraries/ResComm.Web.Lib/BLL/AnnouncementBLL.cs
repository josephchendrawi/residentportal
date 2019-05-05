using ResComm.Web.Lib.DB.DAL;
using ResComm.Web.Lib.Interface.Common.Enums;
using ResComm.Web.Lib.Interface.Models.AccountNote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResComm.Web.Lib.BLL
{
    public class AnnouncementBLL
    {
        private IAccountNoteDAL AccountNoteDALImpl = new AccountNoteDALImpl();

        public AccountNoteVO Get(long AccountNoteId)
        {
            var AccountNoteVO = AccountNoteDALImpl.GetAccountNote(AccountNoteId);
            if (AccountNoteVO != null && AccountNoteVO.ROW_ID != 0)
            {
                return AccountNoteVO;
            }
            else
            {
                return null;
            }
        }

        public List<AccountNoteVO> GetAnnouncementByCustomerId(long CustomerId, int startIdx, int length, ref int TotalCount, string orderBy = "", string orderDirection = "", AccountNoteVO qFilter = null)
        {
            int Total_Count = 0;

            qFilter.ACCNT_ID = CustomerId;
            qFilter.ITEM_TYPE = ((int)ACCNT_NOTE_TYPE.Announcement).ToString();
            var AccountNoteVOList = AccountNoteDALImpl.GetAccountNotes(startIdx, length, ref Total_Count, orderBy, orderDirection, qFilter);

            TotalCount = Total_Count;

            return AccountNoteVOList;
        }

        public long CreateAnnouncement(AccountNoteVO AccountNoteVO)
        {
            AccountNoteVO.ITEM_TYPE = ((int)ACCNT_NOTE_TYPE.Announcement).ToString();
            return AccountNoteDALImpl.CreateAccountNote(AccountNoteVO);
        }

        public void UpdateAnnouncement(AccountNoteVO AccountNoteVO)
        {
            AccountNoteDALImpl.UpdateAccountNote(AccountNoteVO);
        }

    }
}
