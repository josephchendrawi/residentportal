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
    public class MemberController : MemberBaseController
    {
        UserBLL UserBLL = new UserBLL();
        AddressBLL AddressBLL = new AddressBLL();
        AccountBLL AccountBLL = new AccountBLL();
        PropertyBLL PropertyBLL = new PropertyBLL();
        UnitBLL UnitBLL = new UnitBLL();

        public ActionResult Index()
        {
            
            var CurrentUserId = long.Parse((string)Session["UserId"]);

            var UserVO = UserBLL.Get(CurrentUserId);

            EditMemberModel model = new EditMemberModel();
            model.Email = UserVO.USERNAME;
            model.Name = UserVO.FIRST_NAME;
            model.PhoneNumber = UserVO.MOBILE_NO;

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(EditMemberModel Model, FormCollection form)
        {
            ModelState.Remove("UserType");
            ModelState.Remove("UnitId");
            ModelState.Remove("UnitType");

            if (ModelState.IsValid)
            {
                try
                {
                    
                    var CurrentUserId = long.Parse((string)Session["UserId"]);

                    var UserVO = UserBLL.Get(CurrentUserId);
                    UserVO.USERNAME = Model.Email;
                    UserVO.FIRST_NAME = Model.Name;
                    UserVO.MOBILE_NO = Model.PhoneNumber;
                    if (!string.IsNullOrWhiteSpace(Model.Password))
                    {
                        UserVO.PASSWORD = Model.Password;
                    }
                    UserVO.LAST_UPD_BY = CurrentUserId;

                    UserBLL.Update(UserVO, USER_TYPE.Member);

                    TempData["Message"] = "Successfully done.";
                    return RedirectToAction("Index", "Member");
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

            return RedirectToAction("Index", "Member");
        }

    }
}