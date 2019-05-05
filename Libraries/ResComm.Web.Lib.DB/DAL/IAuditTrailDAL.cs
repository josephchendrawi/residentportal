#region Using
using ResComm.Web.Lib.Interface.Models.Audit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
#endregion

namespace ResComm.Web.Lib.DB.DAL
{
    public interface IAuditTrailDAL
    {
        void InsertAuditTrail(AuditTrailVO auditTrailVO);

        List<AuditTrailVO> GetAuditTrail(string moduleName, long auditRowId);
    }
}
