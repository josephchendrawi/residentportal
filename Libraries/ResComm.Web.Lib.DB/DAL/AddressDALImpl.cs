using AutoMapper;
using ResComm.Web.Lib.Interface.Common.Constants;
using ResComm.Web.Lib.DB.EF;
using ResComm.Web.Lib.Interface.Models;
using ResComm.Web.Lib.Interface.Models.Address;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ResComm.Web.Lib.Interface.Common.Enums;
using System.Data.Entity;

namespace ResComm.Web.Lib.DB.DAL
{
    public class AddressDALImpl : ResCommDALAbstract, IAddressDAL
    {
        IMapper Mapper;
        public AddressDALImpl()
        {
            var AutoMapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AddressVO, P_ADDR>();
                cfg.CreateMap<P_ADDR, AddressVO>();
            });

            Mapper = AutoMapperConfig.CreateMapper();
        }

        public override BaseVO GetDefaultData(long rowId)
        {
            throw new NotImplementedException();
        }

        #region Get Address
        public AddressVO GetAddress(long _addressId)
        {
            AddressVO adVO = new AddressVO();
            var DBContext = new PropComDbContext();
            var address = (from x in DBContext.P_ADDR
                           where x.ROW_ID == _addressId
                           select x).FirstOrDefault();

            if (address == null)
            {
                return null;
            }
            else
            {
                adVO.ROW_ID = address.ROW_ID;
                adVO.ADDR_NAME = address.ADDR_NAME;
                adVO.ADDR_TYPE = address.ADDR_TYPE;
                adVO.ADDR = address.ADDR;
                adVO.ADDR_1 = address.ADDR_1;
                adVO.ADDR_2 = address.ADDR_2;
                adVO.BUILDING_NAME = address.BUILDING_NAME;
                adVO.STREET_TYPE = address.STREET_TYPE;
                adVO.STREET_NAME = address.STREET_NAME;
                adVO.SECTION = address.SECTION;
                adVO.POSTAL_CD = address.POSTAL_CD;
                adVO.CITY = address.CITY;
                adVO.STATE = address.STATE;
                adVO.COUNTRY = address.COUNTRY;
                adVO.INTEGRATION_ID = address.INTEGRATION_ID;
                adVO.SITE_NAME = address.SITE_NAME;
                adVO.STATUS_CD = address.STATUS_CD;
                adVO.APPT_DT = address.APPT_DT;
                return adVO;
            }
        }

        public AddressVO GetAddressByIntegrationId(string _integrationId)
        {
            AddressVO adVO = new AddressVO();
            var DBContext = new PropComDbContext();
            var address = (from x in DBContext.P_ADDR
                           where x.INTEGRATION_ID == _integrationId
                           select x).FirstOrDefault();
            adVO = this.GetAddress(address.ROW_ID);
            return adVO;
        }

        public AddressVO GetAddressByUserId(long _userId)
        {
            AddressVO adVO = new AddressVO();
            var DBContext = new PropComDbContext();
            var user = (from x in DBContext.P_USER
                        where x.ROW_ID == _userId
                        select x).FirstOrDefault();
            adVO = this.GetAddress(user.ADDR_ID ?? 0);
            return adVO;
        }

        #endregion

        #region Create new address
        public long CreateAddress(AddressVO _newAddress)
        {
            long newAddressRowId = 0;
            using (var DBContext = new PropComDbContext())
            {
                P_ADDR adVO = new P_ADDR();
                adVO.ADDR_NAME = Guid.NewGuid().ToString(); // temporary set to guid
                adVO.ADDR_TYPE = _newAddress.ADDR_TYPE;
                adVO.ADDR = _newAddress.ADDR;
                adVO.ADDR_1 = _newAddress.ADDR_1;
                adVO.ADDR_2 = _newAddress.ADDR_2;
                adVO.BUILDING_NAME = _newAddress.BUILDING_NAME;
                adVO.STREET_TYPE = _newAddress.STREET_TYPE;
                adVO.STREET_NAME = _newAddress.STREET_NAME;
                adVO.SECTION = _newAddress.SECTION;
                adVO.POSTAL_CD = _newAddress.POSTAL_CD;
                adVO.CITY = _newAddress.CITY;
                adVO.STATE = _newAddress.STATE;
                adVO.COUNTRY = _newAddress.COUNTRY;
                adVO.INTEGRATION_ID = _newAddress.INTEGRATION_ID;
                adVO.CREATED = DateTime.UtcNow;
                adVO.STATUS_CD = _newAddress.STATUS_CD;
                adVO.SITE_NAME = _newAddress.SITE_NAME;
                adVO.APPT_DT = _newAddress.APPT_DT;
                DBContext.P_ADDR.Add(adVO);
                DBContext.SaveChanges();
                newAddressRowId = adVO.ROW_ID;
            }
            return newAddressRowId;
        }
        #endregion

        #region Update Address
        public void UpdateAddress(AddressVO _updatedAddress)
        {
            if (_updatedAddress.ADDR_NAME == null)
            {
                _updatedAddress.ADDR_NAME = Guid.NewGuid().ToString();
            }

            using (var DBContext = new PropComDbContext())
            {
                var P_ADDR = new P_ADDR
                {
                    ADDR = _updatedAddress.ADDR,
                    ADDR_1 = _updatedAddress.ADDR_1,
                    ADDR_2 = _updatedAddress.ADDR_2,
                    ADDR_NAME = _updatedAddress.ADDR_NAME,
                    ADDR_TYPE = _updatedAddress.ADDR_TYPE,
                    APPT_DT = _updatedAddress.APPT_DT,
                    BUILDING_NAME = _updatedAddress.BUILDING_NAME,
                    CITY = _updatedAddress.CITY,
                    COUNTRY = _updatedAddress.COUNTRY,
                    CREATED = _updatedAddress.CREATED,
                    CREATED_BY = _updatedAddress.CREATED_BY,
                    INTEGRATION_ID = _updatedAddress.INTEGRATION_ID,
                    LAST_UPD = DateTime.UtcNow,
                    LAST_UPD_BY = _updatedAddress.LAST_UPD_BY,
                    POSTAL_CD = _updatedAddress.POSTAL_CD,
                    ROW_ID = _updatedAddress.ROW_ID,
                    SECTION = _updatedAddress.SECTION,
                    SITE_NAME = _updatedAddress.SITE_NAME,
                    STATE = _updatedAddress.STATE,
                    STATUS_CD = _updatedAddress.STATUS_CD,
                    STREET_NAME = _updatedAddress.STREET_NAME,
                    STREET_TYPE = _updatedAddress.STREET_TYPE,
                };

                DBContext.P_ADDR.Attach(P_ADDR);
                DBContext.Entry(P_ADDR).State = EntityState.Modified;
                DBContext.SaveChanges();
            }
        }
        #endregion


    }
}
