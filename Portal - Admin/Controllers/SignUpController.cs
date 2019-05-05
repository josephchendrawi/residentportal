using ResComm.Web.Lib.BLL;
using ResComm.Web.Lib.Interface.Common.Enums;
using ResComm.Web.Lib.Interface.Models.Account;
using ResComm.Web.Lib.Interface.Models.Property;
using ResComm.Web.Lib.Interface.Models.User;
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
        public ActionResult Index(string AffiliateReferral)
        {
            PropertyAdminSignup model = new PropertyAdminSignup();
            model.AffiliateReferral = AffiliateReferral;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(PropertyAdminSignup Model, FormCollection form)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //checking payment status
                    //
                    var PaymentStatus = PAYMENT_STATUS.Trial;

                    //create user
                    var UserVO = new UserVO();
                    UserVO.USERNAME = Model.Email;
                    UserVO.PASSWORD = Model.Password;
                    //UserVO.REFERRAL_CODE = Model.AffiliateReferral; //temp storing for checking 'referred by'
                    UserVO.PAYMENT_STATUS = (int)PaymentStatus;

                    UserVO.ACTIVE_FLG = "1";
                    UserVO.ACCNT_STATUS = ((int)USER_ACCNT_STATUS.Prospect).ToString();

                    var result = new UserBLL().CreateAdmin(UserVO);

                    if (result != 0)
                    {
                        //create customer(account)
                        var NewCustomerId = new AccountBLL().CreateAccount(new CustomerVO()
                        {
                            ACCNT_TYPE_CD = "1", ///
                            CREATED_BY = result,

                            CUST_USER_ID = result, //User FK
                            ADDR_ID = null, //Address FK
                        }, Model.AffiliateReferral);

                        //create property
                        var NewPropertyId = new PropertyBLL().CreateProperty(new PropertyVO()
                        {
                            NAME = " ",
                            CREATED_BY = result,

                            ACCNT_ID = NewCustomerId, //Account(Customer) FK
                        });

                        //update "Accnt FK" in user
                        var user = UserBLL.Get(result);
                        user.ACCNT_ID = NewCustomerId; //Accnt FK

                        new UserBLL().Update(user, USER_TYPE.Admin);

                        //store user info
                        Session["Email"] = Model.Email;
                        Session["Name"] = " ";
                        Session["UserId"] = result.ToString();

                        return RedirectToAction("Setup", "Home");
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

            return View(Model);
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