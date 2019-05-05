using ResComm.Web.Lib.BLL;
using ResComm.Web.Lib.DB.DAL;
using ResComm.Web.Lib.Enums;
using ResComm.Web.Lib.Interface.Common.Enums;
using ResComm.Web.Lib.Interface.Models.Account;
using ResComm.Web.Lib.Interface.Models.Property;
using ResComm.Web.Lib.Interface.Models.SubscriptionOrder;
using ResComm.Web.Lib.Interface.Models.User;
using ResComm.Web.Lib.Service;
using ResComm.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ResComm.Web.Controllers
{
    [RoutePrefix("api")]
    public class APIController : ApiController
    {
        private UserBLL UserBLL = new UserBLL();
        private SubscriptionPackageDALImpl SubscriptionPackageBLL = new SubscriptionPackageDALImpl();
        private SubscriptionOrderDALImpl SubscriptionOrderBLL = new SubscriptionOrderDALImpl();

        [Route("affiliate/register")]
        [HttpPost]
        public APIResponse AffiliateRegister(AffiliateRegistration model)
        {
            var result = new APIResponse();

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

                        result.Success = true;

                        result.Result = UserBLL.Get(Affiliate_UserId).REFERRAL_CODE;                        
                    }
                    else
                    {
                        result.Message = "Unknown Error occured.";
                    }
                }
                catch (Exception ex)
                {
                    result.Message = ex.Message;
                }
            }
            else
            {
                result.Message = string.Join(" ", ModelState.Values
                                        .SelectMany(v => v.Errors)
                                        .Select(e => e.ErrorMessage));
            }

            return result;
        }

        [Route("property/register/beta")]
        public APIResponse BetaPropertyRegister(BetaPropertyRegistration Model)
        {
            var result = new APIResponse();

            if (ModelState.IsValid)
            {
                try
                {
                    //checking packageid valid
                    var SubscriptionPackageVO = SubscriptionPackageBLL.GetSubscriptionPackage(Model.PackageId);
                    if (SubscriptionPackageVO == null)
                        throw new Exception("Invalid Subscription Package.");

                    //checking payment status
                    //
                    var PaymentStatus = PAYMENT_STATUS.DebitCard;

                    //create user
                    var UserVO = new UserVO();
                    UserVO.USERNAME = Model.Email;
                    UserVO.PASSWORD = Model.Password;
                    UserVO.PAYMENT_STATUS = (int)PaymentStatus;

                    UserVO.ACTIVE_FLG = "1";
                    UserVO.ACCNT_STATUS = ((int)USER_ACCNT_STATUS.Prospect).ToString();

                    var userid = new UserBLL().CreateAdmin(UserVO);

                    if (userid != 0)
                    {
                        //create customer(account)
                        var NewCustomerId = new AccountBLL().CreateAccount(new CustomerVO()
                        {
                            NAME = Model.Name,
                            
                            ACCNT_TYPE_CD = "1", ///
                            CREATED_BY = userid,

                            CUST_USER_ID = userid, //User FK
                            ADDR_ID = null, //Address FK
                        }, Model.AffiliateReferral);

                        //create property
                        var NewPropertyId = new PropertyBLL().CreateProperty(new PropertyVO()
                        {
                            NAME = Model.PropertyName,
                            CREATED_BY = userid,

                            ACCNT_ID = NewCustomerId, //Account(Customer) FK
                        });

                        //create property subscription order
                        var Reffered_AffiliateID = UserBLL.GetAffiliate(Model.AffiliateReferral);
                        var NewSubscriptionOrder = SubscriptionOrderBLL.CreateSubscriptionOrder(new SubscriptionOrderVO()
                        {
                            ORDER_SUBMITTED_BY = userid,
                            CREATED_BY = userid,
                            STATUS = (int)SUBSCRIPTION_ORDER_STATUS.PendingPayment,
                            PRICE = SubscriptionPackageVO.PRICE,
                            COMMISSION = 10, // fixed for Beta Register
                            AFFILIATE_ID = Reffered_AffiliateID == null ? 0 : Reffered_AffiliateID.ROW_ID,

                            PROPERTY_ID = NewPropertyId,
                            SUBSCRIPTION_PACKAGE_ID = SubscriptionPackageVO.ROW_ID,                            
                        });                        

                        //update user
                        UserVO = UserBLL.Get(userid);

                        UserVO.FIRST_NAME = Model.Name;
                        UserVO.MOBILE_NO = Model.ContactNo;

                        UserVO.ACCNT_ID = NewCustomerId; //Accnt FK

                        new UserBLL().Update(UserVO, USER_TYPE.Admin);

                        EmailService.SendBetaThankYou(UserVO.USERNAME);
                        result.Success = true;
                    }
                }
                catch (Exception ex)
                {
                    result.Message = ex.Message;
                }
            }
            else
            {
                result.Message = string.Join(" ", ModelState.Values
                                        .SelectMany(v => v.Errors)
                                        .Select(e => e.ErrorMessage));
            }

            return result;
        }

    }
}
