using ResComm.Web.Lib.Interface.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResComm.Web.Lib.DB.DAL
{
    public interface IAccountDAL
    {
        List<CustomerVO> GetChildAccounts(long _customerRowId, string accountType);
        List<CustomerVO> GetCustomersByName(string searchValue);

        List<CustomerVO> GetCustomersByNRIC(string searchValue);

        List<CustomerVO> GetCustomersByServiceNumber(string searchValue);

        List<CustomerVO> GetCustomersByContactName(string searchValue);

        List<CustomerVO> GetCustomersByType(string searchValue);

        List<CustomerVO> GetCustomersByCompanyNo(string searchValue);

        List<CustomerVO> GetCustomersByEmailAddress(string searchValue);

        List<CustomerVO> GetCustomersWithContactByContactEmailAddress(string searchValue);

        List<CustomerVO> GetCustomersWithContactByMobileNo(string searchValue);

        List<CustomerVO> GetCustomersWithContactByBillingAccount(string searchValue);

        List<CustomerVO> GetCustomersWithContactByAccountNo(string searchValue);
        
       // List<CustomerVO> GetCustomersWithContactByOldBillingAccount(string searchValue);

        List<CustomerVO> GetCustomersWithContact();

        List<CustomerVO> GetCustomersWithContactByCompanyName(string searchValue);

        List<CustomerVO> GetCustomersWithContactByPrimaryContactName(string searchValue);

        List<CustomerVO> GetCustomersWithContactByCompanyIdType(string searchValue);

        List<CustomerVO> GetCustomersWithContactByCompanyIdNo(string searchValue);

        List<CustomerVO> GetCustomersWithContactByEmailAddress(string searchValue);
        
        List<CustomerVO> GetCustomers();
        
        string CreateAccount(CustomerVO _newCustomer);

        List<CustomerVO> GetAllBillingAccounts();

        void UpdateCustomer(CustomerVO customerVO);

        List<CustomerVO> GetCustomersByUserId(long userId);

        CustomerVO GetCustomer(long CustomerId);

        AccountR GetAccountRelatedIds(long UserId);

    }
}
