using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Main_Website
{
    public class Constant
    {
        public static string AdminURL = ConfigurationManager.AppSettings["AdminURL"];
    }
}