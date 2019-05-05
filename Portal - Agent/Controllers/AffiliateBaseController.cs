using ResComm.Web.Lib.BLL;
using ResComm.Web.Lib.Interface.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace ResComm.Web.Controllers
{
    public class AffiliateBaseController : Controller
    {
        protected override void ExecuteCore()
        {
            if (Request.Path.ToLower().Contains("/login"))
            {
                if (Session["Email"] != null && Session["Name"] != null && Session["UserId"] != null)
                {
                    base.RedirectToAction("Index", "Home").ExecuteResult(ControllerContext);
                }
                else
                {
                    base.ExecuteCore();
                }
            }
            else if (Request.Path.ToLower().Contains("/logout") || Request.Path.ToLower().Contains("/register"))
            {
                base.ExecuteCore();
            }
            else
            {
                if (Session["Email"] != null && Session["Name"] != null && Session["UserId"] != null
                    && UserBLL.IsAuthenticated(long.Parse((string)Session["UserId"]))
                )
                {
                    //checking First Time Login 
                    if (UserBLL.Get(long.Parse((string)Session["UserId"])).ACCNT_STATUS == ((int)USER_ACCNT_STATUS.Prospect).ToString())
                    {
                        base.View("AffiliateFirstTimeLogin").ExecuteResult(ControllerContext);
                    }
                    else
                    {
                        base.ExecuteCore();
                    }
                }
                else
                {
                    ViewBag.ReturnURL = Server.UrlEncode(Request.RawUrl);
                    base.View("AffiliateUnauthenticated").ExecuteResult(ControllerContext);
                }
            }
        }

        protected override bool DisableAsyncSupport
        {
            get { return true; }
        }
    }
}