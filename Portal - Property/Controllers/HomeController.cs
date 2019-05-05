using ResComm.Web.Constants;
using ResComm.Web.Lib.BLL;
using ResComm.Web.Lib.Interface.Common.Enums;
using ResComm.Web.Lib.Interface.Models.Account;
using ResComm.Web.Lib.Interface.Models.Address;
using ResComm.Web.Lib.Interface.Models.Property;
using ResComm.Web.Lib.Interface.Models.Unit;
using ResComm.Web.Lib.Interface.Models.User;
using ResComm.Web.Lib.Service;
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
    public class HomeController : MemberBaseController
    {
        UserBLL UserBLL = new UserBLL();
        AddressBLL AddressBLL = new AddressBLL();
        AccountBLL AccountBLL = new AccountBLL();
        PropertyBLL PropertyBLL = new PropertyBLL();
        UnitBLL UnitBLL = new UnitBLL();

        public ActionResult Authentication(string token)
        {
            try
            {
                UserVO User = UserBLL.IsMemberTokenAuthenticated(token);

                //store user info                
                Session["Email"] = User.USERNAME;
                Session["Name"] = User.FIRST_NAME ?? " ";
                Session["UserId"] = User.ROW_ID.ToString();

                //FormsAuthentication.SetAuthCookie(User.ROW_ID.ToString(), false);

                return RedirectToAction("Setup", "Home");                
            }
            catch(Exception e)
            {
                return RedirectToAction("Error", "Home", new { Message = e.Message });
            }
        }

        public ActionResult Error(string Message)
        {
            Session.RemoveAll();

            ViewBag.Message = Message;
            return View();
        }

        public ActionResult Login(string Property = "", string ReturnURL = "")
        {
            LoginModel model = new LoginModel();

            if (!String.IsNullOrWhiteSpace(Property))
            {
                var PropertyVO = PropertyBLL.Get(Property);
                if (PropertyVO != null && !String.IsNullOrWhiteSpace(PropertyVO.LOGO_URL))
                    ViewBag.LogoURL = Constant.UploadPath + PropertyVO.LOGO_URL;
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, FormCollection form)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var User = UserBLL.MemberLogin(model.Email, model.Password);

                    if (User != null)
                    {
                        //store user info                        
                        Session["Email"] = model.Email;
                        Session["Name"] = User.FIRST_NAME ?? " ";
                        Session["UserId"] = User.ROW_ID.ToString();

                        //FormsAuthentication.SetAuthCookie(User.ROW_ID.ToString(), model.RememberMe);

                        if (form["ReturnURL"] == null || form["ReturnURL"] == "")
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            return Redirect(form["ReturnURL"]);
                        }
                    }
                    else
                        TempData["Message"] = "Invalid Email or Password.";
                }
                catch (Exception ex)
                {
                    TempData["Message"] = ex.Message;
                }
            }

            return View(model);
        }

        public ActionResult LogOut()
        {
            Session.RemoveAll();
            return RedirectToAction("Login", "Home");
        }
        
        public ActionResult Setup()
        {
            Member model = new Member();

            
            var UserId = long.Parse((string)Session["UserId"]);

            var UserVO = UserBLL.Get(UserId);
            model.UserId = UserId;
            model.Email = UserVO.USERNAME;
            model.Password = UserVO.PASSWORD;
            model.ConfirmPassword = UserVO.PASSWORD;
            model.ContactName = UserVO.FIRST_NAME;
            model.ContactNo = UserVO.MOBILE_NO;
            model.UserType = ((USER_TYPE)int.Parse(UserVO.USER_TYPE)).ToString();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Setup(Member model, FormCollection form)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    
                    var UserId = long.Parse((string)Session["UserId"]);

                    //update user
                    var UserVO = UserBLL.Get(UserId);
                    UserVO.PASSWORD = model.Password;

                    UserVO.FIRST_NAME = model.ContactName;
                    UserVO.MOBILE_NO = model.ContactNo;

                    UserVO.ACCNT_STATUS = ((int)USER_ACCNT_STATUS.Active).ToString();

                    UserBLL.Update(UserVO, USER_TYPE.Member);

                    //Send Email
                    var PropertyName = PropertyBLL.Get(UserVO.ACCNT_ID.Value).NAME;
                    EmailService.SendMemberCreatedEmail(UserVO.USERNAME, PropertyName, UserVO.SALUTATION, UserVO.FIRST_NAME);

                    Session["Name"] = model.ContactName ?? " ";

                    TempData["Message"] = "Successfully done.";

                    return RedirectToAction("Index", "Home");
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

            return View(model);
        }
        
        public ActionResult Index()
        {
            return View();
        }

    }
}