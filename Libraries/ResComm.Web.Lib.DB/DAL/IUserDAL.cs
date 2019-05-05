using ResComm.Web.Lib.Interface.Common.Enums;
using ResComm.Web.Lib.Interface.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResComm.Web.Lib.DB.DAL
{
    public interface IUserDAL
    {
        UserVO GetUser(string userName, USER_TYPE USER_TYPE);
        UserVO GetUserById(long Id);
        UserVO GetUserByReferral(string ReferralCode);
        RESIDENT_TYPE GetResidentType(long UserId);
        UserVO GetNewUserByToken(string Token);

        List<UserVO> GetMemberUsers(int startIdx, int length, ref int TotalCount, string orderBy = "", string orderDirection = "", UserVO qFilter = null, MEMBER_TYPE MemberType = MEMBER_TYPE.Resident);
        List<UserVO> GetInactiveMemberUsers(int startIdx, int length, ref int TotalCount, string orderBy = "", string orderDirection = "", UserVO qFilter = null);
        List<UserVO> GetUsers(int startIdx, int length, ref int TotalCount, string orderBy = "", string orderDirection = "", UserVO qFilter = null);

        //List<UserVO> GetUsersByProperty(long PropertyId, int startIdx, int length, ref int TotalCount, string orderBy = "", string orderDirection = "", UserVO qFilter = null);

        List<UserVO> GetAllStaff(long groupRowId);
        List<UserVO> GetAllStaff();
        UserVO GetStaff(long rowId);
        bool CheckUsernameExist(UserVO _newUser);


        long CreateUser(UserVO _newUser);
        void UpdateUser(UserVO User);
        void UpdateUserStatus(long UserId, USER_ACCNT_STATUS Status);
        void UpdateUserActiveFlag(long UserId, USER_ACTIVE_FLAG ActiveFlag);

    }
}
