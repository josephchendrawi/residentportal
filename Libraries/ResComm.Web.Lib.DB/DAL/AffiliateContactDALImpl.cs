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
using ResComm.Web.Lib.Interface.Models.Affiliate;

namespace ResComm.Web.Lib.DB.DAL
{
    public class AffiliateContactDALImpl
    {
        IMapper Mapper;
        public AffiliateContactDALImpl()
        {
            var AutoMapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<P_AFFLIATE_LEADS, AffiliateContactVO>();
                cfg.CreateMap<AffiliateContactVO, P_AFFLIATE_LEADS>();
            });

            Mapper = AutoMapperConfig.CreateMapper();
        }

        #region MapReturnList
        List<AffiliateContactVO> MapReturnList(List<P_AFFLIATE_LEADS> aList)
        {
            // map
            var returnedResult = new List<AffiliateContactVO>();

            foreach (var s in aList)
            {
                var a = Mapper.Map<P_AFFLIATE_LEADS, AffiliateContactVO>(s);

                returnedResult.Add(a);
            }

            return returnedResult;
        }

        #endregion

        #region Get AffiliateContact
        public AffiliateContactVO GetAffiliateContact(long AffiliateContactId)
        {
            using (var DBContext = new PropComDbContext())
            {
                var result = from d in DBContext.P_AFFLIATE_LEADS
                             where d.ROW_ID == AffiliateContactId
                             select d;

                if (result.Count() > 0)
                {
                    var P_AFFLIATE_LEADS = result.First();
                    var AffiliateContactVO = Mapper.Map<P_AFFLIATE_LEADS, AffiliateContactVO>(P_AFFLIATE_LEADS);

                    return AffiliateContactVO;
                }
                else
                {
                    return null;
                }
            }
        }

        public List<AffiliateContactVO> GetAffiliateContacts(int startIdx, int length, ref int TotalCount, string orderBy = "", string orderDirection = "", AffiliateContactVO qFilter = null)
        {
            List<AffiliateContactVO> result = new List<AffiliateContactVO>();
            using (var DBContext = new PropComDbContext())
            {
                var ett = from d in DBContext.P_AFFLIATE_LEADS
                          select d;

                //filtering
                if (qFilter.NAME != null && qFilter.NAME != "")
                    ett = ett.Where(m => m.NAME.ToLower().Contains(qFilter.NAME.ToLower()));
                if (qFilter.PHONE_NO != null && qFilter.PHONE_NO != "")
                    ett = ett.Where(m => m.PHONE_NO.ToLower().Contains(qFilter.PHONE_NO.ToLower()));
                if (qFilter.CREATED != null)
                {
                    DateTime date = qFilter.CREATED.Value;
                    ett = ett.Where(m => m.CREATED != null);
                    ett = ett.Where(m => m.CREATED.Value.Year == date.Year && m.CREATED.Value.Month == date.Month && m.CREATED.Value.Day == date.Day);
                }
                TotalCount = ett.Count();

                //ordering && paging
                if (orderDirection == "asc")
                {
                    if (orderBy == "Name")
                        ett = ett.OrderBy(m => m.NAME);
                    else if (orderBy == "PhoneNo")
                        ett = ett.OrderBy(m => m.PHONE_NO);
                    else if (orderBy == "CreatedDate")
                        ett = ett.OrderBy(m => m.CREATED);
                        //
                    else
                        ett = ett.OrderBy(m => m.ROW_ID);
                }
                else
                {
                    if (orderBy == "Name")
                        ett = ett.OrderByDescending(m => m.NAME);
                    else if (orderBy == "PhoneNo")
                        ett = ett.OrderByDescending(m => m.PHONE_NO);
                    else if (orderBy == "CreatedDate")
                        ett = ett.OrderByDescending(m => m.CREATED);
                        //
                    else
                        ett = ett.OrderByDescending(m => m.ROW_ID);
                }

                ett = ett.Skip(startIdx).Take(length);

                //mapping
                foreach (var v in ett)
                {
                    result.Add(Mapper.Map<P_AFFLIATE_LEADS, AffiliateContactVO>(v));
                }
            }

            return result;
        }

        #endregion

        #region Create New AffiliateContact
        public long CreateAffiliateContact(AffiliateContactVO _newAffiliateContact)
        {
            using (var db = new PropComDbContext())
            {
                P_AFFLIATE_LEADS P_AFFLIATE_LEADS = Mapper.Map<AffiliateContactVO, P_AFFLIATE_LEADS>(_newAffiliateContact);
                P_AFFLIATE_LEADS.CREATED = DateTime.UtcNow;
                db.P_AFFLIATE_LEADS.Add(P_AFFLIATE_LEADS);
                db.SaveChanges();

                return P_AFFLIATE_LEADS.ROW_ID;
            }
        }
        #endregion

        #region Update AffiliateContact
        public void UpdateAffiliateContact(AffiliateContactVO _updatedAffiliateContact)
        {
            using (var DBContext = new PropComDbContext())
            {
                var P_AFFLIATE_LEADS = Mapper.Map<AffiliateContactVO, P_AFFLIATE_LEADS>(_updatedAffiliateContact);
                P_AFFLIATE_LEADS.LAST_UPDATED = DateTime.UtcNow;

                DBContext.P_AFFLIATE_LEADS.Attach(P_AFFLIATE_LEADS);
                DBContext.Entry(P_AFFLIATE_LEADS).State = EntityState.Modified;
                DBContext.SaveChanges();
            }
        }
        #endregion



    }
}
