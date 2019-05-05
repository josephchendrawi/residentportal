using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Main_Website.Controllers
{
    public class CommonController : Controller
    {
        [Route("common/policy-affiliate/privacy-policy")]
        public ActionResult PolicyAffiliate_PrivacyPolicy()
        {
            return View("~/Views/common/policy-affiliate/privacy-policy.cshtml");
        }
        [Route("common/policy-affiliate/terms-of-use-beta")]
        public ActionResult PolicyAffiliate_TermsOfUseBeta()
        {
            return View("~/Views/common/policy-affiliate/terms-of-use-beta.cshtml");
        }
        [Route("common/policy-affiliate/terms-of-use-standard")]
        public ActionResult PolicyAffiliate_TermsOfUseStandard()
        {
            return View("~/Views/common/policy-affiliate/terms-of-use-standard.cshtml");
        }

        [Route("common/policy-general/disclaimer")]
        public ActionResult PolicGeneral_Disclaimer()
        {
            return View("~/Views/common/policy-general/disclaimer.cshtml");
        }
        [Route("common/policy-general/privacy-policy")]
        public ActionResult PolicGeneral_PrivacyPolicy()
        {
            return View("~/Views/common/policy-general/privacy-policy.cshtml");
        }
        [Route("common/policy-general/terms-of-use")]
        public ActionResult PolicGeneral_TermsOfUse()
        {
            return View("~/Views/common/policy-general/terms-of-use.cshtml");
        }

        [Route("common/policy-property/privacy-policy")]
        public ActionResult PolicProperty_PrivacyPolicy()
        {
            return View("~/Views/common/policy-property/privacy-policy.cshtml");
        }
        [Route("common/policy-property/terms-of-use-beta")]
        public ActionResult PolicProperty_TermsOfUseBeta()
        {
            return View("~/Views/common/policy-property/terms-of-use-beta.cshtml");
        }
        [Route("common/policy-property/terms-of-use")]
        public ActionResult PolicProperty_TermsOfUse()
        {
            return View("~/Views/common/policy-property/terms-of-use.cshtml");
        }

        [Route("common/policy-residentowner/PrivacyPolicy")]
        public ActionResult PolicResidentOwner_Disclaimer()
        {
            return View("~/Views/common/policy-residentowner/privacy-policy.cshtml");
        }
        [Route("common/policy-residentowner/terms-of-use")]
        public ActionResult PolicResidentOwner_TermsOfUse()
        {
            return View("~/Views/common/policy-residentowner/terms-of-use.cshtml");
        }

    }
}