
using ResComm.Web.Lib.Interface.Models.Audit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResComm.Web.Lib.DB.DAL
{
    public interface IAuditLogDAL
    {
        void InsertAuditLog(AuditLogVO auditLogVO);
    }
}
