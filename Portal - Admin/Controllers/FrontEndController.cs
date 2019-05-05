using ResComm.Web.Lib.BLL;
using ResComm.Web.Lib.Interface.Common.Enums;
using ResComm.Web.Lib.Interface.Models.Account;
using ResComm.Web.Lib.Interface.Models.Address;
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
    public class FrontEndController : Controller
    {
        [HttpPost]
        public ActionResult ContactUs(ContactUs Model)
        {
            string Message = "";
            if (ModelState.IsValid)
            {
                try
                {
                    EmailService.SendEmail("support@residentportal.my", Model.email + "/" + Model.fname + " : " + Model.subject, Model.message);
                    Message = "Successfully sent.";
                }
                catch (Exception ex)
                {
                    Message = ex.Message;
                }
            }
            else
            {
                Message = string.Join(" ", ModelState.Values
                                        .SelectMany(v => v.Errors)
                                        .Select(e => e.ErrorMessage));
            }

            if (!string.IsNullOrWhiteSpace(Model.returnurl))
            {
                return Redirect(new Uri(Model.returnurl).GetLeftPart(UriPartial.Path) + "?msg=" + Message);
            }
            {
                return Redirect("https://www.residentportal.my/" + "?msg=" + Message);
            }
        }

        [HttpPost]
        public ActionResult SignUp(SignUp Model)
        {
            string Message = "";
            if (ModelState.IsValid)
            {
                try
                {
                    //checking payment status
                    //
                    var PaymentStatus = PAYMENT_STATUS.Trial;

                    //create user
                    var UserVO = new UserVO();
                    UserVO.USERNAME = Model.email;
                    UserVO.PASSWORD = Model.pass;
                    UserVO.PAYMENT_STATUS = (int)PaymentStatus;

                    UserVO.ACTIVE_FLG = "1";
                    UserVO.ACCNT_STATUS = ((int)USER_ACCNT_STATUS.Prospect).ToString();

                    var userid = new UserBLL().CreateAdmin(UserVO);

                    if (userid != 0)
                    {
                        //create address
                        var NewAddressId = new AddressBLL().CreateAddress(new AddressVO()
                        {
                            ADDR = Model.address,
                            CITY = Model.city,
                            STATE = Model.state,
                            POSTAL_CD = Model.postcode,
                            COUNTRY = Model.country,
                            CREATED_BY = userid,
                        });

                        //create customer(account)
                        var NewCustomerId = new AccountBLL().CreateAccount(new CustomerVO()
                        {
                            NAME = Model.name,
                            EMAIL_ADDR = "",
                            OFFICE_NO = "",
                            FAX_NO = "",

                            ACCNT_TYPE_CD = "1", ///
                            CREATED_BY = userid,

                            CUST_USER_ID = userid, //User FK
                            ADDR_ID = NewAddressId, //Address FK
                        });

                        //create property
                        var NewPropertyId = new PropertyBLL().CreateProperty(new PropertyVO()
                        {
                            NAME = Model.property_name,
                            ADDR_1 = Model.address,
                            ADDR_2 = Model.address2,
                            CITY = Model.city,
                            STATE = Model.state,
                            POSTAL_CD = Model.postcode,
                            COUNTRY = Model.country,
                            CREATED_BY = userid,

                            ACCNT_ID = NewCustomerId, //Account(Customer) FK
                        });

                        //update user
                        UserVO = UserBLL.Get(userid);

                        UserVO.FIRST_NAME = Model.name;
                        UserVO.MOBILE_NO = Model.contact;

                        UserVO.ACCNT_ID = NewCustomerId; //Accnt FK

                        new UserBLL().Update(UserVO, USER_TYPE.Admin);

                        /*
                        //update user status
                        new UserBLL().UpdateStatus(userid, USER_ACCNT_STATUS.Active);

                        //store user info
                        Session["Email"] = Model.email;
                        Session["Name"] = Model.name ?? " ";
                        Session["UserId"] = userid.ToString();
                        */

                        return RedirectToAction("Index", "Home");
                    }
                }
                catch (Exception ex)
                {
                    Message = ex.Message;
                }
            }
            else
            {
                Message = string.Join(" ", ModelState.Values
                                        .SelectMany(v => v.Errors)
                                        .Select(e => e.ErrorMessage));
            }

            if (!string.IsNullOrWhiteSpace(Model.returnurl))
            {
                return Redirect(new Uri(Model.returnurl).GetLeftPart(UriPartial.Path) + "?msg=" + Message);
            }
            {
                return Redirect("https://www.residentportal.my/" + "?msg=" + Message);
            }
        }

        [HttpPost]
        public ActionResult Login(Login model)
        {
            string Message = "";

            if (ModelState.IsValid)
            {
                try
                {
                    var User = new UserBLL().AdminLogin(model.email, model.pass);

                    if (User != null)
                    {
                        //store user info
                        Session["Email"] = model.email;
                        Session["Name"] = User.FIRST_NAME ?? " ";
                        Session["UserId"] = User.ROW_ID.ToString();

                        //FormsAuthentication.SetAuthCookie(User.ROW_ID.ToString(), model.RememberMe);

                        if (User.USER_TYPE == ((int)USER_TYPE.Admin).ToString())
                        {
                            //checking First Time Login 
                            if (User.ACCNT_STATUS == ((int)USER_ACCNT_STATUS.Prospect).ToString())
                            {
                                return RedirectToAction("Setup", "Home");
                            }
                            else if (User.ACCNT_STATUS == ((int)USER_ACCNT_STATUS.Active).ToString())
                            {
                                return RedirectToAction("Index", "Home");
                            }
                        }
                        else if (User.USER_TYPE == ((int)USER_TYPE.SuperAdmin).ToString())
                        {
                            return RedirectToAction("Index", "SuperAdmin");
                        }
                    }
                    else
                        Message = "Invalid Email or Password.";
                }
                catch (Exception ex)
                {
                    Message = ex.Message;
                }
            }

            if (!string.IsNullOrWhiteSpace(model.returnurl))
            {
                return Redirect(new Uri(model.returnurl).GetLeftPart(UriPartial.Path) + "?msg=" + Message);
            }
            {
                return Redirect("https://www.residentportal.my/" + "?msg=" + Message);
            }
        }

    }
}