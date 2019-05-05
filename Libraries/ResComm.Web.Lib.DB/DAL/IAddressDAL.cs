using ResComm.Web.Lib.Interface.Common.Enums;
using ResComm.Web.Lib.Interface.Models.Address;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResComm.Web.Lib.DB.DAL
{
    public interface IAddressDAL
    {
        AddressVO GetAddress(long _addressId);

        long CreateAddress(AddressVO _newAddress);

        AddressVO GetAddressByIntegrationId(string _integrationId);

        void UpdateAddress(AddressVO _updatedAddress);

        AddressVO GetAddressByUserId(long _userId);
    }
}
