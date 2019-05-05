using AutoMapper;
using ResComm.Web.Lib.Interface.Common.Constants;
using ResComm.Web.Lib.DB.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ResComm.Web.Lib.Interface.Common.Enums;
using System.Data.Entity;
using ResComm.Web.Lib.Interface.Models.Ticket;

namespace ResComm.Web.Lib.DB.DAL
{
    public class TicketDALImpl : ITicketDAL
    {
        IMapper Mapper;
        public TicketDALImpl()
        {
            var AutoMapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<P_SRV_TIC, TicketVO>();
                cfg.CreateMap<TicketVO, P_SRV_TIC>();
            });

            Mapper = AutoMapperConfig.CreateMapper();
        }

        #region MapReturnList
        List<TicketVO> MapReturnList(List<P_SRV_TIC> aList)
        {
            // map
            var returnedResult = new List<TicketVO>();

            foreach (var s in aList)
            {
                var a = Mapper.Map<P_SRV_TIC, TicketVO>(s);

                returnedResult.Add(a);
            }

            return returnedResult;
        }

        #endregion

        #region Get Ticket
        public TicketVO GetTicket(long TicketId)
        {
            using (var DBContext = new PropComDbContext())
            {
                var result = from d in DBContext.P_SRV_TIC
                             join e in DBContext.P_USER on d.CASE_OWNER_ID equals e.ROW_ID
                             where d.ROW_ID == TicketId
                             select new { SRV_TIC = d, USER = e };

                if (result.Count() > 0)
                {
                    var P_SRV_TIC = result.First().SRV_TIC;
                    var TicketVO = Mapper.Map<P_SRV_TIC, TicketVO>(P_SRV_TIC);

                    TicketVO.REQUESTER_NAME = result.First().USER.FIRST_NAME;
                    TicketVO.CATEGORY_NAME = result.First().SRV_TIC.P_SRV_CATEGORY.NAME;

                    return TicketVO;
                }
                else
                {
                    return null;
                }
            }
        }

        public List<TicketVO> GetTickets(int startIdx, int length, ref int TotalCount, string orderBy = "", string orderDirection = "", TicketVO qFilter = null)
        {
            List<TicketVO> result = new List<TicketVO>();
            using (var DBContext = new PropComDbContext())
            {
                var ett = from d in DBContext.P_SRV_TIC
                          join e in DBContext.P_USER on d.CASE_OWNER_ID equals e.ROW_ID
                          select new { SRV_TIC = d, USER = e };

                //filtering
                if (qFilter.REQUESTER_NAME != null && qFilter.REQUESTER_NAME != "")
                    ett = ett.Where(m => m.USER.FIRST_NAME.ToLower().Contains(qFilter.REQUESTER_NAME.ToLower()));
                if (qFilter.CREATED != null)
                {
                    DateTime date = qFilter.CREATED.Value;
                    ett = ett.Where(m => m.SRV_TIC.CREATED != null);
                    ett = ett.Where(m => m.SRV_TIC.CREATED.Value.Year == date.Year && m.SRV_TIC.CREATED.Value.Month == date.Month && m.SRV_TIC.CREATED.Value.Day == date.Day);
                }
                if (qFilter.PRIORITY != null && qFilter.PRIORITY != "")
                    ett = ett.Where(m => m.SRV_TIC.PRIORITY.ToLower().Contains(qFilter.PRIORITY.ToLower()));
                if (qFilter.STATUS_CD != null && qFilter.STATUS_CD != "")
                    ett = ett.Where(m => m.SRV_TIC.STATUS_CD.ToLower().Contains(qFilter.STATUS_CD.ToLower()));
                if (qFilter.NAME != null && qFilter.NAME != "")
                    ett = ett.Where(m => m.SRV_TIC.NAME.ToLower().Contains(qFilter.NAME.ToLower()));
                if (qFilter.CASE_OWNER_ID != null && qFilter.CASE_OWNER_ID != 0)
                {
                    var TICKET_TYPE_Public = TICKET_TYPE.Public.ToString();
                    ett = ett.Where(m => m.SRV_TIC.SVC_TYPE == TICKET_TYPE_Public || m.SRV_TIC.CASE_OWNER_ID == qFilter.CASE_OWNER_ID);
                }

                TotalCount = ett.Count();

                //ordering && paging
                if (orderDirection == "asc")
                {
                    if (orderBy == "Requester")
                        ett = ett.OrderBy(m => m.USER.FIRST_NAME);
                    else if (orderBy == "CreatedDate")
                        ett = ett.OrderBy(m => m.SRV_TIC.CREATED);
                    else if (orderBy == "Category")
                        ett = ett.OrderBy(m => m.SRV_TIC.P_SRV_CATEGORY.NAME);
                    else if (orderBy == "Priority")
                        ett = ett.OrderBy(m => m.SRV_TIC.PRIORITY);
                    else if (orderBy == "Status")
                        ett = ett.OrderBy(m => m.SRV_TIC.STATUS_CD);
                    else if (orderBy == "Name")
                        ett = ett.OrderBy(m => m.SRV_TIC.NAME);
                    else
                        ett = ett.OrderBy(m => m.SRV_TIC.ROW_ID);
                }
                else
                {
                    if (orderBy == "Requester")
                        ett = ett.OrderByDescending(m => m.USER.FIRST_NAME);
                    else if (orderBy == "Created")
                        ett = ett.OrderByDescending(m => m.SRV_TIC.CREATED);
                    else if (orderBy == "Category")
                        ett = ett.OrderByDescending(m => m.SRV_TIC.P_SRV_CATEGORY.NAME);
                    else if (orderBy == "Priority")
                        ett = ett.OrderByDescending(m => m.SRV_TIC.PRIORITY);
                    else if (orderBy == "Status")
                        ett = ett.OrderByDescending(m => m.SRV_TIC.STATUS_CD);
                    else if (orderBy == "Name")
                        ett = ett.OrderByDescending(m => m.SRV_TIC.NAME);
                    else
                        ett = ett.OrderByDescending(m => m.SRV_TIC.ROW_ID);
                }

                ett = ett.Skip(startIdx).Take(length);

                //mapping
                foreach (var v in ett)
                {
                    result.Add(new TicketVO
                    {
                        CASE_OWNER_ID = v.USER.ROW_ID,
                        REQUESTER_NAME = v.USER.FIRST_NAME,
                        CREATED = v.SRV_TIC.CREATED,
                        CATEGORY_ID = v.SRV_TIC.CATEGORY_ID,
                        CATEGORY_NAME = v.SRV_TIC.P_SRV_CATEGORY.NAME,
                        PRIORITY = v.SRV_TIC.PRIORITY,
                        STATUS_CD = v.SRV_TIC.STATUS_CD,
                        ROW_ID = v.SRV_TIC.ROW_ID,
                        CASE_DESC = v.SRV_TIC.CASE_DESC,
                        NAME = v.SRV_TIC.NAME,
                        CASE_RESL_DT = v.SRV_TIC.CASE_RESL_DT,
                        CASE_DUE_DT = v.SRV_TIC.CASE_DUE_DT,
                        SVC_TYPE = v.SRV_TIC.SVC_TYPE,
                    });
                }
            }

            return result;
        }

        #endregion

        #region Create New Ticket
        public long CreateTicket(TicketVO _newTicket)
        {
            using (var db = new PropComDbContext())
            {
                P_SRV_TIC P_SRV_TIC = Mapper.Map<TicketVO, P_SRV_TIC>(_newTicket);
                P_SRV_TIC.CREATED = DateTime.UtcNow;
                P_SRV_TIC.SR_NUM = 0;
                db.P_SRV_TIC.Add(P_SRV_TIC);
                db.SaveChanges();
                P_SRV_TIC.SR_NUM = P_SRV_TIC.ROW_ID;
                db.SaveChanges();

                return P_SRV_TIC.ROW_ID;
            }
        }
        #endregion

        #region Update Ticket
        public void UpdateTicket(TicketVO _updatedTicket)
        {
            using (var DBContext = new PropComDbContext())
            {
                var P_SRV_TIC = Mapper.Map<TicketVO, P_SRV_TIC>(_updatedTicket);
                P_SRV_TIC.LAST_UPD = DateTime.UtcNow;

                DBContext.P_SRV_TIC.Attach(P_SRV_TIC);
                DBContext.Entry(P_SRV_TIC).State = EntityState.Modified;
                DBContext.SaveChanges();
            }
        }
        #endregion



    }
}
