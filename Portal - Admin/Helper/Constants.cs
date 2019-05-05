using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace ResComm.Web.Constants
{
    public class Constant
    {
        public static string UploadPath = ConfigurationManager.AppSettings["UploadPath"];
    }
}