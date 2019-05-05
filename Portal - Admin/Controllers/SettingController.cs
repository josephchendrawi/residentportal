using ResComm.Web.Lib.BLL;
using ResComm.Web.Lib.Interface.Common.Enums;
using ResComm.Web.Lib.Interface.Models.Account;
using ResComm.Web.Lib.Interface.Models.Address;
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
    public class SettingController : AdminBaseController
    {
        UserBLL UserBLL = new UserBLL();
        AddressBLL AddressBLL = new AddressBLL();
        AccountBLL AccountBLL = new AccountBLL();

        public ActionResult Admin()
        {
            AdminUpdate model = new AdminUpdate();

            
            var UserId = long.Parse((string)Session["UserId"]);

            var UserVO = UserBLL.Get(UserId);
            model.UserInfo.Email = UserVO.USERNAME;
            model.UserInfo.Password = UserVO.PASSWORD;
            model.UserInfo.ConfirmPassword = UserVO.PASSWORD;

            model.UserInfo.ContactName = UserVO.FIRST_NAME;
            model.UserInfo.ContactNo = UserVO.MOBILE_NO;

            var UserR = AccountBLL.GetRelation(UserId);

            if (UserR.AddressId != 0)
            {
                var AddressVO = AddressBLL.Get(UserR.AddressId);

                model.AccountInfo.AddressInfo.AddressId = UserR.AddressId;
                model.AccountInfo.AddressInfo.Address = AddressVO.ADDR;
                model.AccountInfo.AddressInfo.City = AddressVO.CITY;
                model.AccountInfo.AddressInfo.State = AddressVO.STATE;
                model.AccountInfo.AddressInfo.ZIP = AddressVO.POSTAL_CD;
                model.AccountInfo.AddressInfo.Country = AddressVO.COUNTRY;
            }
            if (UserR.CustomerId != 0)
            {
                var CustomerVO = AccountBLL.Get(UserR.CustomerId);

                model.AccountInfo.CustomerId = UserR.CustomerId;
                model.AccountInfo.CompanyName = CustomerVO.NAME;
                model.AccountInfo.CompanyEmail = CustomerVO.EMAIL_ADDR;
                model.AccountInfo.OfficeNo = CustomerVO.OFFICE_NO;
                model.AccountInfo.FaxNo = CustomerVO.FAX_NO;
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Admin(AdminUpdate model, FormCollection form)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    
                    var UserId = long.Parse((string)Session["UserId"]);

                    //create/update address
                    var AddressVO = AddressBLL.Get(model.AccountInfo.AddressInfo.AddressId ?? 0);
                    if (AddressVO == null)
                    {
                        var NewAddressId = AddressBLL.CreateAddress(new AddressVO()
                        {
                            ADDR = model.AccountInfo.AddressInfo.Address,
                            CITY = model.AccountInfo.AddressInfo.City,
                            STATE = model.AccountInfo.AddressInfo.State,
                            POSTAL_CD = model.AccountInfo.AddressInfo.ZIP,
                            COUNTRY = model.AccountInfo.AddressInfo.Country,
                            CREATED_BY = UserId,
                        });

                        model.AccountInfo.AddressInfo.AddressId = NewAddressId;
                    }
                    else
                    {
                        AddressVO.ADDR = model.AccountInfo.AddressInfo.Address;
                        AddressVO.CITY = model.AccountInfo.AddressInfo.City;
                        AddressVO.STATE = model.AccountInfo.AddressInfo.State;
                        AddressVO.POSTAL_CD = model.AccountInfo.AddressInfo.ZIP;
                        AddressVO.COUNTRY = model.AccountInfo.AddressInfo.Country;
                        AddressVO.LAST_UPD_BY = UserId;

                        AddressBLL.UpdateAddress(AddressVO);
                    }

                    //create/update customer(account)
                    var CustomerVO = AccountBLL.Get(model.AccountInfo.CustomerId ?? 0);
                    if (CustomerVO == null)
                    {
                        var NewCustomerId = AccountBLL.CreateAccount(new CustomerVO()
                        {
                            NAME = model.AccountInfo.CompanyName,
                            EMAIL_ADDR = model.AccountInfo.CompanyEmail,
                            OFFICE_NO = model.AccountInfo.OfficeNo,
                            FAX_NO = model.AccountInfo.FaxNo,

                            ACCNT_TYPE_CD = "1", ///
                            CREATED_BY = UserId,

                            CUST_USER_ID = UserId, //User FK
                            ADDR_ID = model.AccountInfo.AddressInfo.AddressId, //Address FK
                        });

                        model.AccountInfo.CustomerId = NewCustomerId;
                    }
                    else
                    {
                        CustomerVO.NAME = model.AccountInfo.CompanyName;
                        CustomerVO.EMAIL_ADDR = model.AccountInfo.CompanyEmail;
                        CustomerVO.OFFICE_NO = model.AccountInfo.OfficeNo;
                        CustomerVO.FAX_NO = model.AccountInfo.FaxNo;

                        CustomerVO.LAST_UPD_BY = UserId;
                        AccountBLL.UpdateCustomer(CustomerVO);
                    }

                    //update user
                    var UserVO = UserBLL.Get(UserId);
                    //UserVO.USERNAME = model.UserInfo.Email;
                    UserVO.PASSWORD = model.UserInfo.Password;

                    UserVO.FIRST_NAME = model.UserInfo.ContactName;
                    UserVO.MOBILE_NO = model.UserInfo.ContactNo;

                    UserVO.ACCNT_ID = model.AccountInfo.CustomerId; //Accnt FK

                    UserBLL.Update(UserVO, USER_TYPE.Admin);

                    //Session["Email"] = model.UserInfo.Email;
                    Session["Name"] = model.UserInfo.ContactName ?? " ";

                    TempData["Message"] = "Successfully done.";

                    return RedirectToAction("Admin", "Setting");
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

    }
}