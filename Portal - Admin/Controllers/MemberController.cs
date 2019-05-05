using ResComm.Web.Lib.BLL;
using ResComm.Web.Lib.Interface.Common.Enums;
using ResComm.Web.Lib.Interface.Models.Account;
using ResComm.Web.Lib.Interface.Models.Address;
using ResComm.Web.Lib.Interface.Models.Property;
using ResComm.Web.Lib.Interface.Models.Unit;
using ResComm.Web.Lib.Interface.Models.User;
using ResComm.Web.Models;
using ResComm.Web.Models.DataTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ResComm.Web.Controllers
{
    public class MemberController : AdminBaseController
    {
        UserBLL UserBLL = new UserBLL();
        AddressBLL AddressBLL = new AddressBLL();
        AccountBLL AccountBLL = new AccountBLL();
        PropertyBLL PropertyBLL = new PropertyBLL();
        UnitBLL UnitBLL = new UnitBLL();

        [NonAction]
        private List<SelectListItem> GetMemberList()
        {
            var UserId = long.Parse((string)Session["UserId"]);
            var CustomerId = AccountBLL.GetRelation(UserId).CustomerId;

            var temp = 0;
            List<SelectListItem> MemberList = new List<SelectListItem>();
            foreach (var v in UserBLL.GetAllMemberUsers(0, int.MaxValue, ref temp, "", "", new UserVO() { ACCNT_ID = CustomerId }))
            {
                MemberList.Add(new SelectListItem() { Text = v.USERNAME, Value = v.USERNAME });
            }

            return MemberList;
        }

        public JsonResult Ajax_GetMemberList()
        {
            var MemberList = GetMemberList();

            return Json(MemberList);
        }

        public ActionResult ResidentList()
        {
            return View();
        }

        public ActionResult ListResident(DTParameterModel param, long CustomerId = 0)
        {
            try
            {
                if (CustomerId == 0)
                {
                    
                    var UserId = long.Parse((string)Session["UserId"]);
                    CustomerId = AccountBLL.GetRelation(UserId).CustomerId;
                }

                int TotalCount = 0;

                //sorting
                string orderingFunction = param.Columns.ElementAt(param.Order.FirstOrDefault().Column).Name == "" ? param.Columns.ElementAt(param.Order.FirstOrDefault().Column).Data : param.Columns.ElementAt(param.Order.FirstOrDefault().Column).Name;
                var sortDirection = param.Order.FirstOrDefault().Dir; // asc or desc

                //filtering
                UserVO qFilter = new UserVO()
                {
                    ACCNT_ID = CustomerId
                };
                var List = UserBLL.GetMemberUsers(param.Start, param.Length, ref TotalCount, orderingFunction, sortDirection, qFilter, MEMBER_TYPE.Resident);

                List<MemberListItem> UserList = new List<MemberListItem>();

                foreach (var v in List)
                {
                    MemberListItem VM = new MemberListItem();
                    VM.UserId = v.ROW_ID;
                    VM.Email = v.USERNAME;
                    VM.ContactName = v.FIRST_NAME ?? "-";
                    VM.ContactNo = v.MOBILE_NO ?? "-";

                    var ActivationLink = Request.Url.GetLeftPart(UriPartial.Authority) + Url.Action("Authentication", "Home") + "?token=" + v.TOKEN;

                    if (v.Unit != null)
                    {
                        VM.Unit.UnitId = v.Unit.ROW_ID;
                        VM.Unit.UnitNo = v.Unit.UNIT_NO;
                        VM.Unit.UnitType.UnitTypeId = v.Unit.UnitType.ROW_ID;
                        VM.Unit.UnitType.CategoryName = v.Unit.UnitType.CATEGORY_NAME;
                        VM.Unit.UnitType.CategoryValue = v.Unit.UnitType.CATEGORY_VALUE;
                    }
                    else
                    {
                        VM.Unit.UnitNo = "-";
                        VM.Unit.UnitType.CategoryName = "-";
                        VM.Unit.UnitType.CategoryValue = "-";
                    }

                    UserList.Add(VM);
                }

                DTResult<MemberListItem> model = new DTResult<MemberListItem>
                {
                    draw = param.Draw,
                    data = UserList,
                    recordsFiltered = TotalCount,
                    recordsTotal = TotalCount
                };

                return Json(model, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public ActionResult AddResident()
        {
            AddMemberModel model = new AddMemberModel();
            model.isNewMember = true;

            ViewBag.MemberList = GetMemberList();

            return View(model);
        }

        [HttpPost]
        public ActionResult AddResident(AddMemberModel Model)
        {
            if (ModelState.IsValid)
            {
                try
                {                    
                    var UserId = long.Parse((string)Session["UserId"]);
                    var CustomerId = AccountBLL.GetRelation(UserId).CustomerId;
                    
                    long Member_UserId = 0;

                    if (Model.isNewMember == true)
                    {
                        var UserVO = new UserVO();
                        UserVO.USERNAME = Model.Email;
                        UserVO.USER_TYPE = ((int)USER_TYPE.Member).ToString();
                        UserVO.FIRST_NAME = Model.Name;
                        UserVO.MOBILE_NO = Model.PhoneNumber;
                        UserVO.CREATED_BY = UserId;

                        UserVO.ACTIVE_FLG = "1";
                        UserVO.ACCNT_STATUS = ((int)USER_ACCNT_STATUS.Prospect).ToString();
                        UserVO.ACCNT_ID = CustomerId;

                        Member_UserId = UserBLL.CreateMember(UserVO);
                    }
                    else
                    {
                        Member_UserId = UserBLL.Get(Model.Email, USER_TYPE.Member).ROW_ID;
                    }

                    if (Member_UserId != 0)
                    {
                        if (Model.UnitId != null && Model.UnitId != 0)
                        {
                            var UnitVO = UnitBLL.Get(Model.UnitId.Value);
                            if (Model.ResidentType == ((int)RESIDENT_TYPE.OwnerOccupied).ToString())
                            {
                                UnitVO.OWNER_ID = Member_UserId;
                                UnitVO.TENANT_ID = Member_UserId;
                            }
                            else if (Model.ResidentType == ((int)RESIDENT_TYPE.Tenant).ToString())
                            {
                                UnitVO.TENANT_ID = Member_UserId;
                            }
                            UnitVO.LAST_UPDATED_BY = UserId;

                            UnitBLL.UpdateUnit(UnitVO);
                        }

                        TempData["Message"] = "Successfully done.";
                        return RedirectToAction("ResidentList", "Member");
                    }
                }
                catch (Exception ex)
                {
                    TempData["Message"] = ex.Message;
                }
            }
            else
            {
                TempData["Message"] = string.Join(" ", ModelState.Values
                                        .SelectMany(v => v.Errors)
                                        .Select(e => e.ErrorMessage));
            }

            ViewBag.MemberList = GetMemberList();

            return View(Model);
        }

        public ActionResult EditResident(int Id)
        {
            var UserVO = UserBLL.Get(Id);

            EditMemberModel model = new EditMemberModel();
            model.UserId = Id;
            model.Email = UserVO.USERNAME;
            model.Name = UserVO.FIRST_NAME;
            model.PhoneNumber = UserVO.MOBILE_NO;

            return View(model);
        }

        [HttpPost]
        public ActionResult EditResident(EditMemberModel Model, FormCollection form)
        {
            ModelState.Remove("UserType");
            ModelState.Remove("UnitId");
            ModelState.Remove("UnitType");

            if (ModelState.IsValid)
            {
                try
                {
                    
                    var CurrentUserId = long.Parse((string)Session["UserId"]);

                    var UserVO = UserBLL.Get(Model.UserId);
                    UserVO.USERNAME = Model.Email;
                    UserVO.FIRST_NAME = Model.Name;
                    UserVO.MOBILE_NO = Model.PhoneNumber;
                    UserVO.LAST_UPD_BY = CurrentUserId;

                    UserBLL.Update(UserVO, USER_TYPE.Member);

                    TempData["Message"] = "Successfully done.";
                    return RedirectToAction("ResidentList", "Member");
                }
                catch (Exception ex)
                {
                    TempData["Message"] = ex.Message;
                }
            }
            else
            {
                TempData["Message"] = string.Join(" ", ModelState.Values
                                        .SelectMany(v => v.Errors)
                                        .Select(e => e.ErrorMessage));
            }

            return View(Model);
        }

        public ActionResult OwnerList()
        {
            return View();
        }

        public ActionResult ListOwner(DTParameterModel param, long CustomerId = 0)
        {
            try
            {
                if (CustomerId == 0)
                {
                    
                    var UserId = long.Parse((string)Session["UserId"]);
                    CustomerId = AccountBLL.GetRelation(UserId).CustomerId;
                }

                int TotalCount = 0;

                //sorting
                string orderingFunction = param.Columns.ElementAt(param.Order.FirstOrDefault().Column).Name == "" ? param.Columns.ElementAt(param.Order.FirstOrDefault().Column).Data : param.Columns.ElementAt(param.Order.FirstOrDefault().Column).Name;
                var sortDirection = param.Order.FirstOrDefault().Dir; // asc or desc

                //filtering
                UserVO qFilter = new UserVO()
                {
                    ACCNT_ID = CustomerId
                };
                var List = UserBLL.GetMemberUsers(param.Start, param.Length, ref TotalCount, orderingFunction, sortDirection, qFilter, MEMBER_TYPE.Owner);

                List<MemberListItem> UserList = new List<MemberListItem>();

                foreach (var v in List)
                {
                    MemberListItem VM = new MemberListItem();
                    VM.UserId = v.ROW_ID;
                    VM.Email = v.USERNAME;
                    VM.ContactName = v.FIRST_NAME ?? "-";
                    VM.ContactNo = v.MOBILE_NO ?? "-";

                    var ActivationLink = Request.Url.GetLeftPart(UriPartial.Authority) + Url.Action("Authentication", "Home") + "?token=" + v.TOKEN;

                    if (v.Unit != null)
                    {
                        VM.Unit.UnitId = v.Unit.ROW_ID;
                        VM.Unit.UnitNo = v.Unit.UNIT_NO;
                        VM.Unit.UnitType.UnitTypeId = v.Unit.UnitType.ROW_ID;
                        VM.Unit.UnitType.CategoryName = v.Unit.UnitType.CATEGORY_NAME;
                        VM.Unit.UnitType.CategoryValue = v.Unit.UnitType.CATEGORY_VALUE;
                    }
                    else
                    {
                        VM.Unit.UnitNo = "-";
                        VM.Unit.UnitType.CategoryName = "-";
                        VM.Unit.UnitType.CategoryValue = "-";
                    }

                    UserList.Add(VM);
                }

                DTResult<MemberListItem> model = new DTResult<MemberListItem>
                {
                    draw = param.Draw,
                    data = UserList,
                    recordsFiltered = TotalCount,
                    recordsTotal = TotalCount
                };

                return Json(model, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public ActionResult AddOwner()
        {
            AddMemberModel model = new AddMemberModel();
            model.isNewMember = true;

            ViewBag.MemberList = GetMemberList();

            return View(model);
        }

        [HttpPost]
        public ActionResult AddOwner(AddMemberModel Model)
        {
            if (ModelState.IsValid)
            {
                try
                {                    
                    var UserId = long.Parse((string)Session["UserId"]);
                    var CustomerId = AccountBLL.GetRelation(UserId).CustomerId;

                    long Member_UserId = 0;

                    if (Model.isNewMember == true)
                    {
                        var UserVO = new UserVO();
                        UserVO.USERNAME = Model.Email;
                        UserVO.USER_TYPE = ((int)USER_TYPE.Member).ToString();
                        UserVO.FIRST_NAME = Model.Name;
                        UserVO.MOBILE_NO = Model.PhoneNumber;
                        UserVO.CREATED_BY = UserId;

                        UserVO.ACTIVE_FLG = "1";
                        UserVO.ACCNT_STATUS = ((int)USER_ACCNT_STATUS.Prospect).ToString();
                        UserVO.ACCNT_ID = CustomerId;

                        Member_UserId = UserBLL.CreateMember(UserVO);
                    }
                    else
                    {
                        Member_UserId = UserBLL.Get(Model.Email, USER_TYPE.Member).ROW_ID;
                    }

                    if (Member_UserId != 0)
                    {
                        if (Model.UnitId != null && Model.UnitId != 0)
                        {
                            var UnitVO = UnitBLL.Get(Model.UnitId.Value);
                            if (Model.isOwnerOccupied == true)
                            {
                                UnitVO.OWNER_ID = Member_UserId;
                                UnitVO.TENANT_ID = Member_UserId;
                            }
                            else
                            {
                                UnitVO.OWNER_ID = Member_UserId;
                            }
                            UnitVO.LAST_UPDATED_BY = UserId;

                            UnitBLL.UpdateUnit(UnitVO);
                        }

                        TempData["Message"] = "Successfully done.";
                        return RedirectToAction("OwnerList", "Member");
                    }
                }
                catch (Exception ex)
                {
                    TempData["Message"] = ex.Message;
                }
            }
            else
            {
                TempData["Message"] = string.Join(" ", ModelState.Values
                                        .SelectMany(v => v.Errors)
                                        .Select(e => e.ErrorMessage));
            }

            ViewBag.MemberList = GetMemberList();

            return View(Model);
        }

        public ActionResult EditOwner(int Id)
        {
            var UserVO = UserBLL.Get(Id);

            EditMemberModel model = new EditMemberModel();
            model.UserId = Id;
            model.Email = UserVO.USERNAME;
            model.Name = UserVO.FIRST_NAME;
            model.PhoneNumber = UserVO.MOBILE_NO;

            return View(model);
        }

        [HttpPost]
        public ActionResult EditOwner(EditMemberModel Model, FormCollection form)
        {
            ModelState.Remove("UserType");
            ModelState.Remove("UnitId");
            ModelState.Remove("UnitType");

            if (ModelState.IsValid)
            {
                try
                {
                    
                    var CurrentUserId = long.Parse((string)Session["UserId"]);

                    var UserVO = UserBLL.Get(Model.UserId);
                    UserVO.USERNAME = Model.Email;
                    UserVO.FIRST_NAME = Model.Name;
                    UserVO.MOBILE_NO = Model.PhoneNumber;
                    UserVO.LAST_UPD_BY = CurrentUserId;

                    UserBLL.Update(UserVO, USER_TYPE.Member);

                    TempData["Message"] = "Successfully done.";
                    return RedirectToAction("OwnerList", "Member");
                }
                catch (Exception ex)
                {
                    TempData["Message"] = ex.Message;
                }
            }
            else
            {
                TempData["Message"] = string.Join(" ", ModelState.Values
                                        .SelectMany(v => v.Errors)
                                        .Select(e => e.ErrorMessage));
            }

            return View(Model);
        }

        public string DeleteMember(long Id)
        {
            try
            {
                UserBLL.Delete(Id);

                return true.ToString();
            }
            catch(Exception e)
            {
                return e.Message;
            }
        }

        public ActionResult InactiveMemberList()
        {
            return View();
        }

        public ActionResult ListInactiveMember(DTParameterModel param, long CustomerId = 0)
        {
            try
            {
                if (CustomerId == 0)
                {
                    
                    var UserId = long.Parse((string)Session["UserId"]);
                    CustomerId = AccountBLL.GetRelation(UserId).CustomerId;
                }

                int TotalCount = 0;

                //sorting
                string orderingFunction = param.Columns.ElementAt(param.Order.FirstOrDefault().Column).Name == "" ? param.Columns.ElementAt(param.Order.FirstOrDefault().Column).Data : param.Columns.ElementAt(param.Order.FirstOrDefault().Column).Name;
                var sortDirection = param.Order.FirstOrDefault().Dir; // asc or desc

                //filtering
                UserVO qFilter = new UserVO()
                {
                    ACCNT_ID = CustomerId
                };
                var List = UserBLL.GetInactiveMemberUsers(param.Start, param.Length, ref TotalCount, orderingFunction, sortDirection, qFilter);

                List<MemberListItem> UserList = new List<MemberListItem>();

                foreach (var v in List)
                {
                    MemberListItem VM = new MemberListItem();
                    VM.UserId = v.ROW_ID;
                    VM.Email = v.USERNAME;
                    VM.ContactName = v.FIRST_NAME ?? "-";
                    VM.ContactNo = v.MOBILE_NO ?? "-";

                    UserList.Add(VM);
                }

                DTResult<MemberListItem> model = new DTResult<MemberListItem>
                {
                    draw = param.Draw,
                    data = UserList,
                    recordsFiltered = TotalCount,
                    recordsTotal = TotalCount
                };

                return Json(model, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}