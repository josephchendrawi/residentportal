using ResComm.Web.Lib.BLL;
using ResComm.Web.Lib.Interface.Common.Enums;
using ResComm.Web.Lib.Interface.Models.Account;
using ResComm.Web.Lib.Interface.Models.Property;
using ResComm.Web.Lib.Interface.Models.User;
using ResComm.Web.Lib.Service;
using ResComm.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ResComm.Web.Controllers
{
    public class SignUpController : Controller
    {
        UserBLL UserBLL = new UserBLL();

        public ActionResult Index()
        {
            AffiliateSignup model = new AffiliateSignup();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(AffiliateSignup model, FormCollection form)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //create affilate
                    var UserVO = new UserVO();

                    UserVO.USERNAME = model.Email;
                    UserVO.FIRST_NAME = model.Name;
                    UserVO.ACCNT_STATUS = ((int)USER_ACCNT_STATUS.Prospect).ToString();
                    UserVO.USER_TYPE = ((int)USER_TYPE.Affiliate).ToString();
                    UserVO.PASSWORD = model.Password;

                    UserVO.ACTIVE_FLG = "1";
                    var Affiliate_UserId = UserBLL.CreateAffiliate(UserVO);

                    if (Affiliate_UserId != 0)
                    {
                        //Send Email
                        EmailService.SendAffiliateCreatedEmail(UserVO.USERNAME, UserVO.SALUTATION, UserVO.FIRST_NAME);
                        
                        return RedirectToAction("Login", "Home");
                    }
                    else
                    {
                        throw new Exception("Unknown Error occured.");
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

        public ActionResult TermAndConditions()
        {
            return View();
        }

        public ActionResult PrivacyPolicy()
        {
            return View();
        }

    }
}