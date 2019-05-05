using ResComm.Web.Lib.DB.DAL;
using ResComm.Web.Lib.Interface.Common.Constants;
using ResComm.Web.Lib.Interface.Common.Enums;
using ResComm.Web.Lib.Interface.Common.Exceptions;
using ResComm.Web.Lib.Interface.Models.User;
using ResComm.Web.Lib.Service;
using ResComm.Web.Lib.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResComm.Web.Lib.BLL
{
    public class UserBLL
    {
        private IUserDAL UserDALImpl = new UserDALImpl();

        private UnitBLL UnitBLL = new UnitBLL();

        public UserVO AdminLogin(string Username, string Password)
        {
            var user = UserDALImpl.GetUser(Username, USER_TYPE.Admin);
            if (user != null && user.ROW_ID != 0
                /*&& Security.checkHMAC(ResCommConstants.USER_PASSWORD_SALT, Password)*/
                && Password == user.PASSWORD
                && user.ACCNT_STATUS == ((int)USER_ACCNT_STATUS.Active).ToString() //status has to be Active
            )
            {
                return user;
            }
            else
            {
                var superadmin = UserDALImpl.GetUser(Username, USER_TYPE.SuperAdmin);
                if (superadmin != null && superadmin.ROW_ID != 0
                    /*&& Security.checkHMAC(ResCommConstants.USER_PASSWORD_SALT, Password)*/
                    && Password == superadmin.PASSWORD
                )
                {
                    return superadmin;
                }
                else
                {
                    return null;
                }
            }
        }
        public UserVO MemberLogin(string Username, string Password)
        {
            var user = UserDALImpl.GetUser(Username, USER_TYPE.Member);
            if (user != null && user.ROW_ID != 0
                /*&& Security.checkHMAC(ResCommConstants.USER_PASSWORD_SALT, Password)*/
                && Password == user.PASSWORD
                && user.ACCNT_STATUS == ((int)USER_ACCNT_STATUS.Active).ToString() //status has to be Active
                //&& UserDALImpl.GetMemberType(user.ROW_ID) != 0
            )
            {
                return user;
            }
            else
            {
                return null;
            }
        }
        public UserVO AffiliateLogin(string Username, string Password)
        {
            var user = UserDALImpl.GetUser(Username, USER_TYPE.Affiliate);
            if (user != null && user.ROW_ID != 0
                /*&& Security.checkHMAC(ResCommConstants.USER_PASSWORD_SALT, Password)*/
                && Password == user.PASSWORD
                && user.ACCNT_STATUS == ((int)USER_ACCNT_STATUS.Active).ToString() //status has to be Active
            )
            {
                return user;
            }
            else
            {
                return null;
            }
        }

        public static bool IsAuthenticated(long UserId)
        {
            IUserDAL UserDALImpl = new UserDALImpl();
            var user = UserDALImpl.GetUserById(UserId);
            if (user != null && user.ROW_ID != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public UserVO IsMemberTokenAuthenticated(string Token)
        {
            var user = UserDALImpl.GetNewUserByToken(Token);
            if (user != null && user.ROW_ID != 0 && user.USER_TYPE == ((int)USER_TYPE.Member).ToString())
            {
                if (user.TOKEN_EXPIRY > DateTime.Now)
                {
                    return user;
                }
                else
                {
                    throw new Exception(Exceptions.TOKEN_EXPIRED);
                }
            }
            else
            {
                throw new Exception(Exceptions.TOKEN_INCORRECT);
            }
        }

        public static UserVO Get(long userId)
        {
            IUserDAL UserDALImpl = new UserDALImpl();
            var user = UserDALImpl.GetUserById(userId);
            if (user != null && user.ROW_ID != 0)
            {
                return user;
            }
            else
            {
                return null;
            }
        }

        public static UserVO Get(string Username, USER_TYPE UserType)
        {
            IUserDAL UserDALImpl = new UserDALImpl();
            var user = UserDALImpl.GetUser(Username, UserType);
            if (user != null && user.ROW_ID != 0)
            {
                return user;
            }
            else
            {
                return null;
            }
        }
        
        public static int GetResidentType(long userId)
        {
            IUserDAL UserDALImpl = new UserDALImpl();
            return (int)UserDALImpl.GetResidentType(userId);
        }

        public List<UserVO> GetMemberUsers(int startIdx, int length, ref int TotalCount, string orderBy = "", string orderDirection = "", UserVO qFilter = null, MEMBER_TYPE MemberType = MEMBER_TYPE.Resident)
        {
            int Total_Count = 0;

            var MemberVOList = UserDALImpl.GetMemberUsers(startIdx, length, ref Total_Count, orderBy, orderDirection, qFilter, MemberType);

            TotalCount = Total_Count;

            return MemberVOList;
        }

        public List<UserVO> GetAllMemberUsers(int startIdx, int length, ref int TotalCount, string orderBy = "", string orderDirection = "", UserVO qFilter = null)
        {
            if (qFilter == null) { qFilter = new UserVO(); }
            qFilter.USER_TYPE = ((int)USER_TYPE.Member).ToString();

            int Total_Count = 0;

            var MemberVOList = UserDALImpl.GetUsers(startIdx, length, ref Total_Count, orderBy, orderDirection, qFilter);

            TotalCount = Total_Count;

            return MemberVOList;
        }

        public List<UserVO> GetInactiveMemberUsers(int startIdx, int length, ref int TotalCount, string orderBy = "", string orderDirection = "", UserVO qFilter = null)
        {
            int Total_Count = 0;

            var MemberVOList = UserDALImpl.GetInactiveMemberUsers(startIdx, length, ref Total_Count, orderBy, orderDirection, qFilter);

            TotalCount = Total_Count;

            return MemberVOList;
        }

        public List<UserVO> GetAffiliateUsers(int startIdx, int length, ref int TotalCount, string orderBy = "", string orderDirection = "", UserVO qFilter = null)
        {
            if (qFilter == null) { qFilter = new UserVO(); }
            qFilter.USER_TYPE = ((int)USER_TYPE.Affiliate).ToString();

            int Total_Count = 0;

            var MemberVOList = UserDALImpl.GetUsers(startIdx, length, ref Total_Count, orderBy, orderDirection, qFilter);

            TotalCount = Total_Count;

            return MemberVOList;
        }
        
        public UserVO GetAffiliate(string ReferralCode)
        {
            var user = UserDALImpl.GetUserByReferral(ReferralCode);
            if (user != null && user.ROW_ID != 0)
            {
                return user;
            }
            else
            {
                return null;
            }
        }

        /*public List<UserVO> GetTenantsByProperty(long PropertyId, int startIdx, int length, ref int TotalCount, string orderBy = "", string orderDirection = "", UserVO qFilter = null)
        {
            int Total_Count = 0;

            qFilter.USER_TYPE = ((int)USER_TYPE.Tenant).ToString();
            var UserVOList = UserDALImpl.GetUsersByProperty(PropertyId, startIdx, length, ref Total_Count, orderBy, orderDirection, qFilter);

            TotalCount = Total_Count;

            return UserVOList;
        }*/

        public void Update(UserVO UserVO, USER_TYPE USER_TYPE)
        {
            var user = UserDALImpl.GetUser(UserVO.USERNAME, USER_TYPE);
            if (user == null || user.ROW_ID == 0
                || (user != null && user.USER_TYPE != UserVO.USER_TYPE) || user.ROW_ID == UserVO.ROW_ID)
            {
                UserDALImpl.UpdateUser(UserVO);
            }
            else
            {
                throw new Exception(Exceptions.USER_ALREADY_EXISTS);
            }
        }

        public void UpdateStatus(long UserId, USER_ACCNT_STATUS Status)
        {
            UserDALImpl.UpdateUserStatus(UserId, Status);
        }

        public long CreateAdmin(UserVO UserVO)
        {
            UserVO.USER_TYPE = ((int)USER_TYPE.Admin).ToString();
            var user = UserDALImpl.GetUser(UserVO.USERNAME, USER_TYPE.Admin);
            if (user == null || user.ROW_ID == 0)
            {
                var NewUserId = UserDALImpl.CreateUser(UserVO);

                return NewUserId;
            }
            else
            {
                throw new Exception(Exceptions.USER_ALREADY_EXISTS);
            }
        }

        public long CreateMember(UserVO UserVO)
        {
            UserVO.USER_TYPE = ((int)USER_TYPE.Member).ToString();
            var user = UserDALImpl.GetUser(UserVO.USERNAME, USER_TYPE.Member);
            if (user == null || user.ROW_ID == 0)
                //|| (user != null && user.USER_TYPE != UserVO.USER_TYPE))
            {
                //token
                UserVO.TOKEN = Security.RandomString(60);
                UserVO.TOKEN_EXPIRY = DateTime.Now.AddDays(ResCommConstants.USER_MEMBER_TOKEN_EXPIRY_DAY);

                var NewUserId = UserDALImpl.CreateUser(UserVO);

                var PropertyName = new PropertyBLL().GetByAccountId(UserVO.ACCNT_ID.Value).NAME;
                EmailService.SendInvitationEmail(UserVO.USERNAME, UserVO.TOKEN, PropertyName, UserVO.SALUTATION, UserVO.FIRST_NAME);

                return NewUserId;
            }
            else
            {
                throw new Exception(Exceptions.USER_ALREADY_EXISTS);
            }
        }

        public long CreateAffiliate(UserVO UserVO)
        {
            UserVO.USER_TYPE = ((int)USER_TYPE.Affiliate).ToString();
            var user = UserDALImpl.GetUser(UserVO.USERNAME, USER_TYPE.Affiliate);
            if (user == null || user.ROW_ID == 0)
            {
                //generate referral code
                UserVO.REFERRAL_CODE = Security.RandomAplhaNumString(8);

                var NewUserId = UserDALImpl.CreateUser(UserVO);

                return NewUserId;
            }
            else
            {
                throw new Exception(Exceptions.USER_ALREADY_EXISTS);
            }
        }

        public void Delete(long UserId)
        {
            UnitBLL.ClearOwnerFromUnit(UserId);
            UnitBLL.ClearTenantFromUnit(UserId);

            UserDALImpl.UpdateUserActiveFlag(UserId, USER_ACTIVE_FLAG.Inactive);
        }

    }
}
