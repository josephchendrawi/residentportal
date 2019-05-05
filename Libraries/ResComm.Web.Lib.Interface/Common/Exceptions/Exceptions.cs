using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResComm.Web.Lib.Interface.Common.Exceptions
{
    public static class Exceptions
    {
        public const string USER_ALREADY_EXISTS = "This Email is already exists. Please use other email.";

        public const string TOKEN_EXPIRED = "The link is already expired.";
        public const string TOKEN_INCORRECT = "The link is incorrect.";
    }
}
