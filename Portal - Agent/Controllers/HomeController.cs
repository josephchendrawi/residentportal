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
    public class HomeController : AffiliateBaseController
    {
        UserBLL UserBLL = new UserBLL();

        public ActionResult Index()
        {
            var model = new DashboardVM();

            var UserId = long.Parse((string)Session["UserId"]);
            var User = UserBLL.Get(UserId);

            model.ReferralCode = User.REFERRAL_CODE;

            return View(model);
        }

        public ActionResult Login(string ReturnURL = "")
        {
            LoginModel model = new LoginModel();

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
                    var User = UserBLL.AffiliateLogin(model.Email, model.Password);

                    if (User != null)
                    {
                        //store user info
                        Session["Email"] = model.Email;
                        Session["Name"] = User.FIRST_NAME ?? " ";
                        Session["UserId"] = User.ROW_ID.ToString();

                        //FormsAuthentication.SetAuthCookie(User.ROW_ID.ToString(), model.RememberMe);

                        return RedirectToAction("Index", "Home");
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
        
        public ActionResult Register()
        {
            Affiliate model = new Affiliate();
            
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Affiliate model, FormCollection form)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //create affilate
                    var UserVO = new UserVO();

                    UserVO.USERNAME = model.Email;
                    UserVO.FIRST_NAME = model.ContactName;
                    UserVO.MOBILE_NO = model.ContactNo;
                    UserVO.ACCNT_STATUS = ((int)USER_ACCNT_STATUS.Prospect).ToString();
                    UserVO.USER_TYPE = ((int)USER_TYPE.Affiliate).ToString();
                    UserVO.PASSWORD = model.Password;

                    UserVO.ACTIVE_FLG = "1";
                    var result = UserBLL.CreateAffiliate(UserVO);
                                        
                    if (result != 0)
                    {
                        //Send Email
                        EmailService.SendAffiliateCreatedEmail(UserVO.USERNAME, UserVO.SALUTATION, UserVO.FIRST_NAME);

                        return RedirectToAction("RegisterSuccess", "Home");
                    }
                    else
                    {
                        TempData["Message"] = "Unknown Error occured.";
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

            return View(model);
        }

        public ActionResult RegisterSuccess()
        {
            return View();
        }

    }
}