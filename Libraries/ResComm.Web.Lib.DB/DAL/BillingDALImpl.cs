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
using ResComm.Web.Lib.Interface.Models.Billing;

namespace ResComm.Web.Lib.DB.DAL
{
    public class BillingDALImpl : IBillingDAL
    {
        IMapper Mapper;
        public BillingDALImpl()
        {
            var AutoMapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<P_BILLING, BillingVO>();
                cfg.CreateMap<BillingVO, P_BILLING>();
            });

            Mapper = AutoMapperConfig.CreateMapper();
        }

        #region MapReturnList
        List<BillingVO> MapReturnList(List<P_BILLING> aList)
        {
            // map
            var returnedResult = new List<BillingVO>();

            foreach (var s in aList)
            {
                var a = Mapper.Map<P_BILLING, BillingVO>(s);

                returnedResult.Add(a);
            }

            return returnedResult;
        }

        #endregion

        #region Get Billing
        public BillingVO GetBilling(long BillingId)
        {
            using (var DBContext = new PropComDbContext())
            {
                var result = from d in DBContext.P_BILLING
                             where d.ROW_ID == BillingId
                             select d;

                if (result.Count() > 0)
                {
                    var P_BILLING = result.First();
                    var BillingVO = Mapper.Map<P_BILLING, BillingVO>(P_BILLING);
                    BillingVO.UnitName = P_BILLING.P_UNIT.P_UNIT_TYPE.CATEGORY_VALUE + "-" + P_BILLING.P_UNIT.UNIT_NO;

                    return BillingVO;
                }
                else
                {
                    return null;
                }
            }
        }
        public BillingVO GetBillingByToken(string token)
        {
            using (var DBContext = new PropComDbContext())
            {
                var result = from d in DBContext.P_BILLING
                             where d.TOKEN == token
                             select d;

                if (result.Count() > 0)
                {
                    var P_BILLING = result.First();
                    var BillingVO = Mapper.Map<P_BILLING, BillingVO>(P_BILLING);
                    BillingVO.UnitName = P_BILLING.P_UNIT.P_UNIT_TYPE.CATEGORY_VALUE + "-" + P_BILLING.P_UNIT.UNIT_NO;

                    return BillingVO;
                }
                else
                {
                    return null;
                }
            }
        }
        public void GetBillingNameEmail(decimal BillingID, ref string Fullname, ref string Email)
        {
            using (var DBContext = new PropComDbContext())
            {
                var result = from a in DBContext.P_BILLING
                             from b in DBContext.P_UNIT
                             from c in DBContext.P_USER
                             where a.UNIT_ID == b.ROW_ID
                             && b.OWNER_ID == c.ROW_ID
                             && a.ROW_ID == BillingID
                             select new {c.USERNAME,c.FIRST_NAME };

                if (result.Count() > 0)
                {
                    var P_BILLING = result.First();
                    Fullname = P_BILLING.FIRST_NAME;
                    Email = P_BILLING.USERNAME;
                }
            }
        }

        public List<BillingVO> GetBillings(int startIdx, int length, ref int TotalCount, string orderBy = "", string orderDirection = "", BillingVO qFilter = null, List<long> UnitIds = null)
        {
            List<BillingVO> result = new List<BillingVO>();
            using (var DBContext = new PropComDbContext())
            {
                var BillingStatus_Deleted = BILLING_STATUS.Deleted.ToString();
                var ett = from d in DBContext.P_BILLING
                          where d.STATUS != BillingStatus_Deleted
                          select d;

                //filtering
                if (qFilter.UNIT_ID != null && qFilter.UNIT_ID != 0)
                {
                    ett = ett.Where(m => m.UNIT_ID == qFilter.UNIT_ID);
                }
                if (qFilter.REFERENCE != null && qFilter.REFERENCE != "")
                    ett = ett.Where(m => m.REFERENCE.ToLower().Contains(qFilter.REFERENCE.ToLower()));
                if (qFilter.CREATED != null)
                {
                    DateTime date = qFilter.CREATED.Value;
                    ett = ett.Where(m => m.CREATED != null);
                    ett = ett.Where(m => m.CREATED.Value.Year == date.Year && m.CREATED.Value.Month == date.Month && m.CREATED.Value.Day == date.Day);
                }
                if (qFilter.STATUS != null && qFilter.STATUS != "")
                    ett = ett.Where(m => m.STATUS.ToLower().Contains(qFilter.STATUS.ToLower()));
                if (qFilter.INVOICE_NO != null && qFilter.INVOICE_NO != "")
                    ett = ett.Where(m => m.INVOICE_NO.ToLower().Contains(qFilter.INVOICE_NO.ToLower()));
                if (qFilter.UnitName != null && qFilter.UnitName != "")
                    ett = ett.Where(m => (m.P_UNIT.P_UNIT_TYPE.CATEGORY_VALUE + "-" + m.P_UNIT.UNIT_NO).ToLower().Contains(qFilter.UnitName.ToLower()));
                if (UnitIds != null && UnitIds.Count() > 0)
                {
                    ett = ett.Where(m => m.UNIT_ID != null && UnitIds.Contains(m.UNIT_ID.Value));
                }

                TotalCount = ett.Count();

                //ordering && paging
                if (orderDirection == "asc")
                {
                    if (orderBy == "InvoiceNo")
                        ett = ett.OrderBy(m => m.INVOICE_NO);
                    else if (orderBy == "BillingDate")
                        ett = ett.OrderBy(m => m.BILLING_DATE);
                    else if (orderBy == "DueDate")
                        ett = ett.OrderBy(m => m.DUE_DATE);
                    else if (orderBy == "Status")
                        ett = ett.OrderBy(m => m.STATUS);
                    else if (orderBy == "Reference")
                        ett = ett.OrderBy(m => m.REFERENCE);
                    else if (orderBy == "UnitName")
                        ett = ett.OrderBy(m => (m.P_UNIT.P_UNIT_TYPE.CATEGORY_VALUE + "-" + m.P_UNIT.UNIT_NO));
                    else
                        ett = ett.OrderBy(m => m.ROW_ID);
                }
                else
                {
                    if (orderBy == "InvoiceNo")
                        ett = ett.OrderByDescending(m => m.INVOICE_NO);
                    else if (orderBy == "BillingDate")
                        ett = ett.OrderByDescending(m => m.BILLING_DATE);
                    else if (orderBy == "DueDate")
                        ett = ett.OrderByDescending(m => m.DUE_DATE);
                    else if (orderBy == "Status")
                        ett = ett.OrderByDescending(m => m.STATUS);
                    else if (orderBy == "Reference")
                        ett = ett.OrderByDescending(m => m.REFERENCE);
                    else if (orderBy == "UnitName")
                        ett = ett.OrderByDescending(m => (m.P_UNIT.P_UNIT_TYPE.CATEGORY_VALUE + "-" + m.P_UNIT.UNIT_NO));
                    else
                        ett = ett.OrderByDescending(m => m.ROW_ID);
                }

                ett = ett.Skip(startIdx).Take(length);

                //mapping
                foreach (var v in ett)
                {
                    var BillingVO = Mapper.Map<P_BILLING, BillingVO>(v);
                    BillingVO.UnitName = v.P_UNIT.P_UNIT_TYPE.CATEGORY_VALUE + "-" + v.P_UNIT.UNIT_NO;

                    result.Add(BillingVO);
                }
            }

            return result;
        }

        #endregion

        #region Create New Billing
        public long CreateBilling(BillingVO _newBilling)
        {
            using (var db = new PropComDbContext())
            {
                P_BILLING P_BILLING = Mapper.Map<BillingVO, P_BILLING>(_newBilling);
                P_BILLING.CREATED = DateTime.UtcNow;
                if (string.IsNullOrWhiteSpace(_newBilling.INVOICE_NO))
                    P_BILLING.INVOICE_NO = DateTime.UtcNow.ToString("yyyyddMM HH:mm:ss");
                db.P_BILLING.Add(P_BILLING);
                db.SaveChanges();
                if (string.IsNullOrWhiteSpace(_newBilling.INVOICE_NO))
                    P_BILLING.INVOICE_NO = "INV-" + DateTime.UtcNow.ToString("yyyyMM") + "-" + P_BILLING.ROW_ID.ToString().PadLeft(5, '0');
                db.SaveChanges();

                P_BILLING.TOKEN = Security.checkHMAC(ResCommConstants.BILLING_SALT, P_BILLING.ROW_ID.ToString());
                db.SaveChanges();

                return P_BILLING.ROW_ID;
            }
        }
        #endregion

        #region Update Billing
        public void UpdateBilling(BillingVO _updatedBilling)
        {
            using (var DBContext = new PropComDbContext())
            {
                var P_BILLING = Mapper.Map<BillingVO, P_BILLING>(_updatedBilling);
                P_BILLING.LAST_UPDATED = DateTime.UtcNow;

                DBContext.P_BILLING.Attach(P_BILLING);
                DBContext.Entry(P_BILLING).State = EntityState.Modified;
                DBContext.SaveChanges();
            }
        }
        #endregion

    }
}
