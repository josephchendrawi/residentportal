using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResComm.Web.Lib.Interface.Common.Enums
{
    public enum USER_TYPE
    {
        SuperAdmin = 0,
        Admin = 1,
        Member = 2,
        Affiliate = 3,
    }

    public enum USER_ACCNT_STATUS
    {
        Prospect = 1,
        Active = 2,
        Suspended = 3,
        Inactive = 4,
    }

    public enum USER_ACTIVE_FLAG
    {
        Active = 1,
        Inactive = 2,
    }
    
    /////////
    public enum MEMBER_TYPE
    {
        Owner,
        Resident
    }
    public enum RESIDENT_TYPE
    {
        [Description("Rental Tenant")]
        Tenant,
        [Description("Owner Occupied")]
        OwnerOccupied
    }

    public enum PAYMENT_STATUS
    {
        Trial = 1,
        CreditCard = 2,
        DebitCard = 3,
    }
    
}
