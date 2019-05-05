using ResComm.Web.Lib.BLL;
using ResComm.Web.Lib.DB.DAL;
using ResComm.Web.Lib.Enums;
using ResComm.Web.Lib.Interface.Models.Affiliate;
using ResComm.Web.Lib.Interface.Models.SubscriptionOrder;
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
    public class CommissionController : AffiliateBaseController
    {
        private SubscriptionOrderDALImpl SubscriptionOrderBLL = new SubscriptionOrderDALImpl();
        private AccountBLL AccountBLL = new AccountBLL();

        public ActionResult List()
        {
            return View();
        }

        public ActionResult ListCommission(DTParameterModel param)
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
                SubscriptionOrderVO qFilter = new SubscriptionOrderVO()
                {
                    AFFILIATE_ID = UserId,
                    STATUS = (int)SUBSCRIPTION_ORDER_STATUS.Paid
                };
                var List = SubscriptionOrderBLL.GetSubscriptionOrders(param.Start, param.Length, ref TotalCount, orderingFunction, sortDirection, qFilter);

                List<Commission> SubscriptionOrderList = new List<Commission>();

                foreach (var v in List)
                {
                    Commission VM = new Commission();

                    VM.CreatedDate = v.CREATED == null ? "" : v.CREATED.Value.ToString("dd MMM yyyy HH:mm");
                    VM.Property = v.PropertyName;
                    VM.SubscriptionPackage = v.SubscriptionPackageName;
                    VM.PackagePrice = v.PRICE;
                    VM.CommissionPercentage = v.COMMISSION;
                    VM.CommissionAmount = (v.PRICE ?? 0) * (decimal)(v.COMMISSION??0);

                    SubscriptionOrderList.Add(VM);
                }

                DTResult<Commission> model = new DTResult<Commission>
                {
                    draw = param.Draw,
                    data = SubscriptionOrderList,
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

    }
}