using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Main_Website.Controllers
{
    public class AffiliateController : Controller
    {
        [Route("affiliate")]
        [Route("affiliate/index")]
        public ActionResult Index()
        {
            return View();
        }

    }
}