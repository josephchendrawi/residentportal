using ResComm.Web.Lib.BLL;
using ResComm.Web.Lib.Interface.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ResComm.Web.Payment.Controllers
{
    public class CallbackController : Controller
    {
        BillingBLL BillingBLL = new BillingBLL();
        BillingPaymentBLL BillingPaymentBLL = new BillingPaymentBLL();
        // GET: Callback
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetPayment(string r = "")
        {
            string v1 = Server.UrlDecode(HttpContext.Request["MerchantCode"]);
            string v2 = Server.UrlDecode(HttpContext.Request["PaymentId"]);
            string v3 = Server.UrlDecode(HttpContext.Request["RefNo"]);
            string v4 = Server.UrlDecode(HttpContext.Request["Amount"]);
            string v5 = Server.UrlDecode(HttpContext.Request["Currency"]);
            string v6 = Server.UrlDecode(HttpContext.Request["Remark"]);
            string v7 = Server.UrlDecode(HttpContext.Request["TransId"]);
            string v8 = Server.UrlDecode(HttpContext.Request["AuthCode"]);
            string v9 = Server.UrlDecode(HttpContext.Request["Status"]);
            string v10 = Server.UrlDecode(HttpContext.Request["ErrDesc"]);
            string v11 = Server.UrlDecode(HttpContext.Request["Signature"]);

            string key = System.Configuration.ConfigurationManager.AppSettings["IPAYKEY"];
            string code = System.Configuration.ConfigurationManager.AppSettings["IPAYCODE"];
            string returntobill = System.Configuration.ConfigurationManager.AppSettings["MEMBERURL"];
            string signature = Helper.SignatureEncrypt(key + code + v2 + v3 + v4.Replace(",", "").Replace(".", "") + v5 + v9);
            v11 = v11.Replace(" ", "+");
            decimal paymentid = 0;
            bool response = SavePayment(v1, v2, v3, v4, v5, v6, v7, v8, v9, v10, v11, signature, ref paymentid);
            if (response)
            {
                HttpContext.Response.Write("RECEIVEOK");
            }
            else
            {
                HttpContext.Response.Write("FAILED");
            }
            if (r != "")//for redirect response by id,
            {
                if (v9 == "1")
                {
                    //success response
                    return Redirect(returntobill + "/" + r);
                }
            }


            return new EmptyResult();
        }
        public bool SavePayment(string MerchantCode, string paymentId, string refno, string stramount, string currency, string remark, string transid, string authcode, string status, string errdesc, string signature, string checksignature, ref decimal paymentid)
        {
            bool response = false;            
            if (status == "1") //if status == success == 1
            {
                if (signature == checksignature)
                {
                    long billingid = 0;
                    long.TryParse(refno, out billingid);
                    decimal amount = 0;
                    decimal.TryParse(stramount, out amount);

                    var check = BillingPaymentBLL.GetByBillingId(billingid);
                    if (check == null)
                    {

                        var BillingPaymentVO = new ResComm.Web.Lib.Interface.Models.Billing.BillingPaymentVO();
                        BillingPaymentVO.STATUS = BILLING_STATUS.Paid.ToString();
                        BillingPaymentVO.PAYMENT_METHOD = "Ipay88";
                        BillingPaymentVO.PAYMENT_DATE = DateTime.Now;
                        BillingPaymentVO.REFERENCE = transid;
                        BillingPaymentVO.AMOUNT = amount;
                        BillingPaymentVO.BILLING_ID = billingid;
                        var result = BillingPaymentBLL.CreateBillingPayment(BillingPaymentVO);

                        if (result != 0)
                        {
                            var BillingVO = BillingBLL.Get(billingid);
                            BillingVO.STATUS = BILLING_STATUS.Paid.ToString();
                            BillingBLL.UpdateBilling(BillingVO);
                        }
                    }
                }
            }
            return response;
        }
    }
}