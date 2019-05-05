using ResComm.Web.Lib.Interface.Common.Enums;
using ResComm.Web.Lib.Interface.Models.Ticket;
using ResComm.Web.Lib.Interface.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResComm.Web.Lib.DB.DAL
{
    public interface ITicketCategoryDAL
    {
        TicketCategoryVO GetTicketCategory(long TicketCategoryId);
        List<TicketCategoryVO> GetTicketCategories(int startIdx, int length, ref int TotalCount, string orderBy = "", string orderDirection = "", TicketCategoryVO qFilter = null);
        long CreateTicketCategory(TicketCategoryVO _newTicketCategory);
        void UpdateTicketCategory(TicketCategoryVO _updatedTicketCategory);
    }
}
