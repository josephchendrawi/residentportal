using ResComm.Web.Lib.BLL;
using ResComm.Web.Lib.DB.DAL;
using ResComm.Web.Lib.Enums;
using ResComm.Web.Lib.Interface.Models.Affiliate;
using ResComm.Web.Lib.Interface.Models.Property;
using ResComm.Web.Models;
using ResComm.Web.Models.DataTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace ResComm.Web.Controllers
{
    public class PropertyController : AffiliateBaseController
    {
        PropertyBLL PropertyBLL = new PropertyBLL();
        AccountBLL AccountBLL = new AccountBLL();
        PropertySubscriptionDALImpl PropertySubscriptionBLL = new PropertySubscriptionDALImpl();
        SubscriptionOrderDALImpl SubscriptionOrderBLL = new SubscriptionOrderDALImpl();
        SubscriptionPackageDALImpl SubscriptionPackageBLL = new SubscriptionPackageDALImpl();

        public ActionResult List()
        {
            return View();
        }

        public ActionResult ListProperty(DTParameterModel param, string Name)
        {
            try
            {                
                var UserId = long.Parse((string)Session["UserId"]);
                var CustomerId = AccountBLL.GetRelation(UserId).CustomerId;

                int TotalCount = 0;

                //sorting
                string orderingFunction = param.Columns.ElementAt(param.Order.FirstOrDefault().Column).Name == "" ? param.Columns.ElementAt(param.Order.FirstOrDefault().Column).Data : param.Columns.ElementAt(param.Order.FirstOrDefault().Column).Name;
                var sortDirection = param.Order.FirstOrDefault().Dir; // asc or desc

                //filtering
                PropertyVO qFilter = new PropertyVO()
                {
                    NAME = Name,
                };
                var List = PropertyBLL.GetAll(param.Start, param.Length, ref TotalCount, orderingFunction, sortDirection, qFilter, UserId);

                List<PropertyInfo> PropertyList = new List<PropertyInfo>();

                foreach (var v in List)
                {
                    PropertyInfo VM = new PropertyInfo();

                    VM.PropertyId = v.ROW_ID;
                    VM.PropertyName = v.NAME;
                    VM.Address1 = v.ADDR_1;
                    VM.Address2 = v.ADDR_2;
                    VM.City = v.CITY;
                    VM.State = v.STATE;
                    VM.ZIP = v.POSTAL_CD;
                    VM.Country = v.COUNTRY;
                    VM.LogoURL = v.LOGO_URL;
                    VM.CreatedAt = v.CREATED.Value.ToString("dd MMM yyyy");

                    var Subscription = PropertySubscriptionBLL.GetPropertySubscriptionByPropertyId(v.ROW_ID);

                    if (Subscription != null)
                    {
                        var SubscriptionOrder = SubscriptionOrderBLL.GetSubscriptionOrder(Subscription.SUBSCRIPTION_ORDER_ID.Value);

                        VM.Status = SubscriptionOrder.STATUS ?? 0;
                        VM.StatusText = ((SUBSCRIPTION_ORDER_STATUS)SubscriptionOrder.STATUS).ToString();
                        VM.PackageName = SubscriptionOrder.SubscriptionPackageName;
                        VM.SubscriptionPeriod = Subscription.START_DATE.Value.ToString("dd/MM/yyyy") + " - " + Subscription.END_DATE.Value.ToString("dd/MM/yyyy");
                        VM.Commission = SubscriptionOrder.COMMISSION.Value;
                        VM.isCommissionPaid = false; /////
                    }
                    else
                    {
                        int temp = 0;
                        var Last_SubscriptionOrder = SubscriptionOrderBLL.GetSubscriptionOrders(0, int.MaxValue, ref temp, qFilter: new Lib.Interface.Models.SubscriptionOrder.SubscriptionOrderVO() { PROPERTY_ID = v.ROW_ID, AFFILIATE_ID = UserId }).FirstOrDefault();

                        if (Last_SubscriptionOrder != null)
                        {
                            var SubscriptionPackage = SubscriptionPackageBLL.GetSubscriptionPackage(Last_SubscriptionOrder.SUBSCRIPTION_PACKAGE_ID.Value);

                            VM.Status = Last_SubscriptionOrder.STATUS ?? 0;
                            VM.StatusText = ((SUBSCRIPTION_ORDER_STATUS)Last_SubscriptionOrder.STATUS).ToString();
                            VM.PackageName = Last_SubscriptionOrder.SubscriptionPackageName;
                            VM.SubscriptionPeriod = SubscriptionPackage.DURATION == 0 ? "LIFETIME" : SubscriptionPackage.DURATION + " years";
                            VM.Commission = Last_SubscriptionOrder.COMMISSION.Value;
                            VM.isCommissionPaid = false;
                        }
                    }

                    PropertyList.Add(VM);
                }

                DTResult<PropertyInfo> model = new DTResult<PropertyInfo>
                {
                    draw = param.Draw,
                    data = PropertyList,
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

        public ActionResult View(int Id)
        {
            ViewPropertyVM VM = new ViewPropertyVM();

            var v = PropertyBLL.Get(Id);

            VM.PropertyId = v.ROW_ID;
            VM.PropertyName = v.NAME;
            VM.Address1 = v.ADDR_1;
            VM.Address2 = v.ADDR_2;
            VM.City = v.CITY;
            VM.State = v.STATE;
            VM.ZIP = v.POSTAL_CD;
            VM.Country = v.COUNTRY;
            VM.LogoURL = v.LOGO_URL;

            var User = UserBLL.Get(AccountBLL.Get(v.ACCNT_ID.Value).CUST_USER_ID.Value);

            VM.ContactName = User.FIRST_NAME;
            VM.ContactNo = User.MOBILE_NO;
            VM.ContactEmail = User.USERNAME;

            return View(VM);
        }

    }
}