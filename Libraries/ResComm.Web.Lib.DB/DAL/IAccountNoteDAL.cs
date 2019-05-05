using ResComm.Web.Lib.Interface.Common.Enums;
using ResComm.Web.Lib.Interface.Models.AccountNote;
using ResComm.Web.Lib.Interface.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResComm.Web.Lib.DB.DAL
{
    public interface IAccountNoteDAL
    {
        AccountNoteVO GetAccountNote(long AccountNoteId);
        List<AccountNoteVO> GetAccountNotes(int startIdx, int length, ref int TotalCount, string orderBy = "", string orderDirection = "", AccountNoteVO qFilter = null);
        long CreateAccountNote(AccountNoteVO _newAccountNote);
        void UpdateAccountNote(AccountNoteVO _updatedAccountNote);
    }
}
