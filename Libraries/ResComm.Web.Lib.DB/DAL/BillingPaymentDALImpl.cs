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
    public class BillingPaymentDALImpl : IBillingPaymentDAL
    {
        IMapper Mapper;
        public BillingPaymentDALImpl()
        {
            var AutoMapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<P_BILLING_PAYMENT, BillingPaymentVO>();
                cfg.CreateMap<BillingPaymentVO, P_BILLING_PAYMENT>();
            });

            Mapper = AutoMapperConfig.CreateMapper();
        }

        #region MapReturnList
        List<BillingPaymentVO> MapReturnList(List<P_BILLING_PAYMENT> aList)
        {
            // map
            var returnedResult = new List<BillingPaymentVO>();

            foreach (var s in aList)
            {
                var a = Mapper.Map<P_BILLING_PAYMENT, BillingPaymentVO>(s);

                returnedResult.Add(a);
            }

            return returnedResult;
        }

        #endregion

        #region Get Billing Payment
        public BillingPaymentVO GetBillingPayment(long BillingPaymentId)
        {
            using (var DBContext = new PropComDbContext())
            {
                var result = from d in DBContext.P_BILLING_PAYMENT
                             where d.ROW_ID == BillingPaymentId
                             select d;

                if (result.Count() > 0)
                {
                    var P_BILLING_PAYMENT = result.First();
                    var BillingPaymentVO = Mapper.Map<P_BILLING_PAYMENT, BillingPaymentVO>(P_BILLING_PAYMENT);
                    BillingPaymentVO.PAIDNAME = P_BILLING_PAYMENT.P_USER.FIRST_NAME + " " + P_BILLING_PAYMENT.P_USER.LAST_NAME;

                    return BillingPaymentVO;
                }
                else
                {
                    return null;
                }
            }
        }
        public BillingPaymentVO GetBillingPaymentByBillingId(long BillingId)
        {
            using (var DBContext = new PropComDbContext())
            {
                var result = from d in DBContext.P_BILLING_PAYMENT
                             where d.BILLING_ID == BillingId
                             select d;

                if (result.Count() > 0)
                {
                    var P_BILLING_PAYMENT = result.First();
                    var BillingPaymentVO = Mapper.Map<P_BILLING_PAYMENT, BillingPaymentVO>(P_BILLING_PAYMENT);
                    BillingPaymentVO.PAIDNAME = P_BILLING_PAYMENT.P_USER.FIRST_NAME + " " + P_BILLING_PAYMENT.P_USER.LAST_NAME;

                    return BillingPaymentVO;
                }
                else
                {
                    return null;
                }
            }
        }

        public List<BillingPaymentVO> GetBillingPayments(long PropertyId, DateTime? FromDate = null, DateTime? ToDate = null)
        {
            using (var DBContext = new PropComDbContext())
            {
                var result = from d in DBContext.P_BILLING_PAYMENT
                             where d.BILLING_ID != null
                             && d.P_BILLING.P_UNIT.P_UNIT_TYPE.P_PROPERTY.ROW_ID == PropertyId
                             select d;

                if (FromDate != null)
                {
                    result = result.Where(m => m.PAYMENT_DATE >= FromDate);
                }
                if (ToDate != null)
                {
                    ToDate = ToDate.Value.AddDays(1);
                    result = result.Where(m => m.PAYMENT_DATE < ToDate);
                }
                
                return MapReturnList(result.ToList());
            }
        }
        #endregion

        #region Create New Billing Payment
        public long CreateBillingPayment(BillingPaymentVO _newBilling)
        {
            using (var db = new PropComDbContext())
            {
                P_BILLING_PAYMENT P_BILLING_PAYMENT = Mapper.Map<BillingPaymentVO, P_BILLING_PAYMENT>(_newBilling);
                P_BILLING_PAYMENT.CREATED = DateTime.UtcNow;
                db.P_BILLING_PAYMENT.Add(P_BILLING_PAYMENT);
                db.SaveChanges();

                return P_BILLING_PAYMENT.ROW_ID;
            }
        }
        #endregion

        #region Update Billing Payment
        public void UpdateBillingPayment(BillingPaymentVO _updatedBilling)
        {
            using (var DBContext = new PropComDbContext())
            {
                var P_BILLING_PAYMENT = Mapper.Map<BillingPaymentVO, P_BILLING_PAYMENT>(_updatedBilling);
                P_BILLING_PAYMENT.LAST_UPDATED = DateTime.UtcNow;

                DBContext.P_BILLING_PAYMENT.Attach(P_BILLING_PAYMENT);
                DBContext.Entry(P_BILLING_PAYMENT).State = EntityState.Modified;
                DBContext.SaveChanges();
            }
        }
        #endregion

    }
}
