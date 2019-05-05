using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Main_Website.Controllers
{
    public class BetaController : Controller
    {
        [Route("beta")]
        [Route("beta/index")]
        public ActionResult Index()
        {
            return View();
        }

        [Route("beta/affiliate")]
        public ActionResult Affiliate()
        {
            return View();
        }

        [Route("beta/BetaProgramFull")]
        public ActionResult BetaProgramFull()
        {
            return View();
        }

        [Route("beta/BetaThankYou")]
        public ActionResult BetaThankYou()
        {
            return View();
        }

    }
}