using ResComm.Web.Lib.DB.DAL;
using ResComm.Web.Lib.Interface.Common.Enums;
using ResComm.Web.Lib.Interface.Models.Ticket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResComm.Web.Lib.BLL
{
    public class TicketBLL
    {
        private ITicketDAL TicketDALImpl = new TicketDALImpl();
        private ITicketCategoryDAL TicketCategoryDALImpl = new TicketCategoryDALImpl();

        public TicketVO Get(long TicketId)
        {
            var TicketVO = TicketDALImpl.GetTicket(TicketId);
            if (TicketVO != null && TicketVO.ROW_ID != 0)
            {
                return TicketVO;
            }
            else
            {
                return null;
            }
        }

        public List<TicketVO> GetTickets(int startIdx, int length, ref int TotalCount, string orderBy = "", string orderDirection = "", TicketVO qFilter = null)
        {
            int Total_Count = 0;

            var TicketVOList = TicketDALImpl.GetTickets(startIdx, length, ref Total_Count, orderBy, orderDirection, qFilter);

            TotalCount = Total_Count;

            return TicketVOList;
        }

        public long CreateTicket(TicketVO TicketVO)
        {
            return TicketDALImpl.CreateTicket(TicketVO);
        }

        public void UpdateTicket(TicketVO TicketVO)
        {
            TicketDALImpl.UpdateTicket(TicketVO);
        }


        public TicketCategoryVO GetTicketCategory(long TicketCategoryId)
        {
            var TicketCategoryVO = TicketCategoryDALImpl.GetTicketCategory(TicketCategoryId);
            if (TicketCategoryVO != null && TicketCategoryVO.ROW_ID != 0)
            {
                return TicketCategoryVO;
            }
            else
            {
                return null;
            }
        }

        public List<TicketCategoryVO> GetTicketCategories(int startIdx, int length, ref int TotalCount, string orderBy = "", string orderDirection = "", TicketCategoryVO qFilter = null)
        {
            int Total_Count = 0;

            var TicketCategoryVOList = TicketCategoryDALImpl.GetTicketCategories(startIdx, length, ref Total_Count, orderBy, orderDirection, qFilter);

            TotalCount = Total_Count;

            return TicketCategoryVOList;
        }

        public long CreateTicketCategory(TicketCategoryVO TicketCategoryVO)
        {
            return TicketCategoryDALImpl.CreateTicketCategory(TicketCategoryVO);
        }

        public void UpdateTicketCategory(TicketCategoryVO TicketCategoryVO)
        {
            TicketCategoryDALImpl.UpdateTicketCategory(TicketCategoryVO);
        }

    }
}
