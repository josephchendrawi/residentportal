using ResComm.Web.Lib.BLL;
using ResComm.Web.Lib.Interface.Common.Enums;
using ResComm.Web.Lib.Interface.Models.Account;
using ResComm.Web.Lib.Interface.Models.Address;
using ResComm.Web.Lib.Interface.Models.Facility;
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
    public class HomeController : AdminBaseController
    {
        UserBLL UserBLL = new UserBLL();
        AddressBLL AddressBLL = new AddressBLL();
        AccountBLL AccountBLL = new AccountBLL();
        PropertyBLL PropertyBLL = new PropertyBLL();
        UnitBLL UnitBLL = new UnitBLL();
        FacilityBLL FacilityBLL = new FacilityBLL();
        FacilityBookingBLL FacilityBookingBLL = new FacilityBookingBLL();

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetStat()
        {
            var UserId = long.Parse((string)Session["UserId"]);
            var UserRelation = AccountBLL.GetRelation(UserId);
            var PropertyId = UserRelation.PropertyId;
            var CustomerId = UserRelation.CustomerId;

            int temp = 0;

            int UnitCount = 0;
            UnitCount = UnitBLL.GetUnitsByProperty(PropertyId, 0, int.MaxValue, ref temp, "", "", new UnitVO()).Count();

            int OwnerCount = 0;
            OwnerCount = UserBLL.GetMemberUsers(0, int.MaxValue, ref temp, "", "", new UserVO() { ACCNT_ID = CustomerId }, MEMBER_TYPE.Owner).Count();

            int ResidentCount = 0;
            ResidentCount = UserBLL.GetMemberUsers(0, int.MaxValue, ref temp, "", "", new UserVO() { ACCNT_ID = CustomerId }, MEMBER_TYPE.Resident).Count();
            
            var StatResult = new
            {
                UnitCount = UnitCount,
                OwnerCount = OwnerCount,
                ResidentCount = ResidentCount,
            };

            return Json(StatResult, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBillingStat()
        {
            var UserId = long.Parse((string)Session["UserId"]);
            var UserRelation = AccountBLL.GetRelation(UserId);
            var PropertyId = UserRelation.PropertyId;
            var CustomerId = UserRelation.CustomerId;

            int temp = 0;

            var CollectedAmountList = new List<decimal>();

            decimal[] t1 = { 34.4m, 23.19m, 15.2m, 49.7m, 19.5m, 52.2m, 46.3m, 19.9m, 23.19m, 15.2m, 46.3m, 19.9m };
            CollectedAmountList = t1.ToList();

            var OutstandingAmountList = new List<decimal>();

            decimal[] t2 = { 19.5m, 52.2m, 46.3m, 19.9m, 34.4m, 23.19m, 15.2m, 49.7m, 52.2m, 46.3m, 19.5m, 52.2m };
            OutstandingAmountList = t2.ToList();

            var MonthList = new List<string>();
            for (int i = 11; i >= 0; i--)
            {
                MonthList.Add(DateTime.UtcNow.AddMonths(i * -1).ToString("MMM yy"));
            }

            var StatResult = new
            {
                CollectedAmountList = CollectedAmountList,
                OutstandingAmountList = OutstandingAmountList,
                MonthList = MonthList,
            };

            return Json(StatResult, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ListTodayFacilityBooking(DTParameterModel param)
        {
            var UserId = long.Parse((string)Session["UserId"]);
            var UserRelation = AccountBLL.GetRelation(UserId);
            var PropertyId = UserRelation.PropertyId;

            try
            {
                int TotalCount = 0;
                var List = FacilityBookingBLL.GetFacilitySlotBookings(0, 10, ref TotalCount, "CreatedDate", "desc",
                    new FacilitySlotBookingVO(), PropertyId);

                List<FacilityBooking> FacilityBookingList = new List<FacilityBooking>();

                foreach (var v in List)
                {
                    FacilityBooking VM = new FacilityBooking();

                    var FacilitySlot = FacilityBLL.GetFacilitySlot(v.FACILITY_SLOT_ID ?? 0);
                    if (FacilitySlot != null)
                    {
                        VM.FacilityId = FacilitySlot.FACILITY_ID ?? 0;
                        VM.FacilityName = FacilityBLL.GetFacility(VM.FacilityId).NAME;
                        VM.UserId = v.USER_ID;
                        VM.UserName = v.VO_USER_FIRST_NAME;
                        VM.BookingTime = v.VO_FACILITY_START_TIME == null ? "" : v.VO_FACILITY_START_TIME.Value.ToString("HH:mm");
                        VM.BookingTime += " - ";
                        VM.BookingTime += v.VO_FACILITY_END_TIME == null ? "" : v.VO_FACILITY_END_TIME.Value.ToString("HH:mm");
                        VM.BookingDate += v.BOOK_DATE == null ? "" : v.BOOK_DATE.Value.ToString("dd MMM yyyy");

                        FacilityBookingList.Add(VM);
                    }
                }

                DTResult<FacilityBooking> model = new DTResult<FacilityBooking>
                {
                    draw = param.Draw,
                    data = FacilityBookingList,
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
                    var User = UserBLL.AdminLogin(model.Email, model.Password);

                    if (User != null)
                    {
                        //store user info
                        Session["Email"] = model.Email;
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
                                if (form["ReturnURL"] == null || form["ReturnURL"] == "")
                                {
                                    return RedirectToAction("Index", "Home");
                                }
                                else
                                {
                                    return Redirect(form["ReturnURL"]);
                                }
                            }
                        }
                        else if (User.USER_TYPE == ((int)USER_TYPE.SuperAdmin).ToString())
                        {
                            if (form["ReturnURL"] == null || form["ReturnURL"] == "")
                            {
                                return RedirectToAction("Index", "SuperAdmin");
                            }
                            else
                            {
                                return Redirect(form["ReturnURL"]);
                            }
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
            AdminSetup model = new AdminSetup();

            
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
            if (UserR.PropertyId != 0)
            {
                var PropertyVO = PropertyBLL.Get(UserR.PropertyId);

                model.PropertyInfo.PropertyId = UserR.PropertyId;
                model.PropertyInfo.PropertyName = PropertyVO.NAME;
                model.PropertyInfo.Address1 = PropertyVO.ADDR_1;
                model.PropertyInfo.Address2 = PropertyVO.ADDR_2;
                model.PropertyInfo.City = PropertyVO.CITY;
                model.PropertyInfo.State = PropertyVO.STATE;
                model.PropertyInfo.ZIP = PropertyVO.POSTAL_CD;
                model.PropertyInfo.Country = PropertyVO.COUNTRY;
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Setup(AdminSetup model, FormCollection form)
        {
            if (ModelState.IsValid)
            {
                try
                {                    
                    var UserId = long.Parse((string)Session["UserId"]);

                    var isSave = form["isSave"] == "true" ? true : false;

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

                    //create/update property
                    var PropertyVO = PropertyBLL.Get(model.PropertyInfo.PropertyId ?? 0);
                    if (PropertyVO == null)
                    {
                        var NewPropertyId = PropertyBLL.CreateProperty(new PropertyVO()
                        {
                            NAME = model.PropertyInfo.PropertyName,
                            ADDR_1 = model.PropertyInfo.Address1,
                            ADDR_2 = model.PropertyInfo.Address2,
                            CITY = model.PropertyInfo.City,
                            STATE = model.PropertyInfo.State,
                            POSTAL_CD = model.PropertyInfo.ZIP,
                            COUNTRY = model.PropertyInfo.Country,
                            CREATED_BY = UserId,

                            ACCNT_ID = model.AccountInfo.CustomerId, //Account(Customer) FK
                        });

                        model.PropertyInfo.PropertyId = NewPropertyId;
                    }
                    else
                    {
                        PropertyVO.NAME = model.PropertyInfo.PropertyName;
                        PropertyVO.ADDR_1 = model.PropertyInfo.Address1;
                        PropertyVO.ADDR_2 = model.PropertyInfo.Address2;
                        PropertyVO.CITY = model.PropertyInfo.City;
                        PropertyVO.STATE = model.PropertyInfo.State;
                        PropertyVO.POSTAL_CD = model.PropertyInfo.ZIP;
                        PropertyVO.COUNTRY = model.PropertyInfo.Country;
                        PropertyVO.LAST_UPD_BY = UserId;
                        PropertyBLL.UpdateProperty(PropertyVO);
                    }

                    //update user
                    var UserVO = UserBLL.Get(UserId);
                    //UserVO.USERNAME = model.UserInfo.Email;
                    UserVO.PASSWORD = model.UserInfo.Password;

                    UserVO.FIRST_NAME = model.UserInfo.ContactName;
                    UserVO.MOBILE_NO = model.UserInfo.ContactNo;

                    UserVO.ACCNT_ID = model.AccountInfo.CustomerId; //Accnt FK

                    UserBLL.Update(UserVO, USER_TYPE.Admin);

                    //update user status
                    if (isSave != true)
                    {
                        UserBLL.UpdateStatus(UserId, USER_ACCNT_STATUS.Active);

                        //Session["Email"] = model.UserInfo.Email;
                        Session["Name"] = model.UserInfo.ContactName ?? " ";
                    }

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

    }
}