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
    public class BillingCompileDALImpl : IBillingCompileDAL
    {
        IMapper Mapper;
        public BillingCompileDALImpl()
        {
            var AutoMapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<P_BILLING_COMPILE, BillingCompileVO>();
                cfg.CreateMap<BillingCompileVO, P_BILLING_COMPILE>();
            });

            Mapper = AutoMapperConfig.CreateMapper();
        }

        #region MapReturnList
        List<BillingCompileVO> MapReturnList(List<P_BILLING_COMPILE> aList)
        {
            // map
            var returnedResult = new List<BillingCompileVO>();

            foreach (var s in aList)
            {
                var a = Mapper.Map<P_BILLING_COMPILE, BillingCompileVO>(s);

                returnedResult.Add(a);
            }

            return returnedResult;
        }

        #endregion

        #region Get Billing Compile
        public BillingCompileVO GetBillingCompile(long BillingCompileId)
        {
            using (var DBContext = new PropComDbContext())
            {
                var result = from d in DBContext.P_BILLING_COMPILE
                             where d.ROW_ID == BillingCompileId
                             select d;

                if (result.Count() > 0)
                {
                    var P_BILLING_COMPILE = result.First();
                    var BillingCompileVO = Mapper.Map<P_BILLING_COMPILE, BillingCompileVO>(P_BILLING_COMPILE);
                    
                    return BillingCompileVO;
                }
                else
                {
                    return null;
                }
            }
        }

        public List<BillingCompileVO> GetBillingCompiles(long PropertyId)
        {
            using (var DBContext = new PropComDbContext())
            {
                var result = from d in DBContext.P_BILLING_COMPILE
                             where d.PROPERTY_ID == PropertyId
                             select d;
                
                return MapReturnList(result.ToList());
            }
        }
        #endregion

        #region Create New Billing Compile
        public long CreateBillingCompile(BillingCompileVO _newBilling)
        {
            using (var db = new PropComDbContext())
            {
                P_BILLING_COMPILE P_BILLING_COMPILE = Mapper.Map<BillingCompileVO, P_BILLING_COMPILE>(_newBilling);
                P_BILLING_COMPILE.CREATED = DateTime.UtcNow;
                db.P_BILLING_COMPILE.Add(P_BILLING_COMPILE);
                db.SaveChanges();

                return P_BILLING_COMPILE.ROW_ID;
            }
        }
        #endregion

        #region Update Billing Compile
        public void UpdateBillingCompile(BillingCompileVO _updatedBilling)
        {
            using (var DBContext = new PropComDbContext())
            {
                var P_BILLING_COMPILE = Mapper.Map<BillingCompileVO, P_BILLING_COMPILE>(_updatedBilling);
                P_BILLING_COMPILE.LAST_UPDATED = DateTime.UtcNow;

                DBContext.P_BILLING_COMPILE.Attach(P_BILLING_COMPILE);
                DBContext.Entry(P_BILLING_COMPILE).State = EntityState.Modified;
                DBContext.SaveChanges();
            }
        }
        #endregion

    }
}
