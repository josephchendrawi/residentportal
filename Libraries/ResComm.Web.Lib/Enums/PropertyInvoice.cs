using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResComm.Web.Lib.Enums
{
    public enum PROPERTY_INVOICE_STATUS
    {
        Active = 1,
        Cancelled = 2,
    }
    public enum PROPERTY_INVOICE_PAYMENT_STATUS
    {
        Unpaid = 1,
        Paid = 2,
    }
    public enum PROPERTY_INVOICE_PAYMENT_METHOD
    {
        Cash = 1,
        Cheque = 2,
        BankTransfer = 3,
        CreditCard = 4,
        Others = 5,
    }
}
