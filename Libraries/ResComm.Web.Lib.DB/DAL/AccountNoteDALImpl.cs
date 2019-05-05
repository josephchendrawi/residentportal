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
using ResComm.Web.Lib.Interface.Models.AccountNote;

namespace ResComm.Web.Lib.DB.DAL
{
    public class AccountNoteDALImpl : IAccountNoteDAL
    {
        IMapper Mapper;
        public AccountNoteDALImpl()
        {
            var AutoMapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<P_ACCNT_NOTE, AccountNoteVO>();
                cfg.CreateMap<AccountNoteVO, P_ACCNT_NOTE>();
            });

            Mapper = AutoMapperConfig.CreateMapper();
        }

        #region MapReturnList
        List<AccountNoteVO> MapReturnList(List<P_ACCNT_NOTE> aList)
        {
            // map
            var returnedResult = new List<AccountNoteVO>();

            foreach (var s in aList)
            {
                var a = Mapper.Map<P_ACCNT_NOTE, AccountNoteVO>(s);

                returnedResult.Add(a);
            }

            return returnedResult;
        }

        #endregion

        #region Get AccountNote
        public AccountNoteVO GetAccountNote(long AccountNoteId)
        {
            using (var DBContext = new PropComDbContext())
            {
                var result = from a in DBContext.P_ACCNT_NOTE
                             where a.ROW_ID == AccountNoteId
                             select a;

                if (result.Count() > 0)
                {
                    var P_ACCNT_NOTE = result.First();
                    var AccountNoteVO = Mapper.Map<P_ACCNT_NOTE, AccountNoteVO>(P_ACCNT_NOTE);

                    return AccountNoteVO;
                }
                else
                {
                    return null;
                }
            }
        }

        public List<AccountNoteVO> GetAccountNotes(int startIdx, int length, ref int TotalCount, string orderBy = "", string orderDirection = "", AccountNoteVO qFilter = null)
        {
            List<AccountNoteVO> result = new List<AccountNoteVO>();
            using (var DBContext = new PropComDbContext())
            {
                var ett = from d in DBContext.P_ACCNT_NOTE
                          join e in DBContext.P_USER on d.CREATED_BY equals e.ROW_ID
                          join f in DBContext.P_USER on d.LAST_UPD_BY equals f.ROW_ID into joined
                          from g in joined.DefaultIfEmpty()
                          select new { ACCNT_NOTE = d, CREATED_BY = e.FIRST_NAME, LAST_UPDATED_USER = g == null ? "" : g.FIRST_NAME };

                if (qFilter.ACCNT_ID != null && qFilter.ACCNT_ID != 0)
                {
                    ett = ett.Where(m => m.ACCNT_NOTE.ACCNT_ID == qFilter.ACCNT_ID);
                }

                //filtering
                if (qFilter.ITEM_TYPE != null && qFilter.ITEM_TYPE != "")
                    ett = ett.Where(m => m.ACCNT_NOTE.ITEM_TYPE.ToLower().Contains(qFilter.ITEM_TYPE.ToLower()));
                if (qFilter.NOTE != null && qFilter.NOTE != "")
                    ett = ett.Where(m => m.ACCNT_NOTE.NOTE.ToLower().Contains(qFilter.NOTE.ToLower()));
                if (qFilter.NAME != null && qFilter.NAME != "")
                    ett = ett.Where(m => m.ACCNT_NOTE.NAME.ToLower().Contains(qFilter.NAME.ToLower()));

                TotalCount = ett.Count();

                //ordering && paging
                if (orderDirection == "asc")
                {
                    if (orderBy == "Type")
                        ett = ett.OrderBy(m => m.ACCNT_NOTE.ITEM_TYPE);
                    else if (orderBy == "Account")
                        ett = ett.OrderBy(m => m.ACCNT_NOTE.ACCNT_ID);
                    else if (orderBy == "Name")
                        ett = ett.OrderBy(m => m.ACCNT_NOTE.NAME);
                    else if (orderBy == "CreatedBy")
                        ett = ett.OrderBy(m => m.CREATED_BY);
                    else if (orderBy == "LastUpdatedBy")
                        ett = ett.OrderBy(m => m.LAST_UPDATED_USER);
                    else
                        ett = ett.OrderBy(m => m.ACCNT_NOTE.ROW_ID);
                }
                else
                {
                    if (orderBy == "Type")
                        ett = ett.OrderByDescending(m => m.ACCNT_NOTE.ITEM_TYPE);
                    else if (orderBy == "Account")
                        ett = ett.OrderByDescending(m => m.ACCNT_NOTE.ACCNT_ID);
                    else if (orderBy == "Name")
                        ett = ett.OrderByDescending(m => m.ACCNT_NOTE.NAME);
                    else if (orderBy == "CreatedBy")
                        ett = ett.OrderByDescending(m => m.CREATED_BY);
                    else if (orderBy == "LastUpdatedBy")
                        ett = ett.OrderByDescending(m => m.LAST_UPDATED_USER);
                    else
                        ett = ett.OrderByDescending(m => m.ACCNT_NOTE.ROW_ID);
                }

                ett = ett.Skip(startIdx).Take(length);

                //mapping
                foreach (var v in ett)
                {
                    var AccountNoteVO = Mapper.Map<P_ACCNT_NOTE, AccountNoteVO>(v.ACCNT_NOTE);

                    AccountNoteVO.CreatedBy = v.CREATED_BY;
                    AccountNoteVO.LastUpdatedBy = v.LAST_UPDATED_USER;

                    result.Add(AccountNoteVO);
                }
            }

            return result;
        }

        #endregion

        #region Create New AccountNote
        public long CreateAccountNote(AccountNoteVO _newAccountNote)
        {
            using (var db = new PropComDbContext())
            {
                P_ACCNT_NOTE P_ACCNT_NOTE = Mapper.Map<AccountNoteVO, P_ACCNT_NOTE>(_newAccountNote);
                P_ACCNT_NOTE.CREATED = DateTime.UtcNow;
                db.P_ACCNT_NOTE.Add(P_ACCNT_NOTE);
                db.SaveChanges();

                return P_ACCNT_NOTE.ROW_ID;
            }
        }
        #endregion

        #region Update AccountNote
        public void UpdateAccountNote(AccountNoteVO _updatedAccountNote)
        {
            using (var DBContext = new PropComDbContext())
            {
                var P_ACCNT_NOTE = Mapper.Map<AccountNoteVO, P_ACCNT_NOTE>(_updatedAccountNote);
                P_ACCNT_NOTE.LAST_UPD = DateTime.UtcNow;

                DBContext.P_ACCNT_NOTE.Attach(P_ACCNT_NOTE);
                DBContext.Entry(P_ACCNT_NOTE).State = EntityState.Modified;
                DBContext.SaveChanges();
            }
        }
        #endregion



    }
}
