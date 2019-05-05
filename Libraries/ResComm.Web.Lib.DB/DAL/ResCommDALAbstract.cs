using ResComm.Web.Lib.DB.EF;
using ResComm.Web.Lib.Interface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResComm.Web.Lib.DB.DAL
{
    public abstract class ResCommDALAbstract
    {
        /// <summary>
        /// This is for AuditTrailCallHandler that will extract the original data from DB
        /// Don't have to implement it in the code (just throw NotImplementedException() will do)
        /// </summary>
        /// <param name="rowId"></param>
        /// <returns></returns>
        public abstract BaseVO GetDefaultData(long rowId);
    }
}
