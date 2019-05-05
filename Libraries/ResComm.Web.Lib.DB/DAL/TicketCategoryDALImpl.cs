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
    public class TicketCategoryDALImpl : ITicketCategoryDAL
    {
        IMapper Mapper;
        public TicketCategoryDALImpl()
        {
            var AutoMapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<P_SRV_CATEGORY, TicketCategoryVO>();
                cfg.CreateMap<TicketCategoryVO, P_SRV_CATEGORY>();
            });

            Mapper = AutoMapperConfig.CreateMapper();
        }

        #region MapReturnList
        List<TicketCategoryVO> MapReturnList(List<P_SRV_CATEGORY> aList)
        {
            // map
            var returnedResult = new List<TicketCategoryVO>();

            foreach (var s in aList)
            {
                var a = Mapper.Map<P_SRV_CATEGORY, TicketCategoryVO>(s);

                returnedResult.Add(a);
            }

            return returnedResult;
        }

        #endregion

        #region Get TicketCategory
        public TicketCategoryVO GetTicketCategory(long TicketCategoryId)
        {
            using (var DBContext = new PropComDbContext())
            {
                var result = from d in DBContext.P_SRV_CATEGORY
                             where d.ROW_ID == TicketCategoryId
                             select d;

                if (result.Count() > 0)
                {
                    var P_SRV_CATEGORY = result.First();
                    var TicketCategoryVO = Mapper.Map<P_SRV_CATEGORY, TicketCategoryVO>(P_SRV_CATEGORY);

                    return TicketCategoryVO;
                }
                else
                {
                    return null;
                }
            }
        }

        public List<TicketCategoryVO> GetTicketCategories(int startIdx, int length, ref int TotalCount, string orderBy = "", string orderDirection = "", TicketCategoryVO qFilter = null)
        {
            List<TicketCategoryVO> result = new List<TicketCategoryVO>();
            using (var DBContext = new PropComDbContext())
            {
                var ett = from d in DBContext.P_SRV_CATEGORY
                          select d;

                //filtering
                if (qFilter.ACCNT_ID != null && qFilter.ACCNT_ID != 0)
                    ett = ett.Where(m => m.ACCNT_ID == qFilter.ACCNT_ID);
                if (qFilter.NAME != null && qFilter.NAME != "")
                    ett = ett.Where(m => m.NAME.ToLower().Contains(qFilter.NAME.ToLower()));
                if (qFilter.DESCRIPTION != null && qFilter.DESCRIPTION != "")
                    ett = ett.Where(m => m.DESCRIPTION.ToLower().Contains(qFilter.DESCRIPTION.ToLower()));

                TotalCount = ett.Count();

                //ordering && paging
                if (orderDirection == "asc")
                {
                    if (orderBy == "Name")
                        ett = ett.OrderBy(m => m.NAME);
                    else if (orderBy == "Description")
                        ett = ett.OrderBy(m => m.DESCRIPTION);
                    else
                        ett = ett.OrderBy(m => m.ROW_ID);
                }
                else
                {
                    if (orderBy == "Name")
                        ett = ett.OrderByDescending(m => m.NAME);
                    else if (orderBy == "Description")
                        ett = ett.OrderByDescending(m => m.DESCRIPTION);
                    else
                        ett = ett.OrderByDescending(m => m.ROW_ID);
                }

                ett = ett.Skip(startIdx).Take(length);

                //mapping
                foreach (var v in ett)
                {
                    result.Add(new TicketCategoryVO
                    {
                        ROW_ID = v.ROW_ID,
                        ACCNT_ID = v.ACCNT_ID,
                        NAME = v.NAME,
                        DESCRIPTION = v.DESCRIPTION,
                        CREATED = v.CREATED,                        
                    });
                }
            }

            return result;
        }

        #endregion

        #region Create New TicketCategory
        public long CreateTicketCategory(TicketCategoryVO _newTicketCategory)
        {
            using (var db = new PropComDbContext())
            {
                P_SRV_CATEGORY P_SRV_CATEGORY = Mapper.Map<TicketCategoryVO, P_SRV_CATEGORY>(_newTicketCategory);
                P_SRV_CATEGORY.CREATED = DateTime.UtcNow;
                db.P_SRV_CATEGORY.Add(P_SRV_CATEGORY);
                db.SaveChanges();

                return P_SRV_CATEGORY.ROW_ID;
            }
        }
        #endregion

        #region Update TicketCategory
        public void UpdateTicketCategory(TicketCategoryVO _updatedTicketCategory)
        {
            using (var DBContext = new PropComDbContext())
            {
                var P_SRV_CATEGORY = Mapper.Map<TicketCategoryVO, P_SRV_CATEGORY>(_updatedTicketCategory);
                P_SRV_CATEGORY.LAST_UPDATED = DateTime.UtcNow;

                DBContext.P_SRV_CATEGORY.Attach(P_SRV_CATEGORY);
                DBContext.Entry(P_SRV_CATEGORY).State = EntityState.Modified;
                DBContext.SaveChanges();
            }
        }
        #endregion



    }
}
