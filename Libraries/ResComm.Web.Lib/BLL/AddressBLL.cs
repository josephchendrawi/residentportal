using ResComm.Web.Lib.DB.DAL;
using ResComm.Web.Lib.Interface.Common.Enums;
using ResComm.Web.Lib.Interface.Models.Address;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResComm.Web.Lib.BLL
{
    public class AddressBLL
    {
        private IAddressDAL AddressDALImpl = new AddressDALImpl();

        public AddressVO Get(long addressId)
        {
            var address = AddressDALImpl.GetAddress(addressId);
            if (address != null && address.ROW_ID != 0)
            {
                return address;
            }
            else
            {
                return null;
            }
        }

        public AddressVO GetByUserId(long userId)
        {
            var address = AddressDALImpl.GetAddressByUserId(userId);
            if (address != null && address.ROW_ID != 0)
            {
                return address;
            }
            else
            {
                return null;
            }
        }

        public long CreateAddress(AddressVO AddressVO)
        {
            return AddressDALImpl.CreateAddress(AddressVO);
        }

        public void UpdateAddress(AddressVO AddressVO)
        {
            AddressDALImpl.UpdateAddress(AddressVO);
        }

    }
}
