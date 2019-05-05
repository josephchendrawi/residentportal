using System;
namespace ResComm.Web.Lib.Interface.Models.Account
{
    public partial class CustomerVO
    {
        public long ROW_ID { get; set; }

        public long? CREATED_BY { get; set; }

        public DateTime? CREATED { get; set; }

        public long? LAST_UPD_BY { get; set; }

        public DateTime? LAST_UPD { get; set; }

        public string NAME { get; set; }

        public string LOC { get; set; }

        public DateTime? BIRTH_DT { get; set; }

        public string SALUTATION { get; set; }

        public string NATIONALITY { get; set; }

        public string ID_TYPE { get; set; }

        public string ID_NUM { get; set; }

        public string CUST_TYPE_CD { get; set; }

        public string ACCNT_TYPE_CD { get; set; }

        public string INTEGRATION_ID { get; set; }

        public string CUSTOMER_NUM { get; set; }

        public string ACCNT_STATUS { get; set; }

        public long? ADDR_ID { get; set; }

        public string EMAIL_ADDR { get; set; }

        public string MOBILE_NO { get; set; }

        public string HOUSE_NO { get; set; }

        public string OFFICE_NO { get; set; }

        public string FAX_NO { get; set; }

        public string BANK_NAME { get; set; }

        public DateTime? CUST_SINCE { get; set; }

        public long? PAR_ACCNT_ID { get; set; }

        public long? MASTER_ACCNT_ID { get; set; }

        public string CONTACT_NAME { get; set; }

        public string BUSINESS_TYPE { get; set; }

        public string WEBSITE_URL { get; set; }

        public int? EMP_NO { get; set; }

        public string VIP_FLG { get; set; }

        public string BILL_ACCNT_NUM { get; set; }

        public string BILL_ACCNT_NAME { get; set; }

        public string BILL_COMP_NAME { get; set; }

        public string BILL_CYCLE { get; set; }

        public string BILL_CURR { get; set; }

        public string PAYMENT_METHOD { get; set; }

        public string SALES_CD { get; set; }

        public string INDUSTRIAL_CD { get; set; }

        public long? CUST_USER_ID { get; set; }

        public string ACCNT_SUB_STATUS { get; set; }

        public long? REFERRED_BY { get; set; }
        
        public string CompanyName {get;set;}
        public string ContactNo {get;set;}
        public long? ServiceId {get;set;}
        public string ContactMobileNo {get;set;}
        public string ContactEmailAddress {get;set;}
        public string CustomerName {get;set;}

    }
    
    public class AccountR
    {
        public long UserId { get; set; }
        public long AddressId { get; set; }
        public long CustomerId { get; set; }
        public long PropertyId { get; set; }
    }

}
