using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Main_Website.Controllers
{
    public class HomeController : Controller
    {
        [Route("")]
        [Route("index")]
        public ActionResult Index()
        {
            return View();
        }

        [Route("pricing")]
        public ActionResult Pricing()
        {
            return View();
        }

    }
}