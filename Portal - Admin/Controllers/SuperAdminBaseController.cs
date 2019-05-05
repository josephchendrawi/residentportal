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
    public class SuperAdminBaseController : Controller
    {
        protected override void ExecuteCore()
        {
            if (Session["Email"] != null && Session["Name"] != null && Session["UserId"] != null)
            {
                if (UserBLL.Get(long.Parse((string)Session["UserId"])).USER_TYPE != ((int)USER_TYPE.SuperAdmin).ToString())
                {
                    base.View("Unauthorized").ExecuteResult(ControllerContext);
                }
                else
                {
                    base.ExecuteCore();
                }
            }
            else
            {
                base.View("AdminUnauthenticated").ExecuteResult(ControllerContext);
            }
        }

        protected override bool DisableAsyncSupport
        {
            get { return true; }
        }
    }
}