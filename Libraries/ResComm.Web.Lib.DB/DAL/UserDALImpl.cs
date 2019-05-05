using AutoMapper;
using ResComm.Web.Lib.Interface.Common.Constants;
using ResComm.Web.Lib.DB.EF;
using ResComm.Web.Lib.Interface.Models;
using ResComm.Web.Lib.Interface.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ResComm.Web.Lib.Interface.Common.Enums;
using System.Data.Entity;
using ResComm.Web.Lib.Interface.Models.Unit;

namespace ResComm.Web.Lib.DB.DAL
{
    public class UserDALImpl : IUserDAL
    {
        IMapper Mapper;
        public UserDALImpl()
        {
            var AutoMapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<P_USER, UserVO>();
                cfg.CreateMap<UserVO, P_USER>().ForMember(t => t.P_UNIT, opt => opt.Ignore()).ForMember(t => t.P_UNIT1, opt => opt.Ignore());
            });

            Mapper = AutoMapperConfig.CreateMapper();
        }

        public IUnitDAL UnitDALImpl = new UnitDALImpl();
        public IUnitTypeDAL UnitTypeDALImpl = new UnitTypeDALImpl();

        #region MapReturnList
        List<UserVO> MapReturnList(List<P_USER> aList)
        {
            // map
            var returnedResult = new List<UserVO>();

            foreach (var s in aList)
            {
                var a = Mapper.Map<P_USER, UserVO>(s);

                returnedResult.Add(a);
            }

            return returnedResult;
        }

        #endregion

        #region GetUser
        public UserVO GetUser(string userName, USER_TYPE USER_TYPE)
        {
            using (var DBContext = new PropComDbContext())
            {
                var userVO = new UserVO();

                string USER_TYPE_string = ((int)USER_TYPE).ToString();
                var s = from a in DBContext.P_USER
                        where a.ACTIVE_FLG == "1"
                        && a.USERNAME == userName
                        && a.USER_TYPE == USER_TYPE_string
                        select a;

                if (s.Count() > 0)
                {
                    var sUser = s.First();
                    userVO = Mapper.Map<P_USER, UserVO>(sUser);
                }
                return userVO;
            }
        }

        public UserVO GetUserById(long Id)
        {
            using (var dbContext = new PropComDbContext())
            {
                var userVO = new UserVO();
                var s = from a in dbContext.P_USER
                        where a.ROW_ID == Id && a.ACTIVE_FLG == "1"
                        select a;
                if (s.Count() > 0)
                {
                    var sUser = s.First();
                    userVO = Mapper.Map<P_USER, UserVO>(sUser);
                }
                return userVO;
            }
        }
        public UserVO GetUserByReferral(string ReferralCode)
        {
            using (var DBContext = new PropComDbContext())
            {
                var userVO = new UserVO();

                var s = from a in DBContext.P_USER
                        where a.ACTIVE_FLG == "1"
                        && a.REFERRAL_CODE == ReferralCode
                        select a;

                if (s.Count() > 0)
                {
                    var sUser = s.First();
                    userVO = Mapper.Map<P_USER, UserVO>(sUser);
                }
                return userVO;
            }
        }

        public UserVO GetNewUserByToken(string Token)
        {
            using (var DBContext = new PropComDbContext())
            {
                var userVO = new UserVO();

                var USER_ACCNT_STATUS_PROSPECT = ((int)USER_ACCNT_STATUS.Prospect).ToString();
                var s = from a in DBContext.P_USER
                        where (a.ACCNT_STATUS == USER_ACCNT_STATUS_PROSPECT && a.ACTIVE_FLG == "1" && a.TOKEN == Token)
                        select a;

                if (s.Count() > 0)
                {
                    var sUser = s.First();
                    userVO = Mapper.Map<P_USER, UserVO>(sUser);
                }
                return userVO;
            }
        }

        #endregion

        #region GetUsers
        public List<UserVO> GetMemberUsers(int startIdx, int length, ref int TotalCount, string orderBy = "", string orderDirection = "", UserVO qFilter = null, MEMBER_TYPE MemberType = MEMBER_TYPE.Resident)
        {
            List<UserVO> result = new List<UserVO>();
            using (var DBContext = new PropComDbContext())
            {
                var USER_TYPE_Member = ((int)USER_TYPE.Member).ToString();

                var ett = from d in DBContext.P_USER
                          where d.ACTIVE_FLG == "1"
                          && d.USER_TYPE == USER_TYPE_Member
                          select d;

                if (MemberType == MEMBER_TYPE.Owner)
                {
                    //Owner = P_UNIT
                    ett = ett.Where(d => d.P_UNIT.Count() > 0 && d.P_UNIT.FirstOrDefault().ACTIVE_FLG == "1" && d.P_UNIT.FirstOrDefault().P_UNIT_TYPE.ACTIVE_FLG == "1");
                }
                else if (MemberType == MEMBER_TYPE.Resident)
                {
                    //Resident = P_UNIT1
                    ett = ett.Where(d => d.P_UNIT1.Count() > 0 && d.P_UNIT1.FirstOrDefault().ACTIVE_FLG == "1" && d.P_UNIT1.FirstOrDefault().P_UNIT_TYPE.ACTIVE_FLG == "1");
                }

                //filtering
                if (qFilter.ACCNT_ID != null && qFilter.ACCNT_ID != 0)
                    ett = ett.Where(m => m.ACCNT_ID == qFilter.ACCNT_ID);
                if (qFilter.FIRST_NAME != null && qFilter.FIRST_NAME != "")
                    ett = ett.Where(m => m.FIRST_NAME.ToLower().Contains(qFilter.FIRST_NAME.ToLower()));
                if (qFilter.USERNAME != null && qFilter.USERNAME != "")
                    ett = ett.Where(m => m.USERNAME.ToLower().Contains(qFilter.USERNAME.ToLower()));

                TotalCount = ett.Count();

                //ordering && paging
                if (orderDirection == "asc")
                {
                    if (orderBy == "Street/Block")
                    {
                        if (MemberType == MEMBER_TYPE.Owner)
                            ett = ett.OrderBy(m => m.P_UNIT.FirstOrDefault().P_UNIT_TYPE.CATEGORY_VALUE);
                        else if (MemberType == MEMBER_TYPE.Resident)
                            ett = ett.OrderBy(m => m.P_UNIT1.FirstOrDefault().P_UNIT_TYPE.CATEGORY_VALUE);
                    }
                    else if (orderBy == "UnitNo")
                    {
                        if (MemberType == MEMBER_TYPE.Owner)
                            ett = ett.OrderBy(m => m.P_UNIT.FirstOrDefault().UNIT_NO);
                        else if (MemberType == MEMBER_TYPE.Resident)
                            ett = ett.OrderBy(m => m.P_UNIT1.FirstOrDefault().UNIT_NO);
                    }
                    else if (orderBy == "ContactName")
                        ett = ett.OrderBy(m => m.FIRST_NAME);
                    else if (orderBy == "Email")
                        ett = ett.OrderBy(m => m.USERNAME);
                    else if (orderBy == "UserType")
                        ett = ett.OrderBy(m => m.USER_TYPE);
                    else if (orderBy == "ContactNo")
                        ett = ett.OrderBy(m => m.MOBILE_NO);
                    else
                        ett = ett.OrderBy(m => m.ROW_ID);
                }
                else
                {
                    if (orderBy == "Street/Block")
                    {
                        if (MemberType == MEMBER_TYPE.Owner)
                            ett = ett.OrderByDescending(m => m.P_UNIT.FirstOrDefault().P_UNIT_TYPE.CATEGORY_VALUE);
                        else if (MemberType == MEMBER_TYPE.Resident)
                            ett = ett.OrderByDescending(m => m.P_UNIT1.FirstOrDefault().P_UNIT_TYPE.CATEGORY_VALUE);
                    }
                    else if (orderBy == "UnitNo")
                    {
                        if (MemberType == MEMBER_TYPE.Owner)
                            ett = ett.OrderByDescending(m => m.P_UNIT.FirstOrDefault().UNIT_NO);
                        else if (MemberType == MEMBER_TYPE.Resident)
                            ett = ett.OrderByDescending(m => m.P_UNIT1.FirstOrDefault().UNIT_NO);
                    }
                    else if (orderBy == "ContactName")
                        ett = ett.OrderByDescending(m => m.FIRST_NAME);
                    else if (orderBy == "Email")
                        ett = ett.OrderByDescending(m => m.USERNAME);
                    else if (orderBy == "UserType")
                        ett = ett.OrderByDescending(m => m.USER_TYPE);
                    else if (orderBy == "ContactNo")
                        ett = ett.OrderByDescending(m => m.MOBILE_NO);
                    else
                        ett = ett.OrderByDescending(m => m.ROW_ID);
                }

                ett = ett.Skip(startIdx).Take(length);

                //mapping
                foreach (var v in ett)
                {
                    var userVO = Mapper.Map<P_USER, UserVO>(v);

                    long UnitId = 0;

                    if (MemberType == MEMBER_TYPE.Owner)
                    {
                        UnitId = v.P_UNIT.FirstOrDefault().ROW_ID;
                    }
                    else if (MemberType == MEMBER_TYPE.Resident)
                    {
                        UnitId = v.P_UNIT1.FirstOrDefault().ROW_ID;
                    }

                    if (UnitId != 0)
                    {
                        var UnitVO = UnitDALImpl.GetUnit(UnitId);
                        if (UnitVO != null && UnitVO.ROW_ID != 0)
                        {
                            UnitVO.UnitType = UnitTypeDALImpl.GetUnitType(UnitVO.UNIT_TYPE_ID.Value);

                            userVO.Unit = UnitVO;
                        }
                    }

                    result.Add(userVO);
                }
            }

            return result;
        }
        public List<UserVO> GetInactiveMemberUsers(int startIdx, int length, ref int TotalCount, string orderBy = "", string orderDirection = "", UserVO qFilter = null)
        {
            List<UserVO> result = new List<UserVO>();
            using (var DBContext = new PropComDbContext())
            {
                var USER_TYPE_Member = ((int)USER_TYPE.Member).ToString();

                var P_USER_UNOCCUPIED = from d in DBContext.P_USER
                                      where d.ACTIVE_FLG == "1"
                                      && d.USER_TYPE == USER_TYPE_Member
                                      && d.P_UNIT.Count() == 0
                                      && d.P_UNIT1.Count() == 0
                                      select d;

                //var P_USER_DELETED = from d in DBContext.P_USER
                //                   where d.ACTIVE_FLG != "1"
                //                   && d.USER_TYPE == USER_TYPE_Member
                //                   select d;

                var ett = P_USER_UNOCCUPIED;//.Union(P_USER_DELETED);

                //filtering
                if (qFilter.ACCNT_ID != null && qFilter.ACCNT_ID != 0)
                    ett = ett.Where(m => m.ACCNT_ID == qFilter.ACCNT_ID);
                if (qFilter.FIRST_NAME != null && qFilter.FIRST_NAME != "")
                    ett = ett.Where(m => m.FIRST_NAME.ToLower().Contains(qFilter.FIRST_NAME.ToLower()));
                if (qFilter.USERNAME != null && qFilter.USERNAME != "")
                    ett = ett.Where(m => m.USERNAME.ToLower().Contains(qFilter.USERNAME.ToLower()));

                TotalCount = ett.Count();

                //ordering && paging
                if (orderDirection == "asc")
                {
                    if (orderBy == "ContactName")
                        ett = ett.OrderBy(m => m.FIRST_NAME);
                    else if (orderBy == "Email")
                        ett = ett.OrderBy(m => m.USERNAME);
                    else if (orderBy == "UserType")
                        ett = ett.OrderBy(m => m.USER_TYPE);
                    else if (orderBy == "ContactNo")
                        ett = ett.OrderBy(m => m.MOBILE_NO);
                    else
                        ett = ett.OrderBy(m => m.ROW_ID);
                }
                else
                {
                    if (orderBy == "ContactName")
                        ett = ett.OrderByDescending(m => m.FIRST_NAME);
                    else if (orderBy == "Email")
                        ett = ett.OrderByDescending(m => m.USERNAME);
                    else if (orderBy == "UserType")
                        ett = ett.OrderByDescending(m => m.USER_TYPE);
                    else if (orderBy == "ContactNo")
                        ett = ett.OrderByDescending(m => m.MOBILE_NO);
                    else
                        ett = ett.OrderByDescending(m => m.ROW_ID);
                }

                ett = ett.Skip(startIdx).Take(length);

                //mapping
                foreach (var v in ett)
                {
                    var userVO = Mapper.Map<P_USER, UserVO>(v);

                    result.Add(userVO);
                }
            }

            return result;
        }

        public List<UserVO> GetUsers(int startIdx, int length, ref int TotalCount, string orderBy = "", string orderDirection = "", UserVO qFilter = null)
        {
            List<UserVO> result = new List<UserVO>();
            using (var DBContext = new PropComDbContext())
            {
                var ett = from d in DBContext.P_USER
                          where d.ACTIVE_FLG == "1"
                          select d;

                //filtering
                if (qFilter.USER_TYPE != null && qFilter.USER_TYPE != "")
                    ett = ett.Where(m => m.USER_TYPE == qFilter.USER_TYPE);
                if (qFilter.FIRST_NAME != null && qFilter.FIRST_NAME != "")
                    ett = ett.Where(m => m.FIRST_NAME.ToLower().Contains(qFilter.FIRST_NAME.ToLower()));
                if (qFilter.USERNAME != null && qFilter.USERNAME != "")
                    ett = ett.Where(m => m.USERNAME.ToLower().Contains(qFilter.USERNAME.ToLower()));
                if (qFilter.USER_TYPE != null && qFilter.USER_TYPE != "")
                    ett = ett.Where(m => m.USER_TYPE.ToLower().Contains(qFilter.USER_TYPE.ToLower()));
                if (qFilter.ACCNT_ID != null && qFilter.ACCNT_ID != 0)
                    ett = ett.Where(m => m.ACCNT_ID == qFilter.ACCNT_ID); 

                TotalCount = ett.Count();

                //ordering && paging
                if (orderDirection == "asc")
                {
                    if (orderBy == "ContactName")
                        ett = ett.OrderBy(m => m.FIRST_NAME);
                    else if (orderBy == "Email")
                        ett = ett.OrderBy(m => m.USERNAME);
                    else if (orderBy == "ContactNo")
                        ett = ett.OrderBy(m => m.MOBILE_NO);
                    else if (orderBy == "AccountStatus")
                        ett = ett.OrderBy(m => m.ACCNT_STATUS);
                    else
                        ett = ett.OrderBy(m => m.ROW_ID);
                }
                else
                {
                    if (orderBy == "ContactName")
                        ett = ett.OrderByDescending(m => m.FIRST_NAME);
                    else if (orderBy == "Email")
                        ett = ett.OrderByDescending(m => m.USERNAME);
                    else if (orderBy == "ContactNo")
                        ett = ett.OrderByDescending(m => m.MOBILE_NO);
                    else if (orderBy == "AccountStatus")
                        ett = ett.OrderByDescending(m => m.ACCNT_STATUS);
                    else
                        ett = ett.OrderByDescending(m => m.ROW_ID);
                }

                ett = ett.Skip(startIdx).Take(length);

                //mapping
                foreach (var v in ett)
                {
                    var userVO = Mapper.Map<P_USER, UserVO>(v);

                    result.Add(userVO);
                }
            }

            return result;
        }

        public RESIDENT_TYPE GetResidentType(long UserId)
        {
            using (var db = new PropComDbContext())
            {
                var ett = from d in db.P_USER
                          where d.ROW_ID == UserId
                          select d;

                if (ett.Count() > 0)
                {
                    //OWNER = P_UNIT
                    if (ett.First().P_UNIT.Count() > 0 && ett.First().P_UNIT1.Count() > 0)
                    {
                        if (ett.First().P_UNIT.FirstOrDefault().ROW_ID == ett.First().P_UNIT1.FirstOrDefault().ROW_ID)
                        {
                            return RESIDENT_TYPE.OwnerOccupied;
                        }
                    }
                    //TENANT = P_UNIT1
                    else if (ett.First().P_UNIT1.Count() > 0)
                    {
                        return RESIDENT_TYPE.Tenant;
                    }
                }
            }

            return 0;
        }

        #endregion

        public bool CheckUsernameExist(UserVO _newUser)
        {
            using (var db = new PropComDbContext())
            {
                bool isCustomerExist = db.P_USER.Any(o => o.USERNAME == _newUser.USERNAME);
                if (isCustomerExist)
                    return true;
            }

            return false;
        }

        public long CreateUser(UserVO _newUser)
        {
            using (var db = new PropComDbContext())
            {
                P_USER oUser = Mapper.Map<UserVO, P_USER>(_newUser);
                oUser.CREATED = DateTime.UtcNow;
                db.P_USER.Add(oUser);
                db.SaveChanges();

                return oUser.ROW_ID;
            }
        }

        public void UpdateUser(UserVO _updatedUser)
        {
            using (var DBContext = new PropComDbContext())
            {
                var P_USER = Mapper.Map<UserVO, P_USER>(_updatedUser);
                P_USER.LAST_UPD = DateTime.UtcNow;

                DBContext.P_USER.Attach(P_USER);
                DBContext.Entry(P_USER).State = EntityState.Modified;
                DBContext.SaveChanges();
            }
        }

        public void UpdateUserStatus(long UserId, USER_ACCNT_STATUS Status)
        {
            using (var db = new PropComDbContext())
            {
                var P_USERs = from d in db.P_USER
                              where d.ROW_ID == UserId
                              select d;

                if (P_USERs.Count() > 0)
                {
                    var P_USER = P_USERs.First();
                    P_USER.ACCNT_STATUS = ((int)Status).ToString();

                    db.SaveChanges();
                }
            }
        }

        public void UpdateUserActiveFlag(long UserId, USER_ACTIVE_FLAG ActiveFlag)
        {
            using (var db = new PropComDbContext())
            {
                var P_USERs = from d in db.P_USER
                              where d.ROW_ID == UserId
                              select d;

                if (P_USERs.Count() > 0)
                {
                    var P_USER = P_USERs.First();
                    P_USER.ACTIVE_FLG = ((int)ActiveFlag).ToString();

                    db.SaveChanges();
                }
            }
        }


        #region GetAllStaff
        public List<UserVO> GetAllStaff()
        {
            using (var DBContext = new PropComDbContext())
            {
                var s = from a in DBContext.P_USER
                        where (a.ACTIVE_FLG == "1" && a.USER_TYPE == ResCommConstants.USER_TYPE_STAFF)
                        select a;

                return MapReturnList(s.ToList());
            }
        }

        public List<UserVO> GetAllStaff(long groupRowId)
        {
            using (var DBContext = new PropComDbContext())
            {
                var s = from u in DBContext.P_USER
                        join ug in DBContext.P_USER_GROUP on u.ROW_ID equals ug.USER_ID
                        where (u.ACTIVE_FLG == "1" && u.USER_TYPE == ResCommConstants.USER_TYPE_STAFF && ug.GROUP_ID == groupRowId)
                        select u;

                return s.ToList().Select(x => Mapper.Map<P_USER, UserVO>(x)).ToList();
            }
        }
        #endregion

        #region GetStaff
        public UserVO GetStaff(long rowId)
        {
            using (var DBContext = new PropComDbContext())
            {
                var s = from u in DBContext.P_USER
                        where (u.ACTIVE_FLG == "1" && u.USER_TYPE == ResCommConstants.USER_TYPE_STAFF && u.ROW_ID == rowId)
                        select u;

                return Mapper.Map<P_USER, UserVO>(s.FirstOrDefault());
            }
        }
        #endregion

    }
}
