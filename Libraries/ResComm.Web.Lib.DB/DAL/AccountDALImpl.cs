using ResComm.Web.Lib.DB.EF;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using ResComm.Web.Lib.Interface.Models.Account;
using ResComm.Web.Lib.Interface.Common.Constants;
using AutoMapper;
using ResComm.Web.Lib.Interface.Models.Audit;


namespace ResComm.Web.Lib.DB.DAL
{
    public class AccountDALImpl : IAccountDAL
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private IAuditTrailDAL AuditTrailDAL = new AuditTrailDALImpl();
        IMapper Mapper;
        public AccountDALImpl()
        {
            var AutoMapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<P_ACCNT, CustomerVO>();
                cfg.CreateMap<CustomerVO, P_ACCNT>();
            });

            Mapper = AutoMapperConfig.CreateMapper();
        }

        public string CreateAccount(CustomerVO _newCustomer)
        {
            string customerAccountRowId = string.Empty;
            using (var DBContext = new PropComDbContext())
            {
                P_ACCNT newCustAccount = new P_ACCNT();
                newCustAccount.NAME = _newCustomer.NAME;
                newCustAccount.SALUTATION = _newCustomer.SALUTATION;
                newCustAccount.MOBILE_NO = _newCustomer.MOBILE_NO;
                newCustAccount.CONTACT_NAME = _newCustomer.CONTACT_NAME;
                newCustAccount.CREATED_BY = _newCustomer.CREATED_BY;
                newCustAccount.FAX_NO = _newCustomer.FAX_NO;
                newCustAccount.HOUSE_NO = _newCustomer.HOUSE_NO;
                newCustAccount.OFFICE_NO = _newCustomer.OFFICE_NO;
                newCustAccount.ID_NUM = _newCustomer.ID_NUM;
                newCustAccount.ID_TYPE = _newCustomer.ID_TYPE;
                newCustAccount.EMAIL_ADDR = _newCustomer.EMAIL_ADDR;
                newCustAccount.CUST_TYPE_CD = _newCustomer.CUST_TYPE_CD;
                newCustAccount.ACCNT_TYPE_CD = _newCustomer.ACCNT_TYPE_CD;
                newCustAccount.LOC = _newCustomer.LOC;
                newCustAccount.PAR_ACCNT_ID = _newCustomer.PAR_ACCNT_ID;
                newCustAccount.MASTER_ACCNT_ID = _newCustomer.MASTER_ACCNT_ID;
                newCustAccount.ADDR_ID = _newCustomer.ADDR_ID;
                newCustAccount.ACCNT_STATUS = _newCustomer.ACCNT_STATUS;
                newCustAccount.VIP_FLG = _newCustomer.VIP_FLG;
                newCustAccount.CUST_SINCE = _newCustomer.CUST_SINCE;
                newCustAccount.BILL_COMP_NAME = _newCustomer.BILL_COMP_NAME;
                newCustAccount.SALES_CD = _newCustomer.SALES_CD;
                newCustAccount.INDUSTRIAL_CD = _newCustomer.INDUSTRIAL_CD;
                newCustAccount.WEBSITE_URL = _newCustomer.WEBSITE_URL;
                newCustAccount.CREATED = DateTime.UtcNow;
                newCustAccount.ACCNT_SUB_STATUS = _newCustomer.ACCNT_SUB_STATUS;
                newCustAccount.CUST_USER_ID = _newCustomer.CUST_USER_ID;
                newCustAccount.REFERRED_BY = _newCustomer.REFERRED_BY;
                DBContext.P_ACCNT.Add(newCustAccount);
                DBContext.SaveChanges();
                if (newCustAccount.ACCNT_TYPE_CD == ResCommConstants.ACCNT_TYPE_CD_CUSTOMER)
                {
                    this.UpdateRecordParent(newCustAccount.ROW_ID.ToString());
                }
                customerAccountRowId = newCustAccount.ROW_ID.ToString();

                /* Move to LIB-BLL
                AuditTrailVO auditTrail = new AuditTrailVO();
                auditTrail.CREATED_BY = newCustAccount.CREATED_BY;
                auditTrail.MODULE_NAME = ResCommConstants.MODULE_ACCOUNT;
                auditTrail.AUDIT_ROW_ID = newCustAccount.ROW_ID;
                auditTrail.ACTION_CD = "Create";
                AuditTrailDAL.InsertAuditTrail(auditTrail);
                 * **/
            }
            return customerAccountRowId;
        }


        #region GetCustomersByName
        public List<CustomerVO> GetCustomersByName(string searchValue)
        {
            using (var DBContext = new PropComDbContext())
            {
                var result = from a in DBContext.P_ACCNT
                             where a.NAME.Contains(searchValue) && a.ACCNT_TYPE_CD == ResCommConstants.ACCNT_TYPE_CD_CUSTOMER
                             select a;

                var aList = result.ToList<P_ACCNT>();

                return MapReturnList(aList);
            }
        }
        #endregion

        #region GetCustomersByNRIC - Return any matching ID Number
        public List<CustomerVO> GetCustomersByNRIC(string searchValue)
        {
            using (var DBContext = new PropComDbContext())
            {
                var result = from a in DBContext.P_ACCNT
                             where a.ID_NUM.Contains(searchValue) && a.ACCNT_TYPE_CD == ResCommConstants.ACCNT_TYPE_CD_CUSTOMER
                             select a;

                var aList = result.ToList<P_ACCNT>();

                return MapReturnList(aList);
            }
        }
        #endregion

        #region GetCustomersByServiceNumber
        public List<CustomerVO> GetCustomersByServiceNumber(string searchValue)
        {
            using (var DBContext = new PropComDbContext())
            {

                var result = from a in DBContext.P_ACCNT
                             join asset in DBContext.P_ASSET on a.ROW_ID equals asset.CUST_ID
                             where asset.SERVICE_NUM.Contains(searchValue) && a.ACCNT_TYPE_CD == ResCommConstants.ACCNT_TYPE_CD_CUSTOMER
                             select a;

                var aList = result.ToList<P_ACCNT>();

                return MapReturnList(aList);
            }
        }
        #endregion

        #region GetCustomersWithContactByAccountNo
        public List<CustomerVO> GetCustomersWithContactByAccountNo(string searchValue)
        {
            int rowId;
            if (searchValue.Length == 10)
                int.TryParse(searchValue.Remove(0, 4), out rowId);
            else
                int.TryParse(searchValue, out rowId);

            using (var DBContext = new PropComDbContext())
            {
                var returnedResult = new List<CustomerVO>();

                //where c.DEFAULT_FLG == "Y" && a.ROW_ID == rowId
                var q = from a in DBContext.P_ACCNT
                        join accountContact in DBContext.P_CON_ACCNT on a.ROW_ID equals accountContact.ACCNT_ID into accountContactGroup
                        from b in accountContactGroup.DefaultIfEmpty()
                        join contact in DBContext.P_CONTACT on b.CON_ID equals contact.ROW_ID into contactGroup
                        from c in contactGroup.DefaultIfEmpty()
                        join asset in DBContext.P_ASSET on a.ROW_ID equals asset.CUST_ID into assetGroup
                        from d in assetGroup.DefaultIfEmpty()
                        where a.ROW_ID == rowId
                        select new { a, b, c, d };

                foreach (var k in q.GroupBy(o => o.a.ROW_ID).ToList())
                {
                    var item = k.FirstOrDefault();

                    if (item.a.ACCNT_TYPE_CD != ResCommConstants.ACCNT_TYPE_CD_CUSTOMER)
                        continue;

                    var customerVO = Mapper.Map<P_ACCNT, CustomerVO>(item.a);

                    customerVO.CompanyName = item.a.NAME;
                    customerVO.ContactNo = item.a.OFFICE_NO;
                    if (item.d != null)
                        customerVO.ServiceId = item.d.SVC_AC_ID;
                    if (item.c != null)
                    {
                        customerVO.ContactMobileNo = (k.Any(o => o.c.DEFAULT_FLG == "Y")) ? k.FirstOrDefault(o => o.c.DEFAULT_FLG == "Y").c.MOBILE_NO : "";
                        customerVO.ContactEmailAddress = (k.Any(o => o.c.DEFAULT_FLG == "Y")) ? k.FirstOrDefault(o => o.c.DEFAULT_FLG == "Y").c.EMAIL_ADDR : "";
                        customerVO.CustomerName = (k.Any(o => o.c.DEFAULT_FLG == "Y")) ? k.FirstOrDefault(o => o.c.DEFAULT_FLG == "Y").c.FULL_NAME : "";
                    }

                    returnedResult.Add(customerVO);
                }

                return returnedResult;
            }
        }
        #endregion

        #region GetCustomersWithContact
        public List<CustomerVO> GetCustomersWithContact()
        {
            using (var DBContext = new PropComDbContext())
            {
                var returnedResult = new List<CustomerVO>();

                var q = from a in DBContext.P_ACCNT
                        join accountContact in DBContext.P_CON_ACCNT on a.ROW_ID equals accountContact.ACCNT_ID into accountContactGroup
                        from b in accountContactGroup.DefaultIfEmpty()
                        join contact in DBContext.P_CONTACT on b.CON_ID equals contact.ROW_ID into contactGroup
                        from c in contactGroup.DefaultIfEmpty()
                        join asset in DBContext.P_ASSET on a.ROW_ID equals asset.CUST_ID into assetGroup
                        from d in assetGroup.DefaultIfEmpty()
                        select new { a, b, c, d };

                foreach (var k in q.GroupBy(o => o.a.ROW_ID).ToList())
                {
                    var item = k.FirstOrDefault();

                    if (item.a.ACCNT_TYPE_CD != ResCommConstants.ACCNT_TYPE_CD_CUSTOMER)
                        continue;

                    var customerVO = Mapper.Map<P_ACCNT, CustomerVO>(item.a);

                    customerVO.CompanyName = item.a.NAME;
                    customerVO.ContactNo = item.a.OFFICE_NO;
                    if (item.d != null)
                        customerVO.ServiceId = item.d.SVC_AC_ID;
                    if (item.c != null)
                    {
                        customerVO.ContactMobileNo = (k.Any(o => o.c.DEFAULT_FLG == "Y")) ? k.FirstOrDefault(o => o.c.DEFAULT_FLG == "Y").c.MOBILE_NO : "";
                        customerVO.ContactEmailAddress = (k.Any(o => o.c.DEFAULT_FLG == "Y")) ? k.FirstOrDefault(o => o.c.DEFAULT_FLG == "Y").c.EMAIL_ADDR : "";
                        customerVO.CustomerName = (k.Any(o => o.c.DEFAULT_FLG == "Y")) ? k.FirstOrDefault(o => o.c.DEFAULT_FLG == "Y").c.FULL_NAME : "";
                    }

                    returnedResult.Add(customerVO);
                }

                return returnedResult;
            }
        }
        #endregion

        #region GetCustomersWithContactByCompanyName
        public List<CustomerVO> GetCustomersWithContactByCompanyName(string searchValue)
        {
            using (var DBContext = new PropComDbContext())
            {
                var returnedResult = new List<CustomerVO>();

                var q = from a in DBContext.P_ACCNT
                        join accountContact in DBContext.P_CON_ACCNT on a.ROW_ID equals accountContact.ACCNT_ID into accountContactGroup
                        from b in accountContactGroup.DefaultIfEmpty()
                        join contact in DBContext.P_CONTACT on b.CON_ID equals contact.ROW_ID into contactGroup
                        from c in contactGroup.DefaultIfEmpty()
                        join asset in DBContext.P_ASSET on a.ROW_ID equals asset.CUST_ID into assetGroup
                        from d in assetGroup.DefaultIfEmpty()
                        where a.NAME.Contains(searchValue)
                        select new { a, b, c, d };

                foreach (var k in q.GroupBy(o => o.a.ROW_ID).ToList())
                {
                    var item = k.FirstOrDefault();

                    if (item.a.ACCNT_TYPE_CD != ResCommConstants.ACCNT_TYPE_CD_CUSTOMER)
                        continue;

                    var customerVO = Mapper.Map<P_ACCNT, CustomerVO>(item.a);

                    customerVO.CompanyName = item.a.NAME;
                    customerVO.ContactNo = item.a.OFFICE_NO;
                    if (item.d != null)
                        customerVO.ServiceId = item.d.SVC_AC_ID;
                    if (item.c != null)
                    {
                        customerVO.ContactMobileNo = (k.Any(o => o.c.DEFAULT_FLG == "Y")) ? k.FirstOrDefault(o => o.c.DEFAULT_FLG == "Y").c.MOBILE_NO : "";
                        customerVO.ContactEmailAddress = (k.Any(o => o.c.DEFAULT_FLG == "Y")) ? k.FirstOrDefault(o => o.c.DEFAULT_FLG == "Y").c.EMAIL_ADDR : "";
                        customerVO.CustomerName = (k.Any(o => o.c.DEFAULT_FLG == "Y")) ? k.FirstOrDefault(o => o.c.DEFAULT_FLG == "Y").c.FULL_NAME : "";
                    }

                    returnedResult.Add(customerVO);
                }

                return returnedResult;
            }
        }
        #endregion

        #region GetCustomersWithContactByPrimaryContactName
        public List<CustomerVO> GetCustomersWithContactByPrimaryContactName(string searchValue)
        {
            using (var DBContext = new PropComDbContext())
            {
                var returnedResult = new List<CustomerVO>();

                var q = from a in DBContext.P_ACCNT
                        join accountContact in DBContext.P_CON_ACCNT on a.ROW_ID equals accountContact.ACCNT_ID into accountContactGroup
                        from b in accountContactGroup.DefaultIfEmpty()
                        join contact in DBContext.P_CONTACT on b.CON_ID equals contact.ROW_ID into contactGroup
                        from c in contactGroup.DefaultIfEmpty()
                        join asset in DBContext.P_ASSET on a.ROW_ID equals asset.CUST_ID into assetGroup
                        from d in assetGroup.DefaultIfEmpty()
                        where c.FULL_NAME.Contains(searchValue)
                        select new { a, b, c, d };

                foreach (var k in q.GroupBy(o => o.a.ROW_ID).ToList())
                {
                    var item = k.FirstOrDefault();

                    if (item.a.ACCNT_TYPE_CD != ResCommConstants.ACCNT_TYPE_CD_CUSTOMER)
                        continue;

                    var customerVO = Mapper.Map<P_ACCNT, CustomerVO>(item.a);

                    customerVO.CompanyName = item.a.NAME;
                    customerVO.ContactNo = item.a.OFFICE_NO;
                    if (item.d != null)
                        customerVO.ServiceId = item.d.SVC_AC_ID;
                    if (item.c != null)
                    {
                        customerVO.ContactMobileNo = (k.Any(o => o.c.DEFAULT_FLG == "Y")) ? k.FirstOrDefault(o => o.c.DEFAULT_FLG == "Y").c.MOBILE_NO : "";
                        customerVO.ContactEmailAddress = (k.Any(o => o.c.DEFAULT_FLG == "Y")) ? k.FirstOrDefault(o => o.c.DEFAULT_FLG == "Y").c.EMAIL_ADDR : "";
                        customerVO.CustomerName = (k.Any(o => o.c.DEFAULT_FLG == "Y")) ? k.FirstOrDefault(o => o.c.DEFAULT_FLG == "Y").c.FULL_NAME : "";
                    }

                    returnedResult.Add(customerVO);
                }

                return returnedResult;
            }
        }
        #endregion

        #region GetCustomersWithContactByBillingAccount
        public List<CustomerVO> GetCustomersWithContactByBillingAccount(string searchValue)
        {
            using (var DBContext = new PropComDbContext())
            {
                var returnedResult = new List<CustomerVO>();

                var q = from a in DBContext.P_ACCNT
                        join billAccount in DBContext.P_ACCNT on a.ROW_ID equals billAccount.PAR_ACCNT_ID into billAccountGroup
                        from bill in billAccountGroup.DefaultIfEmpty()
                        join accountContact in DBContext.P_CON_ACCNT on a.ROW_ID equals accountContact.ACCNT_ID into accountContactGroup
                        from b in accountContactGroup.DefaultIfEmpty()
                        join contact in DBContext.P_CONTACT on b.CON_ID equals contact.ROW_ID into contactGroup
                        from c in contactGroup.DefaultIfEmpty()
                        join asset in DBContext.P_ASSET on a.ROW_ID equals asset.CUST_ID into assetGroup
                        from d in assetGroup.DefaultIfEmpty()
                        where bill.BILL_ACCNT_NUM.Contains(searchValue)
                        select new { a, b, c, d };

                foreach (var k in q.GroupBy(o => o.a.ROW_ID).ToList())
                {
                    var item = k.FirstOrDefault();

                    if (item.a.ACCNT_TYPE_CD != ResCommConstants.ACCNT_TYPE_CD_CUSTOMER)
                        continue;

                    var customerVO = Mapper.Map<P_ACCNT, CustomerVO>(item.a);

                    customerVO.CompanyName = item.a.NAME;
                    customerVO.ContactNo = item.a.OFFICE_NO;
                    if (item.d != null)
                        customerVO.ServiceId = item.d.SVC_AC_ID;
                    if (item.c != null)
                    {
                        customerVO.ContactMobileNo = (k.Any(o => o.c.DEFAULT_FLG == "Y")) ? k.FirstOrDefault(o => o.c.DEFAULT_FLG == "Y").c.MOBILE_NO : "";
                        customerVO.ContactEmailAddress = (k.Any(o => o.c.DEFAULT_FLG == "Y")) ? k.FirstOrDefault(o => o.c.DEFAULT_FLG == "Y").c.EMAIL_ADDR : "";
                        customerVO.CustomerName = (k.Any(o => o.c.DEFAULT_FLG == "Y")) ? k.FirstOrDefault(o => o.c.DEFAULT_FLG == "Y").c.FULL_NAME : "";
                    }

                    returnedResult.Add(customerVO);
                }

                return returnedResult;
            }
        }
        #endregion

        #region GetCustomersWithContactByOldBillingAccount
        //public List<CustomerVO> GetCustomersWithContactByOldBillingAccount(string searchValue)
        //{
        //    using (var DBContext = new PropComDbContext())
        //    {
        //        var returnedResult = new List<CustomerVO>();

        //        var q = from a in DBContext.P_ACCNT
        //                join accountContact in DBContext.P_CON_ACCNT on a.ROW_ID equals accountContact.ACCNT_ID into accountContactGroup
        //                from b in accountContactGroup.DefaultIfEmpty()
        //                join contact in DBContext.P_CONTACT on b.CON_ID equals contact.ROW_ID into contactGroup
        //                from c in contactGroup.DefaultIfEmpty()
        //                join asset in DBContext.P_ASSET on a.ROW_ID equals asset.CUST_ID into assetGroup
        //                from d in assetGroup.DefaultIfEmpty()
        //                where a.OLD_CUSTOMER_NUM.Contains(searchValue)
        //                select new { a, b, c, d };

        //        foreach (var k in q.GroupBy(o => o.a.ROW_ID).ToList())
        //        {
        //            var item = k.FirstOrDefault();

        //            if (item.a.ACCNT_TYPE_CD != ResCommConstants.ACCNT_TYPE_CD_CUSTOMER)
        //                continue;

        //            var customerVO = Mapper.Map<P_ACCNT, CustomerVO>(item.a);

        //            customerVO.CompanyName = item.a.NAME;
        //            customerVO.ContactNo = item.a.OFFICE_NO;
        //            if (item.d != null)
        //                customerVO.ServiceId = item.d.SVC_AC_ID;
        //            if (item.c != null)
        //            {
        //                customerVO.ContactMobileNo = (k.Any(o => o.c.DEFAULT_FLG == "Y")) ? k.FirstOrDefault(o => o.c.DEFAULT_FLG == "Y").c.MOBILE_NO : "";
        //                customerVO.ContactEmailAddress = (k.Any(o => o.c.DEFAULT_FLG == "Y")) ? k.FirstOrDefault(o => o.c.DEFAULT_FLG == "Y").c.EMAIL_ADDR : "";
        //                customerVO.CustomerName = (k.Any(o => o.c.DEFAULT_FLG == "Y")) ? k.FirstOrDefault(o => o.c.DEFAULT_FLG == "Y").c.FULL_NAME : "";
        //            }

        //            returnedResult.Add(customerVO);
        //        }

        //        return returnedResult;
        //    }
        //}
        #endregion

        #region GetCustomersWithContactByCompanyIdType
        public List<CustomerVO> GetCustomersWithContactByCompanyIdType(string searchValue)
        {
            using (var DBContext = new PropComDbContext())
            {
                var returnedResult = new List<CustomerVO>();

                var q = from a in DBContext.P_ACCNT
                        join accountContact in DBContext.P_CON_ACCNT on a.ROW_ID equals accountContact.ACCNT_ID into accountContactGroup
                        from b in accountContactGroup.DefaultIfEmpty()
                        join contact in DBContext.P_CONTACT on b.CON_ID equals contact.ROW_ID into contactGroup
                        from c in contactGroup.DefaultIfEmpty()
                        join asset in DBContext.P_ASSET on a.ROW_ID equals asset.CUST_ID into assetGroup
                        from d in assetGroup.DefaultIfEmpty()
                        where c.ID_TYPE.Contains(searchValue)
                        select new { a, b, c, d };

                foreach (var k in q.GroupBy(o => o.a.ROW_ID).ToList())
                {
                    var item = k.FirstOrDefault();

                    if (item.a.ACCNT_TYPE_CD != ResCommConstants.ACCNT_TYPE_CD_CUSTOMER)
                        continue;

                    var customerVO = Mapper.Map<P_ACCNT, CustomerVO>(item.a);

                    customerVO.CompanyName = item.a.NAME;
                    customerVO.ContactNo = item.a.OFFICE_NO;
                    if (item.d != null)
                        customerVO.ServiceId = item.d.SVC_AC_ID;
                    if (item.c != null)
                    {
                        customerVO.ContactMobileNo = (k.Any(o => o.c.DEFAULT_FLG == "Y")) ? k.FirstOrDefault(o => o.c.DEFAULT_FLG == "Y").c.MOBILE_NO : "";
                        customerVO.ContactEmailAddress = (k.Any(o => o.c.DEFAULT_FLG == "Y")) ? k.FirstOrDefault(o => o.c.DEFAULT_FLG == "Y").c.EMAIL_ADDR : "";
                        customerVO.CustomerName = (k.Any(o => o.c.DEFAULT_FLG == "Y")) ? k.FirstOrDefault(o => o.c.DEFAULT_FLG == "Y").c.FULL_NAME : "";
                    }

                    returnedResult.Add(customerVO);
                }

                return returnedResult;
            }
        }
        #endregion

        #region GetCustomersWithContactByCompanyIdNo
        public List<CustomerVO> GetCustomersWithContactByCompanyIdNo(string searchValue)
        {
            using (var DBContext = new PropComDbContext())
            {
                var returnedResult = new List<CustomerVO>();

                var q = from a in DBContext.P_ACCNT
                        join accountContact in DBContext.P_CON_ACCNT on a.ROW_ID equals accountContact.ACCNT_ID into accountContactGroup
                        from b in accountContactGroup.DefaultIfEmpty()
                        join contact in DBContext.P_CONTACT on b.CON_ID equals contact.ROW_ID into contactGroup
                        from c in contactGroup.DefaultIfEmpty()
                        join asset in DBContext.P_ASSET on a.ROW_ID equals asset.CUST_ID into assetGroup
                        from d in assetGroup.DefaultIfEmpty()
                        where a.ID_NUM.Contains(searchValue)
                        select new { a, b, c, d };

                foreach (var k in q.GroupBy(o => o.a.ROW_ID).ToList())
                {
                    var item = k.FirstOrDefault();

                    if (item.a.ACCNT_TYPE_CD != ResCommConstants.ACCNT_TYPE_CD_CUSTOMER)
                        continue;

                    var customerVO = Mapper.Map<P_ACCNT, CustomerVO>(item.a);

                    customerVO.CompanyName = item.a.NAME;
                    customerVO.ContactNo = item.a.OFFICE_NO;
                    if (item.d != null)
                        customerVO.ServiceId = item.d.SVC_AC_ID;
                    if (item.c != null)
                    {
                        customerVO.ContactMobileNo = (k.Any(o => o.c.DEFAULT_FLG == "Y")) ? k.FirstOrDefault(o => o.c.DEFAULT_FLG == "Y").c.MOBILE_NO : "";
                        customerVO.ContactEmailAddress = (k.Any(o => o.c.DEFAULT_FLG == "Y")) ? k.FirstOrDefault(o => o.c.DEFAULT_FLG == "Y").c.EMAIL_ADDR : "";
                        customerVO.CustomerName = (k.Any(o => o.c.DEFAULT_FLG == "Y")) ? k.FirstOrDefault(o => o.c.DEFAULT_FLG == "Y").c.FULL_NAME : "";
                    }

                    returnedResult.Add(customerVO);
                }

                return returnedResult;
            }
        }
        #endregion

        #region GetCustomersWithContactByEmailAddress
        public List<CustomerVO> GetCustomersWithContactByEmailAddress(string searchValue)
        {
            using (var DBContext = new PropComDbContext())
            {
                var returnedResult = new List<CustomerVO>();

                var q = from a in DBContext.P_ACCNT
                        join accountContact in DBContext.P_CON_ACCNT on a.ROW_ID equals accountContact.ACCNT_ID into accountContactGroup
                        from b in accountContactGroup.DefaultIfEmpty()
                        join contact in DBContext.P_CONTACT on b.CON_ID equals contact.ROW_ID into contactGroup
                        from c in contactGroup.DefaultIfEmpty()
                        join asset in DBContext.P_ASSET on a.ROW_ID equals asset.CUST_ID into assetGroup
                        from d in assetGroup.DefaultIfEmpty()
                        where a.EMAIL_ADDR.Contains(searchValue)
                        select new { a, b, c, d };

                foreach (var k in q.GroupBy(o => o.a.ROW_ID).ToList())
                {
                    var item = k.FirstOrDefault();

                    if (item.a.ACCNT_TYPE_CD != ResCommConstants.ACCNT_TYPE_CD_CUSTOMER)
                        continue;

                    var customerVO = Mapper.Map<P_ACCNT, CustomerVO>(item.a);

                    customerVO.CompanyName = item.a.NAME;
                    customerVO.ContactNo = item.a.OFFICE_NO;
                    if (item.d != null)
                        customerVO.ServiceId = item.d.SVC_AC_ID;
                    if (item.c != null)
                    {
                        customerVO.ContactMobileNo = (k.Any(o => o.c.DEFAULT_FLG == "Y")) ? k.FirstOrDefault(o => o.c.DEFAULT_FLG == "Y").c.MOBILE_NO : "";
                        customerVO.ContactEmailAddress = (k.Any(o => o.c.DEFAULT_FLG == "Y")) ? k.FirstOrDefault(o => o.c.DEFAULT_FLG == "Y").c.EMAIL_ADDR : "";
                        customerVO.CustomerName = (k.Any(o => o.c.DEFAULT_FLG == "Y")) ? k.FirstOrDefault(o => o.c.DEFAULT_FLG == "Y").c.FULL_NAME : "";
                    }

                    returnedResult.Add(customerVO);
                }

                return returnedResult;
            }
        }
        #endregion

        #region GetCustomersWithContactByEmailAddress
        public List<CustomerVO> GetCustomersWithContactByContactEmailAddress(string searchValue)
        {
            using (var DBContext = new PropComDbContext())
            {
                var returnedResult = new List<CustomerVO>();

                var q = from a in DBContext.P_ACCNT
                        join accountContact in DBContext.P_CON_ACCNT on a.ROW_ID equals accountContact.ACCNT_ID into accountContactGroup
                        from b in accountContactGroup.DefaultIfEmpty()
                        join contact in DBContext.P_CONTACT on b.CON_ID equals contact.ROW_ID into contactGroup
                        from c in contactGroup.DefaultIfEmpty()
                        join asset in DBContext.P_ASSET on a.ROW_ID equals asset.CUST_ID into assetGroup
                        from d in assetGroup.DefaultIfEmpty()
                        where c.EMAIL_ADDR.Contains(searchValue)
                        select new { a, b, c, d };

                foreach (var k in q.GroupBy(o => o.a.ROW_ID).ToList())
                {
                    var item = k.FirstOrDefault();

                    if (item.a.ACCNT_TYPE_CD != ResCommConstants.ACCNT_TYPE_CD_CUSTOMER)
                        continue;

                    var customerVO = Mapper.Map<P_ACCNT, CustomerVO>(item.a);

                    customerVO.CompanyName = item.a.NAME;
                    customerVO.ContactNo = item.a.OFFICE_NO;
                    if (item.d != null)
                        customerVO.ServiceId = item.d.SVC_AC_ID;
                    if (item.c != null)
                    {
                        customerVO.ContactMobileNo = (k.Any(o => o.c.DEFAULT_FLG == "Y")) ? k.FirstOrDefault(o => o.c.DEFAULT_FLG == "Y").c.MOBILE_NO : "";
                        customerVO.ContactEmailAddress = (k.Any(o => o.c.DEFAULT_FLG == "Y")) ? k.FirstOrDefault(o => o.c.DEFAULT_FLG == "Y").c.EMAIL_ADDR : "";
                        customerVO.CustomerName = (k.Any(o => o.c.DEFAULT_FLG == "Y")) ? k.FirstOrDefault(o => o.c.DEFAULT_FLG == "Y").c.FULL_NAME : "";
                    }

                    returnedResult.Add(customerVO);
                }

                return returnedResult;
            }
        }
        #endregion

        #region GetCustomersWithContactByMobileNo
        public List<CustomerVO> GetCustomersWithContactByMobileNo(string searchValue)
        {
            using (var DBContext = new PropComDbContext())
            {
                var returnedResult = new List<CustomerVO>();

                var q = from a in DBContext.P_ACCNT
                        join accountContact in DBContext.P_CON_ACCNT on a.ROW_ID equals accountContact.ACCNT_ID into accountContactGroup
                        from b in accountContactGroup.DefaultIfEmpty()
                        join contact in DBContext.P_CONTACT on b.CON_ID equals contact.ROW_ID into contactGroup
                        from c in contactGroup.DefaultIfEmpty()
                        join asset in DBContext.P_ASSET on a.ROW_ID equals asset.CUST_ID into assetGroup
                        from d in assetGroup.DefaultIfEmpty()
                        where c.MOBILE_NO.Contains(searchValue)
                        select new { a, b, c, d };

                foreach (var k in q.GroupBy(o => o.a.ROW_ID).ToList())
                {
                    var item = k.FirstOrDefault();

                    if (item.a.ACCNT_TYPE_CD != ResCommConstants.ACCNT_TYPE_CD_CUSTOMER)
                        continue;

                    var customerVO = Mapper.Map<P_ACCNT, CustomerVO>(item.a);

                    customerVO.CompanyName = item.a.NAME;
                    customerVO.ContactNo = item.a.OFFICE_NO;
                    if (item.d != null)
                        customerVO.ServiceId = item.d.SVC_AC_ID;
                    if (item.c != null)
                    {
                        customerVO.ContactMobileNo = (k.Any(o => o.c.DEFAULT_FLG == "Y")) ? k.FirstOrDefault(o => o.c.DEFAULT_FLG == "Y").c.MOBILE_NO : "";
                        customerVO.ContactEmailAddress = (k.Any(o => o.c.DEFAULT_FLG == "Y")) ? k.FirstOrDefault(o => o.c.DEFAULT_FLG == "Y").c.EMAIL_ADDR : "";
                        customerVO.CustomerName = (k.Any(o => o.c.DEFAULT_FLG == "Y")) ? k.FirstOrDefault(o => o.c.DEFAULT_FLG == "Y").c.FULL_NAME : "";
                    }

                    returnedResult.Add(customerVO);
                }

                return returnedResult;
            }
        }
        #endregion

        #region GetCustomersByContactName
        public List<CustomerVO> GetCustomersByContactName(string searchValue)
        {
            using (var DBContext = new PropComDbContext())
            {

                var result = from a in DBContext.P_ACCNT
                             where a.CONTACT_NAME.Contains(searchValue) && a.ACCNT_TYPE_CD == ResCommConstants.ACCNT_TYPE_CD_CUSTOMER
                             select a;

                var aList = result.ToList();

                return MapReturnList(aList);
            }
        }
        #endregion

        #region GetCustomersByType
        public List<CustomerVO> GetCustomersByType(string searchValue)
        {
            using (var DBContext = new PropComDbContext())
            {

                var result = from a in DBContext.P_ACCNT
                             where a.ACCNT_TYPE_CD.Contains(searchValue) && a.ACCNT_TYPE_CD == ResCommConstants.ACCNT_TYPE_CD_CUSTOMER
                             select a;

                var aList = result.ToList();

                return MapReturnList(aList);
            }
        }
        #endregion

        #region GetCustomersByCompanyNo
        public List<CustomerVO> GetCustomersByCompanyNo(string searchValue)
        {
            using (var DBContext = new PropComDbContext())
            {

                var result = from a in DBContext.P_ACCNT
                             where a.ID_NUM.Contains(searchValue) && a.ACCNT_TYPE_CD == ResCommConstants.ACCNT_TYPE_CD_CUSTOMER
                             select a;

                var aList = result.ToList();

                return MapReturnList(aList);
            }
        }
        #endregion

        #region GetCustomersByEmailAddress
        public List<CustomerVO> GetCustomersByEmailAddress(string searchValue)
        {
            using (var DBContext = new PropComDbContext())
            {

                var result = from a in DBContext.P_ACCNT
                             where a.EMAIL_ADDR.Contains(searchValue) && a.ACCNT_TYPE_CD == ResCommConstants.ACCNT_TYPE_CD_CUSTOMER
                             select a;

                var aList = result.ToList();

                return MapReturnList(aList);
            }
        }
        #endregion

        #region GetCustomers
        public List<CustomerVO> GetCustomers()
        {
            using (var DBContext = new PropComDbContext())
            {

                var result = from a in DBContext.P_ACCNT
                             where a.ACCNT_TYPE_CD == ResCommConstants.ACCNT_TYPE_CD_CUSTOMER
                             select a;

                var aList = result.ToList();

                return MapReturnList(aList);
            }
        }
        #endregion

        #region MapReturn
        CustomerVO MapReturn(P_ACCNT accntVO)
        {
            var a = Mapper.Map<P_ACCNT, CustomerVO>(accntVO);

            return a;
        }


        List<CustomerVO> MapReturnList(List<P_ACCNT> aList)
        {
            // map
            var returnedResult = new List<CustomerVO>();

            foreach (var s in aList)
            {
                var a = Mapper.Map<P_ACCNT, CustomerVO>(s);

                returnedResult.Add(a);
            }

            return returnedResult;
        }
        #endregion

        #region GetCustomerByUserId
        public List<CustomerVO> GetCustomersByUserId(long userId)
        {
            using (var DBContext = new PropComDbContext())
            {
                var result = from a in DBContext.P_ACCNT
                             where a.CUST_USER_ID == userId
                             select a;

                var aList = result.ToList<P_ACCNT>();

                return MapReturnList(aList);
            }
        }
        #endregion

        #region GetCustomerByRowId
        public CustomerVO GetCustomer(long CustomerId)
        {
            using (var DBContext = new PropComDbContext())
            {
                var result = from a in DBContext.P_ACCNT
                             where a.ROW_ID == CustomerId
                             select a;

                if (result.Count() > 0)
                {
                    var P_Customer = result.First();
                    var CustomerVO = Mapper.Map<P_ACCNT, CustomerVO>(P_Customer);

                    return CustomerVO;
                }
                else
                {
                    return null;
                }
            }
        }
        #endregion

        #region GetAllBillingAccounts
        public List<CustomerVO> GetAllBillingAccounts()
        {
            using (var DBContext = new PropComDbContext())
            {
                var result = from a in DBContext.P_ACCNT
                             where a.ACCNT_TYPE_CD == ResCommConstants.ACCNT_TYPE_CD_BILLING && a.BILL_ACCNT_NUM != null
                             orderby a.BILL_ACCNT_NUM
                             select a;

                var aList = result.ToList<P_ACCNT>();

                return MapReturnList(aList);
            }
        }
        #endregion

        #region UpdateCustomer
        public void UpdateCustomer(CustomerVO customerVO)
        {
            using (var DBContext = new PropComDbContext())
            {
                var oCustmerVO = DBContext.P_ACCNT.FirstOrDefault(o => o.ROW_ID == customerVO.ROW_ID);
                P_ACCNT oAccount = Mapper.Map<CustomerVO, P_ACCNT>(customerVO, oCustmerVO);
                oAccount.LAST_UPD = DateTime.UtcNow;
                DBContext.ChangeTracker.DetectChanges();
                var objectState = ((IObjectContextAdapter)DBContext).ObjectContext.ObjectStateManager.GetObjectStateEntry(oCustmerVO);
                IEnumerable<string> modifiedProperties = objectState.GetModifiedProperties();

                foreach (var item in modifiedProperties)
                {
                    if (item == "CREATED" || item == "CREATED_BY" || item == "LAST_UPD_BY" || item == "LAST_UPD")
                        continue;

                    AuditTrailVO auditTrail = new AuditTrailVO();
                    auditTrail.CREATED = DateTime.UtcNow;
                    auditTrail.CREATED_BY = customerVO.LAST_UPD_BY;
                    auditTrail.MODULE_NAME = ResCommConstants.MODULE_ACCOUNT;
                    auditTrail.AUDIT_ROW_ID = customerVO.ROW_ID;
                    auditTrail.ACTION_CD = "Update";
                    auditTrail.OLD_VAL = objectState.OriginalValues[item].ToString();
                    auditTrail.NEW_VAL = objectState.CurrentValues[item].ToString();
                    auditTrail.FIELD_NAME = item;
                    AuditTrailDAL.InsertAuditTrail(auditTrail);
                }

                DBContext.SaveChanges();
            }
        }
        #endregion

        public List<CustomerVO> GetChildAccounts(long _customerRowId, string accountType)
        {
            using (var DBContext = new PropComDbContext())
            {
                var result = from a in DBContext.P_ACCNT
                             where a.ACCNT_TYPE_CD == accountType && a.PAR_ACCNT_ID == _customerRowId
                             select a;
                var aList = result.ToList<P_ACCNT>();
                return MapReturnList(aList);
            }
        }

        // Update the main customer record in P_ACCNT with LOC, MasterAccountID, ParentAccountID
        private void UpdateRecordParent(string _loc)
        {
            using (var DBContext = new PropComDbContext())
            {
                long locCode = long.Parse(_loc);
                var oAccount = (from x in DBContext.P_ACCNT
                                where x.ROW_ID == locCode
                                select x).FirstOrDefault();
                oAccount.LOC = locCode.ToString();
                oAccount.MASTER_ACCNT_ID = locCode;
                oAccount.PAR_ACCNT_ID = locCode;
                DBContext.SaveChanges();
            }
        }
        
        public AccountR GetAccountRelatedIds(long UserId)
        {
            using (var db = new PropComDbContext())
            {
                var P_USERs = from d in db.P_USER
                              where d.ROW_ID == UserId
                              select d;

                if (P_USERs.Count() > 0)
                {
                    AccountR AccountR = new AccountR();

                    AccountR.CustomerId = P_USERs.FirstOrDefault().ACCNT_ID ?? 0;

                    var P_ACCNT = (from d in db.P_ACCNT
                                   where d.ROW_ID == AccountR.CustomerId
                                   select d).FirstOrDefault();

                    if (P_ACCNT != null)
                    {
                        AccountR.AddressId = P_ACCNT.ADDR_ID ?? 0;
                        var P_PROPERTY = (from d in db.P_PROPERTY
                                          where d.ACCNT_ID == AccountR.CustomerId
                                          select d).FirstOrDefault();

                        if(P_PROPERTY != null)
                            AccountR.PropertyId = P_PROPERTY.ROW_ID;
                    }
                    else
                    {
                        AccountR.AddressId = 0;
                        AccountR.PropertyId = 0;
                    }

                    return AccountR;
                }
                else
                {
                    return null;
                }
            }
        }

    }
}
