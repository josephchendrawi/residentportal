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
    public interface ITicketDAL
    {
        TicketVO GetTicket(long TicketId);
        List<TicketVO> GetTickets(int startIdx, int length, ref int TotalCount, string orderBy = "", string orderDirection = "", TicketVO qFilter = null);
        long CreateTicket(TicketVO _newTicket);
        void UpdateTicket(TicketVO _updatedTicket);
    }
}
